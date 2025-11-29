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
            DamageEnemy enemyScript = other.GetComponent<DamageEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(); 
            }

            if (shootScript != null)
            {
                shootScript.OnBulletHit(this.gameObject);
            }
        }
    }
}
