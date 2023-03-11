using System.Collections.Generic;

public class MoveMenuUseMove : MoveMenuMode
{
    public MoveMenuUseMove(MoveMenu moveMenu) : base(moveMenu)
    {
    }

    public override List<Move> GetMoves()
    {
        return moveMenu.PlayerActorController.moves;
    }

    public override bool IsMoveSelectable(Move move)
    {
        return moveMenu.PlayerActorController.IsMoveUsable(move);
    }

    public override bool ConfirmChooseMove(Move move)
    {
        return true;
    }

    public override void SelectMove(Move move)
    {
        moveMenu.PlayerActorController.UseMove(move);
    }

    public override void OnCancel()
    {
        ServicesManager.HudManager.ContextMenu.OnCancelUseMove();
    }
}
