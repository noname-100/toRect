using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


// 게임 시작, 끝, 난이도, 시간설정, 점수설정 등은 여기에 있음.

public class EventController : MonoBehaviour {

    // 난이도요소
    private int combo;
    private float solveTime;
    private float bonusTimeLimit;
    private int currentGame;
    private int currentMode;

    // 게임 요소
    public int movementStatus; // 이동모드 상태(전체모드 0, 회전모드 1, 이동모드 2)
    public GameObject GC;
    private GameController gc;
    public int isPlay; // 0 : 게임 정지 1 : 게임 시작 시그널 2 : 게임 실행중

    // UI요소
    public GameObject GameOverWindow, TotitleButton, RankingButton, RestartButton, ChallengeButton, NextStageButton, GameOverBack, ClearBack;
    public Text GameResultText;

    // Clear 화면 게임요소
    public GameObject RankingMain, RankingSub1, RankingSub2, GameOverBackground, Chapter1ClearBackground, Chapter2ClearBackground, Chapter3ClearBackground;

    // 점수
    private int score;
    public Text ScoreText;

    // 현재 시각
    private float current_Time;
    public Text TimeText;

    // 목숨
    public GameObject[] LifeOn = new GameObject[3];
    public GameObject[] LifeOff = new GameObject[3];
    private int lifes;

    // 주의 : 힌트가 아니라 재시작 횟수이다!
    public GameObject[] HintOn = new GameObject[3];
    public GameObject[] HintOff = new GameObject[3];
    private int hints;


private void Awake()
    {

        // 초기화
        currentMode = PlayerPrefs.GetInt("Mode");
        gc = GC.GetComponent<GameController>();       
        
        hints = 3;
        score = 0;
        combo = 0;
        movementStatus = 0;
        solveTime = 45; // 임의 변수초기화 값

        // TODO : 어느 게임인지 모드 및 난이도를 확인하고 생성까지 여기서 한다. private 변수 활용
        if (currentMode == 0)
        {
            // challenge mode
            lifes = 3;
            isPlay = 2;
            MakeNewGame();
            ResetTimeManager();
            StartCoroutine("Timer");
            gc.makeNew(currentGame);
        }
        else
        {
            // TODO : story mode
            lifes = 1;
            LifeOn[0].SetActive(false);
            LifeOn[1].SetActive(false);
            LifeOn[2].SetActive(false);
            LifeOff[0].SetActive(false);
            LifeOff[1].SetActive(false);
            LifeOff[2].SetActive(false);
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
            current_Time = 0;
            StopCoroutine("Timer");
            GameOver(false);
        }

        // 시간종료
        if (current_Time < 0)
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

        // 스토리모드 게임해결시 시간 멈추고 다음 단계로 넘어가는 부분

        // 게임승리
        Debug.Log("issolved " + gc.isSolved());
        if (gc.isSolved() == 1 || isHelp == 2)
        {
            combo++;
            if (current_Time >= bonusTimeLimit) BonusGift();
            AddPointManager();
            ResetTimeManager();
            MakeNewGame();
        }
    }

    private void BonusGift()
    {
        combo += 2;
        score += 5;
    }

    private void AddPointManager()
    {
        //if (currentMode == 0) return;

        // 게임종류
        if(currentGame == 0)
        {
            score += 10;
        }
        else if(currentGame == 1)
        {
            score += 13;
        }
        else if(currentGame == 2)
        {
            score += 16;
        }
        else if(currentGame == 3)
        {
            score += 20;
        }
        else if(currentGame == 4)
        {
            score += 24;

        }
        else if(currentGame == 5)
        {
            score += 16;
        }
        else
        {
            score += 10;
        }

        // 총시간, 콤보로 추가점수
        score += (int) Math.Floor(0.3 * (60 - solveTime) + combo * 3.5d);
        float norm = 0.4f;
        score = (int) Math.Floor(norm * score);

        ScoreText.text = score.ToString() + " 점";

    }

    private void ResetTimeManager()
    {

        float timeBonus = UnityEngine.Random.Range(0f, 2f);

        if(currentMode == 0)
        {
            // 이부분이랑 콤보 풀리는 부분 조절하는게 게임성 핵심이다
            // 콤보 잃어서 제한시간이 너무 늘어지면 안된다
            // 점수나 콤보때문에 0초가 되면 안된다. 초반에 선형적이되 0으로 수렴하는 함수로
            solveTime = 5; // (float) Math.Floor(30 - 2 * combo - 0.05 * score);
            // 보너스 타임에만 랜덤요소를 넣는다.
            bonusTimeLimit = (float) Math.Floor(solveTime * 0.85 - timeBonus);
        }
        else if(currentMode == 1)
        {   // Biscuit Story Mode
            bonusTimeLimit = 45; // not used
            if (currentGame == 0) solveTime = 60;
            else if (currentGame == 1) solveTime = 50;
            else if (currentGame == 2) solveTime = 40;
            else if (currentGame == 3) solveTime = 45;
            else solveTime = 45;
        }else if(currentMode == 2)
        {   // Rec2Square Story Mode
            solveTime = 60;
        }else if(currentMode == 3)
        {   // Similarity Story Mode
            solveTime = 60;
        }

        current_Time = solveTime;
        
    }

    private void MakeNewGame()
    {
        // generate random game, difficulty - game generation logic comes here
        currentGame = (int)Math.Floor(UnityEngine.Random.Range(0f, 7f));
        gc.makeNew(currentGame);
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
        //if (isPlay == 0) yield return null;
        yield return new WaitForSeconds(0.01f);
        current_Time -= 0.01f;
        TimeText.text = current_Time.ToString("##0.00") + " sec";
        StartCoroutine("Timer");
    }

    public void LostLife() // Life를 잃는 것을 처리해주는 함수, 적이 몸에 닿을 시 실행
    {
        //foreach (Transform Enemy in EnemyPar.transform) Enemy.GetComponent<EnemyBehaviour>().PushBack();
        combo = 0;
        //GetComponent<AudioManager>().DamagedSound();
        // 교체하기
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
    }

    public void LostHelp() // 교체하기 버튼 누르면 반응하는 함수
    {
        if (hints == 0) return;

        ResetTimeManager();
        MakeNewGame();

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
            case 1: 
                HintOn[0].SetActive(false);
                HintOff[0].SetActive(true);
                hints--;
                //GetComponent<AudioManager>().GameOverSound();
                break;
        }
    }

    
    public void GameOver(bool isCleared) // GameOver 시 모달 레이아웃 함수
    {                                    // isCleared면 Clear, 아니면 GameOver
        //StopCoroutine("Timer");
        GameOverWindow.SetActive(true);

        GameOverBackground.SetActive(false);
        Chapter1ClearBackground.SetActive(false);
        Chapter2ClearBackground.SetActive(false);
        Chapter3ClearBackground.SetActive(false);

        switch (currentMode)
        {
            case 0:
                GameOverBackground.SetActive(true);
                GameOverBack.SetActive(true);
                break;
            case 1:
                Chapter1ClearBackground.SetActive(true);
                break;
            case 2:
                Chapter2ClearBackground.SetActive(true);
                break;
            case 3:
                Chapter3ClearBackground.SetActive(true);
                break;
        }



        if (currentMode == 0)
        {
            // 순위전 버튼구성
            RankingMain.SetActive(true);
            RankingSub1.SetActive(true);
            RankingSub2.SetActive(true);
            RestartButton.SetActive(true);
            RankingButton.SetActive(true);            
        }
        else
        {
            // 스토리모드 버튼구성
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
        gc.makeNew(0);
    }

    // 문제를 해결한 경우 점수를 얻는다
    public void AddScore()
    {
        Renew();
    }

}

