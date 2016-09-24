using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    int x = 0;
    int y = 0;

    // Use this for initialization
    void Start () {
       
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colider pos: " + other.transform.position + "x,y" + x + "," + y);
        other.transform.Translate(new Vector3(x, y, 0));
    }

    // Update is called once per frame
    void FixedUpdate () {
        Movement();
        //Invoke("Movement", 2);
	}


    void Movement()
    {
        x = 0; y = 0;

        if (Input.GetKeyDown(KeyCode.D)) x = 1;
        if (Input.GetKeyDown(KeyCode.A)) x = -1;
        if (Input.GetKeyDown(KeyCode.W)) y = 1;
        if (Input.GetKeyDown(KeyCode.S)) y = -1;

        Debug.Log("current pos: , x, z: "+ transform.position + x + "," + y);
        Vector3 targetPosition = (transform.position+ new Vector3(x, y, 0));
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 10);   
    }   
}
