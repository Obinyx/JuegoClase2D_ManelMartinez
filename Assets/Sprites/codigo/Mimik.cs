using UnityEngine;
using System.Collections;

public class Mimik : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 20;
    [SerializeField] private int _damageToPlayer = 50;
    [SerializeField] private float _attackCooldown = 1.5f;

    private int _actualHealth;
    private bool _canDamage = true;
    private Animator _animator; // <--- Referencia al Animator

    void Awake()
    {
        _animator = GetComponent<Animator>(); // <--- Obtenemos el componente
    }

    void Start()
    {
        _actualHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _actualHealth -= damage;

        if (_actualHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
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

                player.TakeDamage(_damageToPlayer);
                StartCoroutine(DamageCooldownRoutine());
            }
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        _canDamage = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canDamage = true;
    }
}