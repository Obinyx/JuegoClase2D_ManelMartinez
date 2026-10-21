using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int coins;
    [SerializeField] private int stars;
    [SerializeField] private GameObject _gameplayCanvas;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Text _coinText; // Text Legacy
    [SerializeField] private Text _starText; // Text Legacy (0/5)
    [SerializeField] private int _levelMaxStarsAmount = 5;
    public float _playerHealth;

    private bool _isPaused = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Seguridad por si el campo se queda en 0 en el Inspector
        if (_levelMaxStarsAmount <= 0)
        {
            _levelMaxStarsAmount = 5;
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StartSoundtrack();
        }

        // Muestra "0/5" en el Text Legacy al iniciar el nivel
        UpdateStarUI();
    }

    public void AddCoin()
    {
        coins += 1;
        if (_coinText != null)
        {
            _coinText.text = coins.ToString();
        }
    }

    public void AddStar()
    {
        stars += 1;
        UpdateStarUI();

        if (stars >= _levelMaxStarsAmount)
        {
            Win();
        }
    }

    private void UpdateStarUI()
    {
        if (_starText != null)
        {
            _starText.text = stars.ToString() + "/" + _levelMaxStarsAmount.ToString();
        }
    }

    public void Pause()
    {
        if (_isPaused)
        {
            _isPaused = false;
            if (AudioManager.Instance != null) AudioManager.Instance.StartSoundtrack();
            Time.timeScale = 1;
        }
        else
        {
            _isPaused = true;
            if (AudioManager.Instance != null) AudioManager.Instance.PauseSoundtrack();
            Time.timeScale = 0;
        }

        if (CanvasManager.Instance != null)
        {
            CanvasManager.Instance.ChangeCanvasStatus(CanvasManager.Instance.pauseCanvas, CanvasManager.Instance.resumeButton);
        }
    }

    // Método para actualizar la barra de vida (recibe un valor entre 0.0 y 1.0)
    public void UpdateHealthBar(float healthPercentage)
    {
        if (_healthBar != null)
        {
            _healthBar.fillAmount = healthPercentage;
        }
    }

    public void Win()
    {
        Time.timeScale = 0f;

        if (AudioManager.Instance != null) 
            AudioManager.Instance.PauseSoundtrack();

        if (CanvasManager.Instance != null)
        {
            CanvasManager.Instance.ChangeCanvasStatus(CanvasManager.Instance.victoryCanvas, CanvasManager.Instance.retryButton);
        }
    }

    public bool IsPaused()
    {
        return _isPaused;
    }
}