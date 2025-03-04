using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIdManager : MonoBehaviour
{
    static ItemIdManager _instance;
    public List<GameObject> idToItem = new List<GameObject>();
    public static ItemIdManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
