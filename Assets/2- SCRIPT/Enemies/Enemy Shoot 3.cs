using UnityEngine;

public class EnemyShoot3 : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 2f;

    private float shootTimer = 0f;
    private MonoBehaviour enemyMove;
    private bool canShoot = false;
    private Transform shootPoint;
    private Transform player;

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove3>();

        if (transform.childCount > 0)
        {
            shootPoint = transform.GetChild(0);
        }
        else
        {
            Debug.LogWarning("No hay punto de disparo en " + gameObject.name);
            shootPoint = transform;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("No se encontró al jugador. Asegúrate de que tenga el tag 'Player'");
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
            if (enemyMove is EnemyMove3 move3)
            {
                targetReached = move3.HasReachedTarget();
            }

            if (targetReached)
            {
                canShoot = true;
                shootTimer = fireRate;
            }
        }

        if (!canShoot) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Disparar();
            shootTimer = fireRate;
        }
    }

    private void Disparar()
    {
        if (player == null)
        {
            Debug.LogError("No hay jugador en la escena");
            return;
        }

        if (EnemyBulletPool3.instance == null)
        {
            Debug.LogError("No hay un pool para los misiles");
            return;
        }

        //obtener la posición del jugador en ese momento
        Vector3 targetPosition = player.position;

        //dirección hacia el jugador
        Vector2 direction = (targetPosition - shootPoint.position).normalized;

        //rotación
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

        GameObject missile = EnemyBulletPool3.instance.GetBullet(shootPoint.position, rotation, Vector2.zero);

        if (missile == null)
        {
            return;
        }

        // Configurar el misil con la dirección
        Missile missileBehavior = missile.GetComponent<Missile>();
        if (missileBehavior != null)
        {
            missileBehavior.SetDirection(direction);
        }
    }

    private void OnDisable()
    {
        canShoot = false;
    }
}
