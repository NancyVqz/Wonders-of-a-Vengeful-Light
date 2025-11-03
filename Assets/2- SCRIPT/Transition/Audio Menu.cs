using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.Play("menu");
    }
}
