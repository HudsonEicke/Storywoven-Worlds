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

    public void UpdateButton(Item newItem)
    {
        itemName.text = newItem.name;
        quantity.text = newItem.quantity.ToString();
        itemImage.sprite = newItem.itemIcon;
    }

    public void Use()
    {
        //need a call to the UI manager
    }

    public void ActivateButton()
    {
        buttonObject.SetActive(true);
    }

    public void DeactivateButton()
    {
        buttonObject.SetActive(false);
    }
}
