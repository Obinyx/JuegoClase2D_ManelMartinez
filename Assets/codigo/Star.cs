using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] private AudioClip _starAudioClip;
    private AudioSource _starAudioSource;

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
        if (_starAudioSource != null && _starAudioClip != null)
        {
            _starAudioSource.PlayOneShot(_starAudioClip);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddStar();
            PlaySFX();
            _collider2D.enabled = false;
            _spriteRenderer.enabled = false;
            Destroy(gameObject, 0.5f); // Se destruye tras 0.5s para que dé tiempo a sonar el audio
        }
    }
}