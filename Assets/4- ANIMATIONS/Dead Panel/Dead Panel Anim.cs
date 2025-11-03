using UnityEngine;

public class DeadPanelAnim : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void OnEnable()
    {
        anim.SetTrigger("murio");
    }
}
