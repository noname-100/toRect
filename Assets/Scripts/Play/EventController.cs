using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour {

    // 난이도요소
    public int Difficulty;
    public float SolveTime;

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

    // TODO : 게임 로직 부분

    
    
    // gamemode를 랜덤하게 생성하자.

    // Revert one state back
    public void Revert()
    {
        // "스테이트 버퍼 스택" 필요!!

        // 버퍼 스택의 top 스테이트를 pop한다

        // top의 스테이트를 load한다

    }

    // 초기화
    private void Awake()
    {

        Lifes = 3;
        Hints = 3;
        Score = 0;
        MovementStatus = 0;
        SolveTime = 60;

        if (PlayerPrefs.GetInt("Mode") == 0) isPlay = 1;
        else isPlay = 0;
        
    }

    // Update function for every timeframe
    public void Update()
    {
        // check for game end
        if (current_Time <= 0)
        {
            LostLife();
            Renew();
        }

        if (isPlay == 1)
        {
            isPlay = 2;
            StartTime();
        }

    }

    public void Renew()
    {
        Debug.Log("Renew");
        current_Time = SolveTime;
        //Generate();
        GC.makeNew();
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

    public void StartTime() // 시간 초기화 및 Timer()함수 실행
    {
        current_Time = SolveTime;
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
