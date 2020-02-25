using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/**
* Name: Tom Tsiliopoulos
* StudentID: 100616336
* Date: Tuesday February 25, 2020
*/

[System.Serializable]
public class GameBoard : MonoBehaviour
{
   public GameController gameController;

  [Header("GameBoard Buttons")]
  #region buttons
  public Button topLeft;
  public Button topCenter;
  public Button topRight;

  public Button centerLeft;

  public Button centerCenter;

  public Button centerRight;

  public Button bottomLeft;

  public Button bottomCenter;

  public Button bottomRight;
  #endregion

  [Header("GameBoard Button Array")]
  public Button[,] buttons;
  public Board board;

  private void Start()
  {
    buttons = new Button[3,3] { { topLeft, centerLeft, bottomLeft }, { topCenter, centerCenter, bottomCenter }, { topRight, centerRight, bottomRight} };
    board = new Board();
  }

  public void playerMove(Button button)
  {
     var label = button.GetComponentInChildren<Text>();
    // check if cell is empty
    if(label.text != "X" && label.text != "O")
    {
      label.text = "X";
      for (int col = 0; col < 3; col++)
      {
          for (int row = 0; row < 3; row++)
          {
              if(button == buttons[col, row])
              {
                Debug.Log("Player selected (" + col + ", " + row + ") - " + button.name);
                board[col, row] = "X";
                gameController.currentNode.board[col, row] = "X";
                gameController.MoveDownTree(new Vector2Int(col, row), "X");
              }
          }
      }
      CheckState();
    }
  }

  public void AIAction()
  {
    // choose the min minimax value
    Node minNode = null;
    var min = int.MaxValue;
    foreach (var node in gameController.currentNode.children)
    {
        if(node.minimaxValue < min)
        {
          min = node.minimaxValue;
          minNode = node;
        }

    }
   Debug.Log("Min is: " + min);
    if(minNode != null)
    {
      var col = minNode.action[0];
      var row = minNode.action[1];

      var label = buttons[col, row].GetComponentInChildren<Text>();
      label.text = "O";
      gameController.currentNode.board[col, row] = "O";
      // perform the AI move now
      Debug.Log("AI moving to (" + col + ", " + row + ") - " + buttons[col,row].name);
      gameController.MoveDownTree(new Vector2Int(col, row), "O");

      CheckState();
    }
    else
    {
        Debug.Log("Error node is null");
        return;
    }
  }

    public void CheckState()
    {
      if(gameController.checkWinState("O", gameController.currentNode.board))
      {
        Debug.Log("Computer Wins!!");
      }
      else if(gameController.checkWinState("X", gameController.currentNode.board))
      {
        Debug.Log("Player Wins!!");
      }
      else if(gameController.checkDrawState(gameController.currentNode.board))
      {
        Debug.Log("It's a Draw!");
      }
    }

  public void OnButton_Click(Button button)
  {
   playerMove(button);
   AIAction();
  }

}
