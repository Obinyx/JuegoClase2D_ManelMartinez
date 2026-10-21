using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <-- Importante para usar SceneManager

public class CanvasManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public Button resumeButton;
    public GameObject victoryCanvas;
    public Button retryButton;

    public static CanvasManager Instance;

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
    }

    public void ChangeCanvasStatus(GameObject canvas, Button selectedButton)
    {
        if (canvas.activeInHierarchy)
        {
            canvas.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
            selectedButton.Select();
        }
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1f; // Restaurar la escala de tiempo antes de cambiar de escena
        SceneManager.LoadScene(sceneName); // Cambio directo y seguro sin depender de SceneLoader
    }

    // Carga la escena de derrota
    public void LoadGameOverScene()
    {
        ChangeScene("GameOverScene");
    }
}