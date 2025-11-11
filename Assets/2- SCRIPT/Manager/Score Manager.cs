using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public static ScoreManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SaveAndShowScore()
    {
        int currentScore = GameManager.instance.score;
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentScore > savedHighScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();

            scoreText.text = "Nuevo High Score: " + currentScore;
        }
        else
        {
            scoreText.text = "Score: " + currentScore + "                 High Score: " + savedHighScore;
        }
    }
}
