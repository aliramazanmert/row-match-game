using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsListManager : MonoBehaviour
{
  [SerializeField] GameObject levelMenuItem;
  void Start()
  {
    SetupLevelItems();
    UnlockLevel();
  }

  private void SetupLevelItems()
  {
    for (int i = 1; i <= 10; i++)
    {
      StreamReader reader = new StreamReader("Assets/Levels/RM_A" + i);
      int levelNumber = int.Parse(reader.ReadLine().Split(':')[1]);
      int gridWidth = int.Parse(reader.ReadLine().Split(':')[1]);
      int gridHeight = int.Parse(reader.ReadLine().Split(':')[1]);
      int moveCount = int.Parse(reader.ReadLine().Split(':')[1]);
      string[] colors = reader.ReadLine().Split(':')[1].Trim().Split(',');
      int highestScore = PlayerPrefs.GetInt("level" + levelNumber + "_highscore", 0);
      bool isLocked = PlayerPrefs.GetInt("level" + levelNumber + "_isLocked", 1) == 1;
      if (levelNumber == 1)
      {
        isLocked = false;
      }

      string[,] grid = new string[gridWidth, gridHeight];

      for (int j = 0; j < gridWidth; j++)
      {
        for (int k = 0; k < gridHeight; k++)
        {
          grid[j, k] = colors[j * gridHeight + k];
        }
      }

      reader.Close();

      GameObject item = Instantiate(levelMenuItem);

      TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
      texts[0].text = "Level " + levelNumber + " - " + moveCount + " Moves";
      texts[1].text = "Highest Score: " + highestScore;

      Button playButton = item.GetComponentInChildren<Button>();

      if (isLocked)
      {
        playButton.interactable = false;
        TextMeshProUGUI playText = playButton.GetComponentInChildren<TextMeshProUGUI>();
        Image lockIcon = playButton.GetComponentsInChildren<Image>(true)[1];
        playText.gameObject.SetActive(false);
        lockIcon.gameObject.SetActive(true);
      }

      playButton.onClick.AddListener(() =>
      {
        CurrentLevel.SetLevelData(grid, gridWidth, gridHeight, moveCount, levelNumber, highestScore);
        SceneManager.LoadScene("Game");
      });
      item.transform.SetParent(this.transform, false);
    }
  }

  private void UnlockLevel()
  {
    if (UnlockNextLevel.shouldUnlock)
    {
      Button playButtonToUnlock = this.GetComponentsInChildren<Button>()[UnlockNextLevel.levelToUnlock - 1];
      TextMeshProUGUI playText = playButtonToUnlock.GetComponentInChildren<TextMeshProUGUI>();
      Image lockIcon = playButtonToUnlock.GetComponentsInChildren<Image>(true)[1];
      iTween.ScaleBy(lockIcon.gameObject, iTween.Hash("amount", new Vector3(0, 0, 0), "time", 1.5f, "easeType", "easeInExpo", "onComplete", "onUnlockAnimationComplete", "oncompletetarget", this.gameObject));

      UnlockNextLevel.shouldUnlock = false;
    }
  }

  void onUnlockAnimationComplete()
  {
    Button playButtonToUnlock = this.GetComponentsInChildren<Button>()[UnlockNextLevel.levelToUnlock - 1];
    TextMeshProUGUI playText = playButtonToUnlock.GetComponentInChildren<TextMeshProUGUI>(true);
    Image lockIcon = playButtonToUnlock.GetComponentsInChildren<Image>(true)[1];
    lockIcon.gameObject.SetActive(false);
    playText.gameObject.SetActive(true);
    playButtonToUnlock.interactable = true;

    PlayerPrefs.SetInt("level" + UnlockNextLevel.levelToUnlock + "_isLocked", 0);
  }
}
