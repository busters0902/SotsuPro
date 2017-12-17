using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    static private ScoreManager instance = null;
    static public ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("ScoreManager");
                var ins = obj.AddComponent<ScoreManager>();
                instance = ins;
                //Debug.Log(obj + " NullInstance");
            }
            return instance;
        }
    }
    
    void Awake()
    {
        //Debug.Log(this + " Awake");
        if(instance != null)
        {
            //Debug.Log("Destroy " + this);
            Destroy(this.gameObject);
        }
    }

}
