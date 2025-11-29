using System.Collections;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int kamikazeDamage;
    [SerializeField] private GameObject deadVfx;

    [Header("Score points")]
    [SerializeField] private int normalPoints;
    [SerializeField] private int comboPoints;

    private Animator anim;
    private int health;

    private EnemySpawn enemySpawnScript;

    private void Start()
    {
        enemySpawnScript = FindAnyObjectByType<EnemySpawn>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health = 100;
    }

    public void TakeDamage()
    {
        health -= damage;
        anim.SetBool("damage", true);

        if (health <= 0)
        {
            AudioManager.instance.Play("enemy death");
            StartCoroutine(SoundTime());
        }
    }

    public void KamikazeDamage()
    {
        health -= kamikazeDamage;
        anim.SetBool("damage", true);

        if (health <= 0)
        {
            AudioManager.instance.Play("enemy death");
            StartCoroutine(SoundTime());
        }
    }

    public void EndDamage()
    {
        anim.SetBool("damage", false);
    }

    private IEnumerator SoundTime()
    {
        yield return new WaitForSeconds(0f);

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
