using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    public List<LevelController> levels = new List<LevelController>();
    public LevelController levelPrefab;
    public int numMapsToGenerate = 10;

    Grid grid;

    private void Awake()
    {
        Initialize();
    }

    public void Start()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        grid = null;
        Initialize();
    }

    void Initialize()
    {
        if (!grid)
        {
            grid = FindObjectOfType<Grid>();
            if (grid)
            {
                for (int i = 0; i < numMapsToGenerate; i++)
                {
                    LevelController level = Instantiate(levelPrefab);
                    level.transform.SetParent(grid.transform, false);
                    level.Initialize();
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
        return grid.GetComponentInChildren<LevelController>();
    }

    public void GoToFloor(int offset)
    {
        LevelController activeLevel = grid.GetComponentInChildren<LevelController>();
        LevelController nextFloor = grid.transform.GetChild(activeLevel.transform.GetSiblingIndex() + offset).GetComponent<LevelController>();
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
