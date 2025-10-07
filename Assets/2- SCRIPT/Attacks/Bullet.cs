using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Shoot shootScript;

    private void Start()
    {
        shootScript = FindAnyObjectByType<Shoot>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //devuelve al enemigo
            DamageEnemy enemyScript = other.GetComponent<DamageEnemy>();
            if (enemyScript != null)
            {
                // Llamar el m�todo que aplica da�o
                enemyScript.TakeDamage(); // <- este es el m�todo que t� creaste
            }

            // Devolver la bala al pool
            if (shootScript != null)
            {
                //danio a enemigo
                shootScript.OnBulletHit(this.gameObject);
            }
        }
    }
}
