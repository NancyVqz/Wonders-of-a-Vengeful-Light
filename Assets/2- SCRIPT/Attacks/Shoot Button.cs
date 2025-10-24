using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Shoot shootScript;

    private void Start()
    {
        shootScript = FindAnyObjectByType<Shoot>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (shootScript != null)
        {
            shootScript.StartButtonHold();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (shootScript != null)
        {
            shootScript.StopButtonHold();
        }
    }
}
