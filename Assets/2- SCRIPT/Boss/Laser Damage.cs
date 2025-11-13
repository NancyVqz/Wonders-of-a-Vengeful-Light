using UnityEngine;
using System.Collections;

public class LaserDamage : MonoBehaviour
{
    public int damage = 1;
    private DamagePlayerVfx damageEffectScript;


    void OnTriggerEnter2D(Collider2D other)
    {
        damageEffectScript = FindAnyObjectByType<DamagePlayerVfx>();

        if (other.CompareTag("Player"))
        {
            AudioManager.instance.Play("prota damage");
            damageEffectScript.TriggerDamageFlash();
            StartCoroutine(SoundTime());

        }
    }

    private IEnumerator SoundTime()
    {
        yield return new WaitForSeconds(0f);

        Debug.Log("Se daño al jugador");
        CameraShake.instance.Shake();
        GameManager.instance.playerHealth -= 1;
        UIPlayerHealth uiScript = FindAnyObjectByType<UIPlayerHealth>();
        uiScript.UpdateHealthDisplay(GameManager.instance.playerHealth);
    }
}
