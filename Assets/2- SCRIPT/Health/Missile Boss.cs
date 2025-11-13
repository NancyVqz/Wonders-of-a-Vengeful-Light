using UnityEngine;
using System.Collections;

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
    private DamagePlayerVfx damageEffectScript;

    private void Awake()
    {
        spriteAnim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Start()
    {
        laserAttack = FindAnyObjectByType<LaserAttack>();
        damageEffectScript = FindAnyObjectByType<DamagePlayerVfx>();
    }

    private void OnEnable()
    {
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
        
        laserAttack.SpawnBeamAtPosition(posPlayer, beamDuration);

        BossBulletPool.instance.ReturnBullet(gameObject);
    }

    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        if (spriteAnim != null && controller != null)
        {
            spriteAnim.runtimeAnimatorController = controller;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            AudioManager.instance.Play("prota damage");
            damageEffectScript.TriggerDamageFlash();
            StartCoroutine(SoundTime());

        }
    }

    private IEnumerator SoundTime()
    {
        yield return new WaitForSeconds(0f);

        CameraShake.instance.Shake();
        GameManager.instance.playerHealth -= 1;
        UIPlayerHealth uiScript = FindAnyObjectByType<UIPlayerHealth>();
        uiScript.UpdateHealthDisplay(GameManager.instance.playerHealth);

        BossBulletPool.instance.ReturnBullet(gameObject);
    }
}
