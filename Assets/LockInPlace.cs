using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInPlace : MonoBehaviour
{
    public Transform lockPos;
    private Vector3 tempPos;
    public bool locked = true;

    void LateUpdate()
    {
        if (locked)
            transform.position = lockPos.position;
    }
}
