using UnityEngine;
using System.Collections;

public class BossMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopPoint = -2f;
    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float shootCooldown = 3f;

    [SerializeField][Range(0f, 1f)] private float prob = 0.7f;

    private BossAttacks bossAttacks;
    private bool hasReachedTarget = false;
    private bool isMoving = false;
    private LaserAttack laserScript;

    private void Start()
    {
        bossAttacks = GetComponent<BossAttacks>();
        laserScript = GetComponent<LaserAttack>();
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

    private void Update()
    {
        if (!hasReachedTarget && isMoving)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            if (transform.position.y <= stopPoint)
            {
                transform.position = new Vector3(transform.position.x, stopPoint, transform.position.z);
                hasReachedTarget = true;
                isMoving = false;
                StartCoroutine(Attack());
            }
        }
    }

    public bool HasReachedTarget()
    {
        return hasReachedTarget;
    }

    private IEnumerator Attack()
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
        //con un metodo asyncrono para hacer el laser
        laserScript.LaserBeamAttack();

        StartCoroutine(Attack());
    }
}
