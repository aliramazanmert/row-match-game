using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour
{
  public int column;
  public int row;
  Board board;
  UIManager uiManager;
  [SerializeField] Sprite matchedSprite;

  bool isBeingDragged = false;

  public bool isMatched = false;

  private Color32 greenColor = new Color32(40, 238, 1, 255);
  void Start()
  {
    board = FindObjectOfType<Board>();
    uiManager = FindObjectOfType<UIManager>();
    row = (int)transform.position.y;
    column = (int)transform.position.x;
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnMouseDown()
  {
    isBeingDragged = true;
  }

  private void OnMouseUp()
  {
    isBeingDragged = false;
  }

  private void OnMouseDrag()
  {
    handleSwapTiles();
  }

  void handleSwapTiles()
  {
    if (!isBeingDragged || isMatched)
    {
      return;
    }
    Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector3 currPosition = transform.position;
    if (column + 1 < board.width && inputPosition.x > currPosition.x + 0.5 && inputPosition.y < currPosition.y + 0.5 && inputPosition.y > currPosition.y - 0.5)
    {
      switchTiles(column + 1, row, false, -1);
    }
    else if (column > 0 && inputPosition.x < currPosition.x - 0.5 && inputPosition.y < currPosition.y + 0.5 && inputPosition.y > currPosition.y - 0.5)
    {
      switchTiles(column - 1, row, false, 1);
    }
    else if (row + 1 < board.height && inputPosition.y > currPosition.y + 0.5 && inputPosition.x < currPosition.x + 0.5 && inputPosition.x > currPosition.x - 0.5)
    {
      switchTiles(column, row + 1, true, -1);
    }
    else if (row > 0 && inputPosition.y < currPosition.y - 0.5 && inputPosition.x < currPosition.x + 0.5 && inputPosition.x > currPosition.x - 0.5)
    {
      switchTiles(column, row - 1, true, 1);
    }
  }
  void switchTiles(int otherTileCol, int otherTileRow, bool isVerticalSwipe, int otherTileDelta)
  {
    GameObject otherTile = board.tiles[otherTileCol, otherTileRow];
    board.tiles[column, row] = otherTile;
    board.tiles[otherTileCol, otherTileRow] = gameObject;
    if (isVerticalSwipe)
    {
      otherTile.GetComponent<Tile>().row += otherTileDelta;
      row -= otherTileDelta;
      iTween.MoveBy(gameObject, iTween.Hash("y", -otherTileDelta, "easeType", "easeInOutExpo", "time", .3f, "onComplete", "checkForMatchedRow", "onCompleteParams", row));
      iTween.MoveBy(otherTile, iTween.Hash("y", otherTileDelta, "easeType", "easeInOutExpo", "time", .3f, "onComplete", "checkForMatchedRow", "onCompleteParams", otherTileRow));
    }
    else
    {
      otherTile.GetComponent<Tile>().column += otherTileDelta;
      column -= otherTileDelta;
      iTween.MoveBy(gameObject, iTween.Hash("x", -otherTileDelta, "easeType", "easeInOutExpo", "time", .3f));
      iTween.MoveBy(otherTile, iTween.Hash("x", otherTileDelta, "easeType", "easeInOutExpo", "time", .3f));
    }
    isBeingDragged = false;
    uiManager.DecrementMovesLeft();
    if (uiManager.movesLeft == 0)
    {
      endLevel();
    }
  }

  void checkForMatchedRow(int row)
  {
    bool isAlreadyMatched = board.tiles[0, row].GetComponent<Tile>().isMatched;
    if (isAlreadyMatched) return;

    string firstTag = board.tiles[0, row].tag;
    bool rowMatched = true;
    for (int i = 0; i < board.width; i++)
    {
      if (firstTag != board.tiles[i, row].tag)
      {
        rowMatched = false;
        break;
      }
    }

    if (rowMatched)
    {
      for (int i = 0; i < board.width; i++)
      {
        board.tiles[i, row].GetComponent<SpriteRenderer>().sprite = matchedSprite;
        board.tiles[i, row].GetComponent<SpriteRenderer>().color = greenColor;
        board.tiles[i, row].GetComponent<Tile>().isMatched = true;
      }
      addScore(firstTag);
      checkIfThereIsPossibleRowMatches();
    }
  }

  void addScore(string tag)
  {
    int amount = 0;
    switch (tag)
    {
      case "Red":
        amount = 100;
        break;
      case "Green":
        amount = 150;
        break;
      case "Blue":
        amount = 200;
        break;
      case "Yellow":
        amount = 250;
        break;
      default:
        break;
    }
    uiManager.IncreaseScore(amount);
  }

  void checkIfThereIsPossibleRowMatches()
  {
    int r = 0;
    int g = 0;
    int b = 0;
    int y = 0;
    for (int i = 0; i < board.height; i++)
    {
      if (board.tiles[0, i].GetComponent<Tile>().isMatched)
      {
        r = 0;
        g = 0;
        b = 0;
        y = 0;
        continue;
      }
      for (int j = 0; j < board.width; j++)
      {
        switch (board.tiles[j, i].tag)
        {
          case "Red":
            r++;
            if (r == board.width) return;
            break;
          case "Green":
            g++;
            if (g == board.width) return;
            break;
          case "Blue":
            b++;
            if (b == board.width) return;
            break;
          case "Yellow":
            y++;
            if (y == board.width) return;
            break;
          default:
            break;
        }
      }
    }

    // no possible row matches
    endLevel();
  }

  void endLevel()
  {
    if (uiManager.score > CurrentLevel.highestScore)
    {
      CurrentLevel.highestScore = uiManager.score;
      PlayerPrefs.SetInt("level" + CurrentLevel.levelNumber + "_highscore", uiManager.score);
      SceneManager.LoadScene("Celebration");
      return;
    }
    SceneManager.LoadScene("LevelsPopup");
  }
}


