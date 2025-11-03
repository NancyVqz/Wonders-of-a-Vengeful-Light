using UnityEngine;

public class AudioTienda : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.StopAll();
        AudioManager.instance.Play("tienda");
    }
}
