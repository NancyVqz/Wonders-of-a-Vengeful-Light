using TMPro;
using UnityEngine;

public class ScoreCount : MonoBehaviour
{
    public static ScoreCount instance;

    private TextMeshProUGUI contador;

    public float timeElapsed = 0f;
    public int comboTime;

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
        contador = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        timeElapsed += Time.deltaTime;

        contador.text = "Score: " + GameManager.instance.score;
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
    }
}
