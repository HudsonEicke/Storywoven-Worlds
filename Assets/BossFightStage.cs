using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightStage : MonoBehaviour
{
    public List<AttackPoint> attackPoints = new List<AttackPoint>();
    public Transform newSafeLocation;
    public Transform newBossPlatformLocation;
}
