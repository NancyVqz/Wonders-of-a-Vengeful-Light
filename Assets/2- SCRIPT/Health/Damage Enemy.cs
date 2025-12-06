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
    private bool isDead = false;
    private void Start()
    {
        enemySpawnScript = FindAnyObjectByType<EnemySpawn>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health = 100;
        isDead = false;

        if (anim != null)
        {
            anim.SetBool("damage", false);
            anim.Rebind(); // Resetea completamente el Animator
            anim.Update(0f); // Fuerza una actualización
        }
    }

    public void TakeDamage()
    {
        if (isDead) return; 

        health -= damage;
        anim.SetBool("damage", true);

        if (health <= 0)
        {
            isDead = true;
            Die();
        }
        else
        {
            StartCoroutine(ResetDamageAnimation());
        }
    }

    public void KamikazeDamage()
    {
        if (isDead) return;

        health -= kamikazeDamage;
        anim.SetBool("damage", true);

        if (health <= 0)
        {
            isDead = true;
            Die();
        }
        else
        {
           
            StartCoroutine(ResetDamageAnimation());
        }
    }
    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(0.3f); 

        if (!isDead && anim != null)
        {
            anim.SetBool("damage", false);
        }
    }

    public void EndDamage()
    {
        if (!isDead && anim != null)
        {
            anim.SetBool("damage", false);
        }
    }
    private void Die()
    {
        AudioManager.instance.Play("enemy death");

        if (anim != null)
        {
            anim.SetBool("damage", false);
        }

        StartCoroutine(SoundTime());
    }

    private IEnumerator SoundTime()
    {
        yield return new WaitForSeconds(0f);

        LevelManager.instance.allEnemiesKilled++;
  
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
