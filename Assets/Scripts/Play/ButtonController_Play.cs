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

    public void Awake()
    {
        SoundOnButton.SetActive(false);
        SoundOffButton.SetActive(true);
        AllChangeModeButton.SetActive(true);
        RotateChangeModeButton.SetActive(false);
        SlideChangeModeButton.SetActive(false);
        RankPage.SetActive(false);
    }

    public void Totitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void Restart()
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

}
