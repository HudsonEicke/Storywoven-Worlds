using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public HealthUI healthUI;

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
        UpdateUI();
    }

    public void DecreaseMaxHealth(int amount)
    {
        maxHealth -= amount;

        if(maxHealth <= 0)
            maxHealth = 1;

        currentHealth = maxHealth;
        UpdateUI();
    }

    public void UpdateUI()
    {
        Debug.Log("Player Health is " + currentHealth);
        healthUI.UpdateHealth(currentHealth);
    }
}
