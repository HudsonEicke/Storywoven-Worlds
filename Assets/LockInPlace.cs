using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInPlace : MonoBehaviour
{
    public Transform lockPos;
    private Vector3 tempPos;
    public bool locked = true;
    public GameObject player;
    public int damping = 2;

    void LateUpdate()
    {
        if (locked)
            transform.position = lockPos.position;


    }

    private void Update()
    {
        transform.LookAt(player.transform.position);
        Quaternion newRot = transform.rotation;
        newRot.x = 0;
        newRot.z = 0;
        transform.rotation = newRot;

    }


}
