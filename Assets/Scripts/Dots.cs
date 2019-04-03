using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dots : MonoBehaviour
{
    public EventController EC;
    public Component[] transforms;
    public Component[] renderes;
    Vector3 center;
    Transform tf;
    Renderer rend;
    float pushTime;
    private LineRenderer lineRenderer;
    public bool isSelected=false;
    public bool selectable = false;
    public bool isVertice = true;
    private CircleCollider2D _circleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        _circleCollider2D = gameObject.AddComponent<CircleCollider2D>();

        transforms = gameObject.GetComponentsInParent(typeof(Transform));
        renderes = gameObject.GetComponentsInParent(typeof(Renderer));

        tf = (Transform)transforms[1];
        rend = (Renderer)renderes[1];

    }
    void Update()
    {
        if (this.isSelected)
        {
            Vector3 mousePosition3D = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new Vector3[] { transform.position, Camera.main.ScreenToWorldPoint(mousePosition3D) });
        }
        else
        {
            if (!this.isSelected)
            {
                if (!this.selectable && this.GetComponent<Renderer>().material.color == Color.blue) { this.GetComponent<Renderer>().material.color = Color.white; }
            }
        }
    }
    void OnMouseEnter()
    {
        if (!this.isSelected && !this.selectable)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        if (this.selectable)
        {
            this.GetComponent<Renderer>().material.color = Color.cyan;
        }
    }
    void OnMouseExit()
    {
        if (!this.isSelected && !this.selectable)
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }
        if (this.selectable)
        {
            this.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
    /*
    private void OnMouseUp()
    {
        //center = rend.bounds.center;
    }*/
    void OnMouseDown()
    {
        pushTime = Time.time;
    }
    void OnMouseUp()
    {
        if( Time.time - pushTime < 0.5f) { 
            if (this.isSelected)
            {
                this.isSelected = false;
                this.transform.parent.GetComponent<Polygon>().dotSelected = false;
                Destroy(this.GetComponent<LineRenderer>());
            }
            else
            {
                center = tf.position;
                if (this.transform.parent.GetComponent<Polygon>().dotSelected == false)
                {
                    this.isSelected = true;
                    this.transform.parent.GetComponent<Polygon>().dotSelected = true;
                    this.transform.parent.GetComponent<Polygon>().selectableDots();
                    LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
                    lineRenderer.positionCount = 2;
                    lineRenderer.alignment = LineAlignment.Local;
                    lineRenderer.SetWidth(0.2f, 0.2f);
                }
            }
            if (this.selectable)
            {
                this.isSelected = true;
                this.transform.parent.GetComponent<Polygon>().cutMyself();
            }
        }
    }

    private void OnMouseDrag()
    {
        /*
        
        GameObject go = GameObject.Find("EC");
        EventController sc = go.GetComponent<EventController>();
        
        if (sc.MovementStatus == 2) return;

        //center = rend.bounds.center;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        float degree;
        if ((360f * (float)Math.Atan2((mouse.y - center.y), (mouse.x - center.x))) / (2 * (float)Math.PI) >= 0)
        {
            degree = (360f * (float)Math.Atan2((mouse.y - center.y), (mouse.x - center.x))) / (2 * (float)Math.PI);
        }
        else
        {
            degree = 360 + (360f * (float)Math.Atan2((mouse.y - center.y), (mouse.x - center.x))) / (2 * (float)Math.PI);
        }

        degree += 180;
        if (degree > 360) degree -= 360;
        Debug.Log(degree);
        tf.rotation = Quaternion.Euler(0, 0, degree);
        */

        Vector3 sum = Vector3.zero;
        foreach (Vector3 vertice in this.transform.parent.GetComponent<Polygon>().vertices3D)
        {
            sum += vertice;
        }
        Vector3 center = transform.parent.transform.TransformPoint(sum / this.transform.parent.GetComponent<Polygon>().vertices3D.Length);
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, center.z));
        float angle = Vector3.SignedAngle(new Vector3((mouse - center).x, (mouse - center).y, 0), new Vector3((transform.position - center).x, (transform.position - center).y, 0), Vector3.forward);
        if (angle > 3)
        {
            transform.parent.transform.RotateAround(center, Vector3.forward, -angle);
        }
        else if (angle < -3)
        {
            transform.parent.transform.RotateAround(center, Vector3.forward, -angle);
        }
        //transform.parent.transform.eulerAngles += new Vector3(0, 0, angle);

    }

    public void render()
    {

    }
}
