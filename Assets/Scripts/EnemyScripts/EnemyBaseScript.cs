using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    public ActorController enemyActor;
    public PlayerInputController player;

    // Start is called before the first frame update
    void Start()
    {
        enemyActor = this.gameObject.GetComponent<ActorController>();
        player = GameObject.FindObjectOfType<PlayerInputController>();
    }

    // Update is called once per frame
    void Update()
    {
        //print("current position: " + enemyActor.gridPosition.ToString());
        //enemyActor.MoveToward(enemyActor.gridPosition, player.GetLocation());
        enemyActor.MoveDiagonal(new Vector3Int(Random.Range(-1, 2), Random.Range(-1, 2), 0));
    }
}
