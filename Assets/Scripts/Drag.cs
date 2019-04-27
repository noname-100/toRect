using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    Vector3 posDiff3D;
    float distance = 10;
    //public EventController EC;
 

    private void Start()
    {

    }

    void OnMouseDown()
    {
        Vector3 mousePosition3D = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        posDiff3D = Camera.main.ScreenToWorldPoint(mousePosition3D) - transform.position;
    }

    void OnMouseDrag()
    {
        GameObject go = GameObject.Find("EC");
        EventController sc = go.GetComponent<EventController>();
        if (EventController.movementStatus == 1) return;

        Vector3 mousePosition3D = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition3D) - posDiff3D;
    }
}

