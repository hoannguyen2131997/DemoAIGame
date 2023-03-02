using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public static BoardManager INSTANCE;
    public int Width;
    public int Height;
    public int CellSpace;
    public Transform ParentCell;
    //[SerializeField] private List<CellManager> listCell;
    
    [SerializeField]private int CellWidth;
    [SerializeField]private int CellHeght;
    public Transform parentBoardTranform;
    [SerializeField] private GameObject prefabCell;
    
    public static List<GameObject> Cells = new();
    public static List<(int,int)> ListPositionsBoard = new();
    public static List<int> ListObstacles = new();
    public static List<(int,int)> ListPositionsObstacle = new();

    public static List<(int, int)> ListCellFrontLine = new();
    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateBoard()
    {
        CreateObstacles(GameManager.INSTANCE.NumberOfObstacles);

        int totalCell = Width * Height;
        
        for (int i = 0; i < totalCell; i++)
        {
            var cellObj = Instantiate(prefabCell);
            var (x, y) = getXYFromID(i, Width);
            cellObj.transform.parent = parentBoardTranform;
            cellObj.name = "Cell " + $"({x} : {y})";
            cellObj.transform.position = new Vector3(x * CellWidth, y * CellHeght, 0);
            cellObj.tag = "cell";
            Cells.Add(cellObj);
            ListPositionsBoard.Add((x,y));
            
            int isObstacles = 0;
            if (FindObstacle(x, y))
            {
                isObstacles = 1;
            } else if (x == GameManager.INSTANCE.StartPos.x && y == GameManager.INSTANCE.StartPos.y)
            {
                isObstacles = 2;
            } else if (x == GameManager.INSTANCE.EndPos.x && y == GameManager.INSTANCE.EndPos.y)
            {
                isObstacles = 3;
            }

            ListObstacles.Add(isObstacles);
        }
    }

    public void CreateObstacles(int NumberOfObstacles)
    {
        for (int i = 0; i < NumberOfObstacles; i++)
        {
            (int x, int y) = RandomPosition(0, 0, Width - 1, (int)Math.Floor(((float)Height/2) - 1));
            ListPositionsObstacle.Add((x,y));
            int xSymmetry = Width - 1 - x;
            int ySymmetry = Height - 1 - y;
            ListPositionsObstacle.Add((xSymmetry,ySymmetry));
        }
    }

    private (int,int) RandomPosition(int startX, int startY, int endX, int endY)
    {
       int x = Random.Range(startX, endX+1);
       int y = Random.Range(startY, endY+1);
       bool checkObstacle = FindObstacle(x,y);
       bool checkPlayer1 = x == GameManager.INSTANCE.StartPos.x && y == GameManager.INSTANCE.StartPos.y;
       bool checkPlayer2 = x == GameManager.INSTANCE.EndPos.x && y == GameManager.INSTANCE.EndPos.y;
       if (checkObstacle || checkPlayer1 || checkPlayer2)
       {
           x = Random.Range(startX, endX+1);
           y = Random.Range(startY, endY+1);
       }
       
       return (x, y);
    }

    public static bool FindObstacle(int x, int y)
         {
             for (int i = 0; i < ListPositionsObstacle.Count; i++)
             {
                 if (x == ListPositionsObstacle[i].Item1 && y == ListPositionsObstacle[i].Item2)
                 {
                     return true;
                 }
             }

             return false;
         }
    
    public static bool FindEmpty(int id)
    {
        if (ListObstacles[id] == 0)
        {
            return true;
        }

        return false;
    }

    public void HighLinePlayerA(int x, int y)
    {
        int idCell = GetIDFromXY(x, y);
        for (int i = 0; i < Cells.Count; i++)
        {
            if (i == idCell)
            {
                Cells[i].GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    public void HighLineCheck(int x, int y)
    {
        int idCell = GetIDFromXY(x, y);
        for (int i = 0; i < Cells.Count; i++)
        {
            if (i == idCell)
            {
                Cells[i].GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }
    public void HighLine(int x, int y, string color)
    {
        int idCell = GetIDFromXY(x, y);
        if (color == "blue")
        {
            HighLinePlayerA(x, y);
        }
        if (color == "red")
        {
            HighLinePlayerB(x, y);
        }
        if (color == "gray")
        {
            HighLineObstacle(x, y);
        }

        if (color == "white")
        {
            HighLineEmpty(x, y);
        }

        if (color == "yello")
        {
            HighLineCheck(x,y);
        }
    }
    
    public void HighLineEmpty(int x, int y)
    {
        int idCell = GetIDFromXY(x, y);
        for (int i = 0; i < Cells.Count; i++)
        {
            if (i == idCell)
            {
                Cells[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    
    public void HighLinePlayerB(int x, int y)
    {
        int idCell = GetIDFromXY(x, y);
        for (int i = 0; i < Cells.Count; i++)
        {
            if (i == idCell)
            {
                Cells[i].GetComponent<SpriteRenderer>().color = Color.red;
                // Cells[i].GetComponent<Transform>().transform.position = new Vector3(x, y,0);
            }
        }
    }
    
    public void HighLineObstacle(int x, int y)
    {
        int idCell = GetIDFromXY(x, y);
        for (int i = 0; i < Cells.Count; i++)
        {
            if (i == idCell)
            {
                Cells[i].GetComponent<SpriteRenderer>().color = Color.gray;
                // Cells[i].GetComponent<Transform>().transform.position = new Vector3(x, y,0);
            }
        }
    }

    public (int, int) getXYFromID(int id, int width)
    {
        int x = id % width;
        int y = (int)Mathf.Floor((id / width));
        return (x,y);
    }

    public int GetIDFromXY(int x, int y)
    {
        int id = Width * y + x;
        return id;
    }

    public bool IsOnBoard((int, int) newXY)
    {
        if ((newXY.Item1 <= Width-1 && newXY.Item1 >= 0) && (newXY.Item2 <= Height-1 && newXY.Item2 >= 0) )
        {
            return true;
        }

        return false;
    }
}