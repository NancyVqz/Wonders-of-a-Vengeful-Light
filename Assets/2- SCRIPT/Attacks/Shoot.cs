using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float velocidadBala = 970f;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private float bulletLifetime = 2f;

    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 1f; // Tiempo entre disparos
    [SerializeField] private float burstDelay = 0.3f; // Tiempo entre balas
    private bool isShooting = false;

    [Header("Pooling Settings")]
    [SerializeField] private int maxBulletsInPool = 20; // Máximo de balas en el pool

    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private Transform puntoDisparo2;
    [SerializeField] private Transform puntoDisparo3;

    private float machineCont = 0;
    private Queue<GameObject> bulletQueue;
    private List<GameObject> activeBullets; // Para trackear las balas activas

    private void Start()
    {
        InitializeBulletPool();
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
        if (Keyboard.current.spaceKey.isPressed && machineCont <= 0 && !isShooting)
        {
            //AudioManager.instance.Play("Shoot");

            StartCoroutine(ShootLvl());
            machineCont = fireRate;
        }
        machineCont -= Time.deltaTime;
    }

    public void ShootButton()
    {
        if (machineCont <= 0 && !isShooting)
        {
            //AudioManager.instance.Play("Shoot");

            StartCoroutine(ShootLvl());
            machineCont = fireRate;
        }
        machineCont -= Time.deltaTime;
    }

    private void Disparar1()
    {
        // Si no hay balas disponibles en el pool, no disparar
        if (bulletQueue.Count == 0)
        {
            Debug.LogWarning("No hay balas disponibles en el pool!");
            return;
        }

        // Obtener una bala del pool
        GameObject bala = bulletQueue.Dequeue();
        bala.SetActive(true);

        // Configurar posición y rotación
        bala.transform.position = puntoDisparo.position;
        bala.transform.rotation = balaPrefab.transform.rotation;

        // Aplicar fuerza
        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector3.zero; // Resetear velocidad anterior
        rb.AddForce(transform.up * velocidadBala);

        // Agregar a la lista de balas activas
        activeBullets.Add(bala);

        // Iniciar corrutina para devolver la bala al pool después del tiempo de vida
        StartCoroutine(ReturnBulletToPool(bala));
    }

    private void Disparar2()
    {
        // Si no hay balas disponibles en el pool, no disparar
        if (bulletQueue.Count == 0)
        {
            Debug.LogWarning("No hay balas disponibles en el pool!");
            return;
        }

        // Obtener una bala del pool
        GameObject bala = bulletQueue.Dequeue();
        bala.SetActive(true);

        // Configurar posición y rotación
        bala.transform.position = puntoDisparo2.position;
        bala.transform.rotation = balaPrefab.transform.rotation;

        // Aplicar fuerza
        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector3.zero; // Resetear velocidad anterior
        rb.AddForce(transform.up * velocidadBala);

        // Agregar a la lista de balas activas
        activeBullets.Add(bala);

        // Iniciar corrutina para devolver la bala al pool después del tiempo de vida
        StartCoroutine(ReturnBulletToPool(bala));


        // Obtener una bala del pool
        GameObject bala2 = bulletQueue.Dequeue();
        bala2.SetActive(true);

        // Configurar posición y rotación
        bala2.transform.position = puntoDisparo3.position;
        bala2.transform.rotation = balaPrefab.transform.rotation;

        // Aplicar fuerza
        Rigidbody2D rb2 = bala2.GetComponent<Rigidbody2D>();
        rb2.linearVelocity = Vector3.zero; // Resetear velocidad anterior
        rb2.AddForce(transform.up * velocidadBala);

        // Agregar a la lista de balas activas
        activeBullets.Add(bala2);

        // Iniciar corrutina para devolver la bala al pool después del tiempo de vida
        StartCoroutine(ReturnBulletToPool(bala2));
    }

    private IEnumerator ShootLvl()
    {
        isShooting = true;
        switch (GameManager.instance.shootLvl)
        {
            case 1:
                Disparar1();
                break;
            case 2:
                Disparar1();
                yield return new WaitForSeconds(burstDelay);
                Disparar1();
                break;
            case 3:
                Disparar1();
                yield return new WaitForSeconds(burstDelay);
                Disparar1();
                yield return new WaitForSeconds(burstDelay);
                Disparar1();
                break;
            case 4:
                Disparar2();
                yield return new WaitForSeconds(burstDelay);
                Disparar2();
                break;
            default:
                break;
        }

        isShooting = false;
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

    // Método para limpiar todas las balas activas (para reiniciar nivel)
    public void ClearAllBullets()
    {
        for (int i = activeBullets.Count - 1; i >= 0; i--)
        {
            if (activeBullets[i] != null)
            {
                ReturnBullet(activeBullets[i]);
            }
        }
    }

    private void OnDisable()
    {
        // Limpiar todas las balas cuando se desactiva el objeto
        ClearAllBullets();
    }
}
