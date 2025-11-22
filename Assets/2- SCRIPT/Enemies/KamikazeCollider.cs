using UnityEngine;

public class KamikazeCollider : MonoBehaviour
{
    private DamagePlayerVfx damageEffectScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        damageEffectScript = FindAnyObjectByType<DamagePlayerVfx>();

        if (other.CompareTag("Player"))
        {
            AudioManager.instance.Play("prota damage");
            damageEffectScript.TriggerDamageFlash();
            
            CameraShake.instance.Shake();
            GameManager.instance.playerHealth -= 1;
            UIPlayerHealth uiScript = FindAnyObjectByType<UIPlayerHealth>();
            uiScript.UpdateHealthDisplay(GameManager.instance.playerHealth);
        }
    }
}
