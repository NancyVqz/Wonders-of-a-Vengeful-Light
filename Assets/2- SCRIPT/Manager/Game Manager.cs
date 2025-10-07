using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Range(0, 8)]
    public int playerHealth;

    [Header("Power - up")]
    [Range(1, 3)]
    public int powerUp;
    [Range(1, 4)]
    public int shootLvl = 1;
    [Range(1, 4)]
    public int shieldLvl = 1;
    [Range(1, 4)]
    public int dashLvl = 1;

    public int energy;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
