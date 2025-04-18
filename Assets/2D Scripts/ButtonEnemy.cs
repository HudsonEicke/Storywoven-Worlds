using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEnemy : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject hoverButton;

    public void OnSelect(BaseEventData eventData)
    {
        if (hoverButton == null)
        {
            Debug.LogError("Hover button is not assigned in the inspector.");
            return;
        }
        hoverButton.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (hoverButton == null)
        {
            Debug.LogError("Hover button is not assigned in the inspector.");
            return;
        }
        hoverButton.SetActive(false);
    }
}
