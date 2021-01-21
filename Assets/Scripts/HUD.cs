using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text deathCountText;

    private Animator menuAnim;
    private const string menuSceneName = "Menu";

    private void Start()
    {
        menuAnim = GetComponentInChildren<Animator>();
        var currentDeaths = DeathCounter.GetDeathCount();
        deathCountText.text = currentDeaths.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartButtonPressed();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButtonPressed();
        }
    }

    public void PauseButtonPressed()
    {
        var pausing = Time.timeScale > 0f;
        menuAnim.SetBool("Paused", pausing);
        if (pausing)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void RestartButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuButtonPressed()
    {
        Time.timeScale = 1f;
        AudioLooper.Shutdown();
        SceneManager.LoadScene(menuSceneName);
    }
}
