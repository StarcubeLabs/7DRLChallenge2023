using UnityEngine;

public class MoveScroll : ItemData
{
    [SerializeField]
    private MoveData taughtMove;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        if (consumer.AddMove(taughtMove))
        {
            ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} learned {taughtMove.MoveName}!"));
            return true;
        }

        return false;
    }
}
