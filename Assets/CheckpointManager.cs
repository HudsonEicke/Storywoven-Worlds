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
    [Space]
    [Header("Tutorial: 1, Fantasy: 2, Sci-fi: 3")]
    public int sceneID = 1;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayerHitCheckpoint(int checkpointID)
    {
        currentPlayerCheckpoint = checkpointID;
        ImportantComponentsManager.Instance.thirdPersonMovement.playerHealthController.Heal(10);
        SaveManager.Instance.SavePlayer();
    }

    public void MovePlayerToCheckpoint()
    {
        player.newPosition = checkpoints[currentPlayerCheckpoint].GetRespawnPostion();
        player.QueuedMove = true;
    }
}
