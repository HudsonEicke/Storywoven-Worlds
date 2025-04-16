using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
public class ButtonInteract : MonoBehaviour
{
	public GameObject Panel;
	public Button Button1;
	public Button Button2;
	public Button Button3;
    public TextMeshProUGUI Button1Text;
    public TextMeshProUGUI Button3Text;
    public int Button1Price;
    public int Button3Price;
    public string ShopSceneName;
    // public GameObject DialogueBubble;
    // public TextMeshProUGUI DialogueText;

    public void Start()
    {
        Button1.onClick.AddListener(() => BuyItem(Button1Price));
        Button2.onClick.AddListener(() => leaveShop());
        Button3.onClick.AddListener(() => BuyItem(Button3Price));
        Button1Text.text = Button1Price.ToString() + "g";
        Button3Text.text = Button3Price.ToString() + "g";
    }
    public void Update()
    {
      if (!DialogueManager.isActive && !DMSceneLoad.isActive)
      {
          UseButtons();
      }
      else
      {
        DisableButtons();
      }
    }
    void UseButtons()
	{
		if (Button1 != null)
        {
            Button1.interactable = true;
        }
        else
        {
            Debug.LogWarning("Button1 not assigned");
        }
        if (Button2 != null)
        {
            Button2.interactable = true;
        }
        else
        {
            Debug.LogWarning("Button2 not assigned");
        }
        if (Button3 != null)
        {
            Button3.interactable = true;
        }
        else
        {
            Debug.LogWarning("Button3 not assigned");
        }
	}

    void DisableButtons()
    {
        if (Button1 != null)
        {
            Button1.interactable = false;
        }
        else
        {
            Debug.LogWarning("Button1 not assigned");
        }
        if (Button2 != null)
        {
            Button2.interactable = false;
        }
        else
        {
            Debug.LogWarning("Button2 not assigned");
        }
        if (Button3 != null)
        {
            Button3.interactable = false;
        }
        else
        {
            Debug.LogWarning("Button3 not assigned");
        }
    }
    void BuyItem(int price)
    {
        if (GameManager3D.Instance.SpendMoney(price))
        {
            // DialogueBubble.SetActive(true);
            // DialogueText.gameObject.SetActive(true);
            // StartCoroutine(TypeDialogue($"Thanks for your purchase!"));
            Debug.Log($"Bought item for {price} coins");
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }

    void leaveShop()
    {
        Debug.Log("Leaving shop");
        if (Panel != null)
        {
            Panel.SetActive(false);
        }
        GameManager3D.Instance.UnFreezeWorld(); 
        SceneManager.UnloadSceneAsync(ShopSceneName);
    }
//     IEnumerator TypeDialogue(string Dialogue){
//         DialogueText.text = string.Empty;
//         foreach (char letter in Dialogue.ToCharArray())
//         {
//             DialogueText.text += letter;
//             yield return new WaitForSeconds(0.04f);
//         }
//     }   
}
