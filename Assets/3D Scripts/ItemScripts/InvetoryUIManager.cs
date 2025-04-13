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
    public bool isInventoryOpen = false;

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
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (isInventoryOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }

        //IF PLAYER HIT INVENTORY BUTTON
        //if(pressedButton)
        //{
        //    pressedButton = false;
        //    if(isInventoryOpen)
        //    {
        //        CloseInventory();
        //    }
        //    else
        //    {
        //        OpenInventory();
        //    }
        //}

        if(isInventoryOpen)
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                ScrollDown();
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                ScrollUp();
            }
        }
    }

    public void OpenInventory()
    {
        inventoryBackground.SetActive(true);

        startViewRange = 0;
        endViewRange = inventoryButtons.Count - 1;
        UpdateRange();

        isInventoryOpen = true;
    }

    void UpdateRange()
    {
        inventoryRange = InventoryManager.Instance.GetInventoryRange(startViewRange, endViewRange);

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

    public void CloseInventory()
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
        Item nextItem = InventoryManager.Instance.GetNextItem(endViewRange);

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
        Item previousItem = InventoryManager.Instance.GetPrevious(startViewRange);

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

    public void UseItem(int itemNum)
    {
        InventoryManager.Instance.UseIndex(itemNum + startViewRange);

        UpdateRange();

        if (!inventoryButtons[inventoryButtons.Count - 1].isButtonActive)
        {
            ScrollUp();
        }
    }
}
