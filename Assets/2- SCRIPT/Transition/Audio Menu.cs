using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] private Animator animIntro;
    void Start()
    {
        AudioManager.instance.Play("menu");

        //if (GameManager.instance.introAppeared)
        //{
        //    return;
        //}
        //else
        //{
        //    animIntro.SetTrigger("intro");
        //}
    }
}
