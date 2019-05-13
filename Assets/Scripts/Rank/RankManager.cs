using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class RankManager : MonoBehaviour
{

    public const string gameName = "ToRect";

    /*
     *  TEST CODE : 내부 기능 구현 위해 JSON 요청/반환 부분 비활성화
     * 
     */    

    private bool disableAll = false;
    private bool userTest = true;
    private string hardCodedToken = "eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NTc3NDU5MTQsInR5cGUiOiJJTkRWIiwiaWQiOiIxNjYyNjgzNzM2NzAzMDExIiwic2Vzc2lvbklkIjoiZTdlNTI5YjItMzMyMC00MDFlLTkwYzgtZDBlNDE5ZTAwZjJjIiwiYXV0aExldmVsIjoxLCJyb2xlcyI6W10sInN1YnNjcmlwdGlvbiI6eyJzdWJzY3JpcHRpb25JZCI6IjE2NjI3NDMzNTU1NzUzMzQiLCJlbmREYXRlIjoiMjAxOS0wMS0xNiIsImFjdGl2ZSI6ZmFsc2V9LCJyZWFkT25seSI6ZmFsc2UsImlhdCI6MTU1NzcyNDMxNH0.vaC5bwFq6LBKnKKNy6jcQvtBmxW_nk9q18PaAv3j5kg";

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
        Debug.Log("start from rankmanager");
        if(!disableAll) LoadData();
        GetRankInfo(0);
    }

    public void SetUserData(string data)
    {
        if (disableAll) return;

        UserJsonData = data;
        // Debug.Log("Set: " + UserJsonData);

        if (UserJsonData != null) user = JsonUtility.FromJson<UserData>(UserJsonData);
    }

    void LoadData()
    {
        Debug.Log("LoadData");
        if (disableAll) return;

        if(!userTest) Application.ExternalCall("SetUserData");
        // Debug.Log("Get: " + UserJsonData);

        // JSON Parsing
        if (userTest)
        {
           UserJsonData = "{ \"host\":\"https:\\/\\/dev-api.quebon.tv\",\"userid\":\"1662593299120151\",\"nickname\":\"\",\"token\":\"" + hardCodedToken +"\",\"closeUrl\":\"https:\\/\\/dev.quebon.tv\\/game\\/toRect\\/exit\"}";           
        }

        if(UserJsonData!=null) user = JsonUtility.FromJson<UserData>(UserJsonData);
    }

    public void GameClose()
    {
        if (disableAll) return;

        Application.OpenURL(user.closeUrl);
    }

    // DB에 정보 전송, 점수-시간-userid 를 보낸다
    public void PutRankInfo(int score) {
        if (disableAll) return;

        if (string.IsNullOrEmpty(user.token)) {
            LoadData();
            //not authorized
            Debug.Log("PutRankInfo : user.token empty");
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
            yield return w.SendWebRequest();

            if (w.isHttpError || w.isNetworkError)
            {
                //TODO handle error
                Debug.Log("error");
            }
            else
            {
                //sucess
                MyRank = JsonUtility.FromJson<RankData>(w.downloadHandler.text);
                RankData r = JsonUtility.FromJson<RankData>(w.downloadHandler.text);
                gameObject.GetComponent<EventController>().SetrequestWaiting(false);                
            }
        }
    }

    // which==0 : 랭크화면 which==1 : 게임오버화면
    public void GetRankInfo(int which) {
        Debug.Log("GetRankInfo");
        if (disableAll) return;

        if (which == 0)
        {
            RankDataWindow.SetActive(false);
            WaitPlz.SetActive(true);
        }
        
        if (string.IsNullOrEmpty(user.token)) {
            LoadData();
            // not authorized : TODO?? request again?
            return;
        }

        StartCoroutine(GetRanking(user.token, which));
    }

    private IEnumerator GetRanking(string token, int which) {
        Debug.Log("GetRanking");
        string url = user.host + "/user/v1/games/" + gameName;
        Debug.Log("url : " + url);

        using (UnityWebRequest w = UnityWebRequest.Get(url)) {
            w.SetRequestHeader("Authorization", "Bearer " + token);
            yield return w.SendWebRequest();

            if (w.isHttpError || w.isNetworkError) {
                //TODO handle error
                Debug.Log("error");
            }
            else {
                // Debug.Log(w.downloadHandler.text);
                // success
                Ranking r = JsonUtility.FromJson<Ranking>(w.downloadHandler.text);

                MyRank = r.my;
                MyRank.nickname = r.my.user.nickname;
                MyRank.level = r.my.user.badges.winner.level;
                Debug.Log(MyRank.score);

                if (which == 0)
                {
                    WaitPlz.SetActive(false);
                    RankDataWindow.SetActive(true);
                }

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

                if(SceneManager.GetActiveScene().name=="Play") GameOverMyRank.GetComponent<RankBox>().SetRankBox(MyRank.score, MyRank.nickname);
                gameObject.GetComponent<EventController>().SetrequestWaiting(false);
            }
        }
    }

}