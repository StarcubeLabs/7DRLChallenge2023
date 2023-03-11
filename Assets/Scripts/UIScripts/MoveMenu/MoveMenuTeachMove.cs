using System;
using System.Collections.Generic;

public class MoveMenuTeachMove : MoveMenuMode
{
    private Move newMove;
    private Action consumeAction;
    private Move chosenMove;
    
    public MoveMenuTeachMove(MoveMenu moveMenu, Move newMove, Action consumeAction) : base(moveMenu)
    {
        this.newMove = newMove;
        this.consumeAction = consumeAction;
    }

    public override List<Move> GetMoves()
    {
        List<Move> moves = new List<Move>(moveMenu.PlayerActorController.moves);
        moves.Add(newMove);
        return moves;
    }

    public override bool IsMoveSelectable(Move move)
    {
        return true;
    }

    public override bool ConfirmChooseMove(Move move)
    {
        return move != newMove;
    }

    public override void SelectMove(Move move)
    {
        moveMenu.PlayerActorController.moveToReplace = move;
        chosenMove = move;
    }

    public override void OnCancel()
    {
        ServicesManager.HudManager.ContextMenu.OnCancelTeachMove();
    }

    public override void OnClose()
    {
        consumeAction.Invoke();
    }
}
