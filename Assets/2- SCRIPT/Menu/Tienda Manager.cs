using UnityEngine;
using UnityEngine.UI;

public class TiendaManager : MonoBehaviour
{
    [Header("Referencia a los datos de habilidad")]
    public HabilidadData habilidad;

    [Header("Elementos de UI")]
    [SerializeField] private Image precio;
    [SerializeField] private Image icono;
    [SerializeField] private Image descripcion;

    [SerializeField] private Animator animPrecio;
    [SerializeField] private Animator animIcono;
    [SerializeField] private Animator animDescripcion;

    void Start()
    {
        ActualizarTienda();
    }

    public void ActualizarTienda()
    {
        int nivel = ObtenerNivelDeHabilidad(habilidad.nombreHabilidad);
        int index = Mathf.Clamp(nivel - 1, 0, habilidad.spritesIcono.Length - 1);

        //sprites
        icono.sprite = habilidad.spritesIcono[index];
        precio.sprite = habilidad.spritesPrecio[index];
        descripcion.sprite = habilidad.spritesDescripcion[index];

        if (nivel >= 4)
        {
            Debug.Log("Nivel maximo: " + habilidad.name);
            precio.raycastTarget = false;
        }

        if (nivel > 4)
        {
            return;
        }
        else
        {
            //animators
            if (habilidad.animatorIcono.Length > index && habilidad.animatorIcono[index] != null)
                animIcono.runtimeAnimatorController = habilidad.animatorIcono[index];

            if (habilidad.animatorPrecio.Length > index && habilidad.animatorPrecio[index] != null)
                animPrecio.runtimeAnimatorController = habilidad.animatorPrecio[index];

            if (habilidad.animatorDescripcion.Length > index && habilidad.animatorDescripcion[index] != null)
                animDescripcion.runtimeAnimatorController = habilidad.animatorDescripcion[index];
        }

    }

    public void Buy()
    {
        int precio = ObtenerPrecio(habilidad.nombreHabilidad);

        if (GameManager.instance.energy < precio)
        {
            Debug.Log("No tienes suficiente energía para comprar esta habilidad.");
            return;
        }

        SubirNivel(habilidad.nombreHabilidad);
        GameManager.instance.energy -= precio;

        Debug.Log("Se gastó: " + precio);

        ActualizarTienda();
    }

    private void SubirNivel(string nombre)
    {
        switch (nombre)
        {
            case "Shoot":
                GameManager.instance.shootLvl++;
                return ;
            case "Shield":
                GameManager.instance.shieldLvl++;
                return;
            case "Dash":
                GameManager.instance.dashLvl++;
                return;
            default:
                Debug.LogWarning("Esa habilidad no existe");
                return;
        }
    }

    private int ObtenerPrecio(string nombre)
    {
        switch (nombre)
        {
            case "Shoot":
                int precio1 = GameManager.instance.shootLvl + 1;
                return precio1;
            case "Shield":
                int precio2 = GameManager.instance.shieldLvl + 2;
                return precio2;
            case "Dash":
                int precio3 = GameManager.instance.dashLvl + 3;
                return precio3;
            default:
                Debug.LogWarning("Esa habilidad no existe");
                return 1;
        }
    }

    private int ObtenerNivelDeHabilidad(string nombre)
    {
        switch (nombre)
        {
            case "Shoot":
                return GameManager.instance.shootLvl;
            case "Shield":
                return GameManager.instance.shieldLvl;
            case "Dash":
                return GameManager.instance.dashLvl;
            default:
                Debug.LogWarning("Esa habilidad no existe");
                return 1;
        }
    }
}
