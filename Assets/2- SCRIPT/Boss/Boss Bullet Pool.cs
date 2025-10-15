using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BossBulletPool : MonoBehaviour
{
    public static BossBulletPool instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 20;
    [SerializeField] private int maxPoolSize = 50;
    [SerializeField] private float bulletLifetime = 5f;

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

        poolContainer = new GameObject("MissilePoolContainer").transform;
        poolContainer.SetParent(transform);

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

        if (bulletPool.Count == 0)
        {
            if (activeBullets.Count >= maxPoolSize)
            {
                Debug.LogWarning("Pool de misiles al máximo.");
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
