using System.Collections.Generic;
using RLDataTypes;

public class StatusAnimation : TurnAnimation
{
    private StatusIcon statusIcon;
    private List<StatusType> statuses;
    
    public StatusAnimation(StatusIcon statusIcon, List<StatusType> statuses)
    {
        this.statusIcon = statusIcon;
        this.statuses = new List<StatusType>(statuses);
    }
    
    public override void StartAnimation()
    {
        statusIcon.UpdateStatuses(statuses);
    }

    public override bool UpdateAnimation()
    {
        return true;
    }
}
