using System.Collections.Generic;

public abstract class MoveMenuMode
{
    protected MoveMenu moveMenu;
    
    public MoveMenuMode(MoveMenu moveMenu)
    {
        this.moveMenu = moveMenu;
    }

    public abstract List<Move> GetMoves();

    public abstract bool IsMoveSelectable(Move move);

    public abstract void SelectMove(Move move);

    public abstract bool ConfirmChooseMove(Move move);

    public abstract void OnCancel();

    public virtual void OnClose()
    {
    }
}
