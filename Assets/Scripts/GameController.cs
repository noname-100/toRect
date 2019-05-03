using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

/*
 * 
 * GameController은 문제 및 문제풀이 도구 생성과 정답 처리 함수를 포함한다.
 * 
 */

public class GameController : MonoBehaviour
{

    // 문제생성 관련
    public List<GameObject> polygonList;
    public bool polygonSelected;
    private Vector2[] problemTriangle;
    private List<Vector2[]> backgroundBorders;
    private List<Vector2> backgroundMidpoints;
    private List<float> maxLength;
    private Vector2[] vertexes;
    private int counter;

    // 게임 관련
    public GameObject EC;
    private MakePolygon mp;
    private EventController ec;
    private StoryScript ss;
    public GameObject Plate;
    public GameObject ScoreSign;
    public GameObject chocodish;
    public Text formula1, formula2, formula3;

    // 중요 : 출제 변경시 여기에서 범위 변경 필수!!!
    private int biscuitProblems = 9;
    private int rec2squareProblems = 11;
    private int similarityProblems = 14;
    // 공식출제종류변수
    private int formulaProblems = 5;
    private int formulaAnswer = 0;

    private void Awake()
    {
        // dish.transform.position = new Vector3(5.46f, 1.62f, 0);
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
        // Debug.Log(counter + " called " + gameType);
        // counter++;
        foreach (GameObject p in polygonList)
        {
            Destroy(p);
        }
        polygonList.Clear();

        /*
         * 
         *  TEST CODE : MakePolygon에서 새로운 형태 추가했을 경우 테스트하는 곳이다.
         * 
         */
        
        /*GameObject polygon = new GameObject("Polygon");
        polygon.AddComponent(System.Type.GetType("Polygon"));
        Vector2[] v = MakePolygon.MakeJig();
        polygon.GetComponent<Polygon>().render(v);
        polygonList.Add(polygon);
        Vector2[] s = MakePolygon.MakeSquare(Polygon.jiktojunglength,0,0,0);
        GameObject polygon2 = new GameObject("Polygon");
        polygon2.AddComponent(System.Type.GetType("Polygon"));
        polygon2.GetComponent<Polygon>().render(s);
        polygonList.Add(polygon2);
        ec.Debug_KillAnswerCheck();
        return;*/
        

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
            case 11: // 직투정2
                vertexes = MakePolygon.MakeJig();
                break;
        }

        // normalize here
        // 가장 긴 변이 최대 범위의 60% ~ 67% 범위로 랜덤 비율적용되게 하고, 다른 변들도 그렇게 적용한다.

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
            Polygon.jiktojunglength *= proportion;

            Vector2[] result = new Vector2[vertexes.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i].x = backgroundMidPointx + proportion * vectors[i].x;
                result[i].y = backgroundMidPointy + proportion * vectors[i].y;
            }
            /*
            float tmpx = Mathf.Pow(Mathf.Pow(vertexes[0].x - vertexes[1].x, 2), 0.5f) + Mathf.Pow(Mathf.Pow(vertexes[0].y - vertexes[1].y, 2), 0.5f);
            float tmpy = Mathf.Pow(Mathf.Pow(result[0].x - result[1].x, 2), 0.5f) + Mathf.Pow(Mathf.Pow(result[0].y - result[1].y, 2), 0.5f);
            Debug.Log("proportion : " + proportion);
            Debug.Log("before : " + tmpx + " after : " + tmpy);
            */
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
            // Debug.Log("area " + area.magnitude);
            if (area.magnitude < 2.4f)
            {
                // Debug.Log(counter + " remaking..");
                Destroy(firstTriangle);
                polygonList.RemoveAt(polygonList.Count - 1);
                makeNew(gameType);
            }

            if (Polygon.jiktojung)
            {
                GenerateSquares(); // 초코칩 생성함수
                MakeFormulas(); // 수식 생성함수
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

    private Vector2 APointOnFryPan()
    {
        Vector2 ret = new Vector2();

        float radiusFrypan = 1.612f - Mathf.Pow(2, 0.5f) * Polygon.jiktojunglength / 2f;
        float angle = UnityEngine.Random.Range(0f, 360f);
        float r = 0;

        // TODO : r 산정할때 정사각형 조각의 최대반경 고려해줘야함
        if (angle <= 32 && angle >= 360 - 32)
        {
            r = UnityEngine.Random.Range(0f, (1.6f / (float) Mathf.Cos(angle)) - Mathf.Pow(2, 0.5f) * Polygon.jiktojunglength / 2f);
        }
        else
        {
            r = UnityEngine.Random.Range(0f, radiusFrypan);
        }

        ret.x = r * Mathf.Cos(angle) + 6.36f;
        ret.y = r * Mathf.Sin(angle) - 2.26f;

        return ret;
    }

    private bool isColliding(Vector2 candidate, float radius, List<Vector3> Collisions)
    {
        for(int i = 0; i < Collisions.Count; i++)
        {
            Vector2 curr = new Vector2(Collisions[i].y, Collisions[i].z);
            if((curr-candidate).magnitude < radius + Collisions[i].x)
            {
                return true;
            }
        }
        return false;
    }

    public void GenerateSquares()
    {
        int howMany = (int) UnityEngine.Random.Range(3f, 5f);
        List<Vector2[]> Squares = new List<Vector2[]>();
        List<Vector3> Collisions = new List<Vector3>();
                
        for(int i = 0; i < howMany; i++)
        {
            try
            {
                int dummyVariable = 0;
                do
                {
                    dummyVariable++;
                    if (dummyVariable == 500000) throw new Exception(); // to prevent infinite loop
                    Vector2 candidate = APointOnFryPan();
                    float length;
                    float answerLength = Polygon.jiktojunglength;
                    if (i == 0) length = answerLength; // answer square size
                    else
                    {
                        if (UnityEngine.Random.Range(0f, 2f) >= 1 && Polygon.jiktojunglength >= 0.5) length = UnityEngine.Random.Range(0.35f, Polygon.jiktojunglength - 0.14f);
                        else length = UnityEngine.Random.Range(Polygon.jiktojunglength + 0.1f, 0.8f);
                    }

                    // check for collision
                    bool loopContinue = false;
                    for (int j = 0; j < Squares.Count; j++)
                    {
                        if (isColliding(candidate, length * Mathf.Pow(2, 0.5f), Collisions))
                        {
                            loopContinue = true;
                            break;
                        }
                    }
                    if (loopContinue) continue;
                    Collisions.Add(new Vector3(length * (float)Mathf.Pow(2, 0.5f), candidate.x, candidate.y));
                    Squares.Add(MakePolygon.MakeSquare(length, candidate.x, candidate.y, UnityEngine.Random.Range(0f, 360f)));
                    break;
                    
                } while (true);
            }catch(Exception e)
            {
                Debug.Log("Failed to find proper point on Frypan : Too many tries");
                howMany = Squares.Count;
            }
        }
        
        for(int i = 0; i < howMany; i++)
        {
            GameObject Square = new GameObject("Square");
            Square.AddComponent<Polygon>();
            Square.GetComponent<Polygon>().render(Squares[i]);
            polygonList.Add(Square);
        }        
        return;
    }

    public void MakeFormulas()
    {

        List<string> candidates = new List<string>();
        candidates.Add(FormulaPool(0));
        candidates.Add(FormulaPool((int)Mathf.Ceil(UnityEngine.Random.Range(0f, 5f))));
        candidates.Add(FormulaPool((int)Mathf.Ceil(UnityEngine.Random.Range(0f, 5f))));
        bool answerInserted = false;

        int curr = (int) UnityEngine.Random.Range(0f, 3f);
        if (curr == 0)
        {
            formulaAnswer = 0;
            answerInserted = true;
        }
        formula1.text = candidates[curr];
        candidates.RemoveAt(curr);

        curr = (int)UnityEngine.Random.Range(0f, 2f);
        if (curr == 0)
        {
            formulaAnswer = 1;
            answerInserted = true;
        }
        formula2.text = candidates[curr];
        candidates.RemoveAt(curr);

        if (!answerInserted) formulaAnswer = 2;
        formula3.text = candidates[0];

    }

    // TODO : X 대신 a, b, c 사용해서 공식 섞어줘야 한다.
    public string FormulaPool(int kind)
    {
        string ret;

        // TODO : need shuffle here
        char x = 'a';
        char y = 'b';
        char z = 'c';

        if(kind == 0) // TODO : ANSWER FOMULA, might differ. Logic comes here
        {
            ret = "ANSWER FORMULA COMES HERE";
        }else if(kind == 1) // (x+A)(x+B) = (x+C)^2 - D
        {
            ret = "("+x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + )" + "+y+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + " = " + "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + "^2-" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f));
        }
        else if(kind == 2) // x^2 + A = (x+B)(x+C)
        {
            ret = "("+x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + )" + "+z+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + " = " + "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + "^2-" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f));
        }
        else if(kind ==3) // TBD
        {
            ret = "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + " = " + "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + "^2-" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f));
        }
        else if (kind == 4) // TBD
        {
            ret = "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + " = " + "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + "^2-" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f));
        }
        else // TBD
        {
            ret = "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + " = " + "(x+" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f)) + ")" + "^2-" + Mathf.Ceil(UnityEngine.Random.Range(1f, 3f));
        }

        return ret;
    }

    public bool isSolvedSimilarity()
    {
        // Debug.Log("Checking Similarity Answer Candidancy");
        if(polygonList.Count != 1)
        {
            // Debug.Log("similarity answer check : polygon more than 1 : " + polygonList.Count);
            return false;
        }

        Vector3[] reference = polygonList[0].GetComponent<Polygon>().vertices3D;
        if (reference == null)
        {
            // Debug.Log("similarity reference not read properly. returns null");
            return false;
        }

        if (reference.Length != 4)
        {
            // Debug.Log("edge not four, instead : " + reference.Length);
            for (int i = 0; i < reference.Length; i++)
            {
                // Debug.Log(i + " " + "x : " + reference[i].x + " " + "y : " + reference[i].y);
            }
            return false;
        }
        
        Vector3 prevpoint = new Vector3(1,1,-45);
        Vector3 currpoint = new Vector3(1, 1, -45);
        Vector3 nextpoint = new Vector3(1, 1, -45);

        for (int i = 0; i < reference.Length + 3; i++)
        {
            int curr = i >= 4 ? i - 4 : i;
            prevpoint = currpoint;
            currpoint = nextpoint;
            nextpoint = reference[curr];
            if(!isNull(prevpoint) && !isNull(currpoint) && !isNull(nextpoint))
            {
                if (Vector3.Dot(currpoint - prevpoint, nextpoint - currpoint) > 0.01)
                {
                   // Debug.Log("found an angle not 90 : " + Vector3.Dot(currpoint - prevpoint, nextpoint - currpoint));
                    return false;
                }
            }
        }
        // Debug.Log("this is rectangle");

        // 여기에서 판 위에 직사각형이 있는지 체크한다.
        /*
        Vector3 worldPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < polygonList[0].GetComponent<Polygon>().vertices3D.Length; i++)
        {
            worldPosition += transform.TransformPoint(polygonList[0].GetComponent<Polygon>().vertices3D[i]);
        }
        worldPosition = transform.TransformPoint(polygonList[0].transform.position); /= polygonList[0].GetComponent<Polygon>().vertices3D.Length;
        Debug.Log("position : " + worldPosition.x + " " + worldPosition.y + " " + worldPosition.z);
        */

        Debug.Log("dist : " + (Plate.transform.position - polygonList[0].transform.position).magnitude);
        Debug.Log("angle : " + polygonList[0].transform.rotation.eulerAngles.z);
        if ((Plate.transform.position-polygonList[0].transform.position).magnitude > 1.6)
        {
            Debug.Log("pie not on plate");
            return false;
        }

        if(!(polygonList[0].transform.eulerAngles.z >= 10 && polygonList[0].transform.rotation.eulerAngles.z <= 50) && !(polygonList[0].transform.rotation.eulerAngles.z <= -130 && polygonList[0].transform.rotation.eulerAngles.z >= -170))
        {
            Debug.Log("pie not in right angle");
            return false;
        }

        // Debug.Log("Similarity : final answer met");
        return true;
    }

    public bool isNull(Vector3 given)
    {
        if(given.x != 1 || given.y != 1 || given.z != -45)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool isSolvedRec2Square()
    {
        for (int z = 0; z < polygonList.Count; z++)
        {

            Vector3[] reference = polygonList[z].GetComponent<Polygon>().vertices3D;
            if (reference == null)
            {
                //Debug.Log("Rec2Square reference not read properly. returns null");
                return false;
            }

            if (reference.Length != 4)
            {
                // Debug.Log("Rec2Square not four, instead : " + reference.Length);
                for (int i = 0; i < reference.Length; i++)
                {
                    //Debug.Log(i + " " + "x : " + reference[i].x + " " + "y : " + reference[i].y);
                }
                return false;
            }

            Vector3 prevpoint = new Vector3(1, 1, -45);
            Vector3 currpoint = new Vector3(1, 1, -45);
            Vector3 nextpoint = new Vector3(1, 1, -45);

            for (int i = 0; i < reference.Length + 3; i++)
            {
                int curr = i >= 4 ? i - 4 : i;
                prevpoint = currpoint;
                currpoint = nextpoint;
                nextpoint = reference[curr];
                if (!isNull(prevpoint) && !isNull(currpoint) && !isNull(nextpoint))
                {
                    if (Vector3.Dot(currpoint - prevpoint, nextpoint - currpoint) > 0.01)
                    {
                        // Debug.Log("Rec2Square found an angle not 90 : " + Vector3.Dot(currpoint - prevpoint, nextpoint - currpoint));
                        return false;
                    }
                    else
                    {
                        if ((currpoint - prevpoint).magnitude - (nextpoint - currpoint).magnitude > 0.01)
                        {
                            // Debug.Log("Rec2Square rectangle is not square, length diff : " + ((currpoint - prevpoint).magnitude - (nextpoint - currpoint).magnitude));
                            return false;
                        }
                    }
                }
            }

            // Debug.Log(z + " quadrangle is square");
        }

        // Debug.Log("All remainders are square");
        return true;
    }

    public bool isSolvedRect()
    {
        if (polygonList.Count != 1)
        {
            // Debug.Log("Biscuit answer check : polygon more than 1 : " + polygonList.Count);
            return false;
        }

        Vector3[] reference = polygonList[0].GetComponent<Polygon>().vertices3D;
        if (reference == null)
        {
            //Debug.Log("Biscuit reference not read properly. returns null");
            return false;
        }

        if (reference.Length != 4)
        {
            // Debug.Log("Biscuit edge not four, instead : " + reference.Length);
            for (int i = 0; i < reference.Length; i++)
            {
                // Debug.Log(i + " " + "x : " + reference[i].x + " " + "y : " + reference[i].y);
            }
            return false;
        }

        Vector3 prevpoint = new Vector3(1, 1, -45);
        Vector3 currpoint = new Vector3(1, 1, -45);
        Vector3 nextpoint = new Vector3(1, 1, -45);

        for (int i = 0; i < reference.Length + 3; i++)
        {
            int curr = i >= 4 ? i - 4 : i;
            prevpoint = currpoint;
            currpoint = nextpoint;
            nextpoint = reference[curr];
            if (!isNull(prevpoint) && !isNull(currpoint) && !isNull(nextpoint))
            {
                if (Vector3.Dot(currpoint - prevpoint, nextpoint - currpoint) > 0.01)
                {
                    // Debug.Log("Biscuit found an angle not 90 : " + Vector3.Dot(currpoint - prevpoint, nextpoint - currpoint));
                    return false;
                }
            }
        }

        // Debug.Log("Biscuit this is rectangle");
        return true;
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

    public int getFormulaProblems()
    {
        return formulaProblems;
    }

    public int getFormulaAnswer()
    {
        return formulaAnswer;
    }
}