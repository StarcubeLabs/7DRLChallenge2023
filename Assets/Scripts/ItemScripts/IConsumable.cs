using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumable
{
    /// <summary>
    /// Handles what happens when you consume said item.
    /// </summary>
    void OnConsume();
}
