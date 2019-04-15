using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePolygon : MonoBehaviour
{
    public static Vector2[] vertices;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector2[] MakeTriangle(int mode)
    {
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
    public static Vector2[] MakeTrapezoid()
    {
        vertices = new Vector2[4];
        Polygon.jiktojung = false;
        float t = Random.Range(3f,5f);
        vertices[0].x = -t/2;
        vertices[0].y = 0;
        vertices[1].x = Random.Range(-t/2,t/2);
        vertices[1].y = Random.Range(3f, 5f);
        vertices[2].x = Random.Range(vertices[1].x,t/2);
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
}
