using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DungeonMap: Map
{
    public List<Rectangle> rooms;
    public Cell start;
    public Cell end;
}