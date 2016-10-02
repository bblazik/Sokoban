using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

public class AI : MonoBehaviour {

    VirtualState InitialState;
    int limit =3;

    // Use this for initialization
    void Start () {
        InitialState = getCurrentState();       
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("Size: " + m.Count);
        //foreach(Vector3 v in m) { AIMove(v); }
        //InvokeRepeating("AIMove(m[0])", 2, 2);
        //AIMove();
    }

    //public void AIMove()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        InitialState = getCurrentState();

    //        List<Vector3> m = (GenerateListOfMoves(InitialState, 0));
    //        Debug.Log(m[0]);
    //        transform.position += m[0];
    //        InitialState = getCurrentState();
    //    }
            
    //}
    //VirtualState getCurrentState()
    //{
    //    VirtualState CurrentState = new VirtualState(
    //        GameObject.FindGameObjectsWithTag("Box"),
    //        GameObject.FindGameObjectsWithTag("Cross"),
    //        GameObject.FindGameObjectWithTag("Player"),
    //        new Vector3(0, 0, 0)
    //    );
    //    Debug.Log("Initial position: " + GameObject.FindGameObjectWithTag("Player").transform.position);

    //    return CurrentState;
    //}
    //float CreateSearchTree(VirtualState state, int n, ref List<Vector3> moves)
    //{
    //    List<VirtualState> PossibleStates = GeneratePossibleStates(state);
    //    VirtualState best = new VirtualState();
    //    foreach (VirtualState v in PossibleStates) {
    //        if (n + 1 < limit)
    //        {
    //            v.EvaluationValue += CreateSearchTree(v, n + 1, ref moves);
    //        }
    //        if (v.EvaluationValue < best.EvaluationValue)
    //        {
    //            best = v;
    //        }
    //    }
    //    //Debug.Log("N: " + n + " , " + best.EvaluationValue + ", " + best.move);

    //    if( n ==0 )
    //        moves.Add(best.move);

    //    return best.EvaluationValue; 
    //}
    //List<Vector3> GenerateListOfMoves(VirtualState state, int n)
    //{
    //    List<Vector3> moves = new List<Vector3>();
    //    CreateSearchTree(state, n, ref moves);
    //    Debug.Log(moves[0]);
    //    return moves;
    //}    
    //List<VirtualState> GeneratePossibleStates(VirtualState state) 
    //{
    //    List<VirtualState> newStates = new List<VirtualState>();
    //    //Possible movement of player, and possible change of box pos
    //    GameObject Player = Instantiate(state.Player, state.Player.transform.position, Quaternion.identity) as GameObject;
    //    GameObject[] Boxes = state.Boxes;
    //    Vector3 currentPos = Player.transform.position;
        
    //    List<Vector3> listOfPossibleMoves = possibleMoves(state);
    //    foreach (Vector3 move in listOfPossibleMoves) {
    //        Player.transform.position = currentPos;
    //        //VirtualState temporaryState = new VirtualState(state.Boxes, state.Crosses, state.Player, state.move);
    //        //temporaryState.Player.transform.position += move;
    //        Player.transform.position += move;
            
    //        //Can Be problem with boxes position. 
    //        //If Box at move position change box position
    //        if (PlayerController.BoxAtPos(Boxes, Player.transform.position + move))
    //        {
    //            //newStates.Add(CreateState(state, move, PlayerController.iBoxAtPos(Boxes, Player.transform.position + move)));
    //            //Boxes[PlayerController.iBoxAtPos(Boxes, Player.transform.position + move)].transform.position += move;                
    //        }
    //        else
    //        {
    //            //newStates.Add(CreateState(temporaryState, move));
    //        }
    //    }
    //    return newStates;
    //}
    //VirtualState CreateState(VirtualState state, Vector3 move)
    //{
    //    state.Player.transform.position += move;
    //    return (new VirtualState(
    //        state.Boxes,
    //        state.Crosses,
    //        state.Player,
    //        move));
    //}
    //List<Vector3> possibleMoves(VirtualState state)
    //{
    //    List<Vector3> possibleMoves = new List<Vector3>();

    //    Vector3 v = state.Player.transform.position + new Vector3(1, 0, 0);
    //    if (PlayerController.MoveIsPossible(
    //        state.Player.transform.position,
    //        v,
    //        state.Boxes))
    //    {
    //        possibleMoves.Add(new Vector3(1, 0, 0));
    //    }


    //    v = state.Player.transform.position + new Vector3(-1, 0, 0);
    //    if (PlayerController.MoveIsPossible(
    //        state.Player.transform.position,
    //        v,
    //        state.Boxes))
    //    {
    //        possibleMoves.Add(new Vector3(-1, 0, 0));
    //    }
    //    v = state.Player.transform.position + new Vector3(0, 1, 0);
    //    if (PlayerController.MoveIsPossible(
    //        state.Player.transform.position,
    //        v,
    //        state.Boxes))
    //    {
    //        possibleMoves.Add(new Vector3(0, 1, 0));
    //    }
    //    v = state.Player.transform.position + new Vector3(0, -1, 0);
    //    if (PlayerController.MoveIsPossible(
    //        state.Player.transform.position,
    //        v,
    //        state.Boxes))
    //    {
    //        possibleMoves.Add(new Vector3(0, -1, 0));
    //    }

    //    if (possibleMoves.Count == 0) throw new System.ArgumentException("There are not possible moves");
    //    return possibleMoves;
    //}
}

public class VirtualState
{
    public Vector3[] Boxes, Crosses;
    public Vector3 Player;
    public float EvaluationValue;
    public Vector3 move;

    public VirtualState(Vector3[]Boxes, Vector3[] Crosses, Vector3 Player, Vector3 move)
    {
        this.Boxes = Boxes;
        this.Crosses = Crosses;
        this.Player = Player;
        this.move = move;

       // this.Player.transform.position += move;
        EvaluationValue = EvaluationFuction(Boxes, Crosses, Player);
    }
    public VirtualState()
    {
        this.EvaluationValue = float.MaxValue;
    }

    public VirtualState(VirtualState state)
        :this(state.Boxes, state.Crosses, state.Player, state.move)
    {
        EvaluationValue = EvaluationFuction(Boxes, Crosses, Player);
        /*
        this.Boxes = state.Boxes;
        this.Crosses = state.Crosses;
        this.Player = state.Player;
        this.EvaluationValue = state.EvaluationValue;
        this.move = state.move;*/
    }

    public static float EvaluationFuction(Vector3[] Boxes, Vector3[] Crosses, Vector3 Player)
    {

        float value = 0;
        for (int i = 0; i < Boxes.Length; i++)
        {
            value += 8 * CalculateDistanceBeetwenObjects(Boxes[i], Crosses[i]);
            value += CalculateDistanceBeetwenObjects(Player, Boxes[i]);
        }
        //Debug.Log("EvaluationFuction: " + value);
        return value;

    }

    static float CalculateDistanceBeetwenObjects(Vector3 ob1, Vector3 ob2)
    {
        return Mathf.Sqrt(
                        Mathf.Pow(ob1.x - ob2.x, 2)
                        + Mathf.Pow(ob1.y - ob2.y, 2)
                        );
    }
}


