using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false;
    private Animator menuAnim;
    private const string menuSceneName = "Menu";

    // Use this for initialization
    private void Start()
    {
        menuAnim = GetComponentInChildren<Animator>();
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
        paused = !paused;
        menuAnim.SetBool("Paused", paused);
        if (paused)
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
        SceneManager.LoadScene(menuSceneName);
    }

}
