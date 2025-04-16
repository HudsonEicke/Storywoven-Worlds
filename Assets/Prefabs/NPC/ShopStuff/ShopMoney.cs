using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class ShopMoney : MonoBehaviour
{
    private int currentMoney = 0;
    public TextMeshProUGUI moneyText; // Reference to the UI text element
    // Update is called once per frame
    void Update()
    {

        this.currentMoney = GameManager3D.Instance.GetMoney();
        if (moneyText != null)
        {
            moneyText.text = currentMoney.ToString() + "g"; // Update the text with the current money value
        }
        else
        {
            Debug.LogWarning("moneyText is not assigned in the inspector.");
        }
    }
}
