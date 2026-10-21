using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _actualHealth;
    [SerializeField] private float _movementSpeed = 4.5f;
    [SerializeField] private float _forceJump = 10f;

    [SerializeField] private Transform _groundSensor;
    [SerializeField] private float _sensorSize = 1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _attackDamage = 7f;
    [SerializeField] private Transform _attackHitBox;
    [SerializeField] private float _hitBoxRadius = 1f;

    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _attackSound;

    // Referencia a la imagen roja de pantalla de muerte
    [SerializeField] private GameObject _deathFlashCanvas;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private AudioSource _playerAudioSource;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _playerCollider;

    private InputAction _pauseAction;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _JumpAction;

    private Vector2 _moveInput;
    private bool _isDead = false;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerAudioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerCollider = GetComponent<Collider2D>();

        _moveAction = InputSystem.actions["Move"];
        _JumpAction = InputSystem.actions["Jump"];
        _attackAction = InputSystem.actions["Attack"];
        _pauseAction = InputSystem.actions["Pause"];
    }

    void Start()
    {
        _actualHealth = _maxHealth;
    }

    void Update()
    {
        // Si el jugador está muerto, impedimos que se mueva o interactúe
        if (_isDead) return;

        _moveInput = _moveAction.ReadValue<Vector2>();

        if (_moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
        }
        else if (_moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }

        if (_JumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        if (_attackAction.WasPressedThisFrame() && IsGrounded())
        {
            Attack();
        }

        _animator.SetBool("IsJumping", !IsGrounded());
    }

    void FixedUpdate()
    {
        if (_isDead) return;
        _rigidbody2D.linearVelocity = new Vector2(_moveInput.x * _movementSpeed, _rigidbody2D.linearVelocity.y);
    }

    void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * _forceJump, ForceMode2D.Impulse);
        PlaySFX(_jumpSound);
    }

    void Attack()
    {
        _animator.SetTrigger("IsAttacking");
        PlaySFX(_attackSound);

        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(_attackHitBox.position, _hitBoxRadius);

        foreach (Collider2D enemy in colliders2D)
        {
            if (enemy.gameObject.layer == 7)
            {
                Mimik enemyScript = enemy.GetComponent<Mimik>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(_attackDamage);
                }
            }
        }
    }

    bool IsGrounded()
    {
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(_groundSensor.position, _sensorSize);

        foreach (Collider2D item in colliders2D)
        {
            if (item.gameObject.layer == 6)
            {
                return true;
            }
        }
        return false;
    }

    public void HealthUp(float Health)
    {
        if (_isDead) return;

        _actualHealth += Health;

        if (_actualHealth > _maxHealth)
        {
            _actualHealth = _maxHealth;
        }

        if (GameManager.Instance != null)
        {
            float healthPercentage = _actualHealth / _maxHealth;
            GameManager.Instance.UpdateHealthBar(healthPercentage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        _actualHealth -= damage;

        if (_actualHealth < 0)
        {
            _actualHealth = 0;
        }

        // Actualiza la barra de vida
        if (GameManager.Instance != null)
        {
            float healthPercentage = _actualHealth / _maxHealth;
            GameManager.Instance.UpdateHealthBar(healthPercentage);
        }

        // Si se queda sin vida, iniciamos la secuencia de muerte estilo Mario
        if (_actualHealth <= 0)
        {
            StartCoroutine(DieSequence());
        }
    }

    private IEnumerator DieSequence()
{
    _isDead = true;

    // 1. Activar la pantalla en rojo suave si está asignada
    if (_deathFlashCanvas != null)
    {
        _deathFlashCanvas.SetActive(true);
    }

    // 2. CONGELAR LA CÁMARA: Buscamos la cámara de Cinemachine y le quitamos el objetivo (Follow = null)
    CinemachineCamera vcam = FindObjectOfType<CinemachineCamera>(); 
    if (vcam != null)
    {
        vcam.Follow = null;
    }

    // 3. EFECTO MARIO: Desactivar colisiones para que traspase objetos y suelos
    if (_playerCollider != null)
    {
        _playerCollider.enabled = false;
    }

    // Darle el pequeño saltito hacia arriba característico antes de caer
    if (_rigidbody2D != null)
    {
        _rigidbody2D.linearVelocity = new Vector2(0f, 7f);
    }

    // 4. Esperar 2 segundos mientras el personaje cae fuera de la pantalla estática
    yield return new WaitForSeconds(2f);

    // 5. Cargar la escena de Game Over
    if (CanvasManager.Instance != null)
    {
        CanvasManager.Instance.LoadGameOverScene();
    }
}

    void PlaySFX(AudioClip clip)
    {
        if (clip != null && _playerAudioSource != null)
        {
            _playerAudioSource.PlayOneShot(clip);
        }
    }

    void OnDrawGizmos()
    {
        if (_groundSensor != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundSensor.position, _sensorSize);
        }

        if (_attackHitBox != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_attackHitBox.position, _hitBoxRadius);
        }
    }
}