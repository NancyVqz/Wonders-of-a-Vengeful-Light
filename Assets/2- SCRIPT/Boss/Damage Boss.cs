using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DamageBoss : MonoBehaviour
{
    [Range(0f, 100f)]
    [SerializeField] private float vidaBoss = 100;
    [SerializeField] private float damageBoss;
    [SerializeField] Image vidaUi;
    [SerializeField] private float waitFadeTime;

    [SerializeField] UnityEvent onBossDead;

    [Header("Change Scene")]
    [SerializeField] private string escena;

    private LaserAttack laserScript;
    private Shoot scriptBulletPool;
    private Animator anim;

    private SpriteRenderer spriteRenderer;
    private Coroutine damageFlashCoroutine;

    private void Start()
    {
        scriptBulletPool = FindAnyObjectByType<Shoot>();
        laserScript = GetComponent<LaserAttack>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage();
            ActualizarUi();
            scriptBulletPool.ReturnBullet(other.gameObject);
        }
    }

    private void ActualizarUi()
    {
        vidaUi.fillAmount = vidaBoss / 100;
    }

    private void TakeDamage()
    {
        vidaBoss -= damageBoss;

        if (!laserScript.ocupado)
        {
            anim.SetTrigger("damage");
        }
        else
        {
            damageFlashCoroutine = StartCoroutine(FlashRojo());
        }

        if (vidaBoss <= 0)
        {
            onBossDead.Invoke();
            StartCoroutine(WaitFadeOutTime());
        }
    }

    private IEnumerator WaitFadeOutTime()
    {
        PixelTransition.instance.StartLowResEffect();
        yield return new WaitForSeconds(waitFadeTime);
        //AudioManager.instance.Stop("boss");
        SceneManager.LoadScene(escena);

    }

    private IEnumerator FlashRojo()
    {
        float duracion = 0.2f;
        float tiempo = 0;

        // Ir a rojo
        while (tiempo < duracion)
        {
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        tiempo = 0;

        // Volver a blanco
        while (tiempo < duracion)
        {
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = Color.white;
    }

}
