using UnityEngine;

public class AudioFinal : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("victory");
    }
}
