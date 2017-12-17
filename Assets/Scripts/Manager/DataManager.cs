using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static private DataManager instance = null;
    static public DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("DataManager");
                var ins = obj.AddComponent<DataManager>();
                instance = ins;
            }
            return instance;
        }
    }

    void Awake()
    {
        //Debug.Log(this + " Awake");
        if (instance != null)
        {
            //Debug.Log("Destroy " + this);
            Destroy(this.gameObject);
        }
    }
}
