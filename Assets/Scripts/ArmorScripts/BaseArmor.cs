using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RLDataTypes;

public class BaseArmor : MonoBehaviour
{
    /// <summary>
    /// The element that determines what attacks against the user are Not Effective and Super Effective
    /// </summary>
    public ElementType armorElement = ElementType.Neutral;

    /// <summary>
    /// The number that reduces how much damage is deal to the user.
    /// </summary>
    public int ArmorStrength = 1;

    /// <summary>
    /// The Status Type that this armor blocks. Choosing (All) makes it so it checks for all types.
    /// </summary>
    public StatusType StatusBlockType = StatusType.None;

    /// <summary>
    /// Precentage Chance of blocking the targeted Status Type, as a range from 0 to 1 (0% to 100%)l
    /// </summary>
    [Range(0f, 1f)]
    public float ProtectionChance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
