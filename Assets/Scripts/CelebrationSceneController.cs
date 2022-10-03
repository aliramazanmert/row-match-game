using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public static class UnlockNextLevel
{
  public static int levelToUnlock;
  public static bool shouldUnlock;
}
public class CelebrationSceneController : MonoBehaviour
{
  public GameObject star;
  public TextMeshProUGUI highestScoreText;
  // Start is called before the first frame update
  void Start()
  {
    iTween.ScaleBy(star, iTween.Hash("amount", new Vector3(2, 2, 0), "time", 1f, "easeType", "easeInOutBack"));
    highestScoreText.text = CurrentLevel.highestScore.ToString();
    StartCoroutine(ExecuteAfterTime(5));
  }

  IEnumerator ExecuteAfterTime(float time)
  {
    yield return new WaitForSeconds(time);
    UnlockNextLevel.shouldUnlock = true;
    UnlockNextLevel.levelToUnlock = CurrentLevel.levelNumber + 1;
    SceneManager.LoadScene("LevelsPopup");
  }
}
