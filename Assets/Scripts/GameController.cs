using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text; 
    public Button button;      
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public string playerSide;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;
    public string computerSide;
    public bool playerMove;
    public float delay;
    public int value;

    private int moveCount;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        moveCount = 0;
        restartButton.SetActive(false);
        startInfo.SetActive(true);
        SetStartingSide("X");
        playerMove = true;
        StartGame();
    }

    void Update()
    {
        if (!playerMove)
        {
            int bestScore = int.MinValue;
            int score = 0;
            value = -1;
            for (int i = 0; i < buttonList.Length; i++)
            {   
                if (buttonList[i].text == "" && buttonList[i].GetComponentInParent<Button>().interactable)
                {   
                    buttonList[i].text = computerSide;
                    score = Minimax(buttonList, 0, false, int.MinValue, int.MaxValue);
                    buttonList[i].text = "";

                    if (score > bestScore)
                    {
                        bestScore = score;
                        value = i;
                    }
                }
            }
           
            if (value != -1)
            {
                buttonList[value].text = computerSide;
                buttonList[value].GetComponentInParent<Button>().interactable = false;
                EndTurn();
            }
        }
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            computerSide = "O";
            SetPlayerColor(playerX, playerO);
        }
        else
        {   
            computerSide = "X";
            SetPlayerColor(playerO, playerX);
        }
        StartGame();
    }

    void StartGame()
    {
        SetBoardInteraction(true);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public bool CheckWin(string players)
    {
        if (players == "player")
        {
            if ((buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide) ||
                (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide) ||
                (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide) ||
                (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide) ||
                (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide) ||
                (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide) ||
                (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide) ||
                (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide))
                return true;
        }
        else if (players == "computer")
        {
            if ((buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide) ||
                (buttonList[0].text == computerSide && buttonList[3].text == computerSide && buttonList[6].text == computerSide) ||
                (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide) ||
                (buttonList[1].text == computerSide && buttonList[4].text == computerSide && buttonList[7].text == computerSide) ||
                (buttonList[2].text == computerSide && buttonList[5].text == computerSide && buttonList[8].text == computerSide) ||
                (buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide) ||
                (buttonList[3].text == computerSide && buttonList[4].text == computerSide && buttonList[5].text == computerSide) ||
                (buttonList[6].text == computerSide && buttonList[7].text == computerSide && buttonList[8].text == computerSide))
                return true;
        }
        return false;
    }

    public void EndTurn()
    {
        moveCount++;
        delay = 10;
        
        if (CheckWin("player") == true)
        {
            GameOver("player");
        }
        else if (CheckWin("computer") == true)
        {
            GameOver("computer");
        }
        else if (moveCount == buttonList.Length)
        {
            GameOver("draw");
            SetPlayerColorInactive();
        }
        else
        {
            SwitchSides();
        }
    }

    void GameOver(string winner)
    {
        SetBoardInteraction(false);
        gameOverPanel.SetActive(true);
        if (winner == "player")
        {
            gameOverText.text = playerSide + " Wins";
        }
        else if (winner == "computer")
        {
            gameOverText.text = computerSide + " Wins";
        }
        else 
        {
            gameOverText.text = "Its a Draw";
        }
        restartButton.SetActive(true);
    }

    void SwitchSides()
    {
        playerMove = !playerMove;
        if (playerMove == true)
        {
            SetPlayerColor(playerX, playerO);
        }
        else 
        {
            SetPlayerColor(playerO, playerX);
        }
    }

    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
        restartButton.SetActive(false);
        playerMove = true;
        SetStartingSide("X");
    }

    void SetBoardInteraction(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerColor(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void SetPlayerColorInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    int Minimax(Text[] buttonList, int depth, bool isMaximizing, int alpha, int beta)
    {
        if (CheckWin("player") == true)
        {
            return -10 + depth;
        }
        else if (CheckWin("computer") == true)
        {
            return 10 - depth;
        }
        else if (IsBoardFull(buttonList))
        {
            return 0;
        }

        if (depth == 9) // increase depth to set difficulty 
        {
            return 0;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            int score = 0;
            for (int i = 0; i < 9; i++)
            {
                if (buttonList[i].text == "")
                {
                    buttonList[i].text = computerSide;
                    score = Minimax(buttonList, depth + 1, false, alpha, beta);
                    buttonList[i].text = "";
                    bestScore = (int)MathF.Max(score, bestScore);
                    alpha = (int)MathF.Max(alpha, bestScore);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }
            return bestScore;
        }
        else 
        {
            int bestScore = int.MaxValue;
            int score = 0;
            for (int i = 0; i < 9; i++)
            {
                if (buttonList[i].text == "")
                {
                    buttonList[i].text = playerSide;
                    score = Minimax(buttonList, depth + 1, true, alpha, beta);
                    buttonList[i].text = "";
                    bestScore = Mathf.Min(score, bestScore);
                    beta = Mathf.Min(beta, bestScore);
                    if (beta <= alpha) break;
                }
            }
            return bestScore;
        }
    }

    bool IsBoardFull(Text[] buttonList)
    {
        foreach (Text button in buttonList)
        {
            if (button.text == "")
                return false;
        }
        return true;
    }


    
}