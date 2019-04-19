using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StoryScript : MonoBehaviour {

    // Scripts
    List<string[]> textscripts;

    // EventController
    public GameObject EC;
    private EventController ec;

    // BiscuitMode text
    public GameObject BiscuitText;
    public Text text_b;

    // Rec2Square text
    public GameObject Rec2SquareText;
    public Text text_r;

    // Similar text
    public GameObject SimilarText;
    public Text text_s;

    // StoryMode Text
    public GameObject TextBox;
    public Text TextBox_text;

    // Story mode internal variables
    private int currentMode;
    public int storyProgress;

    // 1st stage
    string[] textscript1 = {
        "투렉트1어느 날 깨다는 깨봉으로 열심히 삼각함수를 공부를 하였습니다. \n공부를 마친 깨다는 잠시 쉬기 위해 TV를 켰는데 \n마침 중세시대의 기사들이 나오는 영화가 나오고 있었습니다. \n지난 방학 때 로마로 여행을 갔다 온 깨다는 콜로세움의 검투사들의 전투가 멋있어 보였습니다. \n한창 영화를 보던 깨다는 눈꺼풀이 점차 무거워 지며 참을 수 없는 졸음에 빠집니다."
        ,"투렉트정신을 차린 깨다는 콜로세움 경기장 안에 서있었습니다. \n그리고 바로 눈 앞에는 험상궂게 생기고 온몸에 흉터가 있는 검투사가 한 명 서있었습니다. \n깨다를 노려보며 그 검투사는 입을 열었습니다."
        ,"투렉트신참! 정신 똑바로 차려라! \n싸울 검투사가 부족해 지원을 받았더니\n이렇게 흐리멍텅한 사람을 보내주다니…. 나 원 참…"
        , "투렉트1어느 날 깨다는 깨봉으로 열심히 삼각함수를 공부를 하였습니다. \n공부를 마친 깨다는 잠시 쉬기 위해 TV를 켰는데 \n마침 중세시대의 기사들이 나오는 영화가 나오고 있었습니다. \n지난 방학 때 로마로 여행을 갔다 온 깨다는 콜로세움의 검투사들의 전투가 멋있어 보였습니다. \n한창 영화를 보던 깨다는 눈꺼풀이 점차 무거워 지며 참을 수 없는 졸음에 빠집니다."
        ,"투렉트정신을 차린 깨다는 콜로세움 경기장 안에 서있었습니다. \n그리고 바로 눈 앞에는 험상궂게 생기고 온몸에 흉터가 있는 검투사가 한 명 서있었습니다. \n깨다를 노려보며 그 검투사는 입을 열었습니다."
        ,"투렉트1신참! 정신 똑바로 차려라! \n싸울 검투사가 부족해 지원을 받았더니\n이렇게 흐리멍텅한 사람을 보내주다니…. 나 원 참…"
        ,"투렉트2신참! 정신 똑바로 차려라! \n싸울 검투사가 부족해 지원을 받았더니\n이렇게 흐리멍텅한 사람을 보내주다니…. 나 원 참…"
        ,"투렉트3신참! 정신 똑바로 차려라! \n싸울 검투사가 부족해 지원을 받았더니\n이렇게 흐리멍텅한 사람을 보내주다니…. 나 원 참…"
        };

    // 2nd stage
    string[] textscript2 = {
        "직투정2어느 날 깨다는 깨봉으로 열심히 삼각함수를 공부를 하였습니다. \n공부를 마친 깨다는 잠시 쉬기 위해 TV를 켰는데 \n마침 중세시대의 기사들이 나오는 영화가 나오고 있었습니다. \n지난 방학 때 로마로 여행을 갔다 온 깨다는 콜로세움의 검투사들의 전투가 멋있어 보였습니다. \n한창 영화를 보던 깨다는 눈꺼풀이 점차 무거워 지며 참을 수 없는 졸음에 빠집니다."
        ,"직투정정신을 차린 깨다는 콜로세움 경기장 안에 서있었습니다. \n그리고 바로 눈 앞에는 험상궂게 생기고 온몸에 흉터가 있는 검투사가 한 명 서있었습니다. \n깨다를 노려보며 그 검투사는 입을 열었습니다."
        ,"직투정신참! 정신 똑바로 차려라! \n싸울 검투사가 부족해 지원을 받았더니\n이렇게 흐리멍텅한 사람을 보내주다니…. 나 원 참…"
        };


    // 3rd stage
    string[] textscript3 = {
        "합동삼각형3어느 날 깨다는 깨봉으로 열심히 삼각함수를 공부를 하였습니다. \n공부를 마친 깨다는 잠시 쉬기 위해 TV를 켰는데 \n마침 중세시대의 기사들이 나오는 영화가 나오고 있었습니다. \n지난 방학 때 로마로 여행을 갔다 온 깨다는 콜로세움의 검투사들의 전투가 멋있어 보였습니다. \n한창 영화를 보던 깨다는 눈꺼풀이 점차 무거워 지며 참을 수 없는 졸음에 빠집니다."
        ,"합동삼각형정신을 차린 깨다는 콜로세움 경기장 안에 서있었습니다. \n그리고 바로 눈 앞에는 험상궂게 생기고 온몸에 흉터가 있는 검투사가 한 명 서있었습니다. \n깨다를 노려보며 그 검투사는 입을 열었습니다."
        ,"합동삼각형신참! 정신 똑바로 차려라! \n싸울 검투사가 부족해 지원을 받았더니\n이렇게 흐리멍텅한 사람을 보내주다니…. 나 원 참…"
        };


    // 4th stage
    string[] textscript4 = {
        "4어느 날 깨다는 깨봉으로 열심히 삼각함수를 공부를 하였습니다. \n공부를 마친 깨다는 잠시 쉬기 위해 TV를 켰는데 \n마침 중세시대의 기사들이 나오는 영화가 나오고 있었습니다. \n지난 방학 때 로마로 여행을 갔다 온 깨다는 콜로세움의 검투사들의 전투가 멋있어 보였습니다. \n한창 영화를 보던 깨다는 눈꺼풀이 점차 무거워 지며 참을 수 없는 졸음에 빠집니다."
        ,"정신을 차린 깨다는 콜로세움 경기장 안에 서있었습니다. \n그리고 바로 눈 앞에는 험상궂게 생기고 온몸에 흉터가 있는 검투사가 한 명 서있었습니다. \n깨다를 노려보며 그 검투사는 입을 열었습니다."
        ,"신참! 정신 똑바로 차려라! \n싸울 검투사가 부족해 지원을 받았더니\n이렇게 흐리멍텅한 사람을 보내주다니…. 나 원 참…"
        };



    // Use this for initialization
    private void Awake()
    {

        // adding scripts

        textscripts = new List<string[]>();

        textscripts.Add(textscript1);
        textscripts.Add(textscript2);
        textscripts.Add(textscript3);
        textscripts.Add(textscript4);

        // Sync with Event controller

        ec = EC.GetComponent<EventController>();

        // using "storyprogress" state variable

        currentMode = PlayerPrefs.GetInt("Mode") - 1; // for Script indexing and standardization
        storyProgress = 0;
        StoryManager();        

    }

    // Update is called once per frame
    void Update()
    {
        // stage continues with mouse click
        if(currentMode!=-1 && Input.GetKeyDown(KeyCode.Mouse0) && ec.isPlay==0)
        {
            storyProgress++;
            StoryManager();
        }
    }


    public void StoryManager()
    {
        if(currentMode == 0)
        { // Biscuit Story

            switch (storyProgress)
            {
                case 0:
                    Start_TextBox_B(0);
                    break;
                case 1:
                    Start_TextBox_B(1);
                    break;
                case 2:
                    Stop_TextBox_B();
                    PlayerPrefs.SetInt("Game", 0); // set gamemode from here
                    ec.isPlay = 1;
                    break;
                case 3:
                    Start_TextBox_B(2);
                    break;
                case 4:
                    Start_TextBox_B(3);
                    break;
                case 5:
                    Stop_TextBox_B();
                    PlayerPrefs.SetInt("Game", 1);
                    ec.SetisPlay(1);
                    break;
                case 6:
                    Start_TextBox_B(4);
                    break;
                case 7:
                    Start_TextBox_B(5);
                    break;
                case 8:
                    Stop_TextBox_B();
                    PlayerPrefs.SetInt("Game", 1);
                    ec.SetisPlay(1);
                    break;
                case 9:
                    Start_TextBox_B(6);
                    break;
                case 10:
                    Start_TextBox_B(7);
                    break;
                case 11:
                    Stop_TextBox_B();
                    PlayerPrefs.SetInt("Game", 1);
                    ec.SetisPlay(1);
                    break;
            }

        }else if(currentMode == 1)
        { // Rec2Square Story

            switch (storyProgress)
            {
                case 0:
                    Start_TextBox_R(0);
                    break;
                case 1:
                    Start_TextBox_R(1);
                    break;
                case 2:
                    Stop_TextBox_R();
                    ec.SetisPlay(1);
                    break;
            }

        }
        else if(currentMode == 2)
        { // Similarity Story

            switch (storyProgress)
            {
                case 0:
                    Start_TextBox_S(0);
                    break;
                case 1:
                    Start_TextBox_S(1);
                    break;
                case 2:
                    Stop_TextBox_S();
                    ec.SetisPlay(1);
                    break;
            }

        }
        return;
    }

    void Start_TextBox_B(int num)
    {
        BiscuitText.SetActive(true);
        text_b.text = textscripts[currentMode][num];
        return;
    }

    void Start_TextBox_R(int num)
    {
        Rec2SquareText.SetActive(true);
        text_r.text = textscripts[currentMode][num];
        return;
    }

    void Start_TextBox_S(int num)
    {
        SimilarText.SetActive(true);
        text_s.text = textscripts[currentMode][num];
        return;
    }

    void Stop_TextBox_B()
    {
        BiscuitText.SetActive(false);
        return;
    }

    void Stop_TextBox_R()
    {
        Rec2SquareText.SetActive(false);
        return;
    }

    void Stop_TextBox_S()
    {
        SimilarText.SetActive(false);
        return;
    }

    public int GetstoryProgress()
    {
        return storyProgress;
    }

    public void SetstoryProgress(int x)
    {
        storyProgress = x;
    }

}
