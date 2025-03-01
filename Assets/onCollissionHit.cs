using UnityEngine;

public class onCollissionHit : MonoBehaviour
{
    public int triggerIsOn = 0;

    // Define the event
    public event System.Action<bool> OnTriggerChanged;

    private void OnTriggerEnter2D(Collider2D other)
    {
        triggerIsOn = 1;
        OnTriggerChanged?.Invoke(true); // Trigger the event when triggerIsOn changes
        // Debug.Log($"Trigger entered by {other.gameObject.name}. triggerIsOn: {triggerIsOn}");
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        triggerIsOn = 1;
        OnTriggerChanged?.Invoke(true); // Trigger the event when triggerIsOn changes
        // Debug.Log($"Trigger stay by {other.gameObject.name}. triggerIsOn: {triggerIsOn}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        triggerIsOn = 0;
        OnTriggerChanged?.Invoke(false); // Trigger the event when triggerIsOn changes
        // Debug.Log($"Trigger exited by {other.gameObject.name}. triggerIsOn: {triggerIsOn}");
    }
}
