using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        Reset();
    }

    public void SetVisible(bool visible)
    {
        if (sr != null)
        {
            sr.enabled = visible;
        }
    }

    public void Activate(bool isReal)
    {
        SetVisible(true);

        if (col != null)
        {
            col.enabled = isReal;
        }
    }

    public void Deactivate()
    {
        SetVisible(false);

        if (col != null)
        {
            col.enabled = false;
        }
    }

    public void Reset()
    {
        SetVisible(true);

        if (col != null)
        {
            col.enabled = false;
        }
    }

    public void SetAnimator(RuntimeAnimatorController animator)
    {
        GetComponent<Animator>().runtimeAnimatorController = animator;
    }
}
