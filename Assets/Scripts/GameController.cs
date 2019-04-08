using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// 문제 생성, 문제 해결 확인은 여기에 있음

public class GameController : MonoBehaviour
{
    public List<GameObject> polygonList;
    public bool polygonSelected;
    private Vector2[] problemTriangle;
    public EventController EC;
    public GameObject SS;
    private StoryScript ss;

    private void Awake()
    {
        ss = SS.GetComponent<StoryScript>();

        // makenew를 어떻게 설계할 것인가?
        // 현재 게임 난이도를 어디서 설정할 것인가?
        // 챌린지모드의 모든 모듈 랜덤하게 등장도 구현해야 한다.

    }

    // Start is called before the first frame update
    void Start()
    {
        //makeNew();
    }


    // Mode : 
    public void makeNew(int gametype)
    {
        if(gametype == 0)
        {   
            // 정삼각형



        }else if(gametype == 1)
        {
            // 예각삼각형


        }else if(gametype == 2)
        {
            // 직각삼각형


        }else if(gametype == 3)
        {
            // 둔각삼각형


        }else if(gametype == 4)
        {
            // 사다리꼴


        }else if(gametype == 5)
        {
            // 직투정


        }
        else
        {
            // 합동삼각형


        }
        //Debug.Log("new");

        problemTriangle = new Vector2[6];
        /*for(int i = 0; i < 3; i++)
        {
            problemTriangle[i].x = UnityEngine.Random.Range(-3.5f, 2f);
            problemTriangle[i].y = UnityEngine.Random.Range(-3f, 2.5f);
        }*/

        /*problemTriangle[0].x = 0;
        problemTriangle[0].y = 0;
        problemTriangle[1].x = 3;
        problemTriangle[1].y = 0;
        problemTriangle[2].x = 3;
        problemTriangle[2].y = 2.5f;
        problemTriangle[3].x = 2.5f;
        problemTriangle[3].y = 2.5f;
        problemTriangle[4].x = 2.5f;
        problemTriangle[4].y = 3;
        problemTriangle[5].x = 0;
        problemTriangle[5].y = 3;

        foreach (GameObject p in polygonList)
        {
            Destroy(p);
        }
        polygonList.Clear();
        */

        var firstTriangle = new GameObject("Polygon");
        firstTriangle.AddComponent(System.Type.GetType("Polygon"));
        firstTriangle.GetComponent<Polygon>().render(problemTriangle);
        polygonList.Add(firstTriangle);
    }

    // TODO: NEED TO CONNECT SUCCESS WITH UI CODE(Sucess() function)
    public void isSolved()
    {

        if (polygonList.Count != 1) return;


        // there is only one element left, so candidate status
        Vector2[] reference = polygonList[0].GetComponent<Polygon>().VerticesPublic2D;


        if (reference.Length != 4) return;


        float centerX = 0;
        float centerY = 0;
        for (int i = 0; i < 4; i++)
        {
            centerX += reference[i].x;
            centerY += reference[i].y;
        }
        centerX /= 4;
        centerY /= 4;

        float[] distance = new float[4];
        for (int i = 0; i < 4; i++)
        {
            distance[i] = (centerX - reference[i].x) * (centerX - reference[i].x) + (centerY - reference[i].y) * (centerY - reference[i].y);
        }

        if (Math.Abs(distance[0] - distance[1]) <= 0.01 && Math.Abs(distance[0] - distance[2]) <= 0.01 && Math.Abs(distance[0] - distance[3]) <= 0.01)
        {
            //Debug.Log("Solved");
            // Success
            // Success(); 
            EC.AddScore();
        }
        else
        {
            //Debug.Log("Unsolved");
            // not a rectangle
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
