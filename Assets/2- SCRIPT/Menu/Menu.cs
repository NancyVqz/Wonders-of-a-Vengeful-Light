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
        SceneManager.LoadScene("TIENDA");
        //Application.Quit();
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MENU");
        //AudioManager.instance.Play("menu");
        //AudioManager.instance.Stop("lvl 1");
    }

    public void Continuar()
    {
        SceneManager.LoadScene("NIVEL 1");
    }
}
