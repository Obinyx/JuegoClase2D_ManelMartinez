using UnityEngine;
using System.Collections;

public class Mimik : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 20;
    [SerializeField] private float _damageToPlayer = 50;
    [SerializeField] private float _attackCooldown = 1.5f;

    [Header("Efectos de Sonido (SFX)")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _deathSound;
    private AudioSource _mimikAudioSource;

    private float _actualHealth;
    private bool _canDamage = true;
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _mimikAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _actualHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _actualHealth -= damage;

        if (_actualHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Reproducimos el sonido en la posición actual antes de destruir el objeto 
        // para que el audio no se corte de golpe.
        if (_deathSound != null)
        {
            AudioSource.PlayClipAtPoint(_deathSound, transform.position);
        }

        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _canDamage)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // Dispara la animación de ataque
                if (_animator != null)
                {
                    _animator.SetTrigger("IsAttacking");
                }

                // Reproduce el sonido de ataque
                PlayAttackSFX();

                player.TakeDamage(_damageToPlayer);
                StartCoroutine(DamageCooldownRoutine());
            }
        }
    }

    void PlayAttackSFX()
    {
        if (_mimikAudioSource != null && _attackSound != null)
        {
            _mimikAudioSource.PlayOneShot(_attackSound);
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        _canDamage = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canDamage = true;
    }
}