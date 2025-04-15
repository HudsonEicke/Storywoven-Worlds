using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public bool hasDoubleJump = false;
    public bool hasSprint = false;
    public bool hasBoostedHealth = false;
    public GameObject purpleGemWall;
    public GameObject purpleGem;
    public GameObject pinkGemWall;
    public GameObject pinkGem;
    public GameObject orangeGemWall;
    public GameObject orangeGem;
    public float timeToDisplayText = 5f;

    public void GivePowerup(int powerupID)
    {
        switch(powerupID)
        {
            case 0:
                hasDoubleJump = true;
                ImportantComponentsManager.Instance.thirdPersonMovement.hasDoubleJumpPowerUp = true;
                Destroy(purpleGem);
                Destroy(purpleGemWall);
                break;
            case 1:
                hasSprint = true;
                ImportantComponentsManager.Instance.thirdPersonMovement.hasSprintPowerUp = true;
                Destroy(pinkGem);
                Destroy(pinkGemWall);
                break;
            case 2:
                hasBoostedHealth = true;
                ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.IncreaseMaxHealth(2);
                Destroy(orangeGem);
                Destroy(orangeGemWall);
                break;
        }
    }

    public void GivePowerup(int powerupID, string message)
    {
        switch (powerupID)
        {
            case 0:
                hasDoubleJump = true;
                ImportantComponentsManager.Instance.thirdPersonMovement.hasDoubleJumpPowerUp = true;
                Destroy(purpleGem);
                Destroy(purpleGemWall);
                break;
            case 1:
                hasSprint = true;
                ImportantComponentsManager.Instance.thirdPersonMovement.hasSprintPowerUp = true;
                Destroy(pinkGem);
                Destroy(pinkGemWall);
                break;
            case 2:
                hasBoostedHealth = true;
                ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.IncreaseMaxHealth(2);
                Destroy(orangeGem);
                Destroy(orangeGemWall);
                break;
        }

        ImportantComponentsManager.Instance.dialogueBox.DisplayText(message, timeToDisplayText);
    }

    public void LoadPowerups(bool hasDoubleJump, bool hasSprint, bool hasBoostedHealth)
    {
        if (hasDoubleJump)
            GivePowerup(0);
        if (hasSprint)
            GivePowerup(1);
        if (hasBoostedHealth)
            GivePowerup(2);
    }
}
