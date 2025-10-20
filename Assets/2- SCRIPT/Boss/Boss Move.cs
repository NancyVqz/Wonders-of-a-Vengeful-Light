using System.Collections;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopPoint = -2f;
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float shootCooldown = 3f;
    [SerializeField] private float laserCooldown = 6f;

    [Header("Attacks Settings")]
    [SerializeField][Range(0f, 1f)] private float prob = 0.7f;
    [SerializeField] private bool canLaserAttack = false;

    private bool allowAttack = true;
    private BossAttacks bossAttacks;
    private bool hasReachedTarget = false;
    private bool isMoving = false;
    private LaserAttack laserScript;
    private BoxCollider2D colBoss;

    private void Start()
    {
        bossAttacks = GetComponent<BossAttacks>();
        laserScript = GetComponent<LaserAttack>();
        colBoss = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        hasReachedTarget = false;
        isMoving = false;

        StartCoroutine(StartMovementAfterDelay());
    }

    private IEnumerator StartMovementAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        isMoving = true;
    }

    //BOSS ATAQUES RUTINAS
    private void Update()
    {
        if (!hasReachedTarget && isMoving)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            if (transform.position.y <= stopPoint)
            {
                colBoss.enabled = true;
                transform.position = new Vector3(transform.position.x, stopPoint, transform.position.z);
                hasReachedTarget = true;
                isMoving = false;
                StartCoroutine(BulletAttack());
                if (canLaserAttack && allowAttack)
                {
                    StartCoroutine(LaserAttack());
                }
            }
        }
    }

    public bool HasReachedTarget()
    {
        return hasReachedTarget;
    }

    private IEnumerator BulletAttack()
    {
        float randomValue = Random.value;

        if (randomValue < prob)
        {
            bossAttacks.StartRandomBulletAttack();
        }
        else
        {
            bossAttacks.ShootAtPlayerWithDisappear();
        }

        yield return new WaitForSeconds(shootCooldown);

        if (allowAttack)
        {
            StartCoroutine(BulletAttack());
        }
    }

    private IEnumerator LaserAttack()
    {
        if (allowAttack)
        {
            laserScript.LaserBeamAttack();

            yield return new WaitForSeconds(laserCooldown);


            StartCoroutine(BulletAttack());
            StartCoroutine(LaserAttack());
        }
    }

    public void MoveUp()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveUpward());
        }
    }

    private IEnumerator MoveUpward()
    {
        yield return new WaitForSeconds(3);
        isMoving = true;
        hasReachedTarget = false;

        while (transform.position.y < 14.38f)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, 14.38f, transform.position.z);

        isMoving = false;
        hasReachedTarget = true;
    }

    public void StopCoroutine()
    {
        allowAttack = false;
    }
}
