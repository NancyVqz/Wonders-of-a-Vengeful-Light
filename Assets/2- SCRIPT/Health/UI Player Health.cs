using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private List<GameObject> healthIcons = new List<GameObject>();

    [SerializeField] private bool hideExtraIcons = true;

    [SerializeField] private GameObject deadPanel;
    [SerializeField] private GameObject player;

    private int totalDamage;

    private void Start()
    {
        InitializeHealthIcons();
        
        //Esteblecer la vida inicial del nivel
        UpdateHealthDisplay(HeartCount());
    }

    private int HeartCount()
    {
        switch (GameManager.instance.shieldLvl)
        {
            case 1:
                GameManager.instance.playerHealth = 3;
                break;
            case 2:
                GameManager.instance.playerHealth = 4;
                break;
            case 3:
                GameManager.instance.playerHealth = 6;
                break;
            case 4:
                GameManager.instance.playerHealth = 8;
                break;
        }
        return GameManager.instance.playerHealth;
    }
    public void HeartCountShop()
    {
        UpdateHealthDisplay(LivesCountUpdate());
    }
    public void GetActualHealth()
    {
        int actualHealth = GameManager.instance.playerHealth;
        totalDamage = HeartCount() - actualHealth;
    }
    public int LivesCountUpdate()
    {

        switch (GameManager.instance.shieldLvl)
        {
            case 1:
                GameManager.instance.playerHealth = 3 - totalDamage;
                break;
            case 2:
                GameManager.instance.playerHealth = 4 - totalDamage;
                break;
            case 3:
                GameManager.instance.playerHealth = 6 - totalDamage;
                break;
            case 4:
                GameManager.instance.playerHealth = 8 - totalDamage;
                break;
        }
        return GameManager.instance.playerHealth;
    }

    private void InitializeHealthIcons()
    {
        foreach (GameObject icon in healthIcons)
        {
            icon.SetActive(false);
        }
    }

    public void UpdateHealthDisplay(int newHealth)
    {
        // Activar / desactivar los iconos de vida
        for (int i = 0; i < healthIcons.Count; i++)
        {
            if (i < newHealth)
            {
                healthIcons[i].SetActive(true);
            }
            else if (hideExtraIcons)
            {
                healthIcons[i].SetActive(false);
            }
        }

        IsPlayerDead();
    }

    public void IsPlayerDead()
    {
        if (GameManager.instance.playerHealth <= 0)
        {
            AudioManager.instance.StopAll();
            AudioManager.instance.Play("game over");
            Time.timeScale = 0f;
            deadPanel.SetActive(true);
            ScoreManager.instance.SaveAndShowScore();

            player.gameObject.SetActive(false);
            Debug.Log("Player Muerto");
        }
    }
}
