using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioManager.instance.PauseAll();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioManager.instance.UnPauseAll();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        AudioManager.instance.StopAll();
        SceneManager.LoadScene("MENU");
    }

    public void Nivel1()
    {
        Time.timeScale = 1f;
        GameManager.instance.energy = 0;
        GameManager.instance.score = 0;
        SceneManager.LoadScene("NIVEL 1");
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("lvl 1");
    }
}
