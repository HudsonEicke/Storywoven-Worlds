using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    static CheckpointManager _instance;
    public static CheckpointManager Instance { get { return _instance; } }
    public int currentPlayerCheckpoint;
    public List<Checkpoint> checkpoints;
    public ThirdPersonMovement player;
    public CharacterList characterList;
    [Space]
    [Header("Tutorial: 1, Fantasy: 2, Sci-fi: 3")]
    public int sceneID = 1;

    private void Awake()
    {
        _instance = this;
    }

    public void PlayerHitCheckpoint(int checkpointID)
    {
        ImportantComponentsManager.Instance.dialogueBox.DisplayText("Check point reached all health restored", 5f);
        checkpoints[currentPlayerCheckpoint].DeactivateCheckpoint();
        currentPlayerCheckpoint = checkpointID;
        ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.Heal(10);
        characterList = GameManager2D.instance.characterList;
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("Reviving character...");
            characterList.characters[i].playerUnit.revive();
        }
        SaveManager.Instance.SavePlayer();
    }

    public void MovePlayerToCheckpoint()
    {
        player.newPosition = checkpoints[currentPlayerCheckpoint].GetRespawnPostion();
        checkpoints[currentPlayerCheckpoint].StartFire();
        player.QueuedMove = true;
    }
}
