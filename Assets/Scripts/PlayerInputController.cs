using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public ActorController playerActor;
    public Transform cameraSlot;
    ContextMenu contextMenu;
    ServicesManager servicesManager;
    GameStateManager gameStateManager;
    private TurnManager turnManager;
    private TurnAnimationController turnAnimationController;

    // Start is called before the first frame update
    void Start()
    {
        playerActor = GetComponent<ActorController>();

        servicesManager = FindObjectOfType<ServicesManager>();
        servicesManager.mainCamera.transform.SetParent(cameraSlot, false);
        servicesManager.mainCamera.transform.localRotation = Quaternion.identity;
        servicesManager.mainCamera.transform.localPosition = Vector3.zero;
        gameStateManager = FindObjectOfType<GameStateManager>();
        turnManager = FindObjectOfType<TurnManager>();
        turnAnimationController = FindObjectOfType<TurnAnimationController>();

        contextMenu = FindObjectOfType<ContextMenu>(true);

        playerActor.onDie += OnDie;
    }

    public void OnDie(object sender, EventArgs eventArgs)
    {
        if (!playerActor.Inventory.PreventDeath(playerActor))
        {
            gameStateManager.GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnManager.CanMove(playerActor) || turnAnimationController.HasRunningAnimations)
        {
            return;
        }

        if (contextMenu.IsMenuOpen())
        {
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                contextMenu.NavigateDown();
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                contextMenu.NavigateUp();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                contextMenu.NavigateBack();
            }
        }
        else
        {
            InputCheckNumpad();
            InputCheckWASD();
        }
    }

    void InputCheckNumpad()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            PlayerWait();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            PlayerMoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            PlayerMoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlayerMoveDown();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            PlayerMoveUp();
        }


        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            PlayerMoveUpRight();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            PlayerMoveUpLeft();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            PlayerMoveDownLeft();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlayerMoveDownRight();
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            playerActor.GoDownStairs();
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            playerActor.Interact();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            contextMenu.OpenMenu();
        }
    }

    void InputCheckWASD()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // Apparently to get a combo of keys to work you have to do GetKey1, GetKey2, ..., GetFinalKeyDown
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.D)
                || Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.W))
            {
                PlayerMoveUpRight();
            }
            if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.A)
                || Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.W))
            {
                PlayerMoveUpLeft();
            }
            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.A)
                || Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.S))
            {
                PlayerMoveDownLeft();
            }
            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.D)
                || Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.S))
            {
                PlayerMoveDownRight();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerWait();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerMoveRight();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerMoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerMoveDown();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerMoveUp();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerActor.GoDownStairs();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerActor.Interact();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerActor.UseBasicAttack();
            //playerActor.UseMove(playerActor.moves[0]);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            contextMenu.OpenMenu();
        }
    }

    public Vector3Int GetLocation()
    {
        return playerActor.gridPosition;
    }

    private void PlayerWait()
    {
        playerActor.Walk(new Vector3Int(0, 0));
    }

    private void PlayerMoveRight()
    {
        playerActor.Walk(new Vector3Int(1, 0));
    }

    private void PlayerMoveLeft()
    {
        playerActor.Walk(new Vector3Int(-1, 0));
    }

    private void PlayerMoveDown()
    {
        playerActor.Walk(new Vector3Int(0, -1));
    }

    private void PlayerMoveUp()
    {
        playerActor.Walk(new Vector3Int(0, 1));
    }

    private void PlayerMoveUpRight()
    {
        playerActor.Walk(new Vector3Int(1, 1));
    }

    private void PlayerMoveUpLeft()
    {
        playerActor.Walk(new Vector3Int(-1, 1));
    }

    private void PlayerMoveDownLeft()
    {
        playerActor.Walk(new Vector3Int(-1, -1));
    }

    private void PlayerMoveDownRight()
    {
        playerActor.Walk(new Vector3Int(1, -1));
    }
}
