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

        if (gameType == 0)
        {
            firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
        }
        else if(gameType == 1)
        {
            firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
        }
        else if(gameType == 2)
        {
            firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(0));
        }
        else if(gameType == 3)
        {
            firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(2));
        }
        else if(gameType == 4)
        {   // TODO : need fix
            firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
        }
        else if(gameType == 5)
        {   // TODO : need fix
            firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
        }
        
        //firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(0));
        //firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(1));
        //firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(2));
        //firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeParallelogram());
        //firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTrapezoid());
        //firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeJig());
        polygonList.Add(firstTriangle);
    }

    public int isSolved()
    {
        if (polygonList.Count != 1) return 0;

        Vector2[] reference = polygonList[0].GetComponent<Polygon>().VerticesPublic2D;
        if (reference == null) return 0;

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