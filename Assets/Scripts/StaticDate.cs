using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticDate
{
    public static int CountMoves;
    public static int CountCollect;
    public static TypeBlock TypeBlock = TypeBlock.StarDark;
    public static int CurrentLevel = 0;
    public static List<int> LevelOpen = new List<int>() { 0 };
    public static bool isNextLevel = false;
}
