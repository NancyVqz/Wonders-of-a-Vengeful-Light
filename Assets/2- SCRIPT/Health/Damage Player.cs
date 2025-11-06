using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.Play("prota damage");
            StartCoroutine(SoundTime());

        }
    }

    private IEnumerator SoundTime()
    {
        yield return new WaitForSeconds(0f);

        CameraShake.instance.Shake();
        GameManager.instance.playerHealth -= 1;

        UIPlayerHealth uiScript = FindAnyObjectByType<UIPlayerHealth>();
        uiScript.UpdateHealthDisplay(GameManager.instance.playerHealth);

        EnemyBulletPool.instance.ReturnBullet(this.gameObject);
    }
}
