using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

public class AI : MonoBehaviour {

    public Graph<VirtualState> states;
    VirtualState InitialState;
    int limit = 4;

    // Use this for initialization
    void Start () {
        states = new Graph<VirtualState>();
        InitialState = new VirtualState(
            GameObject.FindGameObjectsWithTag("Box"),
            GameObject.FindGameObjectsWithTag("Cross"),
            GameObject.FindGameObjectWithTag("Player"),
            new Vector3(0, 0, 0)
        );
        
        states.AddNode(InitialState);
        CreateSearchTree(InitialState, 1);
    }
	
	// Update is called once per frame
	void Update () {

	}

    void CreateSearchTree(VirtualState state, int n)
    {
        List<VirtualState> PossibleStates = GeneratePossibleStates(state);

        foreach (VirtualState v in PossibleStates) {
            states.AddNode(v);
            
            //states.AddUndirectedEdge(new GraphNode<VirtualState>(v),
            //                       (Gra)InitialState,
            //                       (int)(state.EvaluationValue - v.EvaluationValue)
            //                       );
            if(n  < limit)
                CreateSearchTree(v, n + 1);
        }

        Debug.Log("Nodes: " + states.Count);

        
    }

    static List<VirtualState> GeneratePossibleStates(VirtualState state)
    {
        List<VirtualState> newStates = new List<VirtualState>();
        //Possible movement of player, and possible change of box pos
        GameObject Player = state.Player;
        GameObject[] Boxes = state.Boxes;

        List<Vector3> listOfPossibleMoves = possibleMoves(state);
        foreach (Vector3 move in listOfPossibleMoves) {

            //If Box at move position change box position
            if (PlayerController.BoxAtPos(Boxes, Player.transform.position + move))
                Boxes[PlayerController.iBoxAtPos(Boxes, Player.transform.position + move)].transform.Translate(move);

            newStates.Add(new VirtualState(
                Boxes,
                state.Crosses,
                Player,
                move)
            );
        }

        return newStates;
    }
    static List<Vector3> possibleMoves(VirtualState state)
    {
        List<Vector3> possibleMoves = new List<Vector3>();

        Vector3 v = state.Player.transform.position + new Vector3(1, 0, 0);
        if (PlayerController.MoveIsPossible(
            state.Player.transform.position,
            v,
            state.Boxes))
        {
            possibleMoves.Add(new Vector3(1, 0, 0));
        }


        v = state.Player.transform.position + new Vector3(-1, 0, 0);
        if (PlayerController.MoveIsPossible(
            state.Player.transform.position,
            v,
            state.Boxes))
        {
            possibleMoves.Add(new Vector3(-1, 0, 0));
        }
        v = state.Player.transform.position + new Vector3(0, 1, 0);
        if (PlayerController.MoveIsPossible(
            state.Player.transform.position,
            v,
            state.Boxes))
        {
            possibleMoves.Add(new Vector3(0, 1, 0));
        }
        v = state.Player.transform.position + new Vector3(0, -1, 0);
        if (PlayerController.MoveIsPossible(
            state.Player.transform.position,
            v,
            state.Boxes))
        {
            possibleMoves.Add(new Vector3(0, -1, 0));
        }

        if (possibleMoves.Count ==0) throw new System.ArgumentException("There are not possible moves");
        return possibleMoves;
    }

}

public class VirtualState
{
    public GameObject[] Boxes, Crosses;
    public GameObject Player;
    public float EvaluationValue;
    Vector3 move;

    public VirtualState(GameObject[]Boxes, GameObject[] Crosses, GameObject Player, Vector3 move)
    {
        this.Boxes = Boxes;
        this.Crosses = Crosses;
        this.Player = Player;
        this.move = move;

        EvaluationValue = EvaluationFuction(Boxes, Crosses, Player);
    }

    public static float EvaluationFuction(GameObject[] Boxes, GameObject[] Crosses, GameObject Player)
    {

        float value = 0;
        for (int i = 0; i < Boxes.Length; i++)
        {
            value += 8 * CalculateDistanceBeetwenObjects(Boxes[i], Crosses[i]);
            value += CalculateDistanceBeetwenObjects(Player, Boxes[i]);
        }
        Debug.Log("EvaluationFuction: " + value);
        return value;

    }

    static float CalculateDistanceBeetwenObjects(GameObject ob1, GameObject ob2)
    {
        return Mathf.Sqrt(
                        Mathf.Pow(ob1.transform.position.x - ob2.transform.position.x, 2)
                        + Mathf.Pow(ob1.transform.position.y - ob2.transform.position.y, 2)
                        );
    }
}

/*Help classes - Graph don't like me */
/*
public class Tree<T>
{   
    List<T> Nodes;
}
public class Node <T>
{
    T Parent;

    VirtualState v;
}
*/


public class Node<T>
{
    // Private member-variables
    private T data;
    private NodeList<T> neighbors = null;

    public Node() { }
    public Node(T data) : this(data, null) { }
    public Node(T data, NodeList<T> neighbors)
    {
        this.data = data;
        this.neighbors = neighbors;
    }

    public T Value
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
        }
    }

    protected NodeList<T> Neighbors
    {
        get
        {
            return neighbors;
        }
        set
        {
            neighbors = value;
        }
    }
}

public class NodeList<T> : Collection<Node<T>>
{
    public NodeList() : base() { }

    public NodeList(int initialSize)
    {
        // Add the specified number of items
        for (int i = 0; i < initialSize; i++)
            base.Items.Add(default(Node<T>));
    }

    public Node<T> FindByValue(T value)
    {
        // search the list for the value
        foreach (Node<T> node in Items)
            if (node.Value.Equals(value))
                return node;

        // if we reached here, we didn't find a matching node
        return null;
    }

}

public class GraphNode<T> : Node<T>
{
    private List<int> costs;

    public GraphNode() : base() { }
    public GraphNode(T value) : base(value) { }
    public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }

    new public NodeList<T> Neighbors
    {
        get
        {
            if (base.Neighbors == null)
                base.Neighbors = new NodeList<T>();

            return base.Neighbors;
        }
    }

    public List<int> Costs
    {
        get
        {
            if (costs == null)
                costs = new List<int>();

            return costs;
        }
    }
}

public class Graph<T> : IEnumerable<T>

{
    private NodeList<T> nodeSet;

    public Graph() : this(null) { }
    public Graph(NodeList<T> nodeSet)
    {
        if (nodeSet == null)
            this.nodeSet = new NodeList<T>();
        else
            this.nodeSet = nodeSet;
    }

    public void AddNode(GraphNode<T> node)
    {
        // adds a node to the graph
        nodeSet.Add(node);
    }

    public void AddNode(T value)
    {
        // adds a node to the graph
        nodeSet.Add(new GraphNode<T>(value));
    }

    public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    {
        from.Neighbors.Add(to);
        from.Costs.Add(cost);
    }


    public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
    {
        from.Neighbors.Add(to);
        from.Costs.Add(cost);

        to.Neighbors.Add(from);
        to.Costs.Add(cost);
    }

    public bool Contains(T value)
    {
        return nodeSet.FindByValue(value) != null;
    }

    public bool Remove(T value)
    {
        // first remove the node from the nodeset
        GraphNode<T> nodeToRemove = (GraphNode<T>)nodeSet.FindByValue(value);
        if (nodeToRemove == null)
            // node wasn't found
            return false;

        // otherwise, the node was found
        nodeSet.Remove(nodeToRemove);

        // enumerate through each node in the nodeSet, removing edges to this node
        foreach (GraphNode<T> gnode in nodeSet)
        {
            int index = gnode.Neighbors.IndexOf(nodeToRemove);
            if (index != -1)
            {
                // remove the reference to the node and associated cost
                gnode.Neighbors.RemoveAt(index);
                gnode.Costs.RemoveAt(index);
            }
        }

        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public NodeList<T> Nodes
    {
        get
        {
            return nodeSet;
        }
    }

    public int Count
    {
        get { return nodeSet.Count; }
    }
}

