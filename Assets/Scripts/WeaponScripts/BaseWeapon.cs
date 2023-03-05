using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLDataTypes;

public class BaseWeapon : MonoBehaviour
{

    /// <summary>
    /// The weapon's Element Type.
    /// </summary>
    public ElementType weaponElement = ElementType.Neutral;

    /// <summary>
    /// The base number that gets added to attack strength when the weapon is used.
    /// </summary>
    public int WeaponStrength = 1;

    /// <summary>
    /// The Status Type that the weapon has a chance of afflicting.
    /// </summary>
    public StatusType AfflictionType = StatusType.None;

    /// <summary>
    /// Percentage Chance of Target being afflicted with the selected StatusType, from 0 to 1 (or 0% to 100%).
    /// </summary>
    [Range(0f, 1f)]
    public float AfflictionChance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
