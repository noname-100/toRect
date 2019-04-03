using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameController : MonoBehaviour
{
    public List<GameObject> polygonList;
    public bool polygonSelected;
    private Vector2[] problemTriangle;
    public EventController EC;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(UnityEngine.Random.Range(1f,3f));

        /* Vector2[] firstTriangleVector = new Vector2[] {

             new Vector2(0,0),
             new Vector2(0,3),
             new Vector2(3,0)
         };

     */
        makeNew();
    }

    public void makeNew()
    {
        //Debug.Log("new");

        problemTriangle = new Vector2[6];
        /*for(int i = 0; i < 3; i++)
        {
            problemTriangle[i].x = UnityEngine.Random.Range(-3.5f, 2f);
            problemTriangle[i].y = UnityEngine.Random.Range(-3f, 2.5f);
        }*/

        problemTriangle[0].x = 0;
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
