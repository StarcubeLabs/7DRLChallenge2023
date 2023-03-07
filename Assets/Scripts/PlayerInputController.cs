using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public ActorController playerActor;
    public Transform cameraSlot;
    ServicesManager servicesManager;
    GameStateManager gameStateManager;

    public GameObject InventoryMenu;
    // Start is called before the first frame update
    void Start()
    {
        playerActor = GetComponent<ActorController>();

        servicesManager = FindObjectOfType<ServicesManager>();
        servicesManager.camera.transform.SetParent(cameraSlot, false);
        servicesManager.camera.transform.localRotation = Quaternion.identity;
        servicesManager.camera.transform.localPosition = Vector3.zero;
        gameStateManager = FindObjectOfType<GameStateManager>();

        playerActor.onDie += OnDie;
    }

    public void OnDie(object sender, EventArgs eventArgs)
    {
        gameStateManager.GameOver();
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
    }

    void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            playerActor.Move(new Vector3Int(0, 0));
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            playerActor.Move(new Vector3Int(1, 0));
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            playerActor.Move(new Vector3Int(-1, 0));
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            playerActor.Move(new Vector3Int(0, -1));
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            playerActor.Move(new Vector3Int(0, 1));
        }


        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            playerActor.Move(new Vector3Int(1, 1));
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            playerActor.Move(new Vector3Int(-1, 1));
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            playerActor.Move(new Vector3Int(-1, -1));
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            playerActor.Move(new Vector3Int(1, -1));
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            playerActor.GoDownStairs();
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("TEST");
            playerActor.Interact();
        }

        if (Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            if (InventoryMenu.activeInHierarchy)
            {
                InventoryMenu.SetActive(false);
            }
            else
            {
                InventoryMenu.SetActive(true);
            }
        }

    }

    public Vector3Int GetLocation()
    {
        return playerActor.gridPosition;
    }
}
