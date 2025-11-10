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

    [Header("Sprites")]
    [SerializeField] private RuntimeAnimatorController laserBlinking;
    [SerializeField] private RuntimeAnimatorController laserActive;
    [SerializeField] private RuntimeAnimatorController laserShootActive;


    private Queue<GameObject> laserPool = new Queue<GameObject>();
    private List<GameObject> activeLasers = new List<GameObject>();

    private bool isAttacking = false;
    public bool ocupado = false;
    private Animator anim;
    

    void Start()
    {
        anim = GetComponent<Animator>();
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
        GameObject laserObj = Instantiate(laserPrefab);
        laserObj.SetActive(false);

        // Verificar que el hijo exista
        if (laserObj.transform.childCount == 0)
        {
            Debug.LogError("El prefab del láser no tiene hijos. Asegúrate de que el Beam esté dentro del prefab.");
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

    public GameObject GetLaserFromPool()
    {
        if (laserPool.Count == 0)
        {
            CreateLaserForPool();
        }

        GameObject laserObj = laserPool.Dequeue();
        laserObj.SetActive(true);
        return laserObj;
    }

    public void ReturnToPool(GameObject laserObj)
    {
        laserObj.SetActive(false);

        if (!ocupado)
        {
            AudioManager.instance.Stop("laser");
        }

        // Resetear el hijo que tiene el script LaserBeam
        LaserBeam beam = laserObj.transform.GetChild(0).GetComponent<LaserBeam>();
        if (beam != null)
        {
            beam.Reset();
        }

        laserPool.Enqueue(laserObj);
    }
    public void SpawnBeamAtPosition(Vector3 position, float duration)
    {
        StartCoroutine(SpawnBeamCoroutine(position, duration));
    }

    private IEnumerator SpawnBeamCoroutine(Vector3 position, float duration)
    {
        GameObject beamObj = GetLaserFromPool();

        if (beamObj != null)
        {
            if (!ocupado)
            {
                AudioManager.instance.Play("laser");
            }


            beamObj.transform.position = new Vector3(-8.3f, position.y, 0);
            beamObj.transform.rotation = Quaternion.Euler(0, 0, 90);

            LaserBeam beam = beamObj.transform.GetChild(0).GetComponent<LaserBeam>();
            beam.SetAnimator(laserShootActive);
            if (beam != null)
            {
                beam.Activate(true);
            }

            yield return new WaitForSeconds(duration);
            beam.SetAnimator(laserBlinking);
            ReturnToPool(beamObj);

        }
    }

    public IEnumerator AttackRoutine()
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

            // Dirección aleatoria
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

            if (visible)
            {
                AudioManager.instance.Play("laser warning");
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
        // Elegir índices aleatorios para rayos reales
        List<int> realIndices = new List<int>();
        while (realIndices.Count < realLasers)
        {
            int random = Random.Range(0, activeLasers.Count);
            if (!realIndices.Contains(random))
            {
                realIndices.Add(random);
            }
        }

        if (!ocupado)
        {
            AudioManager.instance.Play("laser");
        }

        anim.SetTrigger("laser");

        // Activar solo los reales, apagar los demás
        for (int i = 0; i < activeLasers.Count; i++)
        {
            LaserBeam beam = activeLasers[i].transform.GetChild(0).GetComponent<LaserBeam>();
            if (beam != null)
            {
                if (realIndices.Contains(i))
                {
                    beam.SetAnimator(laserActive);
                    beam.Activate(true);
                }
                else
                {
                    beam.SetAnimator(laserBlinking);
                    beam.Deactivate();
                }
            }
        }
    }

    public void ReturnAllToPool()
    {
        if (!ocupado)
        {
            AudioManager.instance.Stop("laser");
        }

        foreach (GameObject laserObj in activeLasers)
        {
            LaserBeam beam = laserObj.transform.GetChild(0).GetComponent<LaserBeam>();
            if (beam != null)
            {
                beam.SetAnimator(laserBlinking);
            }

            ReturnToPool(laserObj);
        }
        activeLasers.Clear();
    }

    public void DesactiveAllLasers()
    {
        if (!ocupado)
        {
            AudioManager.instance.Stop("laser");
        }

        foreach (GameObject laserObj in activeLasers)
        {
            laserObj.SetActive(false);  
            ReturnToPool(laserObj);
        }
        activeLasers.Clear();
    }

    public void ForceStopAllLasers()
    {
        if (!ocupado)
        {
            AudioManager.instance.Stop("laser");
        }

        isAttacking = false;
        StopAllCoroutines();
        isAttacking = false;
    }

    public void Ocupado()
    {
        ocupado = true;
    }

    public void NoOcupado()
    {
        ocupado = false;
    }
}
