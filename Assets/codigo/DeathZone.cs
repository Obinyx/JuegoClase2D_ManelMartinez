using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"));
        {
            PlayerController _playerDeath = collision.gameObject.GetComponent<PlayerController>();
            _playerDeath.TakeDamage(100);
        }
    }
}