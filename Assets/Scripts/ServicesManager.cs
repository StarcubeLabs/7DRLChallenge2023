using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicesManager : MonoBehaviour
{
    static ServicesManager instance;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != this)
        {
            GameObject.DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
