using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    int money;

    public void modifyMoney(int amount)
    {
        money += amount;
        money = Mathf.Clamp(money, 0, 999999);
    }

    public int getMoney()
    {
        return money; 
    }
}
