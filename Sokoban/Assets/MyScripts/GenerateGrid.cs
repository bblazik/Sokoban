using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GenerateGrid : MonoBehaviour {

    public GameObject FieldAsset;
    public GameObject BoxAsset;
    public static int AreaSize = 10 ;
    public int NumberOfBoxes = 3;
    public List<Color> TableOfColor = new List<Color> { Color.blue, Color.red, Color.yellow, Color.green };
    public static Tile [,] Board;

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

        
        static public Vector3 RandomField()
        {
            int x, y;
            do
            {
                x = Random.Range(0, AreaSize);
                y = Random.Range(0, AreaSize);
            } while (!IsLocked(x, y));

            return new Vector3(x,y,0);
        }

        static bool IsLocked(int x, int y)
        {
            return Board[x, y].isLocked;
        }
    };

    void Start () {

        Board = new Tile[AreaSize, AreaSize];
        GenerateTiles();
        GenerateBoxes();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GenerateTiles()
    {
        GameObject Tiles = new GameObject();
        Tiles.name = "Tiles";
      
        for (int i = 0; i < AreaSize; i++)
            for (int j = 0; j < AreaSize; j++)
            {
                Tile field = new Tile(i, j, FieldAsset);
                field.Asset.transform.parent = Tiles.transform;
                Board[i, j] = field;
            }
    }

    void GenerateBoxes()
    {

        GameObject Boxes = new GameObject();
        Boxes.name = "Boxes";
        for(int i = 0; i< NumberOfBoxes; i++)
        {
            
            //RandSomeVector in scope 10
              
            GameObject Box = Instantiate(BoxAsset, Tile.RandomField(), Quaternion.identity) as GameObject;
            Box.transform.parent = Boxes.transform;

            int IndexOfColor = Random.Range(0, TableOfColor.Count);
            Box.GetComponent<Renderer>().material.color = TableOfColor[IndexOfColor];
            TableOfColor.RemoveAt(IndexOfColor);
        }
    }



}
