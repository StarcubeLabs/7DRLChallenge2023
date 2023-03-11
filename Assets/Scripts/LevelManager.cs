using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    public List<FloorData> FloorData;
    [HideInInspector]
    public List<LevelController> levels = new List<LevelController>();
    public LevelController levelPrefab;
    public int randomSeed;

    Grid grid;

    public EventHandler<EventArgs> onLevelChange;

    private void Awake()
    {
        FindGrid();
    }

    public void Start()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        InitializeLevel();
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        grid = null;
        FindGrid();
        InitializeLevel();
    }

    void FindGrid()
    {
        if (!grid)
        {
            grid = FindObjectOfType<Grid>();
        }
    }

    void InitializeLevel()
    {
        if (grid)
        {
            for (int i = 0; i < FloorData.Count; i++)
            {
                LevelController level = Instantiate(levelPrefab);
                level.CurrentFloorData = FloorData[i];
                level.transform.SetParent(grid.transform, false);
                level.Initialize(randomSeed);
                levels.Add(level);
                level.gameObject.SetActive(false);
                if (i == 0)
                {
                    level.SetupPlayer();
                    EnterLevel(level);
                }
            }
        }
    }

    void EnterLevel(LevelController level)
    {
        level.gameObject.SetActive(true);
        level.LoadEntities();
    }

    void LeaveLevel(LevelController level)
    {
        level.SaveEntities();
        level.gameObject.SetActive(false);
    }

    public LevelController GetActiveLevel()
    {
        if (grid == null)
        {
            return null;
        }
        return grid.GetComponentInChildren<LevelController>();
    }

    public void GoToFloor(int offset)
    {
        LevelController activeLevel = grid.GetComponentInChildren<LevelController>();
        ServicesManager.TurnAnimationController.ClearAnimations();
        int nextFloorNum = activeLevel.transform.GetSiblingIndex() + offset;
        if (nextFloorNum >= FloorData.Count)
        {
            ServicesManager.GameStateManager.WinGame();
        }
        else
        {
            LevelController nextFloor = grid.transform.GetChild(nextFloorNum).GetComponent<LevelController>();
            ActorController player = FindObjectOfType<PlayerInputController>().playerActor;
            activeLevel.RemoveEntityFromLevel(player);
            EnterLevel(nextFloor);
            LeaveLevel(activeLevel);
            nextFloor.AddEntityToLevel(player);
            if (offset > 0)
            {
                player.gridPosition = nextFloor.GetGridPositionFromCell(nextFloor.somewhatInterestingMap.start);
            }
            else
            {
                player.gridPosition = nextFloor.GetGridPositionFromCell(nextFloor.somewhatInterestingMap.end);
            }

            if (onLevelChange != null)
            {
                onLevelChange(this, EventArgs.Empty);
            }
        }
    }

    public void GoDownFloor()
    {
        this.GoToFloor(1);
    }

    public void GoUpFloor()
    {
        this.GoToFloor(1);
    }
}
