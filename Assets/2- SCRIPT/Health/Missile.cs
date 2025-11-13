using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    [Header("Missile Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distanceBeforeExplosion = 3f;

    [Header("Explosion Settings")]
    [SerializeField] private int numberOfBullets = 5;
    [SerializeField] private float explosionBulletSpeed = 300f;
    [SerializeField] private float explosionBulletScale = 0.5f;

    [Header("Opcional")]
    [SerializeField] private GameObject explosionEffectPrefab;

    private Vector2 direction;
    private Vector3 startPosition;
    private bool hasExploded = false;
    private bool isExplosionBullet = false;
    private SpriteRenderer spriteRenderer;

    private DamagePlayerVfx damageEffectScript;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        damageEffectScript= FindAnyObjectByType<DamagePlayerVfx>();
        startPosition = transform.position;
        hasExploded = false;
        isExplosionBullet = false;

        if (!isExplosionBullet)
        {
            transform.localScale = Vector3.one;
        }
    }

    public void SetAsExplosionBullet(bool isExplosion)
    {
        isExplosionBullet = isExplosion;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (isExplosionBullet)
        {
            return;
        }

        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        if (distanceTraveled >= distanceBeforeExplosion && !hasExploded)
        {
            Explotar();
        }
    }

    private void Explotar()
    {
        hasExploded = true;

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        if (BossBulletPool.instance == null)
        {
            Debug.LogError("No hay pool de misiles disponible");
            return;
        }

        float angleStep = 360f / numberOfBullets;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector2 bulletDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            float spriteAngle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, spriteAngle - 90);

            GameObject bullet = BossBulletPool.instance.GetBullet(transform.position, bulletRotation, bulletDirection * explosionBulletSpeed);

            if (bullet != null)
            {
                Missile missileScript = bullet.GetComponent<Missile>();
                if (missileScript != null)
                {
                    missileScript.SetAsExplosionBullet(true);
                    missileScript.SetDirection(bulletDirection);
                }

                bullet.transform.localScale = Vector3.one * explosionBulletScale;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = bulletDirection * explosionBulletSpeed * Time.deltaTime;
                }
            }
        }
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
