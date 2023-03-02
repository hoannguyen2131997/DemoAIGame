using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  
    string selected;
    public static GameManager INSTANCE;
    public GameObject PrefabPlayer;
    public UISelectedMove PrefabUISelectedMove;
    public Transform tfGui;
    private bool isShowUI;
    private UISelectedMove uiSelectedMove;
    private GameObject player1;
    private GameObject player2;
    public int NumberOfObstacles;
    public Vector3 StartPos;
    public Vector3 EndPos;
    private bool turnPlayer1;
    private bool turnPlayer2;
    
    
    // State
    private bool CheckWin;

    public enum GameMode
    {
        PlayerVsPlayer, 
        PlayerVsAI,
        AiVsAi
    }
    [SerializeField] private GameMode gameMode;
    private void Start()
    {
        turnPlayer1 = true;
        turnPlayer2 = false;
        CreateUISelectedMove();
        GeneratePlayerAndEnemy();
        BoardManager.INSTANCE.GenerateBoard();
        GameplaySelection();
        
        GetOnClick();
        RenderBoard();
    }
    
    private void GameplaySelection()
    {
        GameMode value = gameMode;
        switch (value)
        {
            case GameMode.PlayerVsPlayer:
            {
                Debug.Log("player vs player");
            } break;
            case GameMode.PlayerVsAI:
            {
                Debug.Log("player vs ai");
            } break;
            case GameMode.AiVsAi:
            {
                Debug.Log("ai vs ai");
            } break;
        }
    }

    private void RenderBoard()
    {
        for (int i = 0; i < BoardManager.ListObstacles.Count; i++)
        {
            //Debug.Log($"obstacle {i} : {BoardManager.ListObstacles[i]}");
            int x = BoardManager.ListPositionsBoard[i].Item1;
            int y = BoardManager.ListPositionsBoard[i].Item2;
            switch (BoardManager.ListObstacles[i])
            {
                case 0:
                {
                    BoardManager.INSTANCE.HighLine(x,y, "white");
                } break;
                case 1:
                {
                    BoardManager.INSTANCE.HighLine(x,y, "gray");
                } break;
                case 2:
                {
                    BoardManager.INSTANCE.HighLine(x,y, "blue");
                } break;
                case 3:
                {
                    BoardManager.INSTANCE.HighLine(x,y, "red");
                } break;
            }
        }
    }

    private void ReadBoard()
    {
        int player1 = 0;
        int player2 = 0;
        int emptyCell = 0;
        int obstracle = 0;
        for (int i = 0; i < BoardManager.ListObstacles.Count; i++)
        {
            switch (BoardManager.ListObstacles[i])
            {
                case 0:
                {
                    emptyCell++;
                } break;
                case 1:
                {
                    obstracle++;
                } break;
                case 2:
                {
                    player1++;
                } break;
                case 3:
                {
                    player2++;
                } break;
            }
        }
        
        Debug.Log("####################################");
        Debug.Log($"player1: {player1}");
        Debug.Log($"player2: {player2}");
        Debug.Log($"emptyCell: {emptyCell}");
        Debug.Log($"obstracle: {obstracle}");
        Debug.Log("####################################");
    }

    void GetOnClick()
    {
        uiSelectedMove = Instantiate(PrefabUISelectedMove, tfGui);
        uiSelectedMove.BtnUp.onClick.AddListener(() => TaskOnClick(0));
        uiSelectedMove.BtnDown.onClick.AddListener(() => TaskOnClick(1));
        uiSelectedMove.BtnLeft.onClick.AddListener(() => TaskOnClick(2));
        uiSelectedMove.BtnRight.onClick.AddListener(() => TaskOnClick(3));
    }
    
    void TaskOnClick(int dir){
        Vector2 up = new Vector2(0, 1);
        Vector2 down = new Vector2(0, -1);
        Vector2 left = new Vector2(-1, 0);
        Vector2 right = new Vector2(1, 0);

        switch (dir)
        {
            case 0:
            {
                MovePlayer(up);
            } break;
            case 1:
            {
                MovePlayer(down);
            } break;
            case 2:
            {
                MovePlayer(left);
            } break;
            case 3:
            {
                MovePlayer(right);
            } break;
        }
    }

    void MovePlayer(Vector2 temp)
    {
        GameObject tempObj;
        String color;
        if ( turnPlayer1 == true)
        {
            tempObj = player1;
            color = "blue";
            turnPlayer2 = true;
            turnPlayer1 = false;
        }
        else
        {
            tempObj = player2;
            color = "red";
            turnPlayer1 = true;
            turnPlayer2 = false;
        }
        
        int xCheck = (int)tempObj.transform.position.x + (int)temp.x;
        int yCheck = (int)tempObj.transform.position.y + (int)temp.y;
        int i = BoardManager.INSTANCE.GetIDFromXY(xCheck, yCheck);
        
        bool checkBoard = BoardManager.INSTANCE.IsOnBoard((xCheck,yCheck)) && !BoardManager.FindObstacle(xCheck, yCheck);
        bool checkEmmpty = BoardManager.FindEmpty(i);
        
        if (checkBoard && checkEmmpty)
        {
            tempObj.transform.position += new Vector3(temp.x, temp.y, 0);
            BoardManager.INSTANCE.HighLine((int)tempObj.transform.position.x, (int)tempObj.transform.position.y, color);
            int id = BoardManager.INSTANCE.GetIDFromXY(xCheck, yCheck);

            if (color == "red")
            {
                BoardManager.ListObstacles[id] = 3;    
            }
            else
            {
                BoardManager.ListObstacles[id] = 2;
            }
            
            ReadBoard();
        }
        //Debug.Log("(x,y) : " + player1.transform.position.x + ":" + player1.transform.position.y);
    }

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

    private void CreateUISelectedMove()
    {
        uiSelectedMove = Instantiate(PrefabUISelectedMove, tfGui);
        uiSelectedMove.SetOnOff(true);
    }
    
    private void GeneratePlayerAndEnemy()
    {
        player1 = Instantiate(PrefabPlayer);
        player1.transform.position = StartPos;  // 0,8
        
        player2 = Instantiate(PrefabPlayer);
        player2.transform.position = EndPos;    // 8,0
    }
}