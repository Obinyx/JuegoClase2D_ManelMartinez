using UnityEngine;

public class Stars : MonoBehaviour
{
    private AudioSource _starAudioSource;

    [SerializeField] private AudioClip _starAudioClip;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider2D;

    void Awake()
    {
        _starAudioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<CircleCollider2D>();

    }

    void PlaySFX()
    {
        _starAudioSource.PlayOneShot(_starAudioClip);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddStar();
            PlaySFX();
            _collider2D.enabled = false;
            _spriteRenderer.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}