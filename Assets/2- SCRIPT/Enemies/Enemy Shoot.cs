using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float velocidadBala = 500f;

    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 1f;

    private float shootTimer = 0f;
    private MonoBehaviour enemyMove;
    private bool canShoot = false;
    private Transform shootPoint;

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove1>();
        if (enemyMove == null)
        {
            enemyMove = GetComponent<EnemyMove2>();
        }

        if (transform.childCount > 0)
        {
            shootPoint = transform.GetChild(0);
        }
        else
        {
            Debug.LogWarning("No hay punto de disparo en " + gameObject.name);
            shootPoint = transform;
        }
    }

    private void OnEnable()
    {
        canShoot = false;
        shootTimer = 0f;
    }

    private void Update()
    {
        // Verificar si puede empezar a disparar
        if (!canShoot && enemyMove != null)
        {
            bool targetReached = false;

            if (enemyMove is EnemyMove1 move1)
            {
                targetReached = move1.HasReachedTarget();
            }
            else if (enemyMove is EnemyMove2 move2)
            {
                targetReached = move2.HasReachedTarget();
            }

            if (targetReached)
            {
                canShoot = true;
                shootTimer = fireRate;
            }
        }

        // Si no puede disparar, salir
        if (!canShoot) return;

        // Fire rate
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Disparar();
            shootTimer = fireRate;
        }
    }

    private void Disparar()
    {
        if (EnemyBulletPool.instance == null)
        {
            Debug.LogError("No hay un pool para las balas de los enemigos");
            return;
        }

        Vector2 force = Vector2.down * velocidadBala;
        GameObject bala = EnemyBulletPool.instance.GetBullet(shootPoint.position, Quaternion.identity, force);

        if (bala == null)
        {
            return;
        }
    }

    private void OnDisable()
    {
        canShoot = false;
    }
}
