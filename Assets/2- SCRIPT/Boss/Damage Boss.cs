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

    private Shoot scriptBulletPool;

    private void Start()
    {
        scriptBulletPool = FindAnyObjectByType<Shoot>();
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

}
