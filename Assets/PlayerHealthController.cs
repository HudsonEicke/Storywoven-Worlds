using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth < 0)
        {
            GameManager3D.Instance.OnDeath();
        }

        UpdateUI();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        //UPDATE UI
    }
}
