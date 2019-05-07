using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StoryScript : MonoBehaviour {


    // Tutorials
    private List<GameObject[]> tutorials;
    public GameObject[] BiscuitTutorials = new GameObject[3];
    public GameObject[] CookieTutorials = new GameObject[4];
    public GameObject[] PieTutorials = new GameObject[3];

    // EventController
    public GameObject EC;
    private EventController ec;
    public GameObject GC;
    private GameController gc;
    public GameObject HintButton;
    public GameObject BackGroundFilter;

    // Story mode internal variables
    private int currentMode;
    public int storyProgress;
    private bool isHintAvailable;



    // Use this for initialization
    private void Awake()
    {
        ec = EC.GetComponent<EventController>();
        gc = GC.GetComponent<GameController>();

        tutorials = new List<GameObject[]>();
        tutorials.Add(BiscuitTutorials);
        tutorials.Add(CookieTutorials);
        tutorials.Add(PieTutorials);
        if(ec.GetdebugMode()) Debug.Log("StoryScript tutorials init");

        currentMode = PlayerPrefs.GetInt("Mode") - 1; // for Script indexing and standardization
        storyProgress = 0; // using "storyprogress" state variable
        StoryManager();
        isHintAvailable = true;
        if (ec.GetdebugMode()) Debug.Log("StoryScript Awake");
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
        if(ec.GetdebugMode()) Debug.Log("StoryManager");
        if(currentMode == 0)
        { // Biscuit Story

            switch (storyProgress)
            {
                case 0:
                    Start_TextBox_B(0);
                    break;
                case 1:
                    Stop_TextBox_B(0);
                    Start_TextBox_B(1);
                    break;
                case 3:
                    Stop_TextBox_B(1);
                    Start_TextBox_B(2);
                    break;
                case 4:
                    Stop_TextBox_B(2);
                    PlayerPrefs.SetInt("Game",0);
                    ec.SetisPlay(1);
                    break;
                case 5:
                    PlayerPrefs.SetInt("Game", 2);
                    ec.SetisPlay(1);
                    break;
                case 6:
                    PlayerPrefs.SetInt("Game", 3);
                    ec.SetisPlay(1);
                    break;
                case 7:
                    PlayerPrefs.SetInt("Game", 4);
                    ec.SetisPlay(1);
                    break;
                case 8:
                    PlayerPrefs.SetInt("Game", 5);
                    ec.SetisPlay(1);
                    break;
                case 9:
                    PlayerPrefs.SetInt("Game", 6);
                    HintButton.SetActive(true);
                    isHintAvailable = true;
                    ec.SetisPlay(1);
                    break;
                case 10:
                    PlayerPrefs.SetInt("Game", 7);
                    ec.SetisPlay(1);
                    isHintAvailable = true;
                    break;
                case 11:
                    PlayerPrefs.SetInt("Game", 8);
                    ec.SetisPlay(1);
                    isHintAvailable = true;
                    break;
                case 12:
                    PlayerPrefs.SetInt("Game", 9);
                    ec.SetisPlay(1);
                    isHintAvailable = true;
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
                    Stop_TextBox_R(0);
                    Start_TextBox_R(1);
                    break;
                case 2:
                    Stop_TextBox_R(1);
                    Start_TextBox_R(2);
                    break;
                case 3:
                    Stop_TextBox_R(2);
                    Start_TextBox_R(3);
                    break;
                case 4:
                    Stop_TextBox_R(3);
                    PlayerPrefs.SetInt("Game", 10);
                    ec.SetisPlay(1);
                    break;
                case 5:
                    PlayerPrefs.SetInt("Game", 12);
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
                    Stop_TextBox_S(0);
                    Start_TextBox_S(1);
                    break;
                case 2:
                    Stop_TextBox_S(1);
                    Start_TextBox_S(2);
                    break;
                case 3:
                    Stop_TextBox_S(2);
                    PlayerPrefs.SetInt("Game", 13);
                    ec.SetisPlay(1);
                    break;
            }

        }
        return;
    }

    void Start_TextBox_B(int num)
    {
        BackGroundFilter.SetActive(true);
        tutorials[0][num].SetActive(true);
        return;
    }

    void Start_TextBox_R(int num)
    {
        BackGroundFilter.SetActive(true);
        tutorials[1][num].SetActive(true);
        return;
    }

    void Start_TextBox_S(int num)
    {
        BackGroundFilter.SetActive(true);
        tutorials[2][num].SetActive(true);
        return;
    }

    void Stop_TextBox_B(int num)
    {
        BackGroundFilter.SetActive(false);
        tutorials[0][num].SetActive(false);
        return;
    }

    void Stop_TextBox_R(int num)
    {
        BackGroundFilter.SetActive(false);
        tutorials[1][num].SetActive(false);
        return;
    }

    void Stop_TextBox_S(int num)
    {
        BackGroundFilter.SetActive(false);
        tutorials[2][num].SetActive(false);
        return;
    }

    public int GetstoryProgress()
    {
        return storyProgress;
    }

    public void SetstoryProgress(int x)
    {
        storyProgress = x;
        return;
    }

    public void SetisHintAvailable(bool given)
    {
        isHintAvailable = given;
        return;
    }

    public bool GetisHintAvailable()
    {
        return isHintAvailable;
    }

}
