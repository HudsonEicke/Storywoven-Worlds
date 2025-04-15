using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Money Currency;
    Text moneyT;
    
    private Button buyButton;
    private Text notEnoughMoney;

    public InvetoryUIManager inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        Currency = GetComponent<Money>();
        buyButton = GameObject.Find("Buy10").GetComponent<Button>();
        moneyT = GameObject.Find("moneyText").GetComponent<Text>();
        notEnoughMoney = GameObject.Find("insufMoney").GetComponent<Text>();
        notEnoughMoney.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryUI != null)
        {
            inventoryUI.CloseInventory();
            inventoryUI.OpenInventory();
        }
    }

    public void Sell10(){
        Currency.modifyMoney(10);
        moneyT.text = Currency.getMoney().ToString() + "g";
        Debug.Log("Sold Item");
    }

    public void Buy10(){

        StartCoroutine(moneyCheck(buyButton, 0.5f, 10, 0, 1));
        moneyT.text = Currency.getMoney().ToString() + "g";
    }

    private IEnumerator moneyCheck(Button button, float duration, int price, int id, int quantity)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (Currency.getMoney() >= price)
        {
            Currency.modifyMoney(-price);
            notEnoughMoney.text = "Money Spent";
            notEnoughMoney.color = Color.green;
            buttonImage.color = Color.green;
            if (inventoryUI != null)
            {
                Debug.Log("Inventory UI is not null");
                inventoryUI.CloseInventory();
                inventoryUI.OpenInventory();
            }
            else {
                Debug.Log("Inventory UI is null");
            }
            ItemIdManager.Instance.AddItem(id, quantity);


            Debug.Log("Purchase Successful");
        }
        else
        {
            notEnoughMoney.text = "Not Enough Money";
            notEnoughMoney.color = Color.red;
            buttonImage.color = Color.red;
            Debug.Log("Insufficient Funds");
        }
        notEnoughMoney.enabled = true;
        yield return new WaitForSeconds(duration);
        notEnoughMoney.enabled = false;
        buttonImage.color = Color.white;
    }
}
