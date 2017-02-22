using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Cataloges
{
    public static List<GameObject> ListOfCataloges;

    public List<GameObject> GenerateCataloges(params string[] t)
    {
        if (ListOfCataloges == null || ListOfCataloges[0] == null)
            ListOfCataloges = new List<GameObject>();

        foreach (string name in t)
        {
            GameObject ob = new GameObject();
            ob.name = name;
            ListOfCataloges.Add(ob);
        }
        return ListOfCataloges;
    }

    public bool AddToCatalog(string s, GameObject GameObjectToAdd)
    {
        if (ListOfCataloges != null && ListOfCataloges[0] != null)
            foreach (GameObject Catalog in ListOfCataloges)
            {
                if (Catalog.name == s)
                {
                    GameObjectToAdd.transform.parent = Catalog.transform;
                    return true;
                }
            }
        //if there is not catalog for that name 
        GenerateCataloges(s);
        GameObjectToAdd.transform.parent = ListOfCataloges[ListOfCataloges.Count - 1].transform;
        return false;
    }
};

