using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveMenuSelection: MonoBehaviour
{

    public TextMeshProUGUI name;
    public TextMeshProUGUI ppCount;

    public void UpdateWithMoveData(Move move)
    {
        name.text = move.moveData.MoveName;
        ppCount.text = string.Format("{0}/{1}", move.pp, move.moveData.MaxPP);
    }
}
