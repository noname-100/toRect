using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankBox : MonoBehaviour {

    // 등수, 점수, 시간, 닉네임, 레벨
    public GameObject N, S;
    public GameObject NumBackGroundPar;

    public void SetRankBox( int score, string nickname)
    {
        
        SetText(S, score.ToString() + "점");
        if (nickname.Length <= 10)
            SetText(N, nickname);
        else
            SetText(N, nickname.Substring(0, 9) + "...");
        
    }

    void SetText(GameObject GO, string str) {
        GO.transform.GetComponent<Text>().text = str;
    }
}
