using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private GameObject laserPrefab;

    [Header("Rayos")]
    [SerializeField] private int totalLasers = 8;
    [SerializeField] private int realLasers = 2;

    [Header("Tiempos")]
    [SerializeField] private float warningTime = 1.5f;
    [SerializeField] private float blinkInterval = 0.15f;
    [SerializeField] private float activeLaserTime = 1f;
    [SerializeField] private float laserReload = 5f;

    private Queue<GameObject> laserPool = new Queue<GameObject>();
    private List<GameObject> activeLasers = new List<GameObject>();

    private bool isAttacking = false;

    void Start()
    {
        InitializePool();

    }

    // Crear el pool al inicio
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateLaserForPool();
        }
    }

    private void CreateLaserForPool()
    {
        // Instanciar el prefab padre
        GameObject laserObj = Instantiate(laserPrefab, transform);
        laserObj.SetActive(false);

        // Verificar que el hijo exista
        if (laserObj.transform.childCount == 0)
        {
            Debug.LogError("El prefab del l�ser no tiene hijos. Aseg�rate de que el Beam est� dentro del prefab.");
            Destroy(laserObj);
            return;
        }

        // Verificar que el hijo tenga el componente LaserBeam
        LaserBeam beam = laserObj.transform.GetChild(0).GetComponent<LaserBeam>();
        if (beam == null)
        {
            Debug.LogError("El hijo del prefab no tiene el componente LaserBeam");
            Destroy(laserObj);
            return;
        }

        laserPool.Enqueue(laserObj);
    }

    private GameObject GetLaserFromPool()
    {
        if (laserPool.Count == 0)
        {
            CreateLaserForPool();
        }

        GameObject laserObj = laserPool.Dequeue();
        laserObj.SetActive(true);
        return laserObj;
    }

    private void ReturnToPool(GameObject laserObj)
    {
        laserObj.SetActive(false);

        // Resetear el hijo que tiene el script LaserBeam
        LaserBeam beam = laserObj.transform.GetChild(0).GetComponent<LaserBeam>();
        if (beam != null)
        {
            beam.Reset();
        }

        laserPool.Enqueue(laserObj);
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(laserReload);
            ExecuteLaserAttack();
        }
    }
    public void LaserBeamAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    public void ExecuteLaserAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(LaserAttackSequence());
        }
    }

    private IEnumerator LaserAttackSequence()
    {
        isAttacking = true;

        // Tomar rayos del pool y posicionarlos
        SetupLasers();

        // Parpadear todos
        yield return StartCoroutine(BlinkPhase());

        // Activar solo algunos aleatorios como reales
        ActivateRandomRealLasers();

        // Mantener activos
        yield return new WaitForSeconds(activeLaserTime);

        // Regresar todos al pool
        ReturnAllToPool();

        isAttacking = false;
    }

    private void SetupLasers()
    {
        activeLasers.Clear();

        for (int i = 0; i < totalLasers; i++)
        {
            // Obtener rayo del pool (padre)
            GameObject laserObj = GetLaserFromPool();

            // Elegir fire point aleatorio
            Transform firePoint = Random.value > 0.5f ? firePoint1 : firePoint2;

            // Direcci�n aleatoria
            float angle = Random.Range(-25f, 25f);

            // Posicionar y rotar el PADRE
            laserObj.transform.position = firePoint.position;
            laserObj.transform.rotation = Quaternion.Euler(0, 0, angle);

            activeLasers.Add(laserObj);
        }
    }

    private IEnumerator BlinkPhase()
    {
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < warningTime)
        {
            // Parpadear todos los activos (accediendo al hijo)
            foreach (GameObject laserObj in activeLasers)
            {
                LaserBeam beam = laserObj.transform.GetChild(0).GetComponent<LaserBeam>();
                if (beam != null)
                {
                    beam.SetVisible(visible);
                }
            }

            visible = !visible;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // Todos visibles al final
        foreach (GameObject laserObj in activeLasers)
        {
            LaserBeam beam = laserObj.transform.GetChild(0).GetComponent<LaserBeam>();
            if (beam != null)
            {
                beam.SetVisible(true);
            }
        }
    }

    private void ActivateRandomRealLasers()
    {
        // Elegir �ndices aleatorios para rayos reales
        List<int> realIndices = new List<int>();
        while (realIndices.Count < realLasers)
        {
            int random = Random.Range(0, activeLasers.Count);
            if (!realIndices.Contains(random))
            {
                realIndices.Add(random);
            }
        }

        // Activar solo los reales, apagar los dem�s
        for (int i = 0; i < activeLasers.Count; i++)
        {
            LaserBeam beam = activeLasers[i].transform.GetChild(0).GetComponent<LaserBeam>();
            if (beam != null)
            {
                if (realIndices.Contains(i))
                {
                    // Este es real, activar da�o
                    beam.Activate(true);
                }
                else
                {
                    // Este es falso, apagarlo
                    beam.Deactivate();
                }
            }
        }
    }

    private void ReturnAllToPool()
    {
        foreach (GameObject laserObj in activeLasers)
        {
            ReturnToPool(laserObj);
        }
        activeLasers.Clear();
    }
}
