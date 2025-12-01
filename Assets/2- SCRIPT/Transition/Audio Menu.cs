using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject intro;
    void Start()
    {
        AudioManager.instance.Play("menu");

        if (GameManager.instance.introAppeared)
        {
            intro.SetActive(false);
            return;
        }
        else
        {
            anim.SetTrigger("intro");
        }
    }

}
