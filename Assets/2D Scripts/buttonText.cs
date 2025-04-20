using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buttonText : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Text display;
    public int skillIndex;
    public int characterIndex;

    // public AudioSource hoverSound;

    private SkillSystemPlayer skillSystemPlayer;
    private SkillListPlayer1 skillListPlayer1;
    private SkillListPlayer2 skillListPlayer2;
    private SkillListPlayer3 skillListPlayer3;

    private void Awake()
    {
        skillSystemPlayer = FindObjectOfType<SkillSystemPlayer>();
        skillListPlayer1 = skillSystemPlayer.Load();
        skillListPlayer2 = skillSystemPlayer.Load2();
        skillListPlayer3 = skillSystemPlayer.Load3();
        if (characterIndex == 0)
            display.text = skillListPlayer1.P1Skills[0].description + " (Cost: " + skillListPlayer1.P1Skills[0].cost + ")";
        else if (characterIndex == 1)
            display.text = skillListPlayer2.P2Skills[0].description + " (Cost: " + skillListPlayer2.P2Skills[0].cost + ")";
        else if (characterIndex == 2)
            display.text = skillListPlayer3.P3Skills[0].description + " (Cost: " + skillListPlayer3.P3Skills[0].cost + ")";
        display.gameObject.SetActive(true);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioSystem2D.instance.PlayAudio();
        Debug.Log("Button Highlighted");
        // hoverSound.Play();

        if (characterIndex == 0)
            display.text = skillListPlayer1.P1Skills[skillIndex].description + " (Cost: " + skillListPlayer1.P1Skills[skillIndex].cost + ")";
        else if (characterIndex == 1)
            display.text = skillListPlayer2.P2Skills[skillIndex].description + " (Cost: " + skillListPlayer2.P2Skills[skillIndex].cost + ")";
        else if (characterIndex == 2)
            display.text = skillListPlayer3.P3Skills[skillIndex].description + " (Cost: " + skillListPlayer3.P3Skills[skillIndex].cost + ")";
            

        display.gameObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        display.gameObject.SetActive(false);
    }
}
