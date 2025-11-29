using UnityEngine;

public class KamikazeCollider : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();

        if (other.CompareTag("Player"))
        {
            playerMovement.DanioProta();
        }

        if (other.CompareTag("Enemy"))
        {
            DamageEnemy enemyScript = other.GetComponent<DamageEnemy>();
            if (enemyScript != null)
            {
                enemyScript.KamikazeDamage();
            }
        }
    }
}
