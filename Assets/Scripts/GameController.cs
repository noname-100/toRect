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
    public GameObject EC;
    private EventController ec;
    private StoryScript ss;
    private List<Vector2[]> backgroundBorders;
    private List<Vector2> backgroundMidpoints;
    private List<float> maxLength;
    private Vector2[] vertexes;

    private void Awake()
    {
        ss = EC.GetComponent<StoryScript>();
        ec = EC.GetComponent<EventController>();

        backgroundBorders = new List<Vector2[]>();
        backgroundMidpoints = new List<Vector2>();
        // 외곽 경계값
        Vector2[] tmp = { new Vector2(-3f, -2f), new Vector2(-3f, 1.8f), new Vector2(2f, 1.8f), new Vector2(2f, -2f) }; // 투렉트
        Vector2[] tmp2 = { new Vector2(-3.8f, -2.2f), new Vector2(-3.8f, 1.51f), new Vector2(0.9f, 1.51f), new Vector2(0.9f, -2.2f) }; // 직투정, 합동삼각형
        maxLength = new List<float>();
        maxLength.Add(2.5f);
        maxLength.Add(2.35f);
        backgroundBorders.Add(tmp);
        backgroundBorders.Add(tmp2);
        // 중점
        for (int j = 0; j < 2; j++)
        {
            float midpointsTmpx = 0;
            float midpointsTmpy = 0;
            for (int i = 0; i < 4; i++)
            {
                midpointsTmpx += backgroundBorders[j][i].x;
                midpointsTmpy += backgroundBorders[j][i].y;
            }
            backgroundMidpoints.Add(new Vector2(midpointsTmpx / 4, midpointsTmpy / 4));
        }

        return;
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

        /*Debug.Log(gameType);
        firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeJig());
        polygonList.Add(firstTriangle);
        return;*/

        switch (gameType)
        {
            case 0: // 예각1
                vertexes = MakePolygon.MakeTriangle(1);
                break;
            case 1: // 예각2
                vertexes = MakePolygon.MakeTriangle(1);
                break;
            case 2: // 직각
                vertexes = MakePolygon.MakeTriangle(0);
                break;
            case 3: // 둔각
                vertexes = MakePolygon.MakeTriangle(2);
                break;
            case 4: // 다각형1(사다리꼴)
                vertexes = MakePolygon.MakeTrapezoid();
                break;
            case 5: // 다각형2(사각형) TODO : 생성함수
                vertexes = MakePolygon.MakeTriangle(2);
                break;
            case 6: // 다각형2(오각형) TODO : 생성함수
                vertexes = MakePolygon.MakeTriangle(2);
                break;
            case 7: // 다각형3(팔각형) TODO : 생성함수
                vertexes = MakePolygon.MakeTriangle(2);
                break;
            case 8: // 직투정
                vertexes = MakePolygon.MakeJig();
                break;
            case 9: // 직투정2 TODO : 생성함수
                vertexes = MakePolygon.MakeJig();
                break;
        }

        // normalize here
        // 가장 긴 변이 최대 범위의 50% ~ 80% 범위로 랜덤 비율적용되게 하고, 다른 변들도 그렇게 적용한다.

        if (gameType <= 9)
        {
            float currmidx = 0; float currmidy = 0;
            for (int i = 0; i < vertexes.Length; i++)
            {
                currmidx += vertexes[i].x;
                currmidy += vertexes[i].y;
            }
            currmidx /= vertexes.Length; currmidy /= vertexes.Length;
            float backgroundMidPointx = backgroundMidpoints[gameType <= 7 ? 0 : 1].x; // GAME TYPE HARD CODED HERE : map type
            float backgroundMidPointy = backgroundMidpoints[gameType <= 7 ? 0 : 1].y;
            Vector2 diff = new Vector2(currmidx - backgroundMidPointx, currmidy - backgroundMidPointy);
            Vector2[] vectors = new Vector2[vertexes.Length];
            float maxlength = -99999999f;
            for (int i = 0; i < vertexes.Length; i++)
            {
                vertexes[i].x -= diff.x;
                vertexes[i].y -= diff.y;
                vectors[i].x = vertexes[i].x - backgroundMidPointx;
                vectors[i].y = vertexes[i].y - backgroundMidPointy;
                if (Mathf.Pow(vectors[i].x, 2) + Mathf.Pow(vectors[i].y, 2) > maxlength) maxlength = Mathf.Pow(vectors[i].x, 2) + Mathf.Pow(vectors[i].y, 2);
            }
            float tomatch = UnityEngine.Random.Range(0.6f * maxLength[gameType <= 7 ? 0 : 1], 0.67f * maxLength[gameType <= 7 ? 0 : 1]); // GAME TYPE HARD CODED HERE : map type
            float proportion = 4f * tomatch / maxlength;

            Vector2[] result = new Vector2[vertexes.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i].x = backgroundMidPointx + proportion * vectors[i].x;
                result[i].y = backgroundMidPointy + proportion * vectors[i].y;
            }

            firstTriangle.GetComponent<Polygon>().render(result);
            polygonList.Add(firstTriangle);

            // rotate here
            // 중심을 기준으로 점수와 콤보수에 따라 영향을 받는 회전각도를 적용한다(점수, 콤보가 높을수록 120 ~ 240도 문제가 많이 나오도록).

            float rotateangle = UnityEngine.Random.Range(0f, 360f);
            firstTriangle.transform.RotateAround(new Vector3(backgroundMidPointx, backgroundMidPointy, 0), Vector3.forward, rotateangle);

            // 넓이를 검사해서 너무 작은 삼각형은 다시 한다.
            Mesh mesh = firstTriangle.GetComponent<MeshFilter>().mesh;
            Vector3[] meshVertices = mesh.vertices;
            Vector3 area = Vector3.zero;
            for (int p = meshVertices.Length - 1, q = 0; q < meshVertices.Length; p = q++)
            {
                area += Vector3.Cross(meshVertices[q], meshVertices[p]);
            }
            area *= 0.5f;
            if (area.magnitude < 3.8f)
            {
                Debug.Log("remaking..");
                Destroy(firstTriangle);
                polygonList.RemoveAt(polygonList.Count - 1);
                makeNew(gameType);
            }
        }
        else
        {
            // 합동삼각형은 여기 생성부에서 자체처리 + 렌더 + 리스트추가
            Debug.Log("Similarity triangle problem called");
            firstTriangle.GetComponent<Polygon>().render(MakePolygon.MakeTriangle(0));
            polygonList.Add(firstTriangle);

        }



        return;
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
            //Debug.Log("polyon size is not 1" + polygonList.Count);
            return false;
        }

        Vector3[] reference = polygonList[0].GetComponent<Polygon>().vertices3D;
        if (reference == null)
        {
            //Debug.Log("reference not read properly. returns null");
            return false;
        }

        if (reference.Length != 4)
        {
            //Debug.Log("edge not four" + reference.Length);
            for(int i = 0; i < reference.Length; i++)
            {
                //Debug.Log(i + " " + "x : " + reference[i].x + " " + "y : " + reference[i].y);
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
            //Debug.Log("calculated, but not rectangle");
            return false;
        }          
    }
}