using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        //AudioManager.instance.Play("menu");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("NIVEL 1");
        //AudioManager.instance.Stop("menu");
        //AudioManager.instance.Play("lvl 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MENU");
        //AudioManager.instance.Play("menu");
        //AudioManager.instance.Stop("lvl 1");
        //AudioManager.instance.Stop("lvl 2");
        //AudioManager.instance.Stop("lvl 3");
    }

    public void Continuar()
    {
        SceneManager.LoadScene(GameManager.instance.nivelSiguiente);
    }

    public void ContinuarMenuFinal()
    {
        SceneManager.LoadScene("MENU");
        //AudioManager.instance.Play("menu");
        //AudioManager.instance.Play("final");
    }
}
