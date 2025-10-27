using UnityEngine;
using UnityEngine.UI;

public class Tienda : MonoBehaviour
{
    private Button shootButton;
    private Animator shootBuyAnim;
    private Animator shootIconAnim;
    private Animator shootDescripcionAnim;

    [SerializeField] private GameObject shield;
    private Button shieldButton;
    private Animator shieldBuyAnim;
    private Animator shieldIconAnim;
    private Animator shieldDescripcionAnim;

    [SerializeField] private GameObject dash;
    private Button dashButton;
    private Animator dashBuyAnim;
    private Animator dashIconAnim;
    private Animator dashDescripcionAnim;

    private void Start()
    {
        ActualizarAnimators();
    }

    private void OnEnable()
    {
        ActualizarAnimators();
    }

    public void ActualizarAnimators()
    {
        shootBuyAnim = GetComponent<Animator>();
        shootIconAnim = transform.GetChild(0).GetComponent<Animator>();
        shootDescripcionAnim = transform.GetChild(1).GetComponent<Animator>();
        shootButton = GetComponent<Button>();

        shieldBuyAnim = shield.GetComponent<Animator>();
        shieldIconAnim = shield.transform.GetChild(0).GetComponent<Animator>();
        shieldDescripcionAnim = shield.transform.GetChild(1).GetComponent<Animator>();
        shieldButton = shield.GetComponent<Button>();

        dashBuyAnim = dash.GetComponent<Animator>();
        dashIconAnim = dash.transform.GetChild(0).GetComponent<Animator>();
        dashDescripcionAnim = dash.transform.GetChild(1).GetComponent<Animator>();
        dashButton = dash.GetComponent<Button>();

        SetAnimatorUnscaled(shootBuyAnim);
        SetAnimatorUnscaled(shootIconAnim);
        SetAnimatorUnscaled(shootDescripcionAnim);

        SetAnimatorUnscaled(shieldBuyAnim);
        SetAnimatorUnscaled(shieldIconAnim);
        SetAnimatorUnscaled(shieldDescripcionAnim);

        SetAnimatorUnscaled(dashBuyAnim);
        SetAnimatorUnscaled(dashIconAnim);
        SetAnimatorUnscaled(dashDescripcionAnim);

        ShootIconButton();
    }
    private void SetAnimatorUnscaled(Animator anim)
    {
        if (anim != null)
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    public void ShootIconButton()
    {
        shootBuyAnim.SetBool("select", true);
        shootBuyAnim.SetBool("deselect", false);
        shootIconAnim.SetBool("select", true);
        shootIconAnim.SetBool("deselect", false);
        shootDescripcionAnim.SetBool("select", true);
        shootDescripcionAnim.SetBool("deselect", false);
        shootButton.interactable = true;
    }

    public void ShootResetButton()
    {
        shootBuyAnim.SetBool("select", false);
        shootBuyAnim.SetBool("deselect", true);
        shootIconAnim.SetBool("select", false);
        shootIconAnim.SetBool("deselect", true);
        shootDescripcionAnim.SetBool("select", false);
        shootDescripcionAnim.SetBool("deselect", true);
        shootButton.interactable = false;
    }

    public void ShieldIconButton()
    {
        shieldBuyAnim.SetBool("select", true);
        shieldBuyAnim.SetBool("deselect", false);
        shieldIconAnim.SetBool("select", true);
        shieldIconAnim.SetBool("deselect", false);
        shieldDescripcionAnim.SetBool("select", true);
        shieldDescripcionAnim.SetBool("deselect", false);
        shieldButton.interactable = true;
    }
    public void ShieldResetButton()
    {
        shieldBuyAnim.SetBool("select", false);
        shieldBuyAnim.SetBool("deselect", true);
        shieldIconAnim.SetBool("select", false);
        shieldIconAnim.SetBool("deselect", true);
        shieldDescripcionAnim.SetBool("select", false);
        shieldDescripcionAnim.SetBool("deselect", true);
        shieldButton.interactable = false;
    }

    public void DashIconButton()
    {
        dashBuyAnim.SetBool("select", true);
        dashBuyAnim.SetBool("deselect", false);
        dashIconAnim.SetBool("select", true);
        dashIconAnim.SetBool("deselect", false);
        dashDescripcionAnim.SetBool("select", true);
        dashDescripcionAnim.SetBool("deselect", false);
        dashButton.interactable = true;
    }
    public void DashResetButton()
    {
        dashBuyAnim.SetBool("select", false);
        dashBuyAnim.SetBool("deselect", true);
        dashIconAnim.SetBool("select", false);
        dashIconAnim.SetBool("deselect", true);
        dashDescripcionAnim.SetBool("select", false);
        dashDescripcionAnim.SetBool("deselect", true);
        dashButton.interactable = false;
    }

    public void ShootBuyButton()
    {
        //GameManager.instance.shootLvl += 1;
        Debug.Log("SHOOT Comprado");
    }
}
