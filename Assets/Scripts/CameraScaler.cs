using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
  Board board;
  public float cameraOffset = -10;
  public float padding = 1;
  public int uiHeight = 80;
  void Start()
  {
    board = FindObjectOfType<Board>();
    AdjustCameraPosition();
  }

  void AdjustCameraPosition()
  {
    float uiHeightInWorld = Camera.main.ScreenToWorldPoint(new Vector3(0, uiHeight, 0)).y - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
    transform.position = new Vector3((board.width - 1) / 2f, (board.height - 1) / 2f + uiHeightInWorld / 2f, cameraOffset);
    if (board.width / (float)board.height > Camera.main.aspect)
    {
      Camera.main.orthographicSize = (board.width / 2 + padding) / Camera.main.aspect + uiHeightInWorld / 2f;
    }
    else
    {
      Camera.main.orthographicSize = board.height / 2 + padding + uiHeightInWorld / 2f;
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
