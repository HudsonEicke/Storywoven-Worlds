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

        if(currentHealth <= 0)
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

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
    }

    public void DecreaseMaxHealth(int amount)
    {
        maxHealth -= amount;

        if(maxHealth <= 0)
            maxHealth = 1;

        currentHealth = maxHealth;
    }

    public void UpdateUI()
    {
        //UPDATE UI
    }
}
