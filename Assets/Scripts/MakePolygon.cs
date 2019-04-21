using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePolygon : MonoBehaviour
{
    public static Vector2[] vertices;
    public GameObject EC;
    private EventController ec;

    // Start is called before the first frame update
    void Start()
    {

        ec = EC.GetComponent<EventController>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector2[] MakeTriangle(int mode)
    {
        // TODO : normalize
        vertices = new Vector2[3];
        Polygon.jiktojung = false;
        if(mode==0){//직각
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
        vertices = new Vector2[6];
        Polygon.jiktojung = true;
        float t = Random.Range(3f,5f);
        vertices[0].x = -t/2;
        vertices[0].y = 0;
        vertices[1].x = -t/2;
        vertices[1].y = Random.Range(0, t);
        vertices[2].x = -t/2 + vertices[1].y;
        vertices[2].y = vertices[1].y;
        vertices[3].x = t/2;
        vertices[3].y = vertices[1].y;
        vertices[4].x = t/2;
        vertices[4].y = 0;
        vertices[5].x = -t/2 + vertices[1].y;
        vertices[5].y = 0;
        return vertices;
    }

    // 사다리꼴
    public static Vector2[] MakeTrapezoid()
    {
        vertices = new Vector2[4];
        Polygon.jiktojung = false;
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

        vertices[0].x = 0f;
        vertices[0].y = -2.000f;
        vertices[1].x = -1.902f;
        vertices[1].y = -0.618f;
        vertices[2].x = -1.176f;
        vertices[2].y = 1.618f;
        vertices[3].x = 1.176f;
        vertices[3].y = 1.618f;
        vertices[4].x = 1.902f;
        vertices[4].y = -0.618f;

        return vertices;
    }

    public static Vector2[] MakeHexagon()
    {
        vertices = new Vector2[6];

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
}
