using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class NewBehaviourScript : MonoBehaviour {

    public GameObject EC;
    private EventController ec;

    // Use this for initialization
    void Awake () {
        ec = EC.GetComponent<EventController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("Click");
        ec.isHelp = 2;
        ec.GameManager();
    }
}
