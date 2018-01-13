using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAPScene : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Debug Create SaveData");
            DataManager.Instance.data = GameSaveData.Create();
            DataManager.Instance.SaveData();
        }


    }

}

