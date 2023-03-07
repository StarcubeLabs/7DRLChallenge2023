using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicesManager : MonoBehaviour
{
    public static ServicesManager instance;

    public Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != this && instance != null)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetServices()
    {
        mainCamera.transform.SetParent(this.transform);
    }
}
