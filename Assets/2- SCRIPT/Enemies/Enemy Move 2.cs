using UnityEngine;

public class EnemyMove2 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float downSpeed = 1.5f;
    [SerializeField] private float horizontalSpeed = 3f;
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float despawnDistance = -10f;

    private bool hasReachedTarget = true;
    private bool movingRight = true;
    private EnemySpawn enemySpawn;

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
        //direccion depende el spawn
        hasReachedTarget = true;
        if (transform.position.x > 0)
            movingRight = false;
        else
            movingRight = true;
    }

    private void Update()
    {
        transform.Translate(Vector3.down * downSpeed * Time.deltaTime);

        if (transform.position.y <= despawnDistance)
        {
            enemySpawn.OnEnemyKilled(gameObject);
            return;
        }

        //movimiento zigzag
        if (movingRight)
        {
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);

            if (transform.position.x >= maxX)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime);

            if (transform.position.x <= minX)
            {
                movingRight = true;
            }
        }
    }

    public bool HasReachedTarget()
    {
        return hasReachedTarget;
    }
}
