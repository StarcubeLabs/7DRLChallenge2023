using UnityEngine;

public class MessageAnimation : TurnAnimation
{
    private string message;
    
    public MessageAnimation(string message)
    {
        this.message = message;
    }
    
    public override void StartAnimation()
    {
        Debug.Log(message);
    }

    public override bool UpdateAnimation()
    {
        return true;
    }
}
