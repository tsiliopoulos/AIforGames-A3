using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
* Name: Tom Tsiliopoulos
* StudentID: 100616336
* Date: Tuesday February 25, 2020
*/

[System.Serializable]
/// <summary>
/// This class represents each node in the Decision Tree
/// </summary>
public class Node
{
  public Board board;
  public Node parentNode;
  public List<Node> children;
  public int minimaxValue;
  public string player;

  // Using a Vector 2 to capture each move in the tree
  public Vector2Int action = new Vector2Int();

  public Node()
  {
    board = new Board();
    children = new List<Node>();
  }
}
