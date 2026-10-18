using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //int para guardar la vida maxima
    [SerializeField]private int _maxHealth = 100;
    [SerializeField]private int _actualHealth;
    [SerializeField]private float _movementSpeed = 4.5f;
    [SerializeField]private float _forceJump = 10;
    
    
    /*float decimales = 5,4f;

    bool boleana = true;
    
    string texto = "asdasdadasd";*/

[SerializeField] private Transform _groundSensor;
[SerializeField] private float _sensorSize = 1;
[SerializeField] private LayerMask _groundLayer;
[SerializeField] private int _attackDamage = 7;
[SerializeField] private Transform _attackHitBox;
[SerializeField] private float _hitBoxRadius = 1f;

[SerializeField] private AudioClip _jumpSound;
[SerializeField] private AudioClip _attackSound;


private Rigidbody2D _rigidbody2D;

private Animator _animator;

private AudioSource _playerAudioSource;

private InputAction _pauseAction;
private InputAction _moveAction;

private InputAction _attackAction;

private InputAction _JumpAction;

private Vector2 _moveInput;


    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _moveAction = InputSystem.actions["Move"];

        _JumpAction = InputSystem.actions["Jump"];

        _attackAction = InputSystem.actions["Attack"];

        _animator = GetComponent<Animator>();

        _pauseAction = InputSystem.actions["Pause"];

        _playerAudioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_actualHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(_pauseAction.WasPressedThisFrame())
        {
            GameManager.Instance.Pause();
        }

        if(GameManager.Instance.IsPaused())
        {
            return;
        }
        _moveInput = _moveAction.ReadValue<Vector2>();

        if(_moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
        }
        else if(_moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }


        if(_JumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        if(_attackAction.WasPressedThisFrame() && IsGrounded())
        {
            Attack();
        }

        _animator.SetBool("IsJumping", !IsGrounded());
    }

    void FixedUpdate()
    {  
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
            if(enemy.gameObject.layer == 7)
            {
                Mimik enemyScript = enemy.GetComponent<Mimik>();
                enemyScript.TakeDamage(_attackDamage);
            }
        }
    }

    bool IsGrounded()
    {
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(_groundSensor.position, _sensorSize);

        foreach (Collider2D item in colliders2D)
        {
            if(item.gameObject.layer == 6)
            {
            return true;
            }

        }
        return false;        
    }
    public void HealthUp(int Health)
    {
        _actualHealth += Health;

        if(_actualHealth >=99)
        {
            _actualHealth = _maxHealth;
        }
    }

    void PlaySFX(AudioClip clip)
    {
        _playerAudioSource.PlayOneShot(clip);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundSensor.position, _sensorSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_attackHitBox.position, _hitBoxRadius);
    }

    public void TakeDamage(int damage)
{
    _actualHealth -= damage;

    // Opcional: Aquí puedes añadir feedback visual o de sonido de que el jugador recibe daño

    if (_actualHealth <= 0)
    {
        // Lógica de muerte del jugador (p. ej., reiniciar escena o desactivar movimiento)
        Destroy(gameObject);
    }
}

}