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
    [SerializeField] private bool avoidOverlappingSpawns;

    [SerializeField] Queue<GameObject> enemyQueue;
    [SerializeField] private int enemiesInScene = 0;  //cuantos hay en escena
    [SerializeField] private int maxEnemiesCountInScene = 4; //Maximo de personajes ACTIVOS en la escena

    private static Dictionary<GameObject, bool> occupiedSpawnpoints = new Dictionary<GameObject, bool>();

    private void Start()
    {
        // Inicializar los spawnpoints en el diccionario
        foreach (GameObject spawnpoint in spawnpoints)
        {
            if (!occupiedSpawnpoints.ContainsKey(spawnpoint))
            {
                occupiedSpawnpoints[spawnpoint] = false;
            }
        }

        StartPool();
    }

    private void StartPool()
    {
        enemyQueue = new Queue<GameObject>();

        for (int i = 0; i < maxEnemiesInstanceInQueue; i++)
        {
            GameObject instance = Instantiate(prefabEnemigo);
            instance.SetActive(false);
            enemyQueue.Enqueue(instance);
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
            GameObject selectedSpawnpoint = GetRandomSpawn();

            // Marcar el spawnpoint como ocupado si la opción está activada
            if (avoidOverlappingSpawns && selectedSpawnpoint != null)
            {
                occupiedSpawnpoints[selectedSpawnpoint] = true;
            }

            enemy.transform.position = selectedSpawnpoint.transform.position;
            enemy.transform.rotation = Quaternion.identity;

            // Guardar referencia del spawnpoint en el enemigo para liberarlo después
            EnemySpawnTracker tracker = enemy.GetComponent<EnemySpawnTracker>();
            if (tracker == null)
            {
                tracker = enemy.AddComponent<EnemySpawnTracker>();
            }
            tracker.assignedSpawnpoint = selectedSpawnpoint;
            tracker.parentSpawner = this;

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

    private GameObject GetRandomSpawn()
    {
        if (!avoidOverlappingSpawns)
        {
            return spawnpoints[Random.Range(0, spawnpoints.Count)];
        }

        // Buscar spawnpoints disponibles
        List<GameObject> availableSpawnpoints = new List<GameObject>();

        foreach (GameObject spawnpoint in spawnpoints)
        {
            if (!occupiedSpawnpoints[spawnpoint])
            {
                availableSpawnpoints.Add(spawnpoint);
            }
        }

        // Si no hay spawnpoints disponibles, usar cualquiera (fallback)
        if (availableSpawnpoints.Count == 0)
        {
            Debug.LogWarning("No hay spawnpoints disponibles. Usando uno ocupado.");
            return spawnpoints[Random.Range(0, spawnpoints.Count)];
        }

        // Retornar un spawnpoint disponible aleatorio
        int randomIndex = Random.Range(0, availableSpawnpoints.Count);
        return availableSpawnpoints[randomIndex];
    }

    public void OnEnemyKilled(GameObject killedEnemy)
    {
        // Liberar el spawnpoint
        EnemySpawnTracker tracker = killedEnemy.GetComponent<EnemySpawnTracker>();
        if (tracker != null && tracker.assignedSpawnpoint != null && avoidOverlappingSpawns)
        {
            occupiedSpawnpoints[tracker.assignedSpawnpoint] = false;
        }

        EnergyDrop dropManager = FindAnyObjectByType<EnergyDrop>();
        dropManager.SpawnDrop(killedEnemy.transform.position);

        killedEnemy.SetActive(false);
        enemyQueue.Enqueue(killedEnemy);
        enemiesInScene--;
        LevelManager.instance.allEnemiesInScene--;

        if (LevelManager.instance.allEnemiesInScene <= 0 && LevelManager.instance.enemiesAppeared == LevelManager.instance.enemiesToContinue)
        {
            LevelManager.instance.BossCheck();
        }
    }

    private void OnDestroy()
    {
        // Limpiar los spawnpoints ocupados cuando se destruye el spawner
        if (avoidOverlappingSpawns)
        {
            foreach (GameObject spawnpoint in spawnpoints)
            {
                if (occupiedSpawnpoints.ContainsKey(spawnpoint))
                {
                    occupiedSpawnpoints[spawnpoint] = false;
                }
            }
        }
    }
}

// Clase auxiliar para trackear el spawnpoint asignado a cada enemigo
public class EnemySpawnTracker : MonoBehaviour
{
    public GameObject assignedSpawnpoint;
    public EnemySpawn parentSpawner;
}
