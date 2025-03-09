using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyDetection))]
public class EnemyFieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyDetection enemyDetection = (EnemyDetection)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(enemyDetection.transform.position, Vector3.up, Vector3.forward, 360, enemyDetection.viewRadius);
        Vector3 viewAngleA = enemyDetection.DirectionFromAngle(-enemyDetection.viewAngle / 2, false);
        Vector3 viewAngleB = enemyDetection.DirectionFromAngle(enemyDetection.viewAngle / 2, false);

        Handles.DrawLine(enemyDetection.transform.position, enemyDetection.transform.position + viewAngleA * enemyDetection.viewRadius);
        Handles.DrawLine(enemyDetection.transform.position, enemyDetection.transform.position + viewAngleB * enemyDetection.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in enemyDetection.visibleTargets)
        {
            Handles.DrawLine(enemyDetection.transform.position, visibleTarget.position);
        }
    }
}
