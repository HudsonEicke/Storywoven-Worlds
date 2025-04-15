using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantComponentsManager : MonoBehaviour
{
    static ImportantComponentsManager _instance;
    public static ImportantComponentsManager Instance { get { return _instance; } }

    public ThirdPersonMovement thirdPersonMovement;
    public InvetoryUIManager invetoryUIManager;


    // Start is called before the first frame update
    void Start()
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
