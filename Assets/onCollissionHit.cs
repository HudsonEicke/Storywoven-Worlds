using UnityEngine;

public class onCollissionHit : MonoBehaviour
{
    public int triggerIsOn = 0;
    public event System.Action<bool> OnTriggerChanged;

    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerIsOn = 1;
        OnTriggerChanged?.Invoke(true); // Trigger the event when triggerIsOn changes
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        triggerIsOn = 1;
        OnTriggerChanged?.Invoke(true); // Trigger the event when triggerIsOn changes
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        triggerIsOn = 0;
        OnTriggerChanged?.Invoke(false); // Trigger the event when triggerIsOn changes
    }
}
