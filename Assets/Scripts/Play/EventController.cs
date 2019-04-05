using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


// 게임 시작, 끝, 난이도, 시간설정, 점수설정 등은 여기에 있음.

public class EventController : MonoBehaviour {

    // 난이도요소
    public int Difficulty;
    public float SolveTime;
    private int currentGame;
    private int currentMode;

    // 게임 요소
    public int MovementStatus; // 이동모드 상태(전체모드 0, 회전모드 1, 이동모드 2)
    public GameController GC;
    public int isPlay;

    // UI요소
    public GameObject GameOverWindow, HintWindow, TotitleButton, RankingButton, RestartButton, ChallengeButton, NextStageButton, GameOverBack, ClearBack;
    public Text GameResultText;

    // 점수
    public int Score;
    public Text ScoreText;

    // 현재 시각
    public float current_Time;
    public Text TimeText;


    // 목숨
    public GameObject[] LifeOn = new GameObject[3];
    public GameObject[] LifeOff = new GameObject[3];
    public int Lifes;

    // 주의 : 힌트가 아니라 재시작 횟수이다!
    public GameObject[] HintOn = new GameObject[3];
    public GameObject[] HintOff = new GameObject[3];
    public int Hints;


private void Awake()
    {

        // 초기화
        currentMode = PlayerPrefs.GetInt("Mode");
        
        Lifes = 3;
        Hints = 3;
        Score = 0;
        MovementStatus = 0;
        SolveTime = 14231512; // 그냥 변수초기화 값이고 추후에 난이도에 맞게 설정한다.

        GameManager();
        
    }

    // Update function for every timeframe
    public void Update()
    {
        // check for game end ( different for different levels/modes ) comes here.



        // check for time end
        if (current_Time <= 0)
        {
            LostLife();
            Renew();
        }

        // TODO : 이부분 gamemanager에 구현
        if (isPlay == 1)
        {
            isPlay = 2;
            //StartTime();
        }

    }


    private void GameManager()
    {
        // TODO : 어느 게임인지 모드 및 난이도를 확인하고 생성까지 여기서 한다. private 변수 활용
        if (currentMode == 0)
        {
            // challenge mode
            // generate random game
            currentGame = (int)Math.Floor(UnityEngine.Random.Range(0f, 7f));

            // 점수에 따라서 제한시간을 여기에서 바꿀 수 있다.
            TimeManager();
            StartCoroutine("Timer");

            GC.makeNew(currentGame);


        }
        else
        {
            // TODO : story mode
            isPlay = 0;
        }
    }

    private void TimeManager()
    {
        // TODO : any logic regarding difficulty and time, bonuses come here
        SolveTime = 60;
    }


    public void Renew()
    {
        Debug.Log("Renew");
        current_Time = SolveTime;
        //Generate();
        GC.makeNew(0);
    }

    // 문제를 해결한 경우 점수를 얻는다
    public void AddScore()
    {

        Score += 14;
        ScoreText.text = Score.ToString() + " 점";

        // 난이도 증가
        Difficulty++;
        // SolveTime = SolveTime - 10;

        Renew();

    }




    /*
     * 
     * 
     *  UTIL FUNCTIONS
     * 
     * 
     */

    IEnumerator Timer() // 0.01초 단위로 시간을 측정
    {
        yield return new WaitForSeconds(0.01f);
        current_Time -= 0.01f;
        TimeText.text = current_Time.ToString("##0.00") + " sec";
        StartCoroutine("Timer");
    }

    public void LostLife() // Life를 잃는 것을 처리해주는 함수, 적이 몸에 닿을 시 실행
    {
        //foreach (Transform Enemy in EnemyPar.transform) Enemy.GetComponent<EnemyBehaviour>().PushBack();
        //combo = 0;
        //GetComponent<AudioManager>().DamagedSound();
        switch (Lifes)
        {
            case 3:  // 3개면 2개로
                LifeOn[2].SetActive(false);
                LifeOff[2].SetActive(true);
                Lifes--;
                break;
            case 2: // 2개면 1개로
                LifeOn[1].SetActive(false);
                LifeOff[1].SetActive(true);
                Lifes--;
                break;
            case 1: // 1개면 게임오버
                LifeOn[0].SetActive(false);
                LifeOff[0].SetActive(true);
                Lifes--;
                GameOver(false);
                //GetComponent<AudioManager>().GameOverSound();
                break;
        }
    }

    public void LostHelp() // Help를 잃는 것을 처리해주는 함수
    {
        //foreach (Transform Enemy in EnemyPar.transform) Enemy.GetComponent<EnemyBehaviour>().PushBack();
        //combo = 0;
        //GetComponent<AudioManager>().DamagedSound();
        if (Hints == 0) return;
        //Debug.Log(Hints);
        Renew();
        //Debug.Log("Hints after");
        switch (Hints)
        {
            case 3:  // 3개면 2개로
                HintOn[2].SetActive(false);
                HintOff[2].SetActive(true);
                
                Hints--;
                break;
            case 2: // 2개면 1개로
                HintOn[1].SetActive(false);
                HintOff[1].SetActive(true);
                Hints--;
                break;
            case 1: // 1개면 게임오버
                HintOn[0].SetActive(false);
                HintOff[0].SetActive(true);
                Hints--;
                //GameOver(false);
                //GetComponent<AudioManager>().GameOverSound();
                break;
        }
        //Debug.Log("HH " + Hints);
    }

    
    public void GameOver(bool isCleared) // GameOver 시 해야될 일을 해주는 함수
    {                                    // isCleared면 Clear, 아니면 GameOver
        StopCoroutine("Timer");
        GameOverWindow.SetActive(true);
        //ClearBack.SetActive(true);
        //GameOverBack.SetActive(true);
        /*
        if(PlayerPrefs.GetInt("Mode") != 4){
            isCleared = 0;
         }
         */
        if (isCleared)
            ClearBack.SetActive(true);
        else
            GameOverBack.SetActive(true);

        if (PlayerPrefs.GetInt("Mode") == 4)
        {
            // 순위전 화면구성
            RestartButton.SetActive(true);
            RankingButton.SetActive(true);

            
        }
        else
        {
            // 스토리모드 화면구성
            NextStageButton.SetActive(true);
            ChallengeButton.SetActive(true);
        }
    }

    public void Movemode()
    {
        MovementStatus++;
        if (MovementStatus == 3) MovementStatus = 0;
    }


}
