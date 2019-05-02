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
    public bool isVertice = false;
    public bool ismid = false;
    public bool isperp = false;
    private CircleCollider2D _circleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        _circleCollider2D = gameObject.AddComponent<CircleCollider2D>();

        transforms = gameObject.GetComponentsInParent(typeof(Transform));
        renderes = gameObject.GetComponentsInParent(typeof(Renderer));

        tf = (Transform)transforms[1];
        rend = (Renderer)renderes[1];

        if (ismid) this.GetComponent<Renderer>().material.color = Color.black;

    }
    void Update()
    {
        if (this.isSelected)
        {
            Vector3 mousePosition3D = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new Vector3[] { transform.position, Camera.main.ScreenToWorldPoint(mousePosition3D) }); //**
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
            // 안 고른 상태에서 하나를 hover
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        if (this.selectable)
        {
            // 하나를 고른 상태에서 나머지 하나
            this.GetComponent<Renderer>().material.color = Color.cyan;
        }
    }

    void OnMouseExit()
    {
        if (!this.isSelected && !this.selectable)
        {
            this.GetComponent<Renderer>().material.color = Color.white;
            if (ismid) this.GetComponent<Renderer>().material.color = Color.black;
        }
        if (this.selectable)
        {
            this.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    void OnMouseDown()
    {
        pushTime = Time.time;
    }
    void OnMouseUp()
    {
        if( Time.time - pushTime < 0.2f) { 
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
        this.transform.parent.GetComponent<Polygon>().snap();
    }

    private void OnMouseDrag()
    {
        if (EventController.movementStatus == 2) return;
        if(isVertice){
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
    }

    public void render()
    {

    }
}
