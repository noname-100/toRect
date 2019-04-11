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
    private StoryScript ss;

    private void Awake()
    {
        ss = EC.GetComponent<StoryScript>();
    }

    public void makeNew(int gametype)
    {

        foreach(GameObject p in polygonList)
        {
            Destroy(p);
        }
        polygonList.Clear();

        problemTriangle = new Vector2[6];

        for (int i = 0; i < 3; i++)
        {
            problemTriangle[i].x = UnityEngine.Random.Range(-3.5f, 2f);
            problemTriangle[i].y = UnityEngine.Random.Range(-3f, 2.5f);
        }
        

        if (gametype == 0)
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

        var firstTriangle = new GameObject("Polygon");
        firstTriangle.AddComponent(System.Type.GetType("Polygon"));
        firstTriangle.GetComponent<Polygon>().render(problemTriangle);
        polygonList.Add(firstTriangle);
    }

    public int isSolved()
    {
        if (polygonList.Count != 1) return 0;

        Vector2[] reference = polygonList[0].GetComponent<Polygon>().VerticesPublic2D;

        if (reference.Length != 4) return 0;

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
            return 1;
        else
            return 0;
    }
}