using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


    public class GenerateGrid : MonoBehaviour {

        public GameObject FieldAsset;
        public GameObject BoxAsset;
        public GameObject CrossAsset;
        public GameObject PlayerAsset;
        public GameObject WallAsset;
        public Cataloges Catalog;

        public static int AreaSize = 10 ;
        public int NumberOfBoxes = 3;
        public int NumberOfWalls = 5;
        public List<Color> TableOfColor = new List<Color> { Color.blue, Color.red, Color.yellow, Color.green };
        public static Tile [,] TableOfTiles;
        public static GameObject Player;

        void Start () {
            Catalog = new Cataloges();
        
            TableOfTiles = new Tile[AreaSize, AreaSize];
            GenerateTiles();
            GenerateBoxesAndCrosses();
            GeneratePlayer();
            GenerateWalls();
        }

        void Update () {
            if (GameIsOver())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        void GenerateTiles()
        {
            for (int i = 0; i < AreaSize; i++)
                for (int j = 0; j < AreaSize; j++)
                {
                    Tile field = new Tile(i, j, FieldAsset);
                    Catalog.AddToCatalog("Tiles", field.Asset);
                    TableOfTiles[i, j] = field;
                }
        }

        void GenerateBoxesAndCrosses()
        {
            for(int i = 0; i< NumberOfBoxes; i++)
            {
                int IndexOfColor = Random.Range(0, TableOfColor.Count);

                GameObject Box = Instantiate(BoxAsset, RandomField(), Quaternion.identity) as GameObject;
                Box.tag = "Box";
                Catalog.AddToCatalog("Boxes", Box);            
                Box.GetComponent<Renderer>().material.color = TableOfColor[IndexOfColor];

                GameObject Cross = Instantiate(CrossAsset, RandomField(), Quaternion.Euler(90,0,0)) as GameObject;
                Cross.tag = "Cross";
                Catalog.AddToCatalog("Crosses", Cross);
                Cross.GetComponent<Renderer>().material.color = TableOfColor[IndexOfColor];
                TableOfColor.RemoveAt(IndexOfColor);
            }
        }

        void GenerateWalls()
        {
            for (int i = 0; i < NumberOfWalls; i++)
            {
                GameObject Wall = Instantiate(WallAsset, RandomField(), Quaternion.identity) as GameObject;
                Wall.tag = "Wall";
                Catalog.AddToCatalog("Walls", Wall);
            }
        }

        void GeneratePlayer()
        {
            Player = Instantiate(PlayerAsset, RandomField(), Quaternion.Euler(90,0,0)) as GameObject;
        }

        bool GameIsOver()
        {
            GameObject[] Boxes = GameObject.FindGameObjectsWithTag("Box");
            GameObject[] Crosses = GameObject.FindGameObjectsWithTag("Cross");
            for (int i = 0; i <Boxes.Length; i++)
            {
                if (Boxes[i].transform.position != Crosses[i].transform.position) return false;
            }
            return true;
        }

        static public Vector3 RandomField()
        {
            int x, y;
            do
            {
                x = Random.Range(1, AreaSize - 1);
                y = Random.Range(1, AreaSize - 1);
            } while (IsLocked(x, y));
            TableOfTiles[x, y].isLocked = true;
            return new Vector3(x, y, 0);
        }

        static bool IsLocked(int x, int y)
        {
            return TableOfTiles[x, y].isLocked;
        }
}

