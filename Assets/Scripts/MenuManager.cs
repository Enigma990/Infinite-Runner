using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public void Pause(GameObject pauseMenu)
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        pauseMenu.GetComponentInChildren<Text>().text = "Score: " + PlayerController.Instance.Score.ToString();
    }

    public void Resume(GameObject pauseMenu)
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
