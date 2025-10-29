using UnityEngine;

public class MuerteVfx : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 0.40f);
    }


}
