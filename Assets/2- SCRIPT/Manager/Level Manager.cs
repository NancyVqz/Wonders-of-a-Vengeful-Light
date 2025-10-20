using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    [Header("Conteo Cambio Nivel")]
    public int enemiesAppeared = 0;
    public int allEnemiesInScene = 0;
    public int enemiesToContinue;
    private bool hasTriggeredEvent = false;
    public string nivel;

    [SerializeField] UnityEvent onEnemiesAppearedToContinue;
    [SerializeField] UnityEvent onEnemiesKilled;

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
        GameManager.instance.nivelSiguiente = nivel;
    }

    public void StopCoroutinesCheck()
    {
        if (!hasTriggeredEvent)
        {
            hasTriggeredEvent = true;
            onEnemiesAppearedToContinue.Invoke();
        }
    }

    public void BossCheck()
    {
        onEnemiesKilled.Invoke();
    }
}
