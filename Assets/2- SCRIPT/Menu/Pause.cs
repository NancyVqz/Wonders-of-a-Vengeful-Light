using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0f;
        //AudioManager.instance.PauseAll();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        //AudioManager.instance.UnPauseAll();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MENU");
        //AudioManager.instance.StopAll();
        //AudioManager.instance.Play("menu");
    }
}
