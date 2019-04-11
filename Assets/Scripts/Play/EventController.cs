using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


// 게임 시작, 끝, 난이도, 시간설정, 점수설정 등은 여기에 있음.

public class EventController : MonoBehaviour {

    // 난이도요소
    public int combo;
    public float solveTime;
    public float bonusTimeLimit;
    private int currentGame;
    private int currentMode;

    // 게임 요소
    public int movementStatus; // 이동모드 상태(전체모드 0, 회전모드 1, 이동모드 2)
    public GameController GC;
    public int isPlay; // 0 : 게임 정지 1 : 게임 시작 시그널 2 : 게임 실행중

    // UI요소
    public GameObject GameOverWindow, HintWindow, TotitleButton, RankingButton, RestartButton, ChallengeButton, NextStageButton, GameOverBack, ClearBack;
    public Text GameResultText;

    // 점수
    public int score;
    public Text ScoreText;

    // 현재 시각
    private float current_Time;
    public Text TimeText;

    // 목숨
    public GameObject[] LifeOn = new GameObject[3];
    public GameObject[] LifeOff = new GameObject[3];
    public int lifes;

    // 주의 : 힌트가 아니라 재시작 횟수이다!
    public GameObject[] HintOn = new GameObject[3];
    public GameObject[] HintOff = new GameObject[3];
    public int hints;


private void Awake()
    {

        // 초기화
        currentMode = PlayerPrefs.GetInt("Mode");
        Debug.Log("curr " + currentMode);
        lifes = 3;
        hints = 3;
        score = 0;
        movementStatus = 0;
        solveTime = 45; // 임의 변수초기화 값

        // TODO : 어느 게임인지 모드 및 난이도를 확인하고 생성까지 여기서 한다. private 변수 활용
        if (currentMode == 0)
        {
            // challenge mode
            MakeNewGame();
            ResetTimeManager();
            StartCoroutine("Timer");
            GC.makeNew(currentGame);
        }
        else
        {
            // TODO : story mode
            isPlay = 0;
        }

    }

    // Update function for every timeframe
    public void Update()
    {
        // check for game end ( different for different levels/modes ) comes here.
        if(isPlay != 0) GameManager(0);

    }

    public void GameManager(int isHelp)
    {
        // 목숨이 없는 경우
        if (lifes == 0) {
            StopCoroutine("Timer");
            GameOver(false);
        }

        // 교체하기
        if(isHelp == 1)
        {
            LostHelp();
            ResetTimeManager();
            MakeNewGame();
        }

        // 시간종료
        if (current_Time <= 0)
        {
            LostLife();
            ResetTimeManager();
            MakeNewGame();
        }

        // 게임시작
        if (isPlay == 1)
        {
            isPlay = 2;
            ResetTimeManager();
            MakeNewGame();
            StartCoroutine("Timer");
        }

        // 게임승리
        if (GC.isSolved() == 1 || isHelp == 2)
        {
            AddPointManager();
            ResetTimeManager();
            MakeNewGame();
        }
    }

    private void AddPointManager()
    {        
        score += 14;
        ScoreText.text = score.ToString() + " 점";
    }

    private void ResetTimeManager()
    {
        // TODO : any logic regarding difficulty and time, bonuses come here
        float timeBonus = UnityEngine.Random.Range(0f, 2f);

        if(currentMode == 0)
        {
            bonusTimeLimit = 30;
            solveTime = 5;
        }
        else
        {
            bonusTimeLimit = 45;
            solveTime = 5;
        }
        
    }

    private void MakeNewGame()
    {
        // generate random game, difficulty - game generation logic comes here
        currentGame = (int)Math.Floor(UnityEngine.Random.Range(0f, 7f));
        GC.makeNew(currentGame);
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
        if (isPlay == 0) yield return null;
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
        switch (lifes)
        {
            case 3:  // 3개면 2개로
                LifeOn[2].SetActive(false);
                LifeOff[2].SetActive(true);
                lifes--;
                break;
            case 2: // 2개면 1개로
                LifeOn[1].SetActive(false);
                LifeOff[1].SetActive(true);
                lifes--;
                break;
            case 1: // 1개면 게임오버
                LifeOn[0].SetActive(false);
                LifeOff[0].SetActive(true);
                lifes--;
                //GameOver(false);
                //GetComponent<AudioManager>().GameOverSound();
                break;
        }
        GameManager(0);
    }

    public void LostHelp() // 교체하기 버튼 누르면 반응하는 함수
    {
        if (hints == 0) return;
        GameManager(1);
        switch (hints)
        {
            case 3:  // 3개면 2개로
                HintOn[2].SetActive(false);
                HintOff[2].SetActive(true);                
                hints--;
                break;
            case 2: // 2개면 1개로
                HintOn[1].SetActive(false);
                HintOff[1].SetActive(true);
                hints--;
                break;
            case 1: // 1개면 게임오버
                HintOn[0].SetActive(false);
                HintOff[0].SetActive(true);
                hints--;
                //GameOver(false);
                //GetComponent<AudioManager>().GameOverSound();
                break;
        }
    }

    
    public void GameOver(bool isCleared) // GameOver 시 모달 레이아웃 함수
    {                                    // isCleared면 Clear, 아니면 GameOver
        //StopCoroutine("Timer");
        GameOverWindow.SetActive(true);

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
        movementStatus++;
        if (movementStatus == 3) movementStatus = 0;
    }

    // legacy code(will delete)

    public void Renew()
    {
        Debug.Log("Renew");
        current_Time = solveTime;
        //Generate();
        GC.makeNew(0);
    }

    // 문제를 해결한 경우 점수를 얻는다
    public void AddScore()
    {

        Renew();

    }

}

