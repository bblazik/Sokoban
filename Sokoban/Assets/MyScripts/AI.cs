using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class AI : MonoBehaviour {

    VirtualState InitialState;
    Vector3 lastMove;
    public int TreeDeeplimit =6;
    public float speed = 0.2f;
    List<Vector3> lastMoves;

    // Use this for initialization
    void Start () {
        lastMoves = new List<Vector3>();
        InvokeRepeating("AIMove", 1.0f, speed);
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Colider pos: " + other.transform.position + "x,y" + x + "," + y);
        Debug.Log("Move: " + lastMove);
        other.transform.Translate(lastMove);
    }
    // Update is called once per frame
    void FixedUpdate () {
        // Restart.
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AIMove()
    {
        

        InitialState = getCurrentState();
        List<Vector3> m = (GenerateListOfMoves(InitialState, 0));
        lastMove = m[0];

        lastMoves.Add(lastMove);
        if(lastMoves.Count > 10)
        {
            lastMoves.RemoveAt(0);
        }
        if (lastMoves.Count == 10 && Glitch(lastMoves))
        {
            if (TreeDeeplimit <8)TreeDeeplimit++;
            lastMoves.RemoveRange(0,3);
            Debug.Log("Glitch detected TreeDeeplimit rised");
        }
        else
        {
            if (lastMoves.Count == 10 && !Glitch(lastMoves))
                if (TreeDeeplimit > 7)
                {
                    TreeDeeplimit--;
                }
        }
        transform.position += m[0];
    }

    bool Glitch(List<Vector3> v)
    {
        for(int i = 0; i < (v.Count/2); i++)
        {
            if (v[i] != v[i + 2]) return false;
        }
        return true;
    }

    VirtualState getCurrentState()
    {
        VirtualState CurrentState = new VirtualState(
            CastFromGameObjectListToPositionVector(GameObject.FindGameObjectsWithTag("Box")),
            CastFromGameObjectListToPositionVector(GameObject.FindGameObjectsWithTag("Cross")),
            CastFromGameObjectListToPositionVector(GameObject.FindGameObjectsWithTag("Wall")),
            GameObject.FindGameObjectWithTag("Player").transform.position,
            new Vector3(0, 0, 0)
        );
        //Debug.Log("Initial position: " + GameObject.FindGameObjectWithTag("Player").transform.position);

        return CurrentState;
    }

    public static Vector3[] CastFromGameObjectListToPositionVector(GameObject[] objects)
    {   //Tested, always create new Vector3.
        List<Vector3> newList = new List<Vector3>();
        foreach(GameObject ob in objects)
        {
            newList.Add(ob.transform.position);
        }
        return newList.ToArray();
    }

    int CreateSearchTree(VirtualState state, int n, ref List<Vector3> moves)
    {
        List<VirtualState> PossibleStates = GeneratePossibleStates(state);
        VirtualState best = new VirtualState();
        foreach (VirtualState v in PossibleStates)
        {
            if (n + 1 < TreeDeeplimit)
            {
                //  1'st optimalization
              //  if (state.move == -v.move)
              //      v.EvaluationValue += int.MaxValue;
              //  else
                    v.EvaluationValue += CreateSearchTree(v, n + 1, ref moves);
                   
            }
            if (v.EvaluationValue < best.EvaluationValue)
            {
                best = new VirtualState(v, v.EvaluationValue);
            }
        }
        //Debug.Log("N: " + n + " , " + best.EvaluationValue + ", " + best.move);

        if (n == 0)
           moves.Add(best.Move);

        return best.EvaluationValue;
    }

    List<Vector3> GenerateListOfMoves(VirtualState state, int n)
    {
        List<Vector3> moves = new List<Vector3>();
        CreateSearchTree(state, n, ref moves);
        //Debug.Log(moves[0]);
        return moves;
    }

    List<VirtualState> GeneratePossibleStates(VirtualState state)
    {//Tested ok.
        List<VirtualState> newStates = new List<VirtualState>();
        List<Vector3> listOfPossibleMoves = possibleMoves(state, state.Player);
        foreach (Vector3 move in listOfPossibleMoves)
        {
            VirtualState temporaryState = new VirtualState(state.Boxes, state.Crosses,state.Walls, state.Player, move);
            temporaryState.Player += move;
            if (PlayerController.BoxAtPos(temporaryState.Boxes, temporaryState.Player))
            {                
               temporaryState.Boxes[PlayerController.iBoxAtPos(temporaryState.Boxes, temporaryState.Player)] += move;                
            }
            newStates.Add(temporaryState);
        }
        return newStates;
    }

    public static List<Vector3> possibleMoves(VirtualState state,Vector3 moveableObject )
    {//Tested seems ok. 
        List<Vector3> possibleMoves = new List<Vector3>() { new Vector3(1,0,0), new Vector3(-1, 0, 0), new Vector3(0, -1, 0), new Vector3(0, 1, 0) };
        List<Vector3> returnMoves = new List<Vector3>();
        foreach(Vector3 move in possibleMoves)
        {
            if(PlayerController.MoveIsPossible(moveableObject, moveableObject + move, state.Boxes))
            {
                returnMoves.Add(move);
            }
        }
        if (possibleMoves.Count == 0) throw new System.ArgumentException("There are not possible moves");
        if (possibleMoves.Count < 4) Debug.Log("Less than 4 possible moves");
        return returnMoves;
    }
}


