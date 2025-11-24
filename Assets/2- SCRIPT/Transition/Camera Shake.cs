using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    public static CameraShake instance;

    [Header("Shake Damage Player")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.3f;
    [SerializeField] private float dampingSpeed = 1.0f;

    [Header("Shake Explosion Kamikaze")]
    [SerializeField] private float shakeDuration2 = 0.2f;
    [SerializeField] private float shakeMagnitude2 = 0.3f;

    private Vector3 originalPosition;
    private bool isShaking = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(shakeDuration, shakeMagnitude));
        }
    }

    public void ShakeExplosion()
    {
        StartCoroutine(ShakeCoroutine(shakeDuration2, shakeMagnitude2));
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        if (isShaking) yield break; 

        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-0.6f, 0.6f) * magnitude;
            float offsetY = Random.Range(-0.6f, 0.6f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        isShaking = false;
    }
}