using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameplayManager : MonoBehaviour
{
    public int score { get; private set; }

    public int pelletCount { get; private set; }

    public static GameplayManager instance;
    public Text playerMessage;
    public Text playerScore;

    public UnityEvent resetGame;
    public UnityEvent gameOver;

    public bool gameFinished { get; private set; } = false;

    public void CollectPellet()
    {
        ++score;
        playerScore.text = "Score: " + score.ToString();
        if (score == pelletCount)
        {
            playerMessage.gameObject.SetActive(true);
            playerMessage.text = "You Win !!!";
            gameFinished = true;
        }
    }

    public void CreatePellet()
    {
        ++pelletCount;
    }

    public void PlayerHit()
    {
        playerMessage.gameObject.SetActive(true);
        playerMessage.text = "Game Over !";
        gameFinished = true;
        gameOver.Invoke();
    }

    public void Reset()
    {
        gameFinished = false;
        score = 0;
        playerScore.text = "Score: "+score;
        playerMessage.gameObject.SetActive(false);
    }

    void Awake()
    {
        pelletCount = 0;
        Reset();
        instance = this;

        resetGame = new UnityEvent();
        resetGame.AddListener(Reset);

        gameOver = new UnityEvent();
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetGame.Invoke();
        }
    }
}
