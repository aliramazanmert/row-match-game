﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyButton : MonoBehaviour
{
  private void OnMouseDown()
  {
    SceneManager.LoadScene("LevelsPopup");
  }
}
