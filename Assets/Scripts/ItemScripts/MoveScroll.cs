using UnityEngine;

public class MoveScroll : ItemData
{
    [SerializeField]
    private MoveData taughtMove;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        return consumer.AddMove(taughtMove);
    }
}
