using System.Collections;
using UnityEngine;

public class Kamikaze : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private GameObject alertSign;
    [SerializeField] private GameObject explosionVfx; 

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


    private Vector3 posTarget;
    private bool estaActivo = false;

    void Start()
    {
        enemy.gameObject.SetActive(false);
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(minCooldownAttack);
        StartCoroutine(CooldownAttack());
    }

    void Attack()
    {
        Transform randomSP = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = randomSP.position;
        enemy.gameObject.SetActive(true);
        estaActivo = true;

        float angulo = Mathf.Atan2((posTarget - enemy.transform.position).normalized.y, (posTarget - enemy.transform.position).normalized.x) * Mathf.Rad2Deg;

        enemy.transform.rotation = Quaternion.Euler(0, 0, angulo - 90);

        StartCoroutine(Move());
    }

    void Explotar()
    {
        colliderDanio.transform.position = posTarget;
        explosionVfx.transform.position = posTarget;
        explosionVfx.SetActive(true);
        Desactivar();

        StartCoroutine(Danio());
    }

    void Desactivar()
    {
        estaActivo = false;
        enemy.gameObject.SetActive(false);
    }

    void ShowWarning()
    {
        AudioManager.instance.Play("alerta");
        alertSign.SetActive(true);
        alertSign.transform.SetParent(player);
        alertSign.transform.localPosition = Vector3.zero;
    }

    IEnumerator CooldownAttack()
    {
        StartCoroutine(KamikazeAttack());

        int time = Random.Range(minCooldownAttack, maxCooldownAttack );
        yield return new WaitForSeconds(time);

        StartCoroutine(CooldownAttack());
    }

    IEnumerator KamikazeAttack()
    {
        ShowWarning();

        float timeToAppear = Random.Range(minTimeAlert, maxTimeAlert);
        yield return new WaitForSeconds(timeToAppear);

        alertSign.transform.SetParent(null);
        posTarget = alertSign.transform.position;

        yield return new WaitForSeconds(timeToAttack);
        AudioManager.instance.Stop("alerta");
        alertSign.SetActive(false);

        Attack();
    }

    IEnumerator Move()
    {
        while (estaActivo)
        {
            Vector3 direction = (posTarget - enemy.transform.position).normalized;
            enemy.transform.position += direction * velocidad * Time.deltaTime;

            if (Vector3.Distance(enemy.transform.position, posTarget) < 0.1f)
            {
                Explotar();
                yield break;
            }

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
            //restarle vida al jugador
        }
    }
}
