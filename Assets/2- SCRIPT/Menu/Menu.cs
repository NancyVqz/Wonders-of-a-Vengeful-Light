using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        AudioManager.instance.Stop("menu");
        AudioManager.instance.Play("lvl 1");
        SceneManager.LoadScene("NIVEL 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RegresarMenu()
    {
        AudioManager.instance.StopAll();
        SceneManager.LoadScene("MENU");
    }

    public void Continuar()
    {
        AudioManager.instance.StopAll();
        SceneManager.LoadScene(GameManager.instance.nivelSiguiente);
    }

    public void ContinuarMenuFinal()
    {
        AudioManager.instance.StopAll();
        SceneManager.LoadScene("MENU");
    }
}
