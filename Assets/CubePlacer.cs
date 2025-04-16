using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CubePlacer : MonoBehaviour
{
    // Start is called before the first frame update
    public string SceneID = "2";
    bool inCorrect = false;
    public GameObject cube;

    void Start()
    {
        string[] files = Directory.GetFiles("deathFiles");
        string[] splitLine;

        foreach (string file in files)
        {
            foreach(string line in File.ReadLines(file))
            {
                if(line.Length == 1)
                {
                    if(line == SceneID)
                        inCorrect = true;
                    else
                        inCorrect = false;
                }
                else
                {
                    if(inCorrect)
                    {
                        splitLine = line.Split(' ');
                        splitLine = splitLine[1].Split(",");

                        GameObject.Instantiate(cube, new Vector3(float.Parse(splitLine[0]), 50f, float.Parse(splitLine[2])), Quaternion.identity);
                    }
                }
            }
        }
    }
}
