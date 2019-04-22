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
    private MakePolygon mp;
    private EventController ec;
    private StoryScript ss;
    private List<Vector2[]> backgroundBorders;
    private List<Vector2> backgroundMidpoints;
    private List<float> maxLength;
    private Vector2[] vertexes;
    private int counter;

    // 중요 : 출제 변경시 여기에서 범위 변경 필수!!!
    private int biscuitProblems = 9;
    private int rec2squareProblems = 11;
    private int similarityProblems = 13;

    private void Awake()
    {
        counter++;
        ss = EC.GetComponent<StoryScript>();
        ec = EC.GetComponent<EventController>();
        mp = gameObject.GetComponent<MakePolygon>();

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
        Debug.Log(counter + " called " + gameType);
        counter++;
        foreach (GameObject p in polygonList)
        {
            Destroy(p);
        }
        polygonList.Clear();
        

        // 출제변경시 여기의 biscuitProblems 등 변수 전환 + buttoncontroller_title 변수 전환, 
        switch (gameType)
        {
            case 0: // 예각1
                vertexes = MakePolygon.MakeTriangle(1);
                break;
            case 1: // 예각2
                vertexes = MakePolygon.MakeParallelogram();
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
                vertexes = MakePolygon.MakeQuadrangle();
                break;
            case 6: // 다각형2(오각형) TODO : 생성함수
                vertexes = MakePolygon.MakePentagon();
                break;
            case 7: // 다각형3(육각형) TODO : 생성함수
                vertexes = MakePolygon.MakeHexagon();
                break;
            case 8: // 다각형3(칠각형) TODO : 생성함수
                vertexes = MakePolygon.MakeHeptagon();
                break;
            case 9: // 다각형3(팔각형) TODO : 생성함수
                vertexes = MakePolygon.MakeOctagon();
                break;
            case 10: // 직투정
                vertexes = MakePolygon.MakeJig();
                break;
            case 11: // 직투정2 TODO : 생성함수
                vertexes = MakePolygon.MakeJig();
                break;
        }

        //vertexes = MakePolygon.MakeQuadrangle();

        // normalize here
        // 가장 긴 변이 최대 범위의 50% ~ 80% 범위로 랜덤 비율적용되게 하고, 다른 변들도 그렇게 적용한다.

        if (gameType <= 11) // GAME TYPE HARD CODED HERE : 투렉트 + 직투정 생성
        {
            var firstTriangle = new GameObject("Polygon");
            firstTriangle.AddComponent(System.Type.GetType("Polygon"));
            float currmidx = 0; float currmidy = 0;
            for (int i = 0; i < vertexes.Length; i++)
            {
                currmidx += vertexes[i].x;
                currmidy += vertexes[i].y;
            }
            currmidx /= vertexes.Length; currmidy /= vertexes.Length;
            float backgroundMidPointx = backgroundMidpoints[gameType <= biscuitProblems ? 0 : 1].x;
            float backgroundMidPointy = backgroundMidpoints[gameType <= biscuitProblems ? 0 : 1].y;
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
            float tomatch = UnityEngine.Random.Range(0.6f * maxLength[gameType <= biscuitProblems ? 0 : 1], 0.67f * maxLength[gameType <= biscuitProblems ? 0 : 1]);
            float proportion = 1.5f * tomatch / Mathf.Pow(maxlength,0.5f);

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
            Debug.Log("area " + area.magnitude);
            if (area.magnitude < 2.4f)
            {
                Debug.Log(counter + " remaking..");
                Destroy(firstTriangle);
                polygonList.RemoveAt(polygonList.Count - 1);
                makeNew(gameType);
            }
        }
        else
        {
            // 합동삼각형은 여기 생성부에서 자체처리 + 렌더 + 리스트추가
            List<Vector2[]> similarTriangles = new List<Vector2[]>();
            similarTriangles = mp.MakeSimilars();
            var similarTriangle1 = new GameObject("Polygon");
            var similarTriangle2 = new GameObject("Polygon");
            var similarTriangle3 = new GameObject("Polygon");
            similarTriangle1.AddComponent(System.Type.GetType("Polygon"));
            similarTriangle2.AddComponent(System.Type.GetType("Polygon"));
            similarTriangle3.AddComponent(System.Type.GetType("Polygon"));
            similarTriangle1.GetComponent<Polygon>().render(similarTriangles[0]);
            similarTriangle2.GetComponent<Polygon>().render(similarTriangles[1]);
            similarTriangle3.GetComponent<Polygon>().render(similarTriangles[2]);
            polygonList.Add(similarTriangle1);
            polygonList.Add(similarTriangle2);
            polygonList.Add(similarTriangle3);

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
            // TODO : 이거 그냥 모든 사각형이면 성립하잖아ㅋㅋ
            return false;
        }
        else
        {
            //Debug.Log("calculated, but not rectangle");
            return false;
        }          
    }

    public int getBiscuitProblems()
    {
        return biscuitProblems;
    }

    public int getRec2SquareProblems()
    {
        return rec2squareProblems;
    }

    public int getSimilarityProblems()
    {
        return similarityProblems;
    }
}