using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {

    public GameObject [] Boxes, Crosses;
    GameObject Player;

	// Use this for initialization
	void Start () {
        Boxes = GameObject.FindGameObjectsWithTag("Box");
        Crosses = GameObject.FindGameObjectsWithTag("Cross");
        Player = GameObject.FindGameObjectWithTag("Player");
        //Or generate tags? and find by tag.. 
	}
	
	// Update is called once per frame
	void Update () {
        Boxes = GameObject.FindGameObjectsWithTag("Box");
        Crosses = GameObject.FindGameObjectsWithTag("Cross");
        Player = GameObject.FindGameObjectWithTag("Player");

        EvaluationFuction();
	}

    float EvaluationFuction() {

        float value = 0;
        for (int i =0; i< Boxes.Length; i++)
        {
            value += 4*CalculateDistanceBeetwenObjects(Boxes[i], Crosses[i]);
            value += CalculateDistanceBeetwenObjects(Player, Boxes[i]);
        }
        Debug.Log("EvaluationFuction: " + value);
        return value;

    }

    float CalculateDistanceBeetwenObjects(GameObject ob1, GameObject ob2)
    {
        return Mathf.Sqrt(
                        Mathf.Pow(ob1.transform.position.x - ob2.transform.position.x, 2)
                        + Mathf.Pow(ob1.transform.position.y - ob2.transform.position.y, 2)
                        );
    }
}
