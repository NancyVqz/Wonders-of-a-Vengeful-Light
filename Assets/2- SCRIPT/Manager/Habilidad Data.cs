using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Habilidad", menuName = "Tienda/Habilidad")]
public class HabilidadData : ScriptableObject
{
    [Header("Nombre")]
    public string nombreHabilidad;

    [Header("Elementos icono por Nivel")]
    public Sprite[] spritesIcono;
    public RuntimeAnimatorController[] animatorIcono;

    [Header("Elementos precios por Nivel")]
    public Sprite[] spritesPrecio;
    public RuntimeAnimatorController[] animatorPrecio;

    [Header("Elementos descripcion por Nivel")]
    public Sprite[] spritesDescripcion;
    public RuntimeAnimatorController[] animatorDescripcion;
}
