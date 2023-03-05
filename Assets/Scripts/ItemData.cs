using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemIcon;
    public long ItemValue;
    public bool IsStackable;
    public int MaximumStack;
}
