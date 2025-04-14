using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossFightStage : MonoBehaviour
{
    public List<AttackPoint> attackPoints = new List<AttackPoint>();
    public Transform newSafeLocation;

    public abstract void StageStart();
}
