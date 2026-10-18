using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    public static CanvasManager Instance;

    public GameObject pauseCanvas;

    public Button resumeButton;

    public GameObject gameOverCanvas;

    public Button retryButton;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangeCanvasStatus(GameObject canvas, Button selectedButton)
    {
        if(pauseCanvas.activeInHierarchy)
        {
            pauseCanvas.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
            selectedButton.Select();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
