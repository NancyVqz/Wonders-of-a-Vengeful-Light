using UnityEngine;

public class EnemyMove3 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;

    private bool hasReachedTarget = false;

    private EnemySpawn enemySpawn;
    [SerializeField] private float despawnDistance = -10f;

    private void Awake()
    {
        enemySpawn = FindAnyObjectByType<EnemySpawn>();
        if (enemySpawn == null)
        {
            Debug.LogError("No se encontró EnemySpawn en la escena");
        }
    }

    private void OnEnable()
    {
        hasReachedTarget = false;
    }

    private void Update()
    {
        if (!hasReachedTarget)
        {
            hasReachedTarget = true;
        }

        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= despawnDistance)
        {
            enemySpawn.OnEnemyKilled(gameObject);
            return;
        }


    }

    public bool HasReachedTarget()
    {
        return hasReachedTarget;
    }
}
