using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class TitleRank : MonoBehaviour {

    public const string gameName = "ToRect";
    public GameObject RankDataWindow;
    public GameObject[] RankBoxTop5 = new GameObject[5];
    public GameObject WaitPlz;

    private const int INFINITY_PRAC = 1;
    private const int INFINITY_REAL = 2;
    private const int HELL = 3;

    private Vector3 RankDataDownPos, RankDataPos;

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
        // LoadData();
    }

    void LoadData() {
        Application.ExternalCall("SetUserData");

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

    // DB에서 Top5와 자신의 정보 받아온다.
    // token은 (string) user.token에 저장되어 있다.
    // 받아오는 데이터는 각각의 등수, 점수, 시간, 닉네임, 레벨
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
            }
        }
    }
}