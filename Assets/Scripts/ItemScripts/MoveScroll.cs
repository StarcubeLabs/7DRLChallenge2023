using System;
using UnityEngine;

public class MoveScroll : ItemData
{
    [SerializeField]
    private MoveData taughtMove;

    public override bool OnConsume(ActorController consumer, Item item)
    {
        bool successfullyTaught = false;
        if (consumer.moveToReplace)
        {
            if (consumer.moveToReplace != consumer.moveToBeTaught)
            {
                ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation(
                    $"{consumer.GetDisplayName()} forgot {consumer.moveToReplace.moveData.MoveName} and learned {taughtMove.MoveName}!"));
                consumer.ReplaceMove();
                successfullyTaught = true;
            }
        }
        else if (consumer.AddMove(consumer.moveToBeTaught))
        {
            ServicesManager.TurnAnimationController.AddAnimation(new MessageAnimation($"{consumer.GetDisplayName()} learned {taughtMove.MoveName}!"));
            successfullyTaught = true;
        }

        if (!successfullyTaught)
        {
            Destroy(consumer.moveToBeTaught.gameObject);
        }

        consumer.EndTeachMove();
        return successfullyTaught;
    }

    public void StartTeachMove(Item item, Action consumeAction)
    {
        Move move = item.Owner.StartTeachMove(taughtMove);
        if (item.Owner.IsMovesetFull)
        {
            ServicesManager.HudManager.ContextMenu.OpenTeachMoveMenu(move, consumeAction);
        }
        else
        {
            consumeAction.Invoke();
        }
    }
}
