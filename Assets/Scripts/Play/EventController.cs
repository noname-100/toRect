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
    public GameObject EC;
    private StoryScript ss;

    // UI요소
    public GameObject GameOverWindow, TotitleButton, RankingButton, RestartButton, ChallengeButton, ChallengeButtonforSimilarity, NextStageButton, GameOverBack, ClearBack;
    public Text GameResultText;
    public GameObject RectangleBiscuitBackground, Rec2SquareBackground, SimilarityBackground, ScoreBackground;

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
        ss = EC.GetComponent<StoryScript>();
        
        hints = 3;
        score = 0;
        combo = 0;
        movementStatus = 0;
        solveTime = 45; // 임의 변수초기화 값

        // 배경화면 초기화
        ClearBackground();

        if (currentMode == 0 && currentMode ==1)
        {
            RectangleBiscuitBackground.SetActive(true);
        }
        else if(currentMode == 2)
        {
            Rec2SquareBackground.SetActive(true);
        }
        else
        {
            SimilarityBackground.SetActive(true);
        }

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
            ScoreBackground.SetActive(false);
            isPlay = 0;
        }

    }

    // Update function for every timeframe
    public void Update()
    {
        //Debug.Log("Storyprogress " + ss.storyprogress);
        // check for game end ( different for different levels/modes ) comes here.
        if(isPlay != 0) GameManager(0);

    }

    public void GameManager(int isHelp)
    {
        int winflag = 0;

        if (isHelp == 2) winflag = 1;

        // 목숨이 없는 경우
        if (lifes == 0) {
            Debug.Log("here4");
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

        // 필요한 모든 검증장치를 여기에 추가한다.
        if (gc.isSolvedRect() || (isHelp==2 && currentMode==1))
        {
            if(currentMode == 0)
            {
                winflag = 1;
            }
            else
            {
                // move to storyscript state machine
                if (ss.GetstoryProgress() == 11)
                {
                    Debug.Log("here");
                    GameOver(true);
                }
                isPlay = 0;
                ss.SetstoryProgress(ss.GetstoryProgress()+1);
                ss.StoryManager();
                return;
            }
        }

        if (gc.isSolvedRec2Square() || (isHelp==2 && currentMode == 2))
        {
            if(currentMode == 0)
            {
                winflag = 1;
            }
            else
            {
                if (ss.GetstoryProgress() == 2)
                {
                    Debug.Log("here2");
                    GameOver(true);
                }
                isPlay = 0;
                ss.SetstoryProgress(ss.GetstoryProgress()+1);
                ss.StoryManager();
                return;
            }
        }

        if (gc.isSolvedSimilarity() || (isHelp==2 && currentMode ==3))
        {
            if(currentMode == 0)
            {
                winflag = 1;
            }
            else
            {
                if (ss.GetstoryProgress() == 2)
                {
                    Debug.Log("here3");
                    GameOver(true);
                }
                isPlay = 0;
                ss.SetstoryProgress(ss.GetstoryProgress()+1);
                ss.StoryManager();
                return;
            }
        }

        if(winflag == 1)
        {
            combo++;
            if (current_Time >= bonusTimeLimit) BonusGift();
            AddPointManager();
            ResetTimeManager();
            MakeNewGame();
        }
        return;
    }

    private void BonusGift()
    {
        combo += 2;
        score += 5;
    }

    private void AddPointManager()
    {
        //if (currentMode == 0) return;

        // 게임종류, 여기는 중간로직이 많아질수도 있으니까 if문으로썼다!!
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
        return;
    }

    private void ResetTimeManager()
    {

        float timeBonus = UnityEngine.Random.Range(0f, 2f);

        if(currentMode == 0)
        {
            // 이부분이랑 콤보 풀리는 부분 조절하는게 게임성 핵심이다
            // 콤보 잃어서 제한시간이 너무 늘어지면 안된다
            // 점수나 콤보때문에 0초가 되면 안된다. 초반에 선형적이되 0으로 수렴하는 함수로
            solveTime = 45; // (float) Math.Floor(30 - 2 * combo - 0.05 * score);
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
        return;
    }

    private void MakeNewGame()
    {
        
        if (currentMode == 0)
        {
            // 문제랜덤생성, TODO : 점수 및 콤보증가에 따라 난이도 높은 문제 증가할 확률 증가
            currentGame = (int)Math.Floor(UnityEngine.Random.Range(0f, 10f));
            PlayerPrefs.SetInt("Game", currentGame);
        }
        else
        {
            currentGame = PlayerPrefs.GetInt("Game");
        }

        // 배경화면 및 게임아이템 설정
        // TODO : 프라이팬 등의 도구 세트 변경 작업도 여기서 수행한다.
        // *** (주의) GAMEMODE 설정 변경시 currentGame 숫자 범위 변경 필요함 ***
        ClearBackground();

        if(currentGame >= 0 && currentGame <= 7)
        {
            // 투렉트
            RectangleBiscuitBackground.SetActive(true);
        }else if(currentGame >= 8 && currentGame <= 9)
        {
            // 직투정
            Rec2SquareBackground.SetActive(true);
        }
        else
        {
            // 합동삼각형
            SimilarityBackground.SetActive(true);
        }

        gc.makeNew(currentGame);
        return;
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
        return;
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
        return;
    }

    
    public void GameOver(bool isCleared) // life==0일때 gamemanager에서 호출하는 모달 팝업 매니징 함수
    {                                    // isCleared면 Clear, 아니면 GameOver
        current_Time = 0;
        StopCoroutine("Timer");
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



        if (currentMode == 0 || currentMode == 3)
        {
            // 순위전 버튼구성
            if (currentMode == 0)
            {
                RankingMain.SetActive(true);
                RankingSub1.SetActive(true);
                RankingSub2.SetActive(true);
                RestartButton.SetActive(true);
            }
            else
            {
                ChallengeButtonforSimilarity.SetActive(true);
            }
            
            RankingButton.SetActive(true);            
        }
        else
        {
            // 스토리모드 버튼구성
            NextStageButton.SetActive(true);
            ChallengeButton.SetActive(true);
        }
        return;
    }

    public void Movemode()
    {
        movementStatus++;
        if (movementStatus == 3) movementStatus = 0;
        return;
    }

    public void ClearBackground()
    {
        RectangleBiscuitBackground.SetActive(false);
        Rec2SquareBackground.SetActive(false);
        SimilarityBackground.SetActive(false);
        return;
    }

    public int GetisPlay()
    {
        return isPlay;
    }

    public void SetisPlay(int x)
    {
        isPlay = x;
    }

}

