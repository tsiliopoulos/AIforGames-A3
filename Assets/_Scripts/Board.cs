using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

/**
* Name: Tom Tsiliopoulos
* StudentID: 100616336
* Date: Tuesday February 25, 2020
*/


/// <summary>
/// This class represents the game board. It extends a List of type string. This is part of the memory optimization that I've included for this assignment.
[System.Serializable]
public class Board : List<string>
{
        // memory layout:
        //
        //                row no (=vertical)
        //               |  0   1   2
        //            ---+-------------
        //            0  | m00 m10 m20
        // column no  1  | m01 m11 m21
        // (=horiz)   2  | m02 m12 m22

    // public instance variables
    public string m00;
    public string m10;
    public string m20;

    public string m01;
    public string m11;
    public string m21;

    public string m02;
    public string m12;
    public string m22;


  // PUBLIC PROPERTIES / ACCESSORS

  // to allow [,] referencing
  public string this[int col, int row]
  {
      get
      {
          return this[col + row * 3];
      }

      set
      {
          this[col + row * 3] = value;
      }
  }

  // maps the list index to each position on the board
  public string this[int index]
  {
    get
    {
       switch (index)
      {
          case 0: return m00;
          case 1: return m10;
          case 2: return m20;
          case 3: return m01;
          case 4: return m11;
          case 5: return m21;
          case 6: return m02;
          case 7: return m12;
          case 8: return m22;
          default:
              throw new IndexOutOfRangeException("Invalid index!");
      }
    }

    set
    {
      switch (index)
      {
        case 0: m00 = value; break;
        case 1: m10 = value; break;
        case 2: m20 = value; break;
        case 3: m01 = value; break;
        case 4: m11 = value; break;
        case 5: m21 = value; break;
        case 6: m02 = value; break;
        case 7: m12 = value; break;
        case 8: m22 = value; break;
        default:
            throw new IndexOutOfRangeException("Invalid matrix index!");
      }
    }
  }



  // Get a column of the board.
  public string[] GetColumn(int index)
  {
      switch (index)
      {
          case 0: return new string[] {m00, m10, m20};
          case 1: return new string[] {m01, m11, m21};
          case 2: return new string[] {m02, m12, m22};
          default:
              throw new IndexOutOfRangeException("Invalid column index!");
      }
  }

  // Returns a row of the board.
  public string[] GetRow(int index)
  {
      switch (index)
      {
          case 0: return new string[] {m00, m01, m02};
          case 1: return new string[] {m10, m11, m12};
          case 2: return new string[] {m20, m21, m22};
          default:
              throw new IndexOutOfRangeException("Invalid row index!");
      }
  }

  // Sets a column of the board.
  public void SetColumn(int index, string[] column)
  {
      this[0, index] = column[0];
      this[1, index] = column[1];
      this[2, index] = column[2];
  }

  // Sets a row of the board.
  public void SetRow(int index, string[] row)
  {
      this[index, 0] = row[0];
      this[index, 1] = row[1];
      this[index, 2] = row[2];
  }

  // Use this to compare two boards
  public override bool Equals(object other)
  {
      if (!(other is Board)) return false;
      return Equals((Board)other);
  }

  public bool Equals(Board other)
  {
      return GetColumn(0).Equals(other.GetColumn(0))
          && GetColumn(1).Equals(other.GetColumn(1))
          && GetColumn(2).Equals(other.GetColumn(2));
  }

    // Constructor Functions
    public Board()
    {
      m_buildBoard();
    }

    public Board(string[] column0, string[] column1, string[] column2)
    {
        this.m00 = column0[0]; this.m01 = column1[0]; this.m02 = column2[0];
        this.m10 = column0[1]; this.m11 = column1[1]; this.m12 = column2[1];
        this.m20 = column0[2]; this.m21 = column1[2]; this.m22 = column2[2];
    }

    public void m_buildBoard()
    {
      SetRow(0, new string[]{"", "", ""});
      SetRow(1, new string[]{"", "", ""});
      SetRow(2, new string[]{"", "", ""});
    }

    // overloaded comparison operators
    public static bool operator==(Board lhs, Board rhs)
    {
        return lhs.GetColumn(0) == rhs.GetColumn(0)
            && lhs.GetColumn(1) == rhs.GetColumn(1)
            && lhs.GetColumn(2) == rhs.GetColumn(2);
    }

    public static bool operator!=(Board lhs, Board rhs)
    {
        return !(lhs == rhs);
    }

    // a copy function that enables cloning of another board's state
    public Board Copy()
    {
      var board = new Board(GetColumn(0), GetColumn(1), GetColumn(2));
      return board;

    }

    // allows user to print the current board
    public override string ToString()
    {
      return $"{m00} {m10} {m20}\n" + $"{m01} {m11} {m21}\n" + $"{m02} {m12} {m22}\n";
    }
}
