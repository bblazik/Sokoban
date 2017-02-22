using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirtualState
{
    public Vector3[] Boxes, Crosses, Walls;
    public Vector3 Player;
    public int EvaluationValue;
    public Vector3 Move;


    public VirtualState(Vector3[] boxes, Vector3[] crosses, Vector3[] walls, Vector3 player, Vector3 move)
    {
        Boxes = new Vector3[boxes.Length];
        Array.Copy(boxes, Boxes, boxes.Length);
        Crosses = crosses;
        Walls = walls;
        Player = player;
        Move = move;
        EvaluationValue = EvaluationFuction(boxes, crosses, walls, player);
    }
    public VirtualState()
    {
        EvaluationValue = int.MaxValue;
    }

    public VirtualState(VirtualState state)
        : this(state.Boxes, state.Crosses, state.Walls, state.Player, state.Move)
    {
        EvaluationValue = EvaluationFuction(Boxes, Crosses, Walls, Player);
    }

    public VirtualState(VirtualState state, int ev)
        : this(state.Boxes, state.Crosses, state.Walls, state.Player, state.Move)
    {
        EvaluationValue = ev;
    }

    public int EvaluationFuction(Vector3[] boxes, Vector3[] crosses, Vector3[] walls, Vector3 player)
    {
        int value = 0;
        for (int i = 0; i < boxes.Length; i++)
        {
            value += 40 * (int)CalculateDistanceBeetwenObjects(boxes[i], crosses[i]);
            if (CalculateDistanceBeetwenObjects(boxes[i], crosses[i]) > 0)
                value += 4*(int) CalculateDistanceBeetwenObjects(player, boxes[i]);
        }

        foreach (Vector3 v in boxes)
        {
            if (v.x == GenerateGrid.AreaSize - 1 || v.y == GenerateGrid.AreaSize - 1 || v.x == 0 || v.y == 0) value += 20;
        }
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
