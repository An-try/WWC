using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private const float PRESSED_BUTTON_SCALE_MULTIPLIER = 0.95f;

    public void OnPointerDown(PointerEventData eventData)
    {
        SetNewButtonScale(transform.localScale * PRESSED_BUTTON_SCALE_MULTIPLIER);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetNewButtonScale(Vector3.one);
    }

    private void SetNewButtonScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
