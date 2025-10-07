using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("UI joystick")]
    public RectTransform background;  
    public RectTransform handle;     

    private Vector2 inputVector;
    private Vector2 centerPosition;
    private float backgroundRadius;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    void Start()
    {
        // Guardar la posición inicial del handle
        centerPosition = handle.anchoredPosition;

        // Calcular el radio del círculo grande
        backgroundRadius = (background.sizeDelta.x / 2f) - (handle.sizeDelta.x / 2f);
    }

    // Cuando presionas el joystick
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    // Mientras arrastras el joystick
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        // Convertir la posición del dedo a posición local
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out position))
        {
            position = Vector2.ClampMagnitude(position, backgroundRadius);

            handle.anchoredPosition = position;

            inputVector = position / backgroundRadius;
        }
    }

    // CRegresarlo al centro al soltarlo
    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = centerPosition;
        inputVector = Vector2.zero;
    }
}
