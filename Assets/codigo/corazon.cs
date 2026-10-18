using UnityEngine;

public class corazon : MonoBehaviour
{
    [SerializeField] private AudioClip _HealthAudio;

    [SerializeField] private int _HealthUp = 20;

    
    private AudioSource _HeartHealthRegen;

    private SpriteRenderer _spriteRenderer;

    private BoxCollider2D _collider;


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
        _HeartHealthRegen = GetComponent<AudioSource>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _collider = GetComponent<BoxCollider2D>();
    }

    void PlaySFX()
    {
        _HeartHealthRegen.PlayOneShot(_HealthAudio);
    }

    
    void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController corazonScript = collision.GetComponent<PlayerController>();
            corazonScript.HealthUp(_HealthUp);
            PlaySFX();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            
            Destroy(gameObject, 0.5f);
        }
    }
}
