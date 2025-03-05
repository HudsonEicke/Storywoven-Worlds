using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text itemName;
    [SerializeField]
    private TMP_Text quantity;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private GameObject buttonObject;
    [SerializeField]
    private InvetoryUIManager uiManager;
    [SerializeField]
    private int buttonNum;

    public bool isButtonActive = true;

    public void UpdateButton(Item newItem)
    {
        itemName.text = newItem.itemName;
        quantity.text = newItem.quantity.ToString();
        itemImage.sprite = newItem.itemIcon;
    }

    public void Use()
    {
        uiManager.UseItem(buttonNum);
    }

    public void ActivateButton()
    {
        isButtonActive = true;
        buttonObject.SetActive(true);
    }

    public void DeactivateButton()
    {
        isButtonActive = false;
        buttonObject.SetActive(false);
    }
}
