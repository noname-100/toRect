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
    private string hardCodedToken = "eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NTc4MjYyOTEsInR5cGUiOiJJTkRWIiwiaWQiOiIxMDY4MTgzNjY2NTU2OTI5Iiwic2Vzc2lvbklkIjoiMWZiZWY5ODUtYzE4YS00Y2EyLTk5NTctYjQ2YmQ4NTNkOTg4IiwiYXV0aExldmVsIjo5LCJyb2xlcyI6W3sibmFtZSI6InByZW1pdW1fdXNlciIsInBlcm1pc3Npb25zIjpbIlBSRU1JVU1fVVNFUiJdfV0sInN1YnNjcmlwdGlvbiI6eyJzdWJzY3JpcHRpb25JZCI6IjE0OTUyNTM4NDk2Mzc5MDkiLCJlbmREYXRlIjoiMjAxOS0wOS0wNSIsImFjdGl2ZSI6dHJ1ZX0sInJlYWRPbmx5IjpmYWxzZSwiaWF0IjoxNTU3ODA0NjkyfQ.S6B1HJ3eBzJSd5zerjpqEA3pFo6rNkFjD66adbx0tpc";

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
                
        //Debug.Log("start from rankmanager");
        if(!disableAll) LoadData();

        GetRankInfo(1);
    }

    public void SetUserData(string data)
    {
        if (disableAll) return;

        UserJsonData = data;
        
        // Debug.Log("Set: " + UserJsonData);

        if (UserJsonData != null) user = JsonUtility.FromJson<UserData>(UserJsonData);
        Debug.Log("At setuserdata : " + user.host);
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
           UserJsonData = "{ \"host\":\"https:\\/\\/dev-api.quebon.tv\",\"userid\":\"1068183666556929\",\"nickname\":\"\",\"token\":\"" + hardCodedToken +"\",\"closeUrl\":\"https:\\/\\/dev.quebon.tv\\/game\\/toRect\\/exit\"}";           
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
        Debug.Log("putrankinfo");
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
        Debug.Log("putranking");
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
                Debug.Log("w.downloadHandler.text");
            }
            else
            {
                Debug.Log("success");
                Debug.Log(w.downloadHandler.text);
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

        // Debug.Log(user.token);
        if (string.IsNullOrEmpty(user.token)) {
            Debug.Log("this is called");
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
                Debug.Log(w.downloadHandler.text);
            }
            else {
                Debug.Log("success");
                Debug.Log(w.downloadHandler.text);
                // success
                Ranking r = JsonUtility.FromJson<Ranking>(w.downloadHandler.text);

                MyRank = r.my;
                MyRank.nickname = r.my.user.nickname;
                MyRank.level = r.my.user.badges.winner.level;
                // Debug.Log(MyRank.score);

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
                    // Debug.Log(i + " " + Top5[i].nickname);
                }

                if (i < 5) {
                    for (int j = i; j < 5; j++) {
                        //TODO don't show empty data
                        Top5[j] = new RankData();
                        Top5[j].score = 0;
                        Top5[j].nickname = "-";
                    }
                }

                for (i = 0; i < 5; i++)
                {
                    Debug.Log(i + " " + Top5[i].score + " " + Top5[i].nickname);
                    RankBoxTop5[i].GetComponent<RankBox>().SetRankBox(Top5[i].score, Top5[i].nickname);
                }

                for (i = 0; i < 2; i++)
                    GameOverRankBox[i].GetComponent<RankBox>().SetRankBox(Top5[i].score, Top5[i].nickname);

                if(SceneManager.GetActiveScene().name=="Play") GameOverMyRank.GetComponent<RankBox>().SetRankBox(MyRank.score, MyRank.nickname);
                gameObject.GetComponent<EventController>().SetrequestWaiting(false);
            }
        }
    }

    /*
     * UTILS AND TESTS
     * 
     */

    private IEnumerator GetAPItest()
    {
        Debug.Log("apitest");
        // var url = "https://naver.com";
        var url = "https://dev-api.quebon.tv/user/v1/games/ToRect";
        using (UnityWebRequest w = UnityWebRequest.Get(url))
        {
            Debug.Log(user.token);
            w.SetRequestHeader("Authorization", "Bearer " + user.token);
            yield return w.SendWebRequest();

            if (w.isHttpError || w.isNetworkError)
            {
                //TODO handle error
                Debug.Log(w.downloadHandler.text);
            }
            else
            {
                Debug.Log(w.downloadHandler.text);
                // success
                Debug.Log("success");
                Ranking r = JsonUtility.FromJson<Ranking>(w.downloadHandler.text);

                MyRank = r.my;
                MyRank.nickname = r.my.user.nickname;
                MyRank.level = r.my.user.badges.winner.level;
                Debug.Log(MyRank.score);
            }
        }
    }

    private IEnumerator PutAPITest()
    {
        string url = user.host + "/user/v1/games/" + gameName + "/users/" + user.userid;
        string data = "{\"score\":" + 1444 + "}";

        using (UnityWebRequest w = UnityWebRequest.Put(url, data))
        {
            w.SetRequestHeader("Authorization", "Bearer " + user.token);
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
                Debug.Log("success");
                MyRank = JsonUtility.FromJson<RankData>(w.downloadHandler.text);
                RankData r = JsonUtility.FromJson<RankData>(w.downloadHandler.text);
                // gameObject.GetComponent<EventController>().SetrequestWaiting(false);
            }
        }
    }
}