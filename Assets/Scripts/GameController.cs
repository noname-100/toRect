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

    public void makeNew(int gameType)
    {

        foreach (GameObject p in polygonList)
        {
            Destroy(p);
        }
        polygonList.Clear();

        var firstTriangle = new GameObject("Polygon");
        firstTriangle.AddComponent(System.Type.GetType("Polygon"));

        switch (gameType)
        {
            case 0: // 예각1
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
                break;
            case 1: // 예각2
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
                break;
            case 2: // 직각
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(0));
                break;
            case 3: // 둔각
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(2));
                break;
            case 4: // 다각형1(사다리꼴)
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTrapezoid());
                break;
            case 5: // 다각형2(사각형) TODO : 생성함수
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(2));
                break;
            case 6: // 다각형2(오각형) TODO : 생성함수
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(2));
                break;
            case 7: // 다각형3(팔각형) TODO : 생성함수
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(2));
                break;
            case 8: // 직투정
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeJig());
                break;
            case 9: // 직투정2 TODO : 생성함수
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeJig());
                break;
            case 10: // 합동삼각형 TODO : 생성함수
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
                break;
            case 11: // 합동삼각형2 TODO : 생성함수
                firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
                break;
        }
        polygonList.Add(firstTriangle);
    }

    public bool isSolvedSimilarity()
    {
        return false;
    }

    public bool isSolvedRec2Square()
    {
        return false;
    }

    public bool isSolvedRect()
    {
        if (polygonList.Count != 1)
        {
            Debug.Log("polyon size is not 1" + polygonList.Count);
            return false;
        }

        Vector3[] reference = polygonList[0].GetComponent<Polygon>().vertices3D;
        if (reference == null)
        {
            Debug.Log("reference not read properly. returns null");
            return false;
        }

        if (reference.Length != 4)
        {
            Debug.Log("edge not four" + reference.Length);
            for(int i = 0; i < reference.Length; i++)
            {
                Debug.Log(i + " " + "x : " + reference[i].x + " " + "y : " + reference[i].y);
            }
            return false;
        }

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

        if (Math.Abs(distance[0] - distance[1]) <= 100 && Math.Abs(distance[0] - distance[2]) <= 100 && Math.Abs(distance[0] - distance[3]) <= 100)
        {
            return true;
        }
        else
        {
            Debug.Log("calculated, but not rectangle");
            return false;
        }          
    }
}