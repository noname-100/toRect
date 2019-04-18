using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController_Play : MonoBehaviour
{

    public GameObject SoundOnButton;
    public GameObject SoundOffButton;
    public GameObject AllChangeModeButton;
    public GameObject RotateChangeModeButton;
    public GameObject SlideChangeModeButton;
    public GameObject RankingButton;
    public GameObject RankPage;
    public GameObject EC;
//    public GameObject RectangleBiscuitBackground, Rec2SquareBackground, SimilarityBackground;
    private EventController ec;

    public void Awake()
    {
        SoundOnButton.SetActive(false);
        SoundOffButton.SetActive(true);
        AllChangeModeButton.SetActive(true);
        RotateChangeModeButton.SetActive(false);
        SlideChangeModeButton.SetActive(false);
        RankPage.SetActive(false);
        ec = EC.GetComponent<EventController>();

        int currentMode = PlayerPrefs.GetInt("Mode");

    }

    public void Totitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void RestartChallenge()
    {
        SceneManager.LoadScene("Play");
    }

    public void GameClose()
    {
        Application.Quit();
    }

    public void SoundOn()
    {
        PlayerPrefs.SetFloat("isSoundOn", 1f);
        AudioListener.volume = 1f;
    }

    public void SoundOff()
    {
        PlayerPrefs.SetFloat("isSoundOn", 0f);
        AudioListener.volume = 0f;
    }

    public void ToRank()
    {
        RankPage.SetActive(true);
    }

    // TODO : 각 스테이지에 맞게 즉시클리어 가능한 버튼 만들것.
    public void ImmediateWin()
    {
        ec.GameManager(2);
    }

    public void NextChapter()
    {
        int currentMode = PlayerPrefs.GetInt("Mode");
        int currentGame = PlayerPrefs.GetInt("Game");
        if(currentMode == 0)
        {
            // TODO : 만약 단계별 클리어화면 추가되면 여기에서 작업한다.
            PlayerPrefs.SetInt("Mode", 1);
        }
        else if(currentMode == 1)
        {
            PlayerPrefs.SetInt("Mode", 2);
        }
        else
        {
            PlayerPrefs.SetInt("Mode", 2);
        }
    }

}
