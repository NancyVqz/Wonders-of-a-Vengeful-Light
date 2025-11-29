using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class PixelTransition : MonoBehaviour
{
    public static PixelTransition instance;

    public UniversalRenderPipelineAsset urpAsset;
    [SerializeField] private float lowScale = 0.1f;
    [SerializeField] private float transitionDuration = 1f; 

    private float originalScale = 1;
    private bool isTransitioning = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartHighResEffect();
    }

    public void StartLowResEffect()
    {
        if (!isTransitioning)
        {
            StartCoroutine(LowResRoutine());
        }
    }

    public void StartHighResEffect()
    {
        if (!isTransitioning)
        {
            StartCoroutine(HighResRoutine());
        }
    }
    private IEnumerator HighResRoutine()
    {
        isTransitioning = true;

        yield return StartCoroutine(TransitionScale(lowScale, originalScale, transitionDuration));

        isTransitioning = false;
    }

    private IEnumerator LowResRoutine()
    {
        isTransitioning = true;

        yield return StartCoroutine(TransitionScale(originalScale, lowScale, transitionDuration));

        isTransitioning = false;
    }

    private IEnumerator TransitionScale(float startScale, float endScale, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            urpAsset.renderScale = Mathf.Lerp(startScale, endScale, t);

            yield return null;
        }
        urpAsset.renderScale = endScale;
    }
}
