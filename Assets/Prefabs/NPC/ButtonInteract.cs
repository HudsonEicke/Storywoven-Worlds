using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using System.Data.Common;
public class ButtonInteract : MonoBehaviour
{
	public GameObject Panel;
	public Button Exit;
	public Button Item1;
	public Button Item2;
    public Button Item3;
    public TextMeshProUGUI Button1Text;
    public TextMeshProUGUI Button2Text;
    public TextMeshProUGUI Button3Text;
    public int Button1Price;
    public int Button2Price;
    public int Button3Price;
    public Item item1;
    public Item item2;
    public Item item3;
    public string ShopSceneName;

    List<Item> itemList = new List<Item>();
    // public GameObject DialogueBubble;
    // public TextMeshProUGUI DialogueText;

    public void Start()
    {
        Item1.onClick.AddListener(() => BuyItem(Button1Price, item1));
        Exit.onClick.AddListener(() => leaveShop());
        Item2.onClick.AddListener(() => BuyItem(Button2Price, item2));
        Item3.onClick.AddListener(() => BuyItem(Button3Price, item3));
        Button1Text.text = Button1Price.ToString() + "g";
        Button2Text.text = Button2Price.ToString() + "g";
        Button3Text.text = Button3Price.ToString() + "g";
        itemList = InventoryManager.Instance.GetInventoryRange(0, 5);
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
		if (Item1 != null)
        {
            Item1.interactable = true;
        }
        else
        {
            Debug.LogWarning("Button1 not assigned");
        }
        if (Item2 != null)
        {
            Item2.interactable = true;
        }
        else
        {
            Debug.LogWarning("Button2 not assigned");
        }
        if (Item3 != null)
        {
            Item3.interactable = true;
        }
        else
        {
            Debug.LogWarning("Button3 not assigned");
        }
        if (Exit != null)
        {
            Exit.interactable = true;
        }
        else
        {
            Debug.LogWarning("Exit not assigned");
        }
	}

    void DisableButtons()
    {
        if (Item1 != null)
        {
            Item1.interactable = false;
        }
        else
        {
            Debug.LogWarning("Button1 not assigned");
        }
        if (Item2 != null)
        {
            Item2.interactable = false;
        }
        else
        {
            Debug.LogWarning("Button2 not assigned");
        }
        if (Item3 != null)
        {
            Item3.interactable = false;
        }
        else
        {
            Debug.LogWarning("Button3 not assigned");
        }
        if (Exit != null)
        {
            Exit.interactable = false;
        }
        else
        {
            Debug.LogWarning("Exit not assigned");
        }
    }
    void BuyItem(int price, Item item)
    {
        if (GameManager3D.Instance.SpendMoney(price))
        {
            ItemIdManager.Instance.AddItem(item.itemId, 1);
            // DialogueBubble.SetActive(true);
            // DialogueText.gameObject.SetActive(true);
            // StartCoroutine(TypeDialogue($"Thanks for your purchase!"));
            Debug.Log($"Bought {item.itemName} for {price} coins");
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
