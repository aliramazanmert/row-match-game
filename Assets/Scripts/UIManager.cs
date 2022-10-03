using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
  public TextMeshProUGUI scoreText;
  public TextMeshProUGUI movesLeftText;
  public TextMeshProUGUI highestScoreText;
  public int score;
  public int movesLeft;


  // Start is called before the first frame update
  void Start()
  {
    movesLeft = CurrentLevel.moveCount;
    highestScoreText.text = CurrentLevel.highestScore.ToString();
    score = 0;
  }

  // Update is called once per frame
  void Update()
  {
    movesLeftText.text = movesLeft.ToString();
    scoreText.text = score.ToString();
  }

  public void DecrementMovesLeft()
  {
    movesLeft--;
  }

  public void IncreaseScore(int amount)
  {
    score += amount;
  }
}
