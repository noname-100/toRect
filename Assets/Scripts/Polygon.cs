using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Polygon : MonoBehaviour
{
    public Vector2[] VerticesPublic2D;
    public Vector3[] vertices3D;
    private PolygonCollider2D _polygonCollider2D;
    private bool isSelected = false;
    public bool dotSelected = false;
    public bool mergeable = false;
    public int merger;
    private List<GameObject> dots = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dotSelected) {
            
        }
        else
        {
            foreach(GameObject dot in dots)
            {
                dot.GetComponent<Dots>().selectable = false;
            }
        }
    }   
    void OnDestroy()
    {
        foreach(GameObject dot in dots)
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
            double incline1z = initVertices[(i + 1) % initVertices.Length][0]- initVertices[i][0];
            double incline1m = initVertices[(i + 1) % initVertices.Length][1] - initVertices[i][1];
            double incline2z = initVertices[i][0] - initVertices[(i + initVertices.Length - 1) % initVertices.Length][0];
            double incline2m = initVertices[i][1] - initVertices[(i + initVertices.Length - 1) % initVertices.Length][1];
            if( incline1z * incline2m - incline1m * incline2z < -0.01 || incline1z * incline2m - incline1m * incline2z > 0.01)
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
        var colors = Enumerable.Range(0, vertices3D.Length)
            .Select(i => Random.ColorHSV())
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
    }
    public void OnMouseEnter()
    {
        //if (dots.Count() == 0) { createDots(); }
        if (!isSelected)
        {
            this.GetComponent<Renderer>().material.color = Color.yellow;
        }
        
    }
    public void OnMouseExit()
    {
        if(!this.isSelected)
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
        this.GetComponent<Renderer>().material.color = Color.green;
    }
    public void createDots()
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        vertices3D = mesh.vertices;
        var c = vertices3D.Count();
        for(int i = 0; i < c; i++)
        {
            GameObject dot = Instantiate(Resources.Load("Prefabs/Circle"), transform.TransformPoint(vertices3D[i])+new Vector3(0,0,-1), transform.rotation) as GameObject;
            dot.name = "dot"+i;
            dot.transform.parent = gameObject.transform;
            dot.AddComponent(System.Type.GetType("Dots"));
            dots.Add(dot);
            List<Vector3> midDots = new List<Vector3>();
            midDots.Add(transform.TransformPoint((vertices3D[i] + vertices3D[(i + 1) % c]) / 2) + new Vector3(0, 0, -1));
            //Find perpendicular dot
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
            foreach(Vector3 dotVector in midDots)
            {
                GameObject midDot = Instantiate(Resources.Load("Prefabs/Circle"), dotVector, transform.rotation) as GameObject;
                midDot.name = "dotmid" + i;
                midDot.transform.parent = gameObject.transform;
                midDot.AddComponent(System.Type.GetType("Dots"));
                midDot.GetComponent<Dots>().isVertice = false;
                dots.Add(midDot);
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
    }
    void OnMouseUp()
    {
        if (!this.isSelected)
        {
            GameObject controller = GameObject.Find("GameControllerObject");
            foreach(GameObject pol in controller.GetComponent<GameController>().polygonList)
            {
                if (pol != gameObject && pol!=null) {
                    pol.GetComponent<Polygon>().unActivate();
                }
            }
            this.isSelected = true;
            this.GetComponent<Renderer>().material.color = Color.gray;
            createDots();
        }
        else
        {
            this.isSelected = false;
            unActivate();
            this.GetComponent<Renderer>().material.color = Color.green;
        }
        findMerge();

    }
    public void selectableDots()
    {
        int index=0;
        int c = dots.Count();
        int lowerVertex=1;
        int higherVertex=1;
        for(int i = 0; i < c; i++)
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
        float incline1z = (dots[index].transform.position.y - dots[(index + c - lowerVertex)%c].transform.position.y);
        float incline1m = (dots[index].transform.position.x - dots[(index + c - lowerVertex) % c].transform.position.x);
        float incline2z = (dots[index].transform.position.y - dots[(index + higherVertex) % c].transform.position.y);
        float incline2m = (dots[index].transform.position.x - dots[(index + higherVertex) % c].transform.position.x);

        for (int i = 0; i < dots.Count; i++)
        {
            float inclinez = (dots[i].transform.position.y - dots[index].transform.position.y);
            float inclinem = (dots[i].transform.position.x - dots[index].transform.position.x);
            if ( Mathf.Abs(incline1z * inclinem - incline1m * inclinez)>0.1 && Mathf.Abs(incline2z * inclinem - incline2m * inclinez)>0.1 && i!=index )
            {
                dots[i].GetComponent<Dots>().selectable = true;
                dots[i].GetComponent<Renderer>().material.color = Color.blue;
            }
        }
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
        for (int i = index1+1; i < dots.Count(); i++)
        {
            if (dots[i].GetComponent<Dots>().isSelected)
            {
                index2 = i;
                break;
            }
        }
        List<Vector2> firstHalf = new List<Vector2>();
        for(int i = index1; i < index2+1; i++)
        {
            if(i==index1 || i ==index2 || dots[i].GetComponent<Dots>().isVertice)
            {
                firstHalf.Add(new Vector2(dots[i].transform.position.x, dots[i].transform.position.y));
            }
        }
        List<Vector2> secondHalf = new List<Vector2>();
        for (int i = index2; i < dots.Count(); i++)
        {
            if (i == index1 || i == index2 || dots[i].GetComponent<Dots>().isVertice)
            {
                secondHalf.Add(new Vector2(dots[i].transform.position.x, dots[i].transform.position.y));
            }
        }
        for (int i = 0; i < index1+1; i++)
        {
            if (i == index1 || i == index2 || dots[i].GetComponent<Dots>().isVertice)
            {
                secondHalf.Add(new Vector2(dots[i].transform.position.x, dots[i].transform.position.y));
            }
        }
        Vector2[] testVector = firstHalf.ToArray();
        Vector2[] testVector2 = secondHalf.ToArray();
        var firstPolygon = new GameObject("Polygon");
        firstPolygon.AddComponent(System.Type.GetType("Polygon"));
        firstPolygon.GetComponent<Polygon>().render(testVector);
        var secondPolygon = new GameObject("Polygon");
        secondPolygon.AddComponent(System.Type.GetType("Polygon"));
        secondPolygon.GetComponent<Polygon>().render(testVector2);
        GameObject controller = GameObject.Find("GameControllerObject");
        controller.GetComponent<GameController>().polygonList.Add(firstPolygon);
        controller.GetComponent<GameController>().polygonList.Add(secondPolygon);
        controller.GetComponent<GameController>().polygonList.Remove(gameObject);
        Destroy(gameObject);
    }
    public void findMerge()
    {
        for(int i = 0; i < vertices3D.Count(); i++)
        {
            Vector3 x1 = transform.TransformPoint(vertices3D[i]);
            Vector3 x2 = transform.TransformPoint(vertices3D[(i+1)%vertices3D.Count()]);
            float distX = Vector3.Distance(x1, x2);
            GameObject controller = GameObject.Find("GameControllerObject");
            foreach (GameObject pol in controller.GetComponent<GameController>().polygonList)
            {
                if (pol != gameObject)
                {
                    for(int j = 0; j < pol.GetComponent<Polygon>().vertices3D.Count(); j++)
                    {
                        Vector3 y1 = pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[j]);
                        Vector3 y2 = pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(j + 1) % pol.GetComponent<Polygon>().vertices3D.Count()]);
                        float distY = Vector3.Distance(y1, y2);
                        //Debug.Log("distX: "+distX+"distY: "+distY);
                        if (distX - distY < 0.1 || distX -distY > - 0.1)
                        {
                            //Debug.Log(Vector3.Angle(x1 - x2, y1 - y2));
                            if (Vector3.Distance(x1, y2) < 1 && Vector3.Distance(x2, y1) < 1)
                            {
                                if (Mathf.Abs(transform.eulerAngles.z - pol.transform.eulerAngles.z) < 10 || Mathf.Abs(transform.eulerAngles.z - pol.transform.eulerAngles.z) > 175)
                                {
                                    if(Mathf.Abs(transform.eulerAngles.z - pol.transform.eulerAngles.z) < 10)
                                    transform.eulerAngles = pol.transform.eulerAngles;
                                    if(Mathf.Abs(transform.eulerAngles.z - pol.transform.eulerAngles.z) > 175)
                                    transform.eulerAngles = pol.transform.eulerAngles+new Vector3(0,0,180);
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
    }
    public void merge()
    {
        if (mergeable)
        {
            GameObject controller = GameObject.Find("GameControllerObject");
            for(int x = 0; x < controller.GetComponent<GameController>().polygonList.Count(); x++)
            {
                GameObject pol = controller.GetComponent<GameController>().polygonList[x];
                if (pol != gameObject)
                {
                    if (pol.GetComponent<Polygon>().mergeable)
                    {
                        //Debug.Log(merger + "," + pol.GetComponent<Polygon>().merger);
                        List<Vector2> newPol = new List<Vector2>();
                        for(int i = 0; i<vertices3D.Count(); i++)
                        {
                            newPol.Add(new Vector2(transform.TransformPoint(vertices3D[(i+merger+1)% vertices3D.Count()]).x, transform.TransformPoint(vertices3D[(i + merger + 1) % vertices3D.Count()]).y));
                        }
                        for (int i = 0; i < pol.GetComponent<Polygon>().vertices3D.Count()-2; i++)
                        {
                            newPol.Add(new Vector2(pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(i + pol.GetComponent<Polygon>().merger + 2) % pol.GetComponent<Polygon>().vertices3D.Count()]).x, pol.transform.TransformPoint(pol.GetComponent<Polygon>().vertices3D[(i + pol.GetComponent<Polygon>().merger + 2) % pol.GetComponent<Polygon>().vertices3D.Count()]).y));
                        }
                        var newPolygon = new GameObject("Polygon");
                        newPolygon.AddComponent(System.Type.GetType("Polygon"));
                        newPolygon.GetComponent<Polygon>().render(newPol.ToArray());
                        controller.GetComponent<GameController>().polygonList.Add(newPolygon);
                        controller.GetComponent<GameController>().polygonList.Remove(pol);
                        controller.GetComponent<GameController>().polygonList.Remove(gameObject);
                        Destroy(gameObject);
                        Destroy(pol);
                        break;
                    }
                }
            }
        }
    }
}
