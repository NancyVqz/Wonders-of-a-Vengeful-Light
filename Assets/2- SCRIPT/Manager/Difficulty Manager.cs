using System.Collections;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Enemy 1")]
    [SerializeField] private EnemySpawn enemySpawnScript1;
    [SerializeField] float newMinTimeBetweenSpawns1;
    [SerializeField] float newMaxTimeBetweenSpawns1;
    [SerializeField] float newMinTimeBetweenSpawns1_2;
    [SerializeField] float newMaxTimeBetweenSpawns1_2;

    [Header("Enemy 2")]
    [SerializeField] private EnemySpawn enemySpawnScript2;
    [SerializeField] float newMinTimeBetweenSpawns2;
    [SerializeField] float newMaxTimeBetweenSpawns2;
    [SerializeField] float newMinTimeBetweenSpawns2_2;
    [SerializeField] float newMaxTimeBetweenSpawns2_2;

    [Header("Enemy 3")]
    [SerializeField] private EnemySpawn enemySpawnScript3;
    [SerializeField] float newMinTimeBetweenSpawns3;
    [SerializeField] float newMaxTimeBetweenSpawns3;
    [SerializeField] float newMinTimeBetweenSpawns3_2;
    [SerializeField] float newMaxTimeBetweenSpawns3_2;

    [Header("Level Manager")]
    [SerializeField] int newEnemiesContinue;
    [SerializeField] int newEnemiesContinue2;
    private LevelManager levelManager;

    [Header("Boss")]
    [SerializeField] DamageBoss damageBossScript;
    [SerializeField] float newDamageBoss;
    [SerializeField] float newDamageBoss2;
    [SerializeField] float newDamageBoss3;

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        StartCoroutine(ChangeDifficulty());
    }

    private IEnumerator ChangeDifficulty()
    {
        yield return new WaitForSeconds(2);
        CheckDifficulty();
    }

    private void CheckDifficulty()
    {
        if (GameManager.instance.shootLvl == 2 || GameManager.instance.shieldLvl == 2)
        {
            ValueChange2();
        }
        else if (GameManager.instance.shootLvl == 3 || GameManager.instance.shieldLvl == 3)
        {
            ValueChange3();
        }
        else if (GameManager.instance.shootLvl == 4 || GameManager.instance.shieldLvl == 4)
        {
            ValueChange4();
        }
    }

    private void ValueChange2()
    {
        levelManager.enemiesToContinue = newEnemiesContinue;
        damageBossScript.damageBoss = newDamageBoss;


        enemySpawnScript1.minTimeBetweenSpawns = newMinTimeBetweenSpawns1;
        enemySpawnScript1.maxTimeBetweenSpawns = newMaxTimeBetweenSpawns1;

        if (enemySpawnScript2 == null)
        {
            return;
        }
        else
        {
            enemySpawnScript2.minTimeBetweenSpawns = newMinTimeBetweenSpawns2;
            enemySpawnScript2.maxTimeBetweenSpawns = newMaxTimeBetweenSpawns2;
        }

        if (enemySpawnScript3 == null)
        {
            return;
        }
        else
        {
            enemySpawnScript3.minTimeBetweenSpawns = newMinTimeBetweenSpawns3;
            enemySpawnScript3.maxTimeBetweenSpawns = newMaxTimeBetweenSpawns3;
        }
    }

    private void ValueChange3()
    {
        levelManager.enemiesToContinue = newEnemiesContinue2;
        damageBossScript.damageBoss = newDamageBoss2;


        enemySpawnScript1.minTimeBetweenSpawns = newMinTimeBetweenSpawns1_2;
        enemySpawnScript1.maxTimeBetweenSpawns = newMaxTimeBetweenSpawns1_2;

        if (enemySpawnScript2 == null)
        {
            return;
        }
        else
        {
            enemySpawnScript2.minTimeBetweenSpawns = newMinTimeBetweenSpawns2_2;
            enemySpawnScript2.maxTimeBetweenSpawns = newMaxTimeBetweenSpawns2_2;
        }

        if (enemySpawnScript3 == null)
        {
            return;
        }
        else
        {
            enemySpawnScript3.minTimeBetweenSpawns = newMinTimeBetweenSpawns3_2;
            enemySpawnScript3.maxTimeBetweenSpawns = newMaxTimeBetweenSpawns3_2;
        }
    }

    private void ValueChange4()
    {
        damageBossScript.damageBoss = newDamageBoss3;
    }
}
