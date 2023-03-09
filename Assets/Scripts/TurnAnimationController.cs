using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAnimationController : MonoBehaviour
{
    private List<TurnAnimation> runningAnimations = new List<TurnAnimation>();
    private Queue<TurnAnimation> queuedAnimations = new Queue<TurnAnimation>();
    
    public bool HasRunningAnimations { get { return runningAnimations.Count > 0 || queuedAnimations.Count > 0; }}

    void Update()
    {
        if (runningAnimations.Count > 0)
        {
            List<TurnAnimation> finishedAnimations = new List<TurnAnimation>();
            foreach (TurnAnimation anim in runningAnimations)
            {
                if (anim.UpdateAnimation())
                {
                    finishedAnimations.Add(anim);
                }
            }
            if (finishedAnimations.Count == runningAnimations.Count)
            {
                runningAnimations.Clear();
            }
            else
            {
                finishedAnimations.ForEach(anim => runningAnimations.Remove(anim));
            }
        }
        while (queuedAnimations.Count > 0)
        {
            TurnAnimation nextAnim = queuedAnimations.Peek();
            bool runAnimation = runningAnimations.Count == 0 ? true : runningAnimations[0].CanRunAnimationsConcurrently(nextAnim);
            if (runAnimation)
            {
                queuedAnimations.Dequeue();
                runningAnimations.Add(nextAnim);
                nextAnim.StartAnimation();
            }
            else
            {
                break;
            }
        }
    }

    public void AddAnimation(TurnAnimation anim)
    {
        queuedAnimations.Enqueue(anim);
    }
}
