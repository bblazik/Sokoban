using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Tile : MonoBehaviour
{
    int x, y;
    public GameObject Asset { get; set; }
    public bool isLocked { get; set; }

    //Contructors
    public Tile(int x, int y, GameObject a)
    {
        this.x = x;
        this.y = y;
        Asset = Instantiate(a, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
    }
    public Tile(int x, int y)
    {
        this.x = x;
        this.y = y;
        Asset = new GameObject();
    }
    public Tile()
    {
        Asset = new GameObject();
    }
    //End of constructors
};
