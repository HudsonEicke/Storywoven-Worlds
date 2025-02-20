using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InvetoryUIManager : MonoBehaviour
{
    public List<GameObject> inventoryButtons = new List<GameObject>();
    private List<Item> inventoryRange;
    public GameObject inventoryBackground;
    private int startViewRange;
    private int endViewRange;
    private bool isInventoryOpen = false;

    public bool pressedButton = false;
    public bool scrollDown = false;
    public bool scrollUp = false;


    void Start()
    {
        startViewRange = 0;
        endViewRange = inventoryButtons.Count - 1;
        CloseInventory();
    }

    // Update is called once per frame
    void Update()
    {
        //IF PLAYER HIT INVENTORY BUTTON
        if(pressedButton)
        {
            pressedButton = false;
            if(isInventoryOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }

        if(isInventoryOpen)
        {
            if(scrollDown)
            {
                scrollDown = false;
                ScrollDown();
            }
            else if(scrollUp)
            {
                scrollUp = false;
                ScrollUp();
            }
        }
    }

    void OpenInventory()
    {
        inventoryBackground.SetActive(true);

        inventoryRange = InventoryManager.GetInventoryRange(0, inventoryButtons.Count - 1);

        for (int i = 0; i < inventoryButtons.Count; i++)
        {
            if (inventoryRange[i] != null)
            {
                inventoryButtons[i].SetActive(true);
                UpdateButton(inventoryRange[i], i);
            }
            else
            {
                inventoryButtons[i].SetActive(false);
            }
        }

        isInventoryOpen = true;
    }

    void CloseInventory()
    {
        inventoryBackground.SetActive(false);

        foreach(GameObject button in inventoryButtons)
        {
            button.SetActive(false);
        }

        isInventoryOpen = false;
    }

    void ScrollDown()
    {
        Item nextItem = InventoryManager.GetNextItem(endViewRange);

        if(nextItem == null)
        {
            return;
        }

        for(int i = 1;  i < inventoryButtons.Count; i++)
        {
            UpdateButton(inventoryRange[i], i - 1);
            inventoryRange[i - 1] = inventoryRange[i];
        }
        startViewRange++;
        endViewRange++;
        inventoryRange[inventoryButtons.Count - 1] = nextItem;
        UpdateButton(nextItem, inventoryButtons.Count - 1);
    }

    void ScrollUp()
    {
        if(startViewRange == 0)
        {
            return;
        }

        Item previousItem = InventoryManager.GetPrevious(startViewRange);

        for(int i = inventoryButtons.Count - 1; i > 0; i--)
        {
            UpdateButton(inventoryRange[i - 1], i);
            inventoryRange[i] = inventoryRange[i - 1];
        }

        startViewRange--;
        endViewRange--;
        UpdateButton(previousItem, 0);
        inventoryRange[0] = previousItem;
    }

    void UpdateButton(Item newItem, int buttonNum)
    {
        GameObject currentButton = inventoryButtons[buttonNum];

        TMP_Text textBox = currentButton.GetComponentInChildren<TMP_Text>();

        textBox.text = newItem.itemName;

        Image itemImage = currentButton.GetComponentInChildren<Image>();

        itemImage.sprite = newItem.itemIcon;
    }

    public void UseItem(int itemNum)
    {
        InventoryManager.UseIndex(itemNum + startViewRange);

        //NEED RESET CALL
    }
}
