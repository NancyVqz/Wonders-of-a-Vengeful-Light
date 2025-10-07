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
                // Llamar el método que aplica daño
                enemyScript.TakeDamage(); // <- este es el método que tú creaste
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
