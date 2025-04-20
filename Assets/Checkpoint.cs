using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointId;
    public Transform respawnPoint;
    public ParticleSystem fireParticle = null;

    private void Start()
    {
        if (fireParticle != null)
        {
            fireParticle.Stop();
        }
    }

    public void StartFire()
    {
        if (fireParticle != null)
        {
            fireParticle.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (fireParticle != null)
            {
                fireParticle.Play();
            }

            CheckpointManager.Instance.PlayerHitCheckpoint(checkpointId);
        }
    }

    public Vector3 GetRespawnPostion()
    {
        return respawnPoint.position;
    }

    public void DeactivateCheckpoint()
    {
        if (fireParticle != null)
        {
            fireParticle.Stop();
        }
    }
}
