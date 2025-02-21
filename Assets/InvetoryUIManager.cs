using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InvetoryUIManager : MonoBehaviour
{
    public List<ItemButton> inventoryButtons;
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

        startViewRange = 0;
        endViewRange = inventoryButtons.Count - 1;
        UpdateRange();

        isInventoryOpen = true;
    }

    void UpdateRange()
    {
        inventoryRange = InventoryManager.GetInventoryRange(startViewRange, endViewRange);

        for (int i = 0; i < inventoryButtons.Count; i++)
        {
            if (inventoryRange[i] != null)
            {
                inventoryButtons[i].ActivateButton();
                inventoryButtons[i].UpdateButton(inventoryRange[i]);
            }
            else
            {
                inventoryButtons[i].DeactivateButton();
            }
        }
    }

    void CloseInventory()
    {
        inventoryBackground.SetActive(false);

        foreach(ItemButton button in inventoryButtons)
        {
            button.DeactivateButton();
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
            inventoryButtons[i - 1].UpdateButton(inventoryRange[i]);
            inventoryRange[i - 1] = inventoryRange[i];
        }
        startViewRange++;
        endViewRange++;
        inventoryRange[inventoryButtons.Count - 1] = nextItem;
        inventoryButtons[inventoryButtons.Count - 1].UpdateButton(nextItem);
    }

    void ScrollUp()
    {
        if(startViewRange == 0)
        {
            return;
        }
        inventoryButtons[inventoryButtons.Count - 1].ActivateButton();
        Item previousItem = InventoryManager.GetPrevious(startViewRange);

        for(int i = inventoryButtons.Count - 1; i > 0; i--)
        {
            inventoryButtons[i].UpdateButton(inventoryRange[i - 1]);
            inventoryRange[i] = inventoryRange[i - 1];
        }

        startViewRange--;
        endViewRange--;
        inventoryButtons[0].UpdateButton(previousItem);
        inventoryRange[0] = previousItem;
    }

    //void UpdateButton(Item newItem, int buttonNum)
    //{
    //    GameObject currentButton = inventoryButtons[buttonNum];

    //    TMP_Text textBox = currentButton.GetComponentInChildren<TMP_Text>();

    //    textBox.text = newItem.itemName;

    //    Image itemImage = currentButton.GetComponentInChildren<Image>();

    //    itemImage.sprite = newItem.itemIcon;
    //}

    public void UseItem(int itemNum)
    {
        InventoryManager.UseIndex(itemNum + startViewRange);

        UpdateRange();

        if (!inventoryButtons[inventoryButtons.Count - 1].isButtonActive)
        {
            ScrollUp();
        }
    }
}
