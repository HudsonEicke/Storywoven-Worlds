using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointId;
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CheckpointManager.Instance.PlayerHitCheckpoint(checkpointId);
        }
    }

    public Vector3 GetRespawnPostion()
    {
        return respawnPoint.position;
    }
}
