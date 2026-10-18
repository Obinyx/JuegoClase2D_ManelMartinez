using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int coins;
    [SerializeField] private int stars;
    [SerializeField] private GameObject _gameplayCanvas;
    [SerializeField] private Image _healtBar;
    [SerializeField] Text _coinText;
    [SerializeField] Text _starText;
    [SerializeField] private int _levelMaxStarsAmount;
    public float  _playerHealth;
    

    private bool _isPaused = false;

    void Awake()
    {
        if(Instance != null & Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        AudioManager.Instance.StartSoundtrack();
        
    }
    
    public void AddCoin()
    {
        coins += 1;
        _coinText.text = "x" + coins.ToString();
    }

    public void AddStar()
    {
        stars += 1;
        _starText.text = stars.ToString() + "/" + _levelMaxStarsAmount.ToString();
        if (stars == _levelMaxStarsAmount)
        {
            Win();
        }
    }

    public void Pause()
    {
        if(_isPaused)
        {
            _isPaused = false;
            AudioManager.Instance.StartSoundtrack();
            Time.timeScale = 1;
                        
        }
        else
        {
            _isPaused = true; 
            AudioManager.Instance.PauseSoundtrack();
            Time.timeScale = 0;
        }
        CanvasManager.Instance.ChangeCanvasStatus(CanvasManager.Instance.pauseCanvas, CanvasManager.Instance.resumeButton);   
    }

    public IEnumerator ReduceHealthBar(float actualHealth)
    {
        float startHealth = _healtBar.fillAmount;
        _healtBar.fillAmount = startHealth - actualHealth;
        yield return new WaitForSecondsRealtime (1f);
    }

    public IEnumerator IncreaseHealthBar(float actualHealth)
    {
        float startHealth = _healtBar.fillAmount;
        _healtBar.fillAmount = startHealth + actualHealth;
        yield return new WaitForSecondsRealtime (1f);
    }



    public void Win()
    {
        AudioManager.Instance.PauseSoundtrack();
        CanvasManager.Instance.ChangeCanvasStatus(CanvasManager.Instance.victoryCanvas, CanvasManager.Instance.retryButton);
    }

    public bool IsPaused()
    {
        return _isPaused;
    }
}