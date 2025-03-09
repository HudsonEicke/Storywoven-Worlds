using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DeathChecker : MonoBehaviour
{
    public ThirdPersonMovement player;
    Vector3 lastGroundPos;
    public string fileName;

    // Update is called once per frame
    void Update()
    {
        if (!player.isGrounded)
        {
            lastGroundPos = player.GFX.position;
        }
    }

    public void PlayerFell()
    {
        StringBuilder coordinate = new StringBuilder();
        coordinate.Append(lastGroundPos.x);
        coordinate.Append(',');
        coordinate.Append(lastGroundPos.y);
        coordinate.Append(',');
        coordinate.Append(lastGroundPos.z);
        coordinate.Append(' ');
        coordinate.Append(player.GFX.position.x);
        coordinate.Append(',');
        coordinate.Append(player.GFX.position.y);
        coordinate.Append(',');
        coordinate.Append(player.GFX.position.z);
        coordinate.Append('\n');

        File.AppendAllText(fileName, coordinate.ToString());
    }
}
