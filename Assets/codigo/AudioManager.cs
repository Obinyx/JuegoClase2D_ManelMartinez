using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource _audioSource;

    [Header("Pistas de Música de Fondo")]
    [SerializeField] private AudioClip _menuSoundtrack;
    [SerializeField] private AudioClip _level1Soundtrack;
    [SerializeField] private AudioClip _victorySoundtrack;
    [SerializeField] private AudioClip _defeatSoundtrack;

    [Header("Configuración de Escenas")]
    [SerializeField] private string _menuSceneName = "Main Menu";     // Nombre exacto de tu escena de menú
    [SerializeField] private string _gameOverSceneName = "GameOver";   // Nombre exacto de tu escena de Game Over

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Escena cargada actual: " + scene.name);

        if (scene.name == _menuSceneName)
        {
            PlayMenuMusic();
        }
        else if (scene.name == _gameOverSceneName)
        {
            Debug.Log("¡Detectada la escena de Game Over! Reproduciendo música de derrota.");
            PlayDefeatMusic();
        }
    }

    public void StartSoundtrack()
    {
        PlayMusic(_level1Soundtrack, true);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(_menuSoundtrack, true);
    }

    public void PlayVictoryMusic()
    {
        PlayMusic(_victorySoundtrack, false);
    }

    public void PlayDefeatMusic()
    {
        PlayMusic(_defeatSoundtrack, false);
    }

    private void PlayMusic(AudioClip clip, bool shouldLoop)
    {
        if (clip == null) return;

        if (_audioSource.clip == clip && _audioSource.isPlaying)
        {
            _audioSource.loop = shouldLoop;
            return;
        }

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.loop = shouldLoop; 
        _audioSource.Play();
    }

    public void PauseSoundtrack()
    {
        if (_audioSource != null)
            _audioSource.Pause();
    }

    public void ResumeSoundtrack()
    {
        if (_audioSource != null)
            _audioSource.UnPause();
    }
}