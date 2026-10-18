using UnityEngine;

public class coin : MonoBehaviour

{

    [SerializeField] private AudioClip _coinAudio;
    private AudioSource _coinAudioSource;

    private SpriteRenderer _spriteRenderer;

    private CircleCollider2D _collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Awake()
    {
        _coinAudioSource = GetComponent<AudioSource>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _collider = GetComponent<CircleCollider2D>();
    }

    void PlaySFX()
    {
        _coinAudioSource.PlayOneShot(_coinAudio);
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddCoin();
            PlaySFX();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            
            Destroy(gameObject, 0.5f);
        }
    }
}
