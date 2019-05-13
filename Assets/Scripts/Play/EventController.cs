using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/*
 * 
 *  EventController 은 Gamemanager, Pointmanager, Timemanager 등의 게임 운영요소를 포함한다.
 * 
 * 
 */

public class EventController : MonoBehaviour {

    /* 
     * 
     *  테스트 변수들, 테스트 옵션과 테스트 시간 (사용하지 않음 : -1)
     *  
     */
    
    private int testGameMode = -1;
    private float testGameTime = -1;
    private bool debugMode = false;

    // 난이도요소
    private int combo;
    private float solveTime;
    private float bonusTimeLimit;
    private int currentGame;
    private int currentMode;
    

    // 게임 요소
    public static int movementStatus; // 이동모드 상태(전체모드 0, 회전모드 1, 이동모드 2)
    public GameObject GC;
    private GameController gc;
    public int isPlay; // 0 : 게임 정지 1 : 게임 시작 시그널 2 : 게임 실행중
    public GameObject EC;
    private StoryScript ss;
    private ButtonController_Play bp;
    public GameObject plate;
    private SpriteRenderer sr;
    private bool killAnswerCheck = false;
    private int gamePause = 0;
    public int isHelp = 0;
    private bool similarityOnceSolved = false;
    private RankManager RM;
    public GameObject ComboSign;
    public Text ComboText;
    private bool requestWaiting = false;

    // UI요소
    public GameObject GameOverWindow, TotitleButton, RankingButton, RestartButton, ChallengeButton, ChallengeButtonforSimilarity, NextStageButton, GameOverBack, ClearBack;
    public Text GameResultText;
    public GameObject RectangleBiscuitBackground, Rec2SquareBackground, SimilarityBackground, ScoreBackground;
    public GameObject ScoreSign, GameOverBackStory;
    public GameObject ProgressBar;
    public GameObject NextProblemButton, ReassembleButton;
    public GameObject BackgroundFilter;
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
        Application.runInBackground = true;
        // 초기화
        currentMode = PlayerPrefs.GetInt("Mode");
        Debug.Log("current Game Mode is : " + currentMode);
        gc = GC.GetComponent<GameController>();
        ss = EC.GetComponent<StoryScript>();
        bp = EC.GetComponent<ButtonController_Play>();
        sr = plate.GetComponent<SpriteRenderer>();
        RM = gameObject.GetComponent<RankManager>();
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

        //Debug.Log("EventController Awake");
    }

    // Update function for every timeframe
    public void Update()
    {
        //Debug.Log("Storyprogress " + ss.storyprogress);
        // check for game end ( different for different levels/modes ) comes here.
        if (debugMode) Debug.Log("isPlay : " + isPlay);
        if(isPlay != 0) GameManager();

    }

    public void GameManager()
    {
        if(debugMode) Debug.Log("GameManager");
        int winflag = 0;

        // 목숨이 없는 경우
        if (lifes == 0) {
            Debug.Log("Ran out of lives");
            GameOver(false);
        }

        // 시간종료
        if (current_Time < 0)
        {
            if(currentMode != 0)
            {
                GameOver(false);
                return;
            }            
            gamePause = 0;
            isHelp = 0;
            combo = 0;
            bp.setisFormulaButtonSelectable(0);
            bp.FormulaSelect(4);
            LostLife();
            MakeNewGame();
            ResetTimeManager();
        }

        // 게임시작
        if (isPlay == 1)
        {
            isPlay = 2;
            MakeNewGame();
            ResetTimeManager();
            StartCoroutine("Timer");
        }

        if (killAnswerCheck) goto SkipAnswerCheck;

        // 비스킷 문제 정답체크 및 처리
        if ((gc.isSolvedRect() || isHelp==2 )&& currentGame<=gc.getBiscuitProblems())
        {
            if (gamePause == 0)
            {
                NextGameButton();
                StartCoroutine(ScorePopup());
                StopCoroutine(ScorePopup());
            }
            else
            {
                if (gamePause == 1) return;
                if(currentMode !=0)
                {
                    // move to storyscript state machine
                    if (ss.GetstoryProgress() == 12)
                    {
                        // Debug.Log("here");
                        GameOver(true);
                    }
                    isPlay = 0;
                    ss.SetstoryProgress(ss.GetstoryProgress() + 1);
                    ss.StoryManager();
                    return;
                }else{
                    winflag = 1;
                }
            }
        }        

        // 직투정 문제 정답처리 및 체크
        if ((gc.isSolvedRec2Square() || isHelp==2) && currentGame >= gc.getBiscuitProblems()+1 && currentGame <= gc.getRec2SquareProblems()-1)
        {
            if (bp.getisFormulaBoardSelected()==0)
            {
                bp.FormulaBoardOn();
                return;
            }else if(bp.getisFormulaBoardSelected() == 1)
            { // waiting for choice
                return;
            }
            else if(bp.getisFormulaBoardSelected() == 3)
            { // win
                if (gamePause == 0)
                {
                    NextGameButton();
                    StartCoroutine(ScorePopup());
                    StopCoroutine(ScorePopup());
                }
                else
                {
                    if (gamePause == 1) return;
                    if (currentMode != 0)
                    {
                        isPlay = 0;
                        ss.SetstoryProgress(ss.GetstoryProgress() + 1);
                        ss.StoryManager();
                        return;
                    }
                    else
                    {
                        winflag = 1;
                    }
                }
            }
            else
            {
                // lose
                if (currentMode != 0)
                {
                    GameOver(false);
                    return;
                }
                LostLife();
                combo = 0;
                MakeNewGame();
                ResetTimeManager();
            }
        }

        // 정투직 문제 정답체크 및 처리
        if ((gc.isSolvedRect() || isHelp == 2) && currentGame == gc.getRec2SquareProblems())
        {
            if (bp.getisFormulaBoardSelected() == 0)
            {
                bp.FormulaBoardOn();
                return;
            }
            else if (bp.getisFormulaBoardSelected() == 1)
            { // waiting for choice
                return;
            }
            else if (bp.getisFormulaBoardSelected() == 3)
            { // win
                if (gamePause == 0)
                {
                    NextGameButton();
                    StartCoroutine(ScorePopup());
                    StopCoroutine(ScorePopup());
                }
                else
                {
                    if (gamePause == 1) return;
                    if (currentMode != 0)
                    {
                        GameOver(true);
                        return;
                    }
                    else
                    {
                        winflag = 1;
                    }
                }
            }
            else
            {
                // lose
                if (currentMode != 0)
                {
                    GameOver(false);
                    return;
                }
                LostLife();
                combo = 0;
                MakeNewGame();
                ResetTimeManager();
            }
        }

        // 합동삼각형 문제 정답체크 및 처리
        if ((gc.isSolvedSimilarity() || isHelp==2 || similarityOnceSolved) && currentGame >= gc.getRec2SquareProblems()+1 && currentGame <= gc.getSimilarityProblems())
        {
            if (gamePause == 0)
            {
                NextGameButton();
                StartCoroutine(ScorePopup());
                StopCoroutine(ScorePopup());
            }
            else
            {
                if (gamePause == 1) return;
                if(currentMode !=0)
                {
                    if (ss.GetstoryProgress() == 3)
                    {
                        Debug.Log("here3");
                        GameOver(true);
                    }
                    isPlay = 0;
                    ss.SetstoryProgress(ss.GetstoryProgress() + 1);
                    ss.StoryManager();
                    return;
                }
                else
                {
                    winflag = 1;
                }
                plate.transform.localScale = new Vector3(1f, 1f, 0);
            }
        }

        SkipAnswerCheck:
            

        if(winflag == 1 && gamePause == 2)
        {
            combo++;
            if (current_Time >= bonusTimeLimit) BonusGift();
            AddPointManager();            
            MakeNewGame();
            ResetTimeManager();
        }
        return;
    }

    // ButtonController_Play에서 조작한다. 점수 부여 일관성을 위해 함수는 여기에 둔다.
    public void FormulaBonusGift() // legacy code
    {
        score += 0;
        return;
    }

    private void BonusGift()
    {
        score += 2;
        return;
    }

    /*
     * 
     * 
     *  점수체계 결정하는 메소드
     * 
     * 
     */

    private void AddPointManager()
    {
        // 콤보, 제한시간(점수)로 추가점수
        score += 3 + (int) Mathf.Floor(0.6f * (float)combo) + (int) Mathf.Floor(0.005f * (float) score);
        // Debug.Log((int)Mathf.Floor(0.4f * (float)combo));
        ComboText.text = combo.ToString();
        // 문제종류별 추가점수
        switch (currentGame)
        {
            case 0: // 예각1
                score += 5;
                break;
            case 1: // 예각2
                score += 5;
                break;
            case 2: // 직각
                score += 4;
                break;
            case 3: // 둔각
                score += 0;
                break;
            case 4: // 사다리꼴
                score += 1;
                break;
            case 5: // 임의사각형
                score += 4;
                break;
            case 6: // 오각형
                score += 12;
                break;
            case 7: // 육각형
                score += 5;
                break;
            case 8: // 칠각형
                score += 6;
                break;
            case 9: // 팔각형
                score += 5;
                break;
            case 10: // 직투정1
                score += 1;
                break;
            case 11: // 직투정2
                score += 1;
                break;
            case 12: // 정투직
                score += 0;
                break;
            case 13: // 합동삼각형1
                score += 12;
                break;
            case 14: // 함동삼각형2
                score += 12;
                break;
        }
        ScoreText.text = score.ToString() + " 점";
        return;
    }

    /*
     * 
     * 
     *  문제풀이별 시간제한 조절메소드
     * 
     */

    private void ResetTimeManager()
    {
        if (testGameTime != -1)
        {
            current_Time = testGameTime;
            return;
        }

        if(currentMode == 0)
        {
            float maxtime = 20f;
            float mintime = 20f;

            // 최소시간 문제별 세부조정
            switch (currentGame)
            {
                case 0: // 예각
                    maxtime = 80;
                    mintime = 20;
                    break;
                case 1: // 예각2
                    maxtime = 80;
                    mintime = 20;
                    break;
                case 2: // 직각
                    maxtime = 80;
                    mintime = 20;
                    break;
                case 3: // 둔각
                    maxtime = 80;
                    mintime = 20;
                    break;
                case 4: // 사다리꼴
                    maxtime = 90;
                    mintime = 20;
                    break;
                case 5: // 임의사각형
                    maxtime = 240;
                    mintime = 30;
                    break;
                case 6: // 오각형
                    maxtime = 300;
                    mintime = 30;
                    break;
                case 7: // 육각형
                    maxtime = 300;
                    mintime = 30;
                    break;
                case 8: // 칠각형
                    maxtime = 360;
                    mintime = 30;
                    break;
                case 9: // 팔각형
                    maxtime = 300;
                    mintime = 30;
                    break;
                case 10: // 직투정
                    maxtime = 130;
                    mintime = 30;
                    break;
                case 11: // 직투정2
                    maxtime = 130;
                    mintime = 30;
                    break;
                case 12: // 정투직
                    maxtime = 100;
                    mintime = 25;
                    break;
                case 13: // 합동삼각형1
                    maxtime = 90;
                    mintime = 25;
                    break;
                case 14: // 함동삼각형2
                    maxtime = 90;
                    mintime = 25;
                    break;
            }
            float halftime = 1000f;
            float norm = 0.006f;
            solveTime = ((maxtime-mintime) / (1 + Mathf.Exp(norm * ((10 * combo + score) - halftime)))) + mintime;
            // Debug.Log(solveTime);
            // 보너스 타임에만 랜덤요소를 넣는다.
            float timeBonus = UnityEngine.Random.Range(0f, 2f);
            bonusTimeLimit = (float) Math.Floor(solveTime * 0.7 - timeBonus);
        }
        else if(currentMode == 1)
        {   // Biscuit Story Mode
            bonusTimeLimit = 45; // not used
            if (currentGame == 0) solveTime = 120; // 직각삼각형
            else if (currentGame == 2) solveTime = 120; // 직각삼각형
            else if (currentGame == 3) solveTime = 130; // 둔각삼각형
            else if (currentGame == 4) solveTime = 180; // 사다리꼴
            else if (currentGame == 5) solveTime = 240; // 사각형
            else if (currentGame == 6) solveTime = 300; // 오각형
            else if (currentGame == 7) solveTime = 300; // 육각형
            else if (currentGame == 8) solveTime = 360; // 칠각형
            else if (currentGame == 9) solveTime = 300; // 팔각형
            else solveTime = 5; // 에러
        }else if(currentMode == 2)
        {   // Rec2Square Story Mode
            solveTime = 150;
        }else if(currentMode == 3)
        {   // Similarity Story Mode
            solveTime = 110;
        }
        current_Time = solveTime;
        return;
    }

    /*
     * 
     * 순위모드 새로운 게임 분포확률 결정하는 메소드
     *  
     * 
     */
    
    private void MakeNewGame()
    {
        plate.transform.localScale = new Vector3(1f, 1f, 0);
        gamePause = 0;
        isHelp = 0;
        similarityOnceSolved = false;
        bp.setisFormulaButtonSelectable(0);
        if (currentMode == 0)
        {
            // 문제랜덤생성
            List<int> pool = new List<int>();
            int[] difficulty = new int[gc.getSimilarityProblems()];
            difficulty[0] = 10; // 예각
            difficulty[1] = 10; // 예각2
            difficulty[2] = 9; // 직각
            difficulty[3] = 13; // 둔각
            difficulty[4] = 14; // 사다리꼴
            difficulty[5] = 20; // 임의사각형
            difficulty[6] = 30; // 오각형
            difficulty[7] = 30; // 육각형
            difficulty[8] = 40; // 칠각형
            difficulty[9] = 37; // 팔각형
            difficulty[10] = 25; // 직투정1 - (주) 공식선택 연습 및 점수주기 문제를 포함시키기 위함
            difficulty[11] = 25; // 직투정2
            difficulty[12] = 14; // 합동1
            difficulty[13] = 14; // 합동2

            int coeff = 10;
            for(int i = 0; i < difficulty.Length; i++)
            {
                int howMany = 3; // coeff * (int) (1f / (1f + Mathf.Exp((score - 200) * difficulty[i] - 10)));
                for(int j = 0; j < howMany; j++)
                {
                    pool.Add(i);
                }
            }
            pool = ShuffleArray(pool);            
            currentGame = pool[(int)UnityEngine.Random.Range(0, pool.Count)];
            PlayerPrefs.SetInt("Game", currentGame);
        }
        else
        {
            currentGame = PlayerPrefs.GetInt("Game");
        }

        if(testGameMode != -1) currentGame = testGameMode; // TEST 값
        
        ClearBackground();
        if(currentGame >= 0 && currentGame <= gc.getBiscuitProblems())
        {
            // 투렉트
            RectangleBiscuitBackground.SetActive(true);
            ReassembleButton.SetActive(true);
        }
        else if(currentGame >= gc.getBiscuitProblems()+1 && currentGame <= gc.getRec2SquareProblems())
        {
            // 직투정
            Rec2SquareBackground.SetActive(true);
            ReassembleButton.SetActive(true);
        }
        else
        {
            // 합동삼각형
            SimilarityBackground.SetActive(true);
            ReassembleButton.SetActive(false);
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

    public void NextGameButton()
    {
        StopCoroutine("Timer");
        gamePause = 1;
        NextProblemButton.SetActive(true);
        return;
    }

    public void NextGameButtonPress()
    {
        StartCoroutine("Timer");
        NextProblemButton.SetActive(false);
        gamePause = 2;
        return;
    }

    // 주의 : 이거는 문제해결시 팝업, 공식선택시 보너스팝업은 ButtonController_Play에 있음
    IEnumerator ScorePopup()
    {
        // Debug.Log("ScorePopup Called");
        ScoreSign.SetActive(true);
        if(currentMode==0 && combo>0)
        ComboSign.SetActive(true);
        yield return new WaitForSeconds(1f);
        ScoreSign.SetActive(false);
        if(currentMode==0)
        ComboSign.SetActive(false);
    }

    IEnumerator Timer() // 0.01초 단위로 시간을 측정
    {
        //if (isPlay == 0) yield return null;
        yield return new WaitForSeconds(0.01f);
        current_Time -= 0.017f;
        TimeText.text = current_Time.ToString("##0.00") + " sec";
        ProgressBar.transform.localScale = new Vector3(1f*current_Time/solveTime,1,1);
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
        
        StopCoroutine("Timer");
        GameOverWindow.SetActive(true);
        current_Time = 0;
        TimeText.text = "0 sec";
        requestWaiting = true;
        gameObject.GetComponent<RankManager>().PutRankInfo(score);
        int count = 0;
        try
        {
            while (requestWaiting) { Debug.Log("Waiting for response..."); count++; }
            if (count >= 10000000) throw new Exception("Failed to put data on server, callback timeout");

        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        GameOverBackground.SetActive(false);
        Chapter1ClearBackground.SetActive(false);
        Chapter2ClearBackground.SetActive(false);
        Chapter3ClearBackground.SetActive(false);
        BackgroundFilter.SetActive(true);

        if (isCleared)
        {
            switch (currentMode)
            {
                case 0: // 순위모드 실패시
                    GameOverBackground.SetActive(true);
                    GameOverBack.SetActive(true);
                    requestWaiting = true;
                    gameObject.GetComponent<RankManager>().GetRankInfo(1);
                    count = 0;
                    try
                    {
                        while (requestWaiting) { Debug.Log("Waiting for response..."); count++; }
                        if (count >= 10000000) throw new Exception("Failed to put data on server, callback timeout");

                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                    break;
                case 1: // 스토리모드 성공시
                    Chapter1ClearBackground.SetActive(true);
                    break;
                case 2:
                    Chapter2ClearBackground.SetActive(true);
                    break;
                case 3:
                    Chapter3ClearBackground.SetActive(true);
                    break;
            }
        }
        else
        {
            // 스토리모드 실패시
            GameOverBackground.SetActive(true);
            if (currentMode != 0) GameOverBackStory.SetActive(true);
            else GameOverBack.SetActive(true);
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
                if (isCleared) ChallengeButtonforSimilarity.SetActive(true);
                else RestartButton.SetActive(true);
            }
            
            //RankingButton.SetActive(true);            
        }
        else
        {
            // 스토리모드 버튼구성
            if (isCleared) NextStageButton.SetActive(true);
            else RestartButton.SetActive(true);
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

    public void RefreshScore()
    {
        ScoreText.text = score.ToString() + " 점";
    }

    public int GetisPlay()
    {
        return isPlay;
    }

    public void SetisPlay(int x)
    {
        isPlay = x;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int given)
    {
        score = given;
        return;
    }

    public void SetdebugMode(bool given)
    {
        debugMode = given;
        return;
    }

    public bool GetdebugMode()
    {
        return debugMode;
    }

    public bool GetrequestWaiting()
    {
        return requestWaiting;
    }

    public void SetrequestWaiting(bool given)
    {
        requestWaiting = given;
        return;
    }

    public int GetCombo()
    {
        return combo;
    }

    public bool getSimilarityOnceSolved()
    {
        return similarityOnceSolved;
    }

    public void setSimilarityOnceSolved(bool given)
    {
        similarityOnceSolved = given;
        return;
    }

    public void Debug_KillAnswerCheck()
    {
        killAnswerCheck = true;
        return;
    }

    private List<int> ShuffleArray(List<int> given)
    {
        for(int i = 0; i < given.Count-1; i++)
        {
            int r = UnityEngine.Random.Range(i, given.Count);
            int tmp = given[i];
            given[i] = given[r];
            given[r] = tmp;
        }
        return given;        
    }
}

