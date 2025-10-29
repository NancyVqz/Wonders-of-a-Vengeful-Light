using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject deadVfx;

    [Header("Score points")]
    [SerializeField] private int normalPoints;
    [SerializeField] private int comboPoints;

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

            LevelManager.instance.allEnemiesKilled++;
            //particula de score
            if (ScoreCount.instance.timeElapsed > ScoreCount.instance.comboTime)
            {
                GameManager.instance.score += normalPoints;
            }
            else
            {
                GameManager.instance.score += comboPoints;
            }
            LevelManager.instance.AparecerTiendaCheck();
            ScoreCount.instance.ResetTimer();
            Instantiate(deadVfx, transform.position, Quaternion.identity);
            enemySpawnScript.OnEnemyKilled(this.gameObject);
        }
    }
}
