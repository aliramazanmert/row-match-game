using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
  [SerializeField] GameObject backgroundTile;
  [SerializeField] GameObject blueTile;
  [SerializeField] GameObject redTile;
  [SerializeField] GameObject greenTile;
  [SerializeField] GameObject yellowTile;
  public int width;
  public int height;
  public string[,] grid;
  public GameObject[,] tiles;

  private Dictionary<string, GameObject> STR_TO_TILE;

  private void Awake()
  {
    STR_TO_TILE = new Dictionary<string, GameObject>{
      {"y", yellowTile},
      {"g", greenTile},
      {"r", redTile},
      {"b", blueTile},
    };

    width = CurrentLevel.gridWidth;
    height = CurrentLevel.gridHeight;
    grid = CurrentLevel.grid;
    tiles = new GameObject[width, height];
  }
  void Start()
  {
    for (int i = 0; i < width; i++)
    {
      for (int j = 0; j < height; j++)
      {
        Vector2 pos = new Vector2(i, j);
        GameObject backgroundTileObject = Instantiate(backgroundTile, pos, Quaternion.identity);
        backgroundTileObject.transform.SetParent(transform);
        backgroundTileObject.name = "(" + i + "," + j + ")";

        GameObject newTile = Instantiate(STR_TO_TILE[grid[i, j]], pos, Quaternion.identity);
        newTile.transform.SetParent(transform);
        newTile.name = "(" + i + "," + j + ")";

        tiles[i, j] = newTile;
      }
    }

  }
}
