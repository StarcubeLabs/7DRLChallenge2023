using RLDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveMenuDetail: MonoBehaviour
{

    public TextMeshProUGUI name;
    public TextMeshProUGUI details;
    public TextMeshProUGUI attackType;
    public TextMeshProUGUI element;
    public TextMeshProUGUI power;
    public TextMeshProUGUI ppCount;

    public void UpdateWithMoveData(Move move)
    {
        name.text = move.moveData.MoveName;
        details.text = move.moveData.MoveDescription;
        attackType.text = "Attack Type: Direct";
        element.text = string.Format("Element: {0}", Enum.GetName(typeof(ElementType), move.moveData.Element));
        string powerFlavor = "Pathetic!";
        if (move.moveData.Power >= 10)
        {
            powerFlavor = "Gigaton Powernuke";
        }
        else if (move.moveData.Power >= 6)
        {
            powerFlavor = "Ultra Strong";
        }
        else if (move.moveData.Power >= 4)
        {
            powerFlavor = "Strong";
        }
        else if (move.moveData.Power >= 2)
        {
            powerFlavor = "Respectable";
        }
        else
        {
            powerFlavor = "Weak";
        }
        power.text = string.Format("Attack Power: {0}", powerFlavor);
        ppCount.text = string.Format("{0}/{1}", move.pp, move.moveData.MaxPP);
    }
}
