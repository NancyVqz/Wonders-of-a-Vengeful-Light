using UnityEngine;

public class MissileBoss : MonoBehaviour
{
    [Header("Missile Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distanceBeforeExplosion = 3f;
    [SerializeField] private float beamDuration = 2f;

    private Vector2 direction;
    private Animator spriteAnim;
    public bool canExplode = false;
    private Transform player;
    private Vector3 posPlayer;
    private LaserAttack laserAttack;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        spriteAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        CacheReferences();
    }

    private void OnEnable()
    {
        canExplode = false;
        direction = Vector2.zero;

        // Cachear referencias cada vez que se activa (importante para pooling)
        CacheReferences();

        // Solo guardar posición si player existe
        if (player != null)
        {
            posPlayer = player.position;
        }
    }


    private void CacheReferences()
    {
        // Buscar player si aún no está asignado o si es null
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerMovement = playerObj.GetComponent<PlayerMovement>();
            }
        }

        // Buscar laserAttack si es null
        if (laserAttack == null)
        {
            laserAttack = FindAnyObjectByType<LaserAttack>();
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (canExplode)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, posPlayer);
            if (distanceToPlayer <= distanceBeforeExplosion)
            {
                Explotar();
            }
        }
    }

    private void Explotar()
    {
        // Verificar que laserAttack existe antes de usarlo
        if (laserAttack != null)
        {
            laserAttack.SpawnBeamAtPosition(posPlayer, beamDuration);
        }
        else
        {
            Debug.LogWarning("LaserAttack no encontrado al explotar el misil");
        }

        // Verificar que BossBulletPool existe antes de retornar
        if (BossBulletPool.instance != null)
        {
            BossBulletPool.instance.ReturnBullet(gameObject);
        }
    }

    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        if (spriteAnim != null && controller != null)
        {
            spriteAnim.runtimeAnimatorController = controller;
        }
    }

    public void ResetMissile()
    {
        canExplode = false;
        direction = Vector2.zero;

        // Resetear al animator por defecto (puedes asignar uno en el inspector)
        if (spriteAnim != null)
        {
            spriteAnim.runtimeAnimatorController = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerMovement != null)
        {
            playerMovement.DanioProta();
        }
    }
}
