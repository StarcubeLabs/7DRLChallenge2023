using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicesManager : MonoBehaviour
{
    public static ServicesManager instance;
    private TurnAnimationController turnAnimationController;
    public static TurnAnimationController TurnAnimationController { get { return instance.turnAnimationController; } }
    
    private EntityManager entityManager;
    public static EntityManager EntityManager { get { return instance.entityManager; } }
    
    private LevelManager levelManager;
    public static LevelManager LevelManager { get { return instance.levelManager; } }
    
    private MoveRegistry moveRegistry;
    public static MoveRegistry MoveRegistry { get { return instance.moveRegistry; } }

    private HudManager hudManager;
    public static HudManager HudManager { get { return instance.hudManager; } }
    

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
            turnAnimationController = GetComponentInChildren<TurnAnimationController>();
            entityManager = GetComponentInChildren<EntityManager>();
            levelManager = GetComponentInChildren<LevelManager>();
            moveRegistry = GetComponentInChildren<MoveRegistry>();
            hudManager = FindObjectOfType<HudManager>();
        }
    }

    public void ResetServices()
    {
        mainCamera.transform.SetParent(this.transform);
        turnAnimationController.ClearAnimations();
    }
}
