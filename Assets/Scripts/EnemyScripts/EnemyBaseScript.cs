using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_STATES
{
    WANDERING,
    PURSUING,
    ATTACKING,
    IDLE,
}

public class EnemyBaseScript : MonoBehaviour
{
    public ActorController enemyActor;
    public PlayerInputController player;
    public TurnManager turnManager;
    public AI_STATES state;

    // Start is called before the first frame update
    void Start()
    {
        enemyActor = this.gameObject.GetComponent<ActorController>();
        player = GameObject.FindObjectOfType<PlayerInputController>();
        turnManager = FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != AI_STATES.PURSUING && state != AI_STATES.ATTACKING && enemyActor.CanSeePosition(player.GetLocation()))
        {
            state = AI_STATES.PURSUING;
        }

        switch (state)
        {
            case AI_STATES.WANDERING:
                enemyActor.Move(new Vector3Int(Random.Range(-1, 2), Random.Range(-1, 2), 0));
                break;
            case AI_STATES.PURSUING:
                enemyActor.MoveToward(enemyActor.gridPosition, player.GetLocation());
                break;
            default:
                break;
        }

        if (turnManager.CanMove(enemyActor))
        {
            turnManager.KickToBackOfTurnOrder(enemyActor);
        }
    }
}
