using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> spawnpoints;
    [SerializeField] private GameObject prefabEnemigo;
    [SerializeField] private int maxEnemiesInstanceInQueue = 10; //Maximo de personajes DISPONIBLES en fila
    [SerializeField] private float minTimeBetweenSpawns = 2f; 
    [SerializeField] private float maxTimeBetweenSpawns = 5f;

    [SerializeField] Queue<GameObject> enemyQueue;
    [SerializeField] private int enemiesInScene = 0;  //cuantos hay en escena
    [SerializeField] private int maxEnemiesCountInScene = 4; //Maximo de personajes ACTIVOS en la escena

    private void Start()
    {
        StartPool();
    }

    private void StartPool()
    {
        enemyQueue = new Queue<GameObject>(); //se inicia la fila

        for (int i = 0; i < maxEnemiesInstanceInQueue; i++)
        {
            GameObject instance = Instantiate(prefabEnemigo); //los instancia
            instance.SetActive(false); //desactiva
            enemyQueue.Enqueue(instance); //se agrega a la fila
        }

        StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy()
    {
        yield return new WaitUntil(() => enemiesInScene < maxEnemiesCountInScene);

        for (int i = enemiesInScene; i < maxEnemiesCountInScene; i++)
        {
            if (LevelManager.instance.enemiesAppeared >= LevelManager.instance.enemiesToContinue)
            {
                LevelManager.instance.StopCoroutinesCheck(); 
                yield break; 
            }

            yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));

            if (LevelManager.instance.enemiesAppeared >= LevelManager.instance.enemiesToContinue)
            {
                LevelManager.instance.StopCoroutinesCheck();
                yield break;
            }

            GameObject enemy = enemyQueue.Dequeue();
            Vector3 randomSpawn = GetRandomSpawn();
            enemy.transform.position = randomSpawn;
            enemy.transform.rotation = Quaternion.identity;
            enemy.SetActive(true);

            enemiesInScene++;        
            LevelManager.instance.enemiesAppeared++;
            LevelManager.instance.allEnemiesInScene++;
        }
        if (LevelManager.instance.enemiesAppeared < LevelManager.instance.enemiesToContinue)
        {
            StartCoroutine(SpawnEnemy()); 
        }
        else
        {
            LevelManager.instance.StopCoroutinesCheck(); 
        }
    }

    private Vector3 GetRandomSpawn()
    {
        int randomIndex = Random.Range(0, spawnpoints.Count);
        GameObject randomSpawnpoint = spawnpoints[randomIndex];

        return randomSpawnpoint.transform.position;
    }

    public void OnEnemyKilled(GameObject killedEnemy)
    {
        EnergyDrop dropManager = FindAnyObjectByType<EnergyDrop>();
        dropManager.SpawnDrop(killedEnemy.transform.position);

        killedEnemy.SetActive(false);
        enemyQueue.Enqueue(killedEnemy);
        enemiesInScene--;
        LevelManager.instance.allEnemiesInScene--;
        if (LevelManager.instance.allEnemiesInScene <= 0)
        {
            LevelManager.instance.BossCheck();
        }
    }
}
