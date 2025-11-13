using UnityEngine;
using System.Collections;

public class DamagePlayerVfx : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TriggerDamageFlash()
    {
        StartCoroutine(FlashRojo());
    }

    private IEnumerator FlashRojo()
    {
        float duracion = 0.2f;
        float tiempo = 0;

        // Ir a rojo
        while (tiempo < duracion)
        {
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        tiempo = 0;

        // Volver a blanco
        while (tiempo < duracion)
        {
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = Color.white;
    }
}
