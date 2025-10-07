using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float velocidadBala = 500;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private float bulletLifetime = 2f;

    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 1f; // Tiempo entre disparos

    [Header("Pooling Settings")]
    [SerializeField] private int maxBulletsInPool = 20; // Máximo de balas en el pool

    private Queue<GameObject> bulletQueue;
    private List<GameObject> activeBullets; // Para trackear las balas activas

    private float shootTimer = 0;
    private EnemyMove1 enemyMove;
    private bool canShoot = false;
    private bool poolInitialized = false;

    private void Awake()
    {
        if (!poolInitialized)
        {
            InitializeBulletPool();
            poolInitialized = true;
        }
        enemyMove = GetComponent<EnemyMove1>();
    }

    private void OnEnable()
    {
        // Resetear valores cada vez que el enemigo se activa
        canShoot = false;
        shootTimer = 0;
    }

    private void InitializeBulletPool()
    {
        bulletQueue = new Queue<GameObject>();
        activeBullets = new List<GameObject>();

        // Crear el pool inicial de balas
        for (int i = 0; i < maxBulletsInPool; i++)
        {
            GameObject bullet = Instantiate(balaPrefab);
            bullet.SetActive(false);
            bulletQueue.Enqueue(bullet);
        }
    }

    void Update()
    {
        if (enemyMove != null && !canShoot)
        {
            if (enemyMove.HasReachedTarget())
            {
                canShoot = true;
                shootTimer = fireRate;
            }
        }

        if (!canShoot) return;

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            Disparar();
            shootTimer = fireRate;
        }
    }

    private void Disparar()
    {
        // Si no hay balas disponibles en el pool, no disparar
        if (bulletQueue.Count == 0)
        {
            Debug.LogWarning("No hay balas disponibles en el pool!");
            return;
        }

        // Obtener una bala del pool
        GameObject bala = bulletQueue.Dequeue();
        //posicionarla
        Transform puntoDisparo = transform.GetChild(0);
        bala.transform.position = puntoDisparo.position;
        bala.transform.rotation = balaPrefab.transform.rotation;

        bala.SetActive(true);

        // Aplicar fuerza
        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector3.zero; // Resetear velocidad anterior
        rb.AddForce(Vector2.down * velocidadBala);

        // Agregar a la lista de balas activas
        activeBullets.Add(bala);

        // Iniciar corrutina para devolver la bala al pool después del tiempo de vida
        StartCoroutine(ReturnBulletToPool(bala));
    }

    private IEnumerator ReturnBulletToPool(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifetime);

        if (bullet.activeInHierarchy)
        {
            ReturnBullet(bullet);
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        if (bullet != null && bullet.activeInHierarchy)
        {
            // Resetear la física
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = 0f;
            }

            // Desactivar y devolver al pool
            bullet.SetActive(false);
            bulletQueue.Enqueue(bullet);

            // Remover de la lista de balas activas
            activeBullets.Remove(bullet);
        }
    }
    public void OnBulletHit(GameObject bullet)
    {
        ReturnBullet(bullet);
    }
}
