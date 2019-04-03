using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController_Title : MonoBehaviour {

    public GameObject[] HelpContents = new GameObject[5];
    public GameObject ModeSelect;
    public GameObject StoryModeSelect;
    public GameObject RankPage;
    public GameObject LeftButton;
    public GameObject RightButton;
    public GameObject SoundOnButton;
    public GameObject SoundOffButton;
    private int Page;
    private int helpLength;

    // Used to auto upload from PlayerPrefs
    public void Awake()
    {
        helpLength = HelpContents.Length;
        // INFO : game menu is at bottom the bottom layer
        for (int i = 0; i < helpLength; i++) HelpContents[i].SetActive(false);
        ModeSelect.SetActive(false);
        RankPage.SetActive(false);
        StoryModeSelect.SetActive(false);
        //SoundOffButton.SetActive(true);
        //SoundOnButton.SetActive(false);

        PlayerPrefs.SetInt("isMonsterTypeOn", 1);
        if (!PlayerPrefs.HasKey("isSoundOn"))
            SoundOn();

        if (PlayerPrefs.GetFloat("isSoundOn") == 0f)
        {
            SoundOnButton.SetActive(true);
            SoundOffButton.SetActive(false);
        }
        else
        {
            SoundOnButton.SetActive(false);
            SoundOffButton.SetActive(true);
        }

    }

    public void MomStoryModeStart()
    {
        PlayerPrefs.SetInt("Mode", 0);
        SceneManager.LoadScene("Play");
    }

    public void DadStoryModeStart()
    {
        PlayerPrefs.SetInt("Mode", 1);
        SceneManager.LoadScene("Play");
    }

    public void FriendStoryModeStart()
    {
        PlayerPrefs.SetInt("Mode", 2);
        SceneManager.LoadScene("Play");
    }

    public void TeacherStoryModeStart()
    {
        PlayerPrefs.SetInt("Mode", 3);
        SceneManager.LoadScene("Play");
    }

    public void ChallengeModeStart()
    {
        PlayerPrefs.SetInt("Mode", 4);
        SceneManager.LoadScene("Play");
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

    public void SetFirstPage()
    {
        Page = 0;
        HelpContents[Page].SetActive(true);
        for (int i = 1; i < helpLength; i++) HelpContents[i].SetActive(false);
        LeftButton.SetActive(false);
        RightButton.SetActive(true);
    }

    public void RightPage()
    {
        // At page 0, left button is desabled as default
        if (Page == 0) LeftButton.SetActive(true);
        HelpContents[Page].SetActive(false);
        Page++;
        HelpContents[Page].SetActive(true);
        if (Page == helpLength-1) RightButton.SetActive(false);
    }

    public void LeftPage()
    {
        if (Page == helpLength) RightButton.SetActive(true);
        HelpContents[Page].SetActive(false);
        Page--;
        HelpContents[Page].SetActive(true);
        if (Page == 0) LeftButton.SetActive(false);
    }



}
