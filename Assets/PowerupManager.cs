using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject gemWarp;
    public float timeToDisplayText = 5f;
    public float timeBetweenWarp = 5f;
    private bool firstRun = false;
    public GameObject fadeImage;
    private bool warping = false;
    public float timeRemaining;
    public AudioSource gemNoise;

    private void Update()
    {
        if (!warping)
            return;

        timeRemaining -= Time.deltaTime;

        if (firstRun)
        {
            ImportantComponentsManager.Instance.thirdPersonMovement.newPosition = gemWarp.transform.position;
            ImportantComponentsManager.Instance.thirdPersonMovement.QueuedMove = true;
            firstRun = false;
        }

        if(timeRemaining <= 0)
        {
            warping = false;
            fadeImage.SetActive(false);
        }
    }

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

        firstRun = true;
        warping = true;
        timeRemaining = timeBetweenWarp;
        gemNoise.Play();
        fadeImage.SetActive(true);
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
