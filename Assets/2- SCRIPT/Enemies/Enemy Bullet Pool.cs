using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 50; 
    [SerializeField] private int maxPoolSize = 100; 
    [SerializeField] private float bulletLifetime = 2f;

    private Queue<GameObject> bulletPool;
    private List<GameObject> activeBullets;
    private Transform poolContainer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        bulletPool = new Queue<GameObject>();
        activeBullets = new List<GameObject>();

        // Crear contenedor para organizar jerarquía
        poolContainer = new GameObject("BulletPoolContainer").transform;
        poolContainer.SetParent(transform);

        // Pre-instanciar balas
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewBullet();
        }
    }

    private GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, poolContainer);
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
        return bullet;
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation, Vector2 force)
    {
        GameObject bullet;

        // Si no hay balas disponibles, crear una nueva (hasta el límite)
        if (bulletPool.Count == 0)
        {
            if (activeBullets.Count >= maxPoolSize)
            {
                Debug.LogWarning("Pool al máximo. No se pueden crear más balas.");
                return null;
            }
            bullet = CreateNewBullet();
        }
        else
        {
            bullet = bulletPool.Dequeue();
        }

        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.AddForce(force);
        }

        activeBullets.Add(bullet);

        StartCoroutine(ReturnBulletAfterTime(bullet, bulletLifetime));

        return bullet;
    }

    private IEnumerator ReturnBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);

        if (bullet != null && bullet.activeInHierarchy)
        {
            ReturnBullet(bullet);
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null || !bullet.activeInHierarchy)
            return;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
        activeBullets.Remove(bullet);
    }
}
