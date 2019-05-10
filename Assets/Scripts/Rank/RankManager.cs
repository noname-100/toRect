using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class RankManager : MonoBehaviour
{

    public const string gameName = "ToRect";

    private Vector3 RankDataDownPos, RankDataPos;
    public GameObject RankDataWindow;
    public GameObject[] RankBoxTop5 = new GameObject[5];
    public GameObject[] GameOverRankBox = new GameObject[2];
    public GameObject GameOverMyRank;
    public GameObject WaitPlz;

    // UserData 저장용 구조체
    struct UserData {
        public string host;
        public string userid;
        public string nickname;
        public string token;
        public string closeUrl;
        public UserData(string host, string userid, string nickname, string token, string closeUrl) {
            this.host = host;
            this.userid = userid;
            this.nickname = nickname;
            this.token = token;
            this.closeUrl = closeUrl;
        } 
    }

    public void SetUserData(string data)
    {
        UserJsonData = data;
        // Debug.Log("Set: " + UserJsonData);

        user = JsonUtility.FromJson<UserData>(UserJsonData);
    }

    // UserData 받아올 JSON과 구조체
    public string UserJsonData;
    UserData user = new UserData();

    // 시작하면서 UserData 받아오고 저장
    void Start() {
        LoadData();
    }

    void LoadData()
    {
        Application.ExternalCall("SetUserData");
        // Debug.Log("Get: " + UserJsonData);

        // JSON Parsing
        user = JsonUtility.FromJson<UserData>(UserJsonData);
    }

    public void GameClose()
    {
        Application.OpenURL(user.closeUrl);
    }

    [System.Serializable]
    struct Badges
    {
        public Badge winner;
    }

    [System.Serializable]
    struct Badge
    {
        public int level;
    }

    [System.Serializable]
    struct User
    {
        public string userId;
        public string nickname;
        public Badges badges;
    }

    [System.Serializable]
    struct Ranking
    {
        public RankData my;
        public List<RankData> ranking;
    }

    // RankData 저장할 구조체
    [System.Serializable]
    struct RankData
    {
        public User user;
        public int rank;        // 등수
        public int score;       // 점수
        public string nickname; // 닉네임
        public int level;       // 레벨 (깨봉홈페이지 레벨)
    }

    // 상위 5등과 자신의 RankData 저장할 구조체
    RankData[] Top5 = new RankData[5];
    RankData MyRank;

    // UserData 받아올 JSON과 구조체
    public string UserJsonData = null;
    UserData user = new UserData();

    // 시작하면서 UserData 받아오고 저장
    void Start() {
        LoadData();
    }

    public void SetUserData(string data)
    {
        UserJsonData = data;
        // Debug.Log("Set: " + UserJsonData);

        if (UserJsonData != null) user = JsonUtility.FromJson<UserData>(UserJsonData);
    }

    void LoadData()
    {
        Application.ExternalCall("SetUserData");
        // Debug.Log("Get: " + UserJsonData);

        // JSON Parsing
        if(UserJsonData!=null) user = JsonUtility.FromJson<UserData>(UserJsonData);
    }

    public void GameClose()
    {
        Application.OpenURL(user.closeUrl);
    }

    // DB에 정보 전송, 점수-시간-userid 를 보낸다
    public void PutRankInfo(int score) {
        if (string.IsNullOrEmpty(user.token)) {
            LoadData();
            //not authorized
            return;
        }

        StartCoroutine(PutRanking(user.token, score));
    }

    private IEnumerator PutRanking(string token, int score) {
        string url = user.host + "/user/v1/games/" + gameName + "/users/" + user.userid;
        string data = "{\"score\":" + score + "}";

        using (UnityWebRequest w = UnityWebRequest.Put(url, data))
        {
            w.SetRequestHeader("Authorization", "Bearer " + token);
            w.SetRequestHeader("Content-Type", "application/json");
            // Debug.Log(url + "\n\n" + data);
            yield return w.SendWebRequest();

            if (w.isHttpError || w.isNetworkError)
            {
                //TODO handle error
            }
            else
            {
                //sucess
                MyRank = JsonUtility.FromJson<RankData>(w.downloadHandler.text);
                
                //success
                RankData r = JsonUtility.FromJson<RankData>(w.downloadHandler.text);
                
                
            }
        }
    }

    public void GetRankInfo() {
        RankDataWindow.SetActive(false);
        WaitPlz.SetActive(true);
        
        if (string.IsNullOrEmpty(user.token)) {
            LoadData();
            //not authorized
            return;
        }

        StartCoroutine(GetRanking(user.token));
    }

    private IEnumerator GetRanking(string token) {
        string url = user.host + "/user/v1/games/" + gameName;

        using (UnityWebRequest w = UnityWebRequest.Get(url)) {
            w.SetRequestHeader("Authorization", "Bearer " + token);
            yield return w.SendWebRequest();

            if (w.isHttpError || w.isNetworkError) {
                //TODO handle error
            }
            else {
                // Debug.Log(w.downloadHandler.text);
                // success
                Ranking r = JsonUtility.FromJson<Ranking>(w.downloadHandler.text);

                MyRank = r.my;
                MyRank.nickname = r.my.user.nickname;
                MyRank.level = r.my.user.badges.winner.level;

                WaitPlz.SetActive(false);
                RankDataWindow.SetActive(true);

                int size = Math.Min(r.ranking.Count, 5);
                int i = 0;
                for (i = 0; i < size; i++) {
                    Top5[i] = r.ranking[i];
                    Top5[i].nickname = r.ranking[i].user.nickname;
                    Top5[i].level = r.ranking[i].user.badges.winner.level;
                }

                if (i < 5) {
                    for (int j = i; j < 5; j++) {
                        //TODO don't show empty data
                        Top5[j] = new RankData();
                    }
                }

                for (i = 0; i < 5; i++)
                    RankBoxTop5[i].GetComponent<RankBox>().SetRankBox(Top5[i].score, Top5[i].nickname);

                for (i = 0; i < 2; i++)
                    GameOverRankBox[i].GetComponent<RankBox>().SetRankBox(Top5[i].score, Top5[i].nickname);

                GameOverMyRank.GetComponent<RankBox>().SetRankBox(MyRank.score, MyRank.nickname);
            }
        }
    }

}