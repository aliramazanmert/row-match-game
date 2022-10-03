using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentLevel
{
  public static string[,] grid;
  public static int gridWidth;
  public static int gridHeight;
  public static int moveCount;
  public static int levelNumber;
  public static int highestScore;

  public static void SetLevelData(string[,] grid, int gridWidth, int gridHeight, int moveCount, int levelNumber, int highestScore)
  {
    CurrentLevel.grid = grid;
    CurrentLevel.gridWidth = gridWidth;
    CurrentLevel.gridHeight = gridHeight;
    CurrentLevel.moveCount = moveCount;
    CurrentLevel.levelNumber = levelNumber;
    CurrentLevel.highestScore = highestScore;
  }
}
