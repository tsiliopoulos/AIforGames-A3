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
public class GameController : MonoBehaviour
{
  public Node currentNode;

  public int count;

    void Start()
    {
      // Create the initial node - an empty board
      currentNode = new Node();
      currentNode.board = GetComponent<GameBoard>().board;
      currentNode.player = "X";

      // create Decision Tree
      CreateDecisionTree(currentNode, "X");
      var leafNodes = new List<Node>();
      GetLeafNodes(currentNode, leafNodes);
      Debug.Log("Number of Leaf Nodes " + leafNodes.Count + " terminal nodes");
      Debug.Log("Total Nodes " + count + " nodes");
    }

 // This function is recursive and creates the decision tree
  public void CreateDecisionTree(Node root, string player)
  {
    count++;

    for (int col = 0; col < 3; col++)
    {
      for (int row = 0; row < 3; row++)
      {
        var value = root.board[col, row];
        if(value != "X" && value != "O" && !checkEndState(root.board))
        {
          var clonedBoard = root.board.Copy();
          clonedBoard[col, row] = player;

          var newNode = new Node();
          newNode.board = clonedBoard;
          newNode.parentNode = root;
          root.children.Add(newNode);

          newNode.action = new Vector2Int(col, row);
          Debug.Log(newNode.action.ToString());

          if(count < 1000)
          {
            if(player == "X")
            {
              newNode.player = "O";
              CreateDecisionTree(newNode, "O");
            }
            else if(player == "O")
            {
              newNode.player = "X";
              CreateDecisionTree(newNode, "X");
            }
          }
        }
      }
    }

    // if leaf node then compute value - bottom up approach
    if(root.children.Count == 0)
    {
      var value = ComputeMoveValue(root.board);
      root.minimaxValue = value;
    }
    else
    {
      if(root.player == "X")
      {
        // the maximizing player
        var max = int.MinValue;

        foreach (var child in root.children)
        {
            if(child.minimaxValue > max)
            {
              max = child.minimaxValue;
            }
        }

        root.minimaxValue = max;
      }
      else
      {
        // the minimizing player
        var min = int.MaxValue;

        foreach (var child in root.children)
        {
            if(child.minimaxValue < min)
            {
              min = child.minimaxValue;
            }
        }

        root.minimaxValue = min;
      }
    }
  }

#region moveDownTree
  // this function moves down the Decision tree every move
 public void MoveDownTree(Vector2Int action, string player)
  {
    foreach (var node in currentNode.children)
    {
      Debug.Log("Player: " + player + "Action: ("+action[0]+", "+action[1]+")");

        if(node.action[0] == action[0] && node.action[1] == action[1])
        {
          currentNode = node;
          Debug.Log("Moving Down the tree");
          return;
        }
    }
  }
#endregion

#region computeMoveValue
// computes the minimax value of each move
// +1 for "X" win   -1 for "O" win and 0 for a draw
public int ComputeMoveValue(Board board)
{
  var value = 0;
  if(checkWinState("X", board))
  {
    // "X" win
    value = 1;
  }
  else if(checkWinState("O", board))
  {
    // "O" win
    value = -1;
  }
  else
  {
    // draw
    value = 0;
  }

  return value;
}
#endregion

#region checkEndState
    // utility function to check if win for either player or draw
    private bool checkEndState(Board board)
    {
        return checkWinState("X", board) || checkWinState("O", board) || checkDrawState(board);
    }
#endregion

#region checkWinState
    // check if a player wins across various configurations - horizontally - vertically or diagonally
    public bool checkWinState(string player, Board board)
    {
      // check rows
      for (int col = 0; col < 3; col++)
      {
          var rowCount = 0;
          for (int row = 0; row < 3; row++)
          {
            var value = board[col, row];
            if(value == player)
            {
              rowCount += 1;
            }

            if(rowCount == 3)
            {
              //Debug.Log(player + " Wins across");
              return true;
            }

          }
      }

      // check columns
      for (int col = 0; col < 3; col++)
      {
          var colCount = 0;
          for (int row = 0; row < 3; row++)
          {
            var value = board[row, col];
            if(value == player)
            {
              colCount += 1;
            }

            if(colCount == 3)
            {
              //Debug.Log(player + " Wins down");
              return true;
            }

          }
      }

      // check diagonal top left to bottom right
      if((board[0, 0] == player) &&
         (board[1, 1] == player) &&
         (board[2, 2] == player))
      {
        //Debug.Log(player + "Wins Diagonally - top left - bottom right");
        return true;
      }

      // check diagonal top right to bottom left
      if((board[2, 0] == player) &&
         (board[1, 1] == player) &&
         (board[0, 2] == player))
      {
        //Debug.Log(player + " Wins Diagonally - top right - bottom left");
        return true;
      }

      // player does not win
      return false;
    }
#endregion

#region checkDrawState
  // check if end state is a draw
  public bool checkDrawState(Board board)
  {
    var selectedSquares = 0;
    for (int col = 0; col < 3; col++)
    {
        for (int row = 0; row < 3; row++)
        {
          var value = board[col, row];
          if(value == "X" || value == "O")
          {
            selectedSquares += 1;
          }
        }
    }
    //Debug.Log("selected squares: " + selectedSquares);

    // if all squares have been selected
    if(selectedSquares == 9)
    {
      // ..and neither X nor O has won
      if(!checkWinState("X", board) && !checkWinState("O", board))
      {
        // it's a draw
        //Debug.Log("It's a draw");
        return true;
      }
    }

    return false;
  }
#endregion

#region GetLeafNodes
    // creates a List of leaf nodes - this will be inspected for minimax value
    public void GetLeafNodes(Node root, List<Node> leafNodes)
    {
        if (root.children.Count == 0)
        {
            leafNodes.Add(root);
        }
        else
        {
            foreach (var node in root.children)
            {
                GetLeafNodes(node, leafNodes);
            }
        }
    }
#endregion


#region otherLifeCycleFunctions
    void Update()
    {

    }
#endregion

}
