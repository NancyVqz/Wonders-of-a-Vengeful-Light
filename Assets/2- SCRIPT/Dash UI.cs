using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Image fillImage;
    private Button dashButton;

    private void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        fillImage = GetComponent<Image>();
        dashButton = GetComponent<Button>();
    }

    private void Update()
    {
        fillImage.fillAmount = playerMovement.GetDashCooldownPercent();
        dashButton.interactable = playerMovement.CanDash();
    }
}
