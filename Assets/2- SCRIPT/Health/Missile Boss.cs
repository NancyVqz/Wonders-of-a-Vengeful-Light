using UnityEngine;

public class MissileBoss : MonoBehaviour
{
    [Header("Missile Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distanceBeforeExplosion = 3f;

    private Vector2 direction;
    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;
    public bool canExplode = false;
    private Transform player;
    private Vector3 posPlayer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        startPosition = transform.position;
        canExplode = false;        
        direction = Vector2.zero;
        posPlayer = player.position;
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
        //aparecer el rayo
        BossBulletPool.instance.ReturnBullet(gameObject);
    }

    public void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer != null && newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Daño al jugador
            GameManager.instance.playerHealth -= 1;
            UIPlayerHealth uiScript = FindAnyObjectByType<UIPlayerHealth>();
            uiScript.UpdateHealthDisplay(GameManager.instance.playerHealth);

            BossBulletPool.instance.ReturnBullet(gameObject);
            return;

        }
    }
}
