using UnityEngine;
using UnityEngine.UI;

public class TiendaManager : MonoBehaviour
{
    [Header("Referencia a los datos de habilidad")]
    public HabilidadData habilidad;

    [Header("Elementos de UI")]
    [SerializeField] private Animator mensaje;
    [SerializeField] private Image precio;
    [SerializeField] private Image icono;
    [SerializeField] private Image descripcion;

    [SerializeField] private Animator animPrecio;
    [SerializeField] private Animator animIcono;
    [SerializeField] private Animator animDescripcion;

    void Start()
    {
        mensaje.updateMode = AnimatorUpdateMode.UnscaledTime;
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
            mensaje.SetTrigger("mensaje");
            Debug.Log("No tienes suficiente energía para comprar esta habilidad.");
            return;
        }

        AudioManager.instance.Play("comprar");
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
        int nivelActual = ObtenerNivelDeHabilidad(nombre) - 1;

        switch (nombre)
        {
            case "Shoot":
                int[] preciosShoot = { 10, 20, 40 };
                return (nivelActual >= 0 && nivelActual < preciosShoot.Length) ? preciosShoot[nivelActual] : 0;
            case "Shield":
                int[] preciosShield = { 11, 22, 44 };
                return (nivelActual >= 0 && nivelActual < preciosShield.Length) ? preciosShield[nivelActual] : 0;
            case "Dash":
                int[] preciosDash = { 9, 18, 36 };
                return (nivelActual >= 0 && nivelActual < preciosDash.Length) ? preciosDash[nivelActual] : 0;
            default:
                Debug.LogWarning("Esa habilidad no existe");
                return 0;
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
