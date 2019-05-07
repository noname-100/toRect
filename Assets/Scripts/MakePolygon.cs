using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 *  문제 출제 클래스
 * 
 */

public class MakePolygon : MonoBehaviour
{
    public static Vector2[] vertices;
    public GameObject EC;
    public GameObject plate;
    private EventController ec;
    private SpriteRenderer sprite;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {

        ec = EC.GetComponent<EventController>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector2[] MakeSquare(float length, float x, float y, float angle)
    {

        vertices = new Vector2[4];
        float radius = (length / Mathf.Pow(2, 0.5f));
        Polygon.jiktojung = true;
        Polygon.jungtojik = false;
        vertices[0].x = 0 + x;
        vertices[0].y = 0 + y;
        vertices[1].x = 0 + x;
        vertices[1].y = length + y;
        vertices[2].x = length + x;
        vertices[2].y = length + y;
        vertices[3].x = length + x;
        vertices[3].y = 0 + y;

        return vertices;

    }

    public static Vector2[] MakeTriangle(int mode)
    {
        // TODO : normalize
        vertices = new Vector2[3];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        if (mode==0){//직각
            vertices[0].x = 0;
            vertices[0].y = 0;
            vertices[1].x = 0;
            vertices[1].y = Random.Range(3f,5f);
            vertices[2].x = Random.Range(3f,5f);
            vertices[2].y = 0;
        }else if(mode==1){//예각
            float t = Random.Range(3f,5f);
            vertices[0].x = -t/2;
            vertices[0].y = 0;
            vertices[1].x = Random.Range(-t/2,t/2);
            vertices[1].y = Mathf.Sqrt(Mathf.Pow(t,2)/4-Mathf.Pow(vertices[1].x,2))+Random.Range(0,3f);
            vertices[2].x = t/2;
            vertices[2].y = 0;
        }else{//둔각
            float t = Random.Range(3f,5f);
            vertices[0].x = -t/2;
            vertices[0].y = 0;
            vertices[1].x = Random.Range(-t/2,t/2);
            vertices[1].y = Random.Range(Mathf.Sqrt(Mathf.Pow(t,2)/4-Mathf.Pow(vertices[1].x,2))/2,Mathf.Sqrt(Mathf.Pow(t,2)/4-Mathf.Pow(vertices[1].x,2)));
            vertices[2].x = t/2;
            vertices[2].y = 0;
        }
        return vertices;
    }

    // 평행사변형
    public static Vector2[] MakeParallelogram()
    {
        vertices = new Vector2[4];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        float t = Random.Range(3f,5f);
        vertices[0].x = -t/2;
        vertices[0].y = 0;
        vertices[1].x = Random.Range(-t/2, t);
        vertices[1].y = Random.Range(3f, 5f);
        vertices[2].x = vertices[1].x + t;
        vertices[2].y = vertices[1].y;
        vertices[3].x = t/2;
        vertices[3].y = 0;
        return vertices;
    }

    // 직투정
    public static Vector2[] MakeJig()
    {
        vertices = new Vector2[8];
        Polygon.jiktojung = true;
        Polygon.jungtojik = false;
        float t = Random.Range(3f,5f);
        vertices[0].x = -t/2;
        vertices[0].y = 0;
        vertices[1].x = -t/2;
        vertices[1].y = Random.Range(t - 1.2f, t - 0.6f);
        vertices[2].x = -t/2 + vertices[1].y;
        vertices[2].y = vertices[1].y;
        vertices[3].x = -t/2 + vertices[1].y + (t-vertices[1].y)/2;
        vertices[3].y = vertices[1].y;
        vertices[4].x = t/2;
        vertices[4].y = vertices[1].y;
        vertices[5].x = t/2;
        vertices[5].y = 0;
        vertices[6].x = -t/2 + vertices[1].y + (t-vertices[1].y)/2;;
        vertices[6].y = 0;
        vertices[7].x = -t/2 + vertices[1].y;
        vertices[7].y = 0;
        Polygon.jiktojunglength = 0.5f * (t - vertices[1].y);
        // Debug.Log(t + " " + Polygon.jiktojunglength);
        return vertices;
    }
    //정투직
    
    public static Vector2[] MakeJung()
    {
        vertices = new Vector2[6];
        Polygon.jiktojung = false;
        Polygon.jungtojik = true;
        float t = Random.Range(3f,5f);
        float k = Random.Range(0.3f, 0.6f);
        vertices[0].x = -t/2;
        vertices[0].y = t/2;
        vertices[1].x = t/2;
        vertices[1].y = t/2;
        vertices[2].x = t/2;
        vertices[2].y = -t/2+k;
        vertices[3].x = t/2-k;
        vertices[3].y = -t/2+k;
        vertices[4].x = t/2-k;
        vertices[4].y = -t/2;
        vertices[5].x = -t/2;
        vertices[5].y = -t/2;
        return vertices;
    }
    
    // 사다리꼴
    public static Vector2[] MakeTrapezoid()
    {
        vertices = new Vector2[4];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        float t = Random.Range(3f,5f);
        vertices[0].x = -t/2;
        vertices[0].y = 0;
        vertices[1].x = Random.Range(-t/2,t/2);
        vertices[1].y = Random.Range(3f, 5f);
        vertices[2].x = Random.Range(vertices[1].x + 1.5f,t/2);
        vertices[2].y = vertices[1].y;
        vertices[3].x = t/2;
        vertices[3].y = 0;
        return vertices;
    }

    public static Vector2[] MakeRectangle()
    {
        vertices = new Vector2[4];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        vertices[0].x = 0;
        vertices[0].y = 0;
        vertices[1].x = 2;
        vertices[1].y = 0;
        vertices[2].x = 2;
        vertices[2].y = 3;
        vertices[3].x = 0;
        vertices[3].y = 3;
        return vertices;
    }

    public static Vector2[] MakeQuadrangle()
    {
        vertices = new Vector2[4];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        vertices[0].x = 0;
        vertices[0].y = 0;
        vertices[1].x = Random.Range(-0.3f , 0.3f );
        vertices[1].y = Random.Range(0.3f , 0.7f );
        vertices[2].x = Random.Range(0.7f , 1.3f );
        vertices[2].y = Random.Range(0.3f , 0.7f );
        vertices[3].x = 1;
        vertices[3].y = 0;

        return vertices;
    }

    public static Vector2[] MakePentagon()
    {
        vertices = new Vector2[5];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        vertices[0].x = 0f;
        vertices[0].y = -2.00000f;
        vertices[1].x = -1.90211f;
        vertices[1].y = -0.61803f;
        vertices[2].x = -1.17557f;
        vertices[2].y = 1.61803f;
        vertices[3].x = 1.17557f;
        vertices[3].y = 1.61803f;
        vertices[4].x = 1.90211f;
        vertices[4].y = -0.61803f;

        return vertices;
    }

    public static Vector2[] MakeHexagon()
    {
        vertices = new Vector2[6];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        vertices[0].x = 5f;
        vertices[0].y = -8.660f;
        vertices[1].x = -5f;
        vertices[1].y = -8.660f;
        vertices[2].x = -10f;
        vertices[2].y = 0f;
        vertices[3].x = -5f;
        vertices[3].y = 8.660f;
        vertices[4].x = 5f;
        vertices[4].y = 8.660f;
        vertices[5].x = 10f;
        vertices[5].y = 0f;

        return vertices;
    }

    public static Vector2[] MakeHeptagon()
    {
        vertices = new Vector2[7];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        vertices[0].x = 0f;
        vertices[0].y = -10f;
        vertices[1].x = -7.818f;
        vertices[1].y = -6.235f;
        vertices[2].x = -9.749f;
        vertices[2].y = 2.225f;
        vertices[3].x = -4.339f;
        vertices[3].y = 9.010f;
        vertices[4].x = 4.339f;
        vertices[4].y = 9.010f;
        vertices[5].x = 9.749f;
        vertices[5].y = 2.225f;
        vertices[6].x = 7.818f;
        vertices[6].y = -6.235f;

        return vertices;
    }

    public static Vector2[] MakeOctagon()
    {
        vertices = new Vector2[8];
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        vertices[0].x = 3.827f;
        vertices[0].y = -9.239f;
        vertices[1].x = -3.827f;
        vertices[1].y = -9.239f;
        vertices[2].x = -9.239f;
        vertices[2].y = -3.827f;
        vertices[3].x = -9.239f;
        vertices[3].y =  3.827f;
        vertices[4].x = -3.827f;
        vertices[4].y = 9.239f;
        vertices[5].x = 3.827f;
        vertices[5].y = 9.239f;
        vertices[6].x = 9.239f;
        vertices[6].y = 3.827f;
        vertices[7].x = 9.239f;
        vertices[7].y = -3.827f;

        return vertices;
    }

    public List<Vector2[]> MakeSimilars()
    {
        List<Vector2[]> triangles = new List<Vector2[]>();
        Polygon.jiktojung = false;
        Polygon.jungtojik = false;
        float a = 2.5f;
        float b = a * Random.Range(1f, 2.1f);
        float rightedge = 1.5f;
        float dist = 0.45f;
        Vector2[] triangle = new Vector2[3];
        float coeff = 1f / (float)Mathf.Pow(b * b + a * a, 0.5f); // 1/(a^2+b^2)^0.5
        float xzero = 4.7f - 2 * dist - a - a * b * coeff - a * coeff; // 바닥 오른쪽에서의 이격
        xzero /= 2f;
        float groundy = -0.345f - b / 2f; // 합동삼각형 바닥 좌표

        // 맨 오른쪽 가장 큰 삼각형
        triangle[0].x = rightedge - xzero;
        triangle[0].y = groundy;
        triangle[1].x = rightedge - a - xzero;
        triangle[1].y = groundy;
        triangle[2].x = rightedge - xzero;
        triangle[2].y = groundy + b;
        triangles.Add(triangle);

        // 중간 삼각형
        triangle = new Vector2[3];
        triangle[0].x = rightedge - dist - a - xzero;
        triangle[0].y = groundy;
        triangle[1].x = rightedge - dist - a - xzero - a * b * coeff;
        triangle[1].y = groundy;
        triangle[2].x = rightedge - dist - a - xzero;
        triangle[2].y = groundy + + b*b*coeff;
        triangles.Add(triangle);

        // 맨 왼쪽 작은 삼각형
        triangle = new Vector2[3];
        triangle[0].x = rightedge - 2 * dist - a - xzero - a * b * coeff;
        triangle[0].y = groundy;
        triangle[1].x = rightedge - 2 * dist - a - xzero - a * b * coeff - a * a * coeff;
        triangle[1].y = groundy;
        triangle[2].x = rightedge - 2 * dist - a - xzero - a * b * coeff;
        triangle[2].y = groundy + a * b * coeff;
        triangles.Add(triangle);

        // resize plate
        sprite = plate.GetComponent<SpriteRenderer>();
        
        //Debug.Log("a : " + a + " b : " + b);
        //Debug.Log(counter + "before x : " + 2 * sprite.bounds.extents.x + " y : " + 2 * sprite.bounds.extents.y);
        //Debug.Log(a / (2 * sprite.bounds.extents.x));
        //Debug.Log(b / (2 * sprite.bounds.extents.y));
        plate.transform.localScale = new Vector3(b / (2 * sprite.bounds.extents.y) + 0.1f, a / (2 * sprite.bounds.extents.x)+ 0.1f, 0);
        //plate.transform.localScale = new Vector3(2, 1, 0);
        //Debug.Log(counter + "after x : " + 2 * sprite.bounds.extents.x + " y : " + 2 * sprite.bounds.extents.y);
        

        counter++;
        return triangles;
    }
}
