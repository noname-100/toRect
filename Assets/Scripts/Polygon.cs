using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * 
 * 가장 핵심이 되는 도형 클래스. split/merge/createdots 등의 동작 포함
 * 
 */

public class Polygon : MonoBehaviour
{
    public Vector2[] VerticesPublic2D;
    public Vector3[] vertices3D;
    private PolygonCollider2D _polygonCollider2D;
    public GameObject audioManager;
    private bool isSelected = false;
    public bool dotSelected = false;
    public bool mergeable = false;
    public static bool jiktojung = false;
    public static bool jungtojik = false;
    public static float jiktojunglength = -1f;
    private bool rendered;
    public int merger;
    private List<GameObject> dots = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        audioManager = GameObject.Find("AudioManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            if (Input.GetKeyDown("space"))
            {
                flip();
                //merge();
            }
        }
        if (dotSelected)
        {

        }
        else
        {
            foreach (GameObject dot in dots)
            {
                dot.GetComponent<Dots>().selectable = false; // **
            }
        }
        /*
        for(int i = 0 ; i < vertices3D.Count() ; i++){
            GameObject child = transform.GetChild(i).gameObject;
            LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new Vector3[] { transform.TransformPoint(vertices3D[i])-0.5f*Vector3.forward, transform.TransformPoint(vertices3D[(i+1)%vertices3D.Count()])-0.5f*Vector3.forward }); //**
        }
        */
        
        /*Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        vertices3D = mesh.vertices;*/
        //Debug.Log("vert3dlength : " + vertices3D.Length);
    }
    
    void OnDestroy()
    {
        foreach (GameObject dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
    }
    public void render(Vector2[] initVertices)
    {
        List<Vector2> verticeList = new List<Vector2>();
        VerticesPublic2D = initVertices;

        for (int i = 0; i < initVertices.Length; i++)
        {
            double incline1z = initVertices[(i + 1) % initVertices.Length][0] - initVertices[i][0];
            double incline1m = initVertices[(i + 1) % initVertices.Length][1] - initVertices[i][1];
            double incline2z = initVertices[i][0] - initVertices[(i + initVertices.Length - 1) % initVertices.Length][0];
            double incline2m = initVertices[i][1] - initVertices[(i + initVertices.Length - 1) % initVertices.Length][1];
            if (incline1z * incline2m - incline1m * incline2z < -0.01 || incline1z * incline2m - incline1m * incline2z > 0.01 || jiktojung)
            {
                verticeList.Add(initVertices[i]);
                //Debug.Log(((i + initVertices.Length-1) % initVertices.Length )+ "th vector: " + "X1 is:" + initVertices[(i + initVertices.Length - 1) % initVertices.Length][0] + "Y1 is:" + initVertices[(i + initVertices.Length - 1) % initVertices.Length][1]);
                //Debug.Log(i + "th vector: " + "X2 is:" + initVertices[i][0] + "Y2 is:" + initVertices[i][1]);
                //Debug.Log(((i+1)%initVertices.Length) + "th vector: X3 is:" + initVertices[(i + 1) % initVertices.Length][0] + "Y3 is:" + initVertices[(i + 1) % initVertices.Length][1]);
                //Debug.Log("----------------------------------");
            }

        }

        var vertices2D = verticeList.ToArray();

        gameObject.AddComponent(System.Type.GetType("Drag"));
        _polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);
        _polygonCollider2D.points = vertices2D;

        // Use the triangulator to get indices for creating triangles
        var triangulator = new Triangulator(vertices2D);
        var indices = triangulator.Triangulate();

        // Generate a color for each vertex
        Color color = Color.HSVToRGB(34f/360f, 0.165f, 1);
        if(gameObject.name=="Square"){
            color = new Color(234f/255f, 82f/255f, 61f/255f, 1);
        }
        var colors = Enumerable.Range(0, vertices3D.Length)
            .Select(i => color)
            .ToArray();

        // Create the mesh
        Mesh mesh = new Mesh
        {
            vertices = vertices3D,
            triangles = indices,
            colors = colors
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        vertices3D = mesh.vertices;
        // Set up game object with mesh;
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.sortingLayerName = "ModeBackground";
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = mesh;
        /*
        for(int i=0; i<vertices3D.Count(); i++){
            GameObject line = new GameObject("line");
            line.transform.SetParent(transform);
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth=0.03f;
            lineRenderer.endWidth=0.03f;
            lineRenderer.sortingLayerName = "ModeBackground";
            //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.material.color = new Color(1, 119f/255f, 51f/255f, 1);
        }
        */
        rendered = true;
        return;
    }
    public void OnMouseEnter()
    {
        //if (dots.Count() == 0) { createDots(); }
        if (!isSelected)
        {
            //this.GetComponent<Renderer>().material.color = Color.yellow;
        }

    }
    public void OnMouseExit()
    {
        if (!this.isSelected)
            unActivate();
    }
    public void unActivate()
    {
        foreach (GameObject dot in dots)
        {
            Destroy(dot);
        }
        dotSelected = false;
        dots.Clear();
        this.isSelected = false;
        //this.GetComponent<Renderer>().material.color = Color.green;
        return;
    }

    public void createDots()
    {
        
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        vertices3D = mesh.vertices;
        var c = vertices3D.Count();
        for (int i = 0; i < c; i++)
        {
            // vertex
            GameObject dot = Instantiate(Resources.Load("Prefabs/Circle"), transform.TransformPoint(vertices3D[i]) + new Vector3(0, 0, -1), transform.rotation) as GameObject;
            dot.name = "dot" + i;
            dot.transform.parent = gameObject.transform;
            dot.AddComponent(System.Type.GetType("Dots"));
            dot.GetComponent<Dots>().isVertice = true;
            dots.Add(dot);
            if(!jiktojung){
                // Find mid dot
                List<Vector3> midDots = new List<Vector3>();       
                midDots.Add(transform.TransformPoint((vertices3D[i] + vertices3D[(i + 1) % c]) / 2) + new Vector3(0, 0, -3));
                
                // Find perpendicular dot
                for (int j = 0; j < c - 2; j++)
                {
                    Vector3 vector = vertices3D[(i + j + 2) % c] - vertices3D[i];
                    Vector3 onNormal = vertices3D[(i + 1) % c] - vertices3D[i];
                    Vector3 perpDot = transform.TransformPoint(Vector3.Project(vector, onNormal) + vertices3D[i]);
                    if ((perpDot.x - transform.TransformPoint(vertices3D[i]).x) * (perpDot.x - transform.TransformPoint(vertices3D[(i + 1) % c]).x) < 0)
                    {
                            midDots.Add(perpDot + new Vector3(0, 0, -1));                              
                    }
                }

                midDots = midDots.OrderBy(o => (Mathf.Abs(o.x - transform.TransformPoint(vertices3D[i]).x))).ToList();
                

                // create dot instances
                foreach (Vector3 dotVector in midDots)
                {
                    if (Vector3.Distance(transform.TransformPoint(vertices3D[i]), dotVector) > 1.001 && Vector3.Distance(transform.TransformPoint(vertices3D[(i + 1) % c]), dotVector) > 1.001 && Vector3.Distance(dots[dots.Count - 1].transform.position, dotVector) > 0.001)
                    {
                        GameObject midDot = Instantiate(Resources.Load("Prefabs/Circle"), dotVector, transform.rotation) as GameObject;
                        midDot.name = "dotmid" + i;
                        midDot.transform.parent = gameObject.transform;
                        midDot.AddComponent(System.Type.GetType("Dots"));

                        // sort if mid or perp point
                        if (midDot.transform.position.z == -3)
                        {
                            midDot.GetComponent<Dots>().ismid = true;
                            Vector3 tmp = midDot.transform.position;
                            tmp.z = -1;
                            midDot.transform.position = tmp;
                        }
                        else
                        {
                            midDot.GetComponent<Dots>().isperp = true;
                        }
                        dots.Add(midDot);
                    }
                }
            }

            /*
            dot2.name = "dotm";
            dot2.transform.parent = gameObject.transform;
            dot2.AddComponent(System.Type.GetType("Dots"));
            dot2.GetComponent<Dots>().isVertice = false;
            dots.Add(dot2);
            //Find perpendicular dot
            for(int j = 0; j < c-2; j++)
            {
                Vector3 vector = vertices3D[i]- vertices3D[(j + i + 1) % c];
                Vector3 onNormal = vertices3D[(j+i+2) % c] - vertices3D[(j+i+1) % c];
                Vector3 perpDot = transform.TransformPoint(Vector3.Project(vector, onNormal)+ vertices3D[(j + i + 1) % c]);
                if((perpDot.x- transform.TransformPoint(vertices3D[(j + i + 1)%c]).x)* (perpDot.x - transform.TransformPoint(vertices3D[(j + i + 2)%c]).x)< 0){
                    GameObject dot3 = Instantiate(Resources.Load("Prefabs/Circle"), perpDot + new Vector3(0, 0, -1), transform.rotation) as GameObject;
                    dot3.name = "dotp" + i;
                    dot3.transform.parent = gameObject.transform;
                    dot3.AddComponent(System.Type.GetType("Dots"));
                    dot3.GetComponent<Dots>().isVertice = false;
                    dots.Add(dot3);
                }
            }*/          
        }

        
        int vertex1 = -1;
        int vertex2 = -1;
        List<int> midpoints = new List<int>();
        List<int> deletepoints = new List<int>();
        int midpoint = -1;
        for(int i = 0; i <= dots.Count; i++)
        {
            if (i==dots.Count || dots[i].GetComponent<Dots>().isVertice)
            {
                //Debug.Log(i + " is vertex");
                vertex2 = vertex1;
                vertex1 = i;
                if (i == dots.Count) vertex1 = 0;
                if (vertex1 != -1 && vertex2 != -1)
                {
                    for (int j = 0; j < midpoints.Count; j++)
                    {
                        if (Vector3.Distance(dots[midpoints[j]].transform.position, dots[midpoint].transform.position) / Vector3.Distance(dots[vertex1].transform.position, dots[vertex2].transform.position) < 0.07)
                        {
                            //if (dots[midpoint].GetComponent<Dots>().isVertice) Debug.Log("vertex changing to perp");
                            //Debug.Log("midpoint to overlap " + midpoint + " mm " + midpoints[j]);
                            dots[midpoint].GetComponent<Dots>().isperp = true;
                            deletepoints.Add(midpoints[j]);
                        }
                    }
                }
                midpoints.Clear();
            }
            else if (dots[i].GetComponent<Dots>().ismid)
            {
                midpoint = i;
            }
            else if(dots[i].GetComponent<Dots>().isperp)
            {
                midpoints.Add(i);
            }
        }
        
        int diff = 0;
        for(int i = 0; i < dots.Count; i++)
        {
            if (deletepoints.Count!=0 && i == deletepoints[0]-diff)
            {
                Destroy(dots[i]);
                dots.RemoveAt(i);
                deletepoints.RemoveAt(0);
                diff++;
                i--;
            }
        }              
        return;
    }

    void OnMouseUp()
    {
        
        if (!this.isSelected)
        {
            GameObject controller = GameObject.Find("GameControllerObject");
            foreach (GameObject pol in controller.GetComponent<GameController>().polygonList)
            {
                if (pol != gameObject && pol != null)
                {
                    pol.GetComponent<Polygon>().unActivate();
                }
            }
            this.isSelected = true;
            //this.GetComponent<Renderer>().material.color = Color.gray;
            createDots();
        }
        else
        {
            this.isSelected = false;
            unActivate();
            //this.GetComponent<Renderer>().material.color = Color.green;
        }
        findMerge();
        
    }
    public void selectableDots()
    {
        int index = 0;
        int c = dots.Count();
        int lowerVertex = 1;
        int higherVertex = 1;
        for (int i = 0; i < c; i++)
        {
            if (dots[i].GetComponent<Dots>().isSelected)
            {
                index = i;
                break;
            }
        }
        while (!dots[(index + c - lowerVertex) % c].GetComponent<Dots>().isVertice)
        {
            lowerVertex++;
        }
        while (!dots[(index + higherVertex) % c].GetComponent<Dots>().isVertice)
        {
            higherVertex++;
        }
        float incline1z = (dots[index].transform.position.y - dots[(index + c - lowerVertex) % c].transform.position.y);
        float incline1m = (dots[index].transform.position.x - dots[(index + c - lowerVertex) % c].transform.position.x);
        float incline2z = (dots[index].transform.position.y - dots[(index + higherVertex) % c].transform.position.y);
        float incline2m = (dots[index].transform.position.x - dots[(index + higherVertex) % c].transform.position.x);

        for (int i = 0; i < c - (lowerVertex + higherVertex + 1); i++)
        {

            dots[(i + index + higherVertex + 1) % c].GetComponent<Dots>().selectable = true; // **
            if (dots[(i + index + higherVertex + 1) % c].GetComponent<Dots>().ismid) {
                dots[(i + index + higherVertex + 1) % c].GetComponent<Renderer>().material.color = Color.black;
            }else{
                dots[(i + index + higherVertex + 1) % c].GetComponent<Renderer>().material.color = Color.blue;
            }

        }
        return;
    }
    public void cutMyself()
    {
        int index1 = 1;
        int index2 = 3;
        for (int i = 0; i < dots.Count(); i++)
        {
            if (dots[i].GetComponent<Dots>().isSelected)
            {
                index1 = i;
                break;
            }
        }
        for (int i = index1 + 1; i < dots.Count(); i++)
        {
            if (dots[i].GetComponent<Dots>().isSelected)
            {
                index2 = i;
                break;
            }
        }
        var midX= 0f;
        var midY= 0f;
        List<Vector2> firstHalf = new List<Vector2>();
        for (int i = index1; i < index2 + 1; i++)
        {
            if (i == index1 || i == index2 || dots[i].GetComponent<Dots>().isVertice)
            {
                firstHalf.Add(new Vector2(dots[i].transform.position.x, dots[i].transform.position.y));
                midX += dots[i].transform.position.x;
                midY += dots[i].transform.position.y;
            }
        }
        midX /= firstHalf.Count;
        midY /= firstHalf.Count;
        
        List<Vector2> secondHalf = new List<Vector2>();
        for (int i = index2; i < dots.Count(); i++)
        {
            if (i == index1 || i == index2 || dots[i].GetComponent<Dots>().isVertice)
            {
                secondHalf.Add(new Vector2(dots[i].transform.position.x, dots[i].transform.position.y));
            }
        }
        for (int i = 0; i < index1 + 1; i++)
        {
            if (i == index1 || i == index2 || dots[i].GetComponent<Dots>().isVertice)
            {
                secondHalf.Add(new Vector2(dots[i].transform.position.x, dots[i].transform.position.y));
            }
        }
        Vector3 onNormal = new Vector3(dots[index1].transform.position.x, dots[index1].transform.position.y, 0)-new Vector3(dots[index2].transform.position.x, dots[index2].transform.position.y,0);
        Vector3 vector = new Vector3(midX,midY,0)-new Vector3(dots[index2].transform.position.x, dots[index2].transform.position.y,0);
        
        Vector3 dir = vector - Vector3.Project(vector, onNormal);
        Vector2[] testVector = firstHalf.ToArray();
        Vector2[] testVector2 = secondHalf.ToArray();
        //audioSource.PlayOneShot(splitSound, 1f);
        audioManager.GetComponent<SoundEffects>().playSplit();
        var firstPolygon = new GameObject("Polygon");
        firstPolygon.AddComponent(System.Type.GetType("Polygon"));
        firstPolygon.GetComponent<Polygon>().render(testVector);
        firstPolygon.transform.position += 0.1f*dir.normalized;
        var secondPolygon = new GameObject("Polygon");
        secondPolygon.AddComponent(System.Type.GetType("Polygon"));
        secondPolygon.GetComponent<Polygon>().render(testVector2);
        secondPolygon.transform.position -= 0.1f*dir.normalized;
        GameObject controller = GameObject.Find("GameControllerObject");
        controller.GetComponent<GameController>().polygonList.Add(firstPolygon);
        controller.GetComponent<GameController>().polygonList.Add(secondPolygon);
        controller.GetComponent<GameController>().polygonList.Remove(gameObject);
        Destroy(gameObject);
        return;
    }

    public void findMerge()
    {
        for (int i = 0; i < vertices3D.Count(); i++)
        {
            Vector3 x1 = transform.TransformPoint(vertices3D[i]);
            Vector3 x2 = transform.TransformPoint(vertices3D[(i + 1) % vertices3D.Count()]);
            float distX = Vector3.Distance(x1, x2);
            GameObject controller = GameObject.Find("GameControllerObject");
            foreach (GameObject pol in controller.GetComponent<GameController>().polygonList)
            {
                if (pol != gameObject)
                {
                    for (int j = 0; j < pol.GetComponent<Polygon>().vertices3D.Count(); j++)
                    {
                        Vector3 y1 = pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[j]);
                        Vector3 y2 = pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(j + 1) % pol.GetComponent<Polygon>().vertices3D.Count()]);
                        float distY = Vector3.Distance(y1, y2);
                        if (distX - distY < 0.01 && distX - distY > -0.01)
                        {
                            //Debug.Log(Vector3.Angle(x1 - x2, y1 - y2));
                            //Debug.Log("distX: "+distX+"distY: "+distY);
                            if (Vector3.Distance(x1, y2) < 0.2 && Vector3.Distance(x2, y1) < 0.2 && Vector3.Distance(x1, y2) + Vector3.Distance(x2, y1) > 0)
                            {
                                float d = Mathf.Abs(Vector3.Angle(x1 - x2, y1 - y2));
                                if (d > 170)
                                {
                                    Vector3 diffX = new Vector3(x1.x - x2.x, x1.y - x2.y, 0);
                                    Vector3 diffY = new Vector3(y2.x - y1.x, y2.y - y1.y, 0);
                                    //Debug.Log("The signed angle between v" + i + "-v" + (i+1) + "and t" + (j+1) + "-t"+j);
                                    //Debug.Log(Mathf.Abs(Vector3.SignedAngle(transform.TransformPoint(vertices3D[i]) - transform.TransformPoint(vertices3D[(i+1)%vertices3D.Count()]), pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[j]) - pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(j + 1) % pol.GetComponent<Polygon>().vertices3D.Count()]), Vector3.forward)));
                                    //Debug.Log(d);
                                    transform.eulerAngles += new Vector3(0, 0, d + 180);
                                    //Debug.Log("We're going to add:");
                                    //Debug.Log(d);
                                    if (Mathf.Abs(Mathf.Abs(Vector3.Angle(transform.TransformPoint(vertices3D[i]) - transform.TransformPoint(vertices3D[(i + 1) % vertices3D.Count()]), pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[j]) - pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(j + 1) % pol.GetComponent<Polygon>().vertices3D.Count()])))-180)>0.1)
                                    {
                                        //Debug.Log("Angle still weird:");
                                        //Debug.Log(Mathf.Abs(Vector3.Angle(transform.TransformPoint(vertices3D[i]) - transform.TransformPoint(vertices3D[(i + 1) % vertices3D.Count()]), pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[j]) - pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(j + 1) % pol.GetComponent<Polygon>().vertices3D.Count()]))));
                                        transform.eulerAngles -= 2 * (new Vector3(0, 0, d + 180));
                                        //Debug.Log("So subtract:");
                                        //Debug.Log(2*d);
                                    }
                                    //Debug.Log("Changed Angle:");
                                    //Debug.Log(Mathf.Abs(Vector3.Angle(transform.TransformPoint(vertices3D[i]) - transform.TransformPoint(vertices3D[(i + 1) % vertices3D.Count()]), pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[j]) - pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(j + 1) % pol.GetComponent<Polygon>().vertices3D.Count()]))));
                                    transform.position += (pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[j]) - transform.TransformPoint(vertices3D[(i + 1) % vertices3D.Count()]));
                                    foreach (GameObject pols in controller.GetComponent<GameController>().polygonList)
                                    {
                                        pols.GetComponent<Polygon>().mergeable = false;
                                    }
                                    mergeable = true;
                                    merger = i;
                                    pol.GetComponent<Polygon>().mergeable = true;
                                    pol.GetComponent<Polygon>().merger = j;
                                    merge();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        return;
    }
    public void merge()
    {
        if (mergeable)
        {
            GameObject controller = GameObject.Find("GameControllerObject");
            for (int x = 0; x < controller.GetComponent<GameController>().polygonList.Count(); x++)
            {
                GameObject pol = controller.GetComponent<GameController>().polygonList[x];
                if (pol != gameObject)
                {
                    if (pol.GetComponent<Polygon>().mergeable)
                    {
                        if(pol.name=="Square" || gameObject.name=="Square"){
                            jiktojung=false;
                        }
                        //Debug.Log(merger + "," + pol.GetComponent<Polygon>().merger);
                        List<Vector2> newPol = new List<Vector2>();
                        for (int i = 0; i < vertices3D.Count(); i++)
                        {
                            newPol.Add(new Vector2(transform.TransformPoint(vertices3D[(i + merger + 1) % vertices3D.Count()]).x, transform.TransformPoint(vertices3D[(i + merger + 1) % vertices3D.Count()]).y));
                        }
                        for (int i = 0; i < pol.GetComponent<Polygon>().vertices3D.Count() - 2; i++)
                        {
                            newPol.Add(new Vector2(pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(i + pol.GetComponent<Polygon>().merger + 2) % pol.GetComponent<Polygon>().vertices3D.Count()]).x, pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(i + pol.GetComponent<Polygon>().merger + 2) % pol.GetComponent<Polygon>().vertices3D.Count()]).y));
                        }
                        //Debug.Log("merging.. " + vertices3D.Length);
                        for (int i = 0; i < newPol.Count; i++)
                        {
                            //Debug.Log("new vertex x : " + newPol[i].x + " y : " + newPol[i].y);
                        }

                        float newmidx = 0;
                        float newmidy = 0;
                        for (int i = 0; i < newPol.Count; i++)
                        {
                            newmidx += newPol[i].x;
                            newmidy += newPol[i].y;
                        }
                        newmidx /= newPol.Count;
                        newmidy /= newPol.Count;

                        for(int i = 0; i < newPol.Count; i++)
                        {
                            newPol[i] = new Vector2(newPol[i].x - newmidx, newPol[i].y - newmidy);
                        }
                        if(pol.name=="Square" || gameObject.name=="Square"){
                            List<Vector2> newPol2 = new List<Vector2>();
                            for(int i = 0; i < newPol.Count; i++)
                            {
                                double incline1z = newPol[(i + 1) % newPol.Count][0] -newPol[i][0];
                                double incline1m = newPol[(i + 1) % newPol.Count][1] - newPol[i][1];
                                double incline2z = newPol[i][0] - newPol[(i + newPol.Count - 1) % newPol.Count][0];
                                double incline2m = newPol[i][1] - newPol[(i + newPol.Count - 1) % newPol.Count][1];
                                if (incline1z * incline2m - incline1m * incline2z < -0.01 || incline1z * incline2m - incline1m * incline2z > 0.01)
                                {
                                    newPol2.Add(newPol[i]);
                //Debug.Log(((i + initVertices.Length-1) % initVertices.Length )+ "th vector: " + "X1 is:" + initVertices[(i + initVertices.Length - 1) % initVertices.Length][0] + "Y1 is:" + initVertices[(i + initVertices.Length - 1) % initVertices.Length][1]);
                //Debug.Log(i + "th vector: " + "X2 is:" + initVertices[i][0] + "Y2 is:" + initVertices[i][1]);
                //Debug.Log(((i+1)%initVertices.Length) + "th vector: X3 is:" + initVertices[(i + 1) % initVertices.Length][0] + "Y3 is:" + initVertices[(i + 1) % initVertices.Length][1]);
                //Debug.Log("----------------------------------");
            }
                            }
                            newPol = newPol2;
                        }
                        var newPolygon = new GameObject("Polygon");
                        newPolygon.AddComponent(System.Type.GetType("Polygon"));
                        newPolygon.GetComponent<Polygon>().render(newPol.ToArray());
                        
                        /*
                         *  mesh의 position을 보정해주는 코드 추가함
                         */ 
                        
                        newPolygon.transform.position = new Vector3(newmidx, newmidy, 0);                        
                        controller.GetComponent<GameController>().polygonList.Add(newPolygon);
                        controller.GetComponent<GameController>().polygonList.Remove(pol);
                        controller.GetComponent<GameController>().polygonList.Remove(gameObject);
                        //audioSource.PlayOneShot(mergeSound, 1f);
                        audioManager.GetComponent<SoundEffects>().playMerge();
                        Destroy(gameObject);
                        Destroy(pol);
                        break;
                    }
                }
            }
        }
        return;
    }
    public void snap(){
        if(jiktojung){
            for(int i=0; i < 5; i++){
                if(Mathf.Abs(gameObject.transform.eulerAngles.z-90*i)<10 && gameObject.transform.eulerAngles.z!=90*i){
                    // Debug.Log(90*i);
                    // Debug.Log(Mathf.Abs(gameObject.transform.eulerAngles.z-90*i));
                    gameObject.transform.eulerAngles=new Vector3(0,0,90*i);
                    //audioSource.PlayOneShot(snapSound, 1f);
                    audioManager.GetComponent<SoundEffects>().playSnap();
                }
            }
        }
    }
    public void flip()
    {
        Vector2[] newVertices = new Vector2[vertices3D.Count()];
        float centerX=0;
        float centerY=0;
        for (int i = 0; i < vertices3D.Count(); i++)
        {
            centerX += transform.TransformPoint(vertices3D[i]).x;
        }   
        centerX/=vertices3D.Count();
        centerY/=vertices3D.Count();
        for (int i = 0; i < vertices3D.Count(); i++)
        {
            newVertices[i].x = 2*centerX-transform.TransformPoint(vertices3D[vertices3D.Count() - 1 - i]).x;
            newVertices[i].y = transform.TransformPoint(vertices3D[vertices3D.Count() - 1 - i]).y;
        }
        GameObject controller = GameObject.Find("GameControllerObject");
        controller.GetComponent<GameController>().polygonList.Remove(gameObject);

        var newPol = new GameObject("Polygon");
        newPol.AddComponent(System.Type.GetType("Polygon"));
        newPol.GetComponent<Polygon>().render(newVertices);
        controller.GetComponent<GameController>().polygonList.Add(newPol);
        Destroy(gameObject);
        return;
    }

}
