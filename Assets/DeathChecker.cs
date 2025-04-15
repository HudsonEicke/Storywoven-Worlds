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
    private void Start()
    {
        File.AppendAllText(fileName, CheckpointManager.Instance.sceneID + "\n");

    }

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
        coordinate.Append(player.lastGroundPosition.x);
        coordinate.Append(',');
        coordinate.Append(player.lastGroundPosition.y);
        coordinate.Append(',');
        coordinate.Append(player.lastGroundPosition.z);
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
