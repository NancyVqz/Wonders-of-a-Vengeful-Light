using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] private int damage;
    private int health;

    private EnemySpawn enemySpawnScript;

    private void Start()
    {
        enemySpawnScript = FindAnyObjectByType<EnemySpawn>();
    }

    private void OnEnable()
    {
        health = 100;
    }

    public void TakeDamage()
    {
        health -= damage;

        if (health <= 0)
        {
            enemySpawnScript.OnEnemyKilled(this.gameObject);
        }
    }
}
