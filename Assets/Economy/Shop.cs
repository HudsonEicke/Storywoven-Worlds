using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Money Currency;
    Text moneyT;
    // Start is called before the first frame update
    void Start()
    {
        Currency = GetComponent<Money>();
        moneyT = GameObject.Find("moneyText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyT.text = "Total: $" + Currency.getMoney().ToString();
    }
    public void Sell10(){
        Currency.modifyMoney(10);
    }

    public void Buy10(){
        Currency.modifyMoney(-10);
    }
}
