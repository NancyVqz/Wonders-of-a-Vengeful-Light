using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private List<GameObject> healthIcons = new List<GameObject>();

    [SerializeField] private Sprite starInactiveSprite;

    [SerializeField] private GameObject deadPanel;
    [SerializeField] private GameObject player;

    private int totalDamage;

    private void Start()
    {
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

    public void UpdateHealthDisplay(int newHealth)
    {
        for (int i = 0; i < healthIcons.Count; i++)
        {
            // TODAS las estrellas se mantienen activas
            healthIcons[i].SetActive(true);

            Animator animator = healthIcons[i].GetComponent<Animator>();

            if (i < newHealth)
            {
                // Vida activa: activar animator
                if (animator != null)
                {
                    animator.enabled = true;
                }
            }
            else
            {
                // Vida perdida o no disponible: desactivar animator y poner sprite gris
                if (animator != null)
                {
                    animator.enabled = false;
                }

                Image imageComponent = healthIcons[i].GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = starInactiveSprite;
                }
                else
                {
                    SpriteRenderer spriteRenderer = healthIcons[i].GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = starInactiveSprite;
                    }
                }
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
