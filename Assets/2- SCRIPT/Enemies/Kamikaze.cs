using System.Collections;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private GameObject alertSign;
    [SerializeField] private GameObject explosionVfx; 
    [SerializeField] private GameObject directionAlertSign; 

    [Header("Settings")]
    [SerializeField] private int minCooldownAttack = 8;
    [SerializeField] private int maxCooldownAttack = 12;
    [SerializeField] private float minTimeAlert = 3f;
    [SerializeField] private float maxTimeAlert = 6f;
    [SerializeField] private float timeToAttack = 1f;
    [SerializeField] private float velocidad = 15f;
    [SerializeField] private float explosionTime = 2f;
    [SerializeField] private GameObject colliderDanio;
    [SerializeField] private GameObject enemy;

    [Header("Direction Alert Settings")]
    [SerializeField] private float margenDesdeBorde = 0.5f;

    [SerializeField] private float distanciaExplosion = 0.5f;
    private Vector3 posTarget;
    private bool estaActivo = false;
    private float distanciaRecorridaTotal = 0f;
    private float distanciaMaxima = 0f;
    private Transform currentSpawnPoint;
    private PlayerMovement playerMovement;

    private bool canAttack = true;

    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(minCooldownAttack);

        if (canAttack)
        {
            StartCoroutine(CooldownAttack());
        }
    }

    void Attack()
    {
        enemy.transform.position = currentSpawnPoint.position;
        enemy.gameObject.SetActive(true);
        estaActivo = true;

        distanciaMaxima = Vector3.Distance(enemy.transform.position, posTarget);
        distanciaRecorridaTotal = 0f;

        float angulo = Mathf.Atan2((posTarget - enemy.transform.position).normalized.y, (posTarget - enemy.transform.position).normalized.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0, 0, angulo - 90);

        StartCoroutine(Move());
    }

    void Explotar()
    {
        colliderDanio.transform.position = posTarget;
        explosionVfx.transform.position = posTarget;
        CameraShake.instance.ShakeExplosion();
        explosionVfx.SetActive(true);
        Desactivar();

        StartCoroutine(Danio());
    }

    void Desactivar()
    {
        estaActivo = false;
        enemy.gameObject.SetActive(false);
        distanciaRecorridaTotal = 0f;
    }

    void ShowWarning()
    {
        AudioManager.instance.Play("alerta");
        alertSign.SetActive(true);
        alertSign.transform.SetParent(player);
        alertSign.transform.localPosition = Vector3.zero;

        ShowDirectionAlert();
    }

    void ShowDirectionAlert()
    {
        // Obtener los límites de la cámara en coordenadas del mundo
        Camera cam = Camera.main;
        float alturaCamara = cam.orthographicSize;
        float anchoCamara = alturaCamara * cam.aspect;

        Vector3 centerScreen = cam.transform.position;

        // Límites de la pantalla
        float limiteIzquierdo = centerScreen.x - anchoCamara + margenDesdeBorde;
        float limiteDerecho = centerScreen.x + anchoCamara - margenDesdeBorde;
        float limiteInferior = centerScreen.y - alturaCamara + margenDesdeBorde;
        float limiteSuperior = centerScreen.y + alturaCamara - margenDesdeBorde;

        Vector3 spawnPos = currentSpawnPoint.position;
        Vector3 alertPosition = spawnPos;

        // Determinar qué borde es el más cercano al spawn point
        // y colocar la alerta en ese borde manteniendo la posición Y o X según corresponda

        float distIzquierda = Mathf.Abs(spawnPos.x - limiteIzquierdo);
        float distDerecha = Mathf.Abs(spawnPos.x - limiteDerecho);
        float distArriba = Mathf.Abs(spawnPos.y - limiteSuperior);
        float distAbajo = Mathf.Abs(spawnPos.y - limiteInferior);

        float minDist = Mathf.Min(distIzquierda, distDerecha, distArriba, distAbajo);

        if (minDist == distIzquierda)
        {
            // Spawn está a la izquierda
            alertPosition.x = limiteIzquierdo;
            alertPosition.y = Mathf.Clamp(spawnPos.y, limiteInferior, limiteSuperior);
        }
        else if (minDist == distDerecha)
        {
            // Spawn está a la derecha
            alertPosition.x = limiteDerecho;
            alertPosition.y = Mathf.Clamp(spawnPos.y, limiteInferior, limiteSuperior);
        }
        else if (minDist == distArriba)
        {
            // Spawn está arriba
            alertPosition.y = limiteSuperior;
            alertPosition.x = Mathf.Clamp(spawnPos.x, limiteIzquierdo, limiteDerecho);
        }
        else // distAbajo
        {
            // Spawn está abajo
            alertPosition.y = limiteInferior;
            alertPosition.x = Mathf.Clamp(spawnPos.x, limiteIzquierdo, limiteDerecho);
        }

        alertPosition.z = 0;

        directionAlertSign.transform.position = alertPosition;
        directionAlertSign.SetActive(true);
    }
    void HideDirectionAlert()
    {
        directionAlertSign.SetActive(false);
    }

    IEnumerator CooldownAttack()
    {
        if (canAttack)
        {
            StartCoroutine(KamikazeAttack());
        }

        int time = Random.Range(minCooldownAttack, maxCooldownAttack);
        yield return new WaitForSeconds(time);

        if (canAttack)
        {
            StartCoroutine(CooldownAttack());
        }
    }

    IEnumerator KamikazeAttack()
    {
        currentSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        ShowWarning();

        float timeToAppear = Random.Range(minTimeAlert, maxTimeAlert);
        yield return new WaitForSeconds(timeToAppear);

        alertSign.transform.SetParent(null);
        posTarget = alertSign.transform.position;

        yield return new WaitForSeconds(timeToAttack);
        AudioManager.instance.Stop("alerta");
        alertSign.SetActive(false);
        HideDirectionAlert();

        Attack();
    }

    IEnumerator Move()
    {
        while (estaActivo)
        {
            float distanciaActual = Vector3.Distance(enemy.transform.position, posTarget);

            // Triple verificación para asegurar la explosión en móviles
            if (distanciaActual < distanciaExplosion ||
                distanciaRecorridaTotal >= distanciaMaxima ||
                distanciaActual > distanciaMaxima) // Si se pasó del objetivo
            {
                Explotar();
                yield break;
            }

            Vector3 direction = (posTarget - enemy.transform.position).normalized;
            float movimiento = velocidad * Time.deltaTime;

            // Limitar movimiento para no pasar el objetivo
            if (movimiento > distanciaActual)
            {
                movimiento = distanciaActual;
            }

            enemy.transform.position += direction * movimiento;
            distanciaRecorridaTotal += movimiento;

            // Verificación de distancia máxima del jugador
            if (Vector3.Distance(enemy.transform.position, player.position) > 30f)
            {
                Desactivar();
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator Danio()
    {
        AudioManager.instance.Play("explosion");
        colliderDanio.SetActive(true);

        yield return new WaitForSeconds(explosionTime);

        colliderDanio.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Explotar();
        }

        if (other.CompareTag("Player"))
        {
            playerMovement.DanioProta();
        }
    }

    public void DetenerKamikaze()
    {
        canAttack = false;
    }
}
