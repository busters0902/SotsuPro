using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI3DManager : MonoBehaviour {

    

    static private UI3DManager instance;
    static public UI3DManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Create UI3DManager Instance");
                var obj = new GameObject("UI3DManager");
                var _instance = obj.AddComponent<UI3DManager>();
            }
            return instance;
        }
    }


    private void Awake()
    {
        if (instance != null) Destroy(this.gameObject);
        instance = this;
    }

    [HideInInspector]
    public Dictionary<string, UIAnimation> uis = new Dictionary<string, UIAnimation>();

   
    

   
    public void loadUIAnimation(string _name ,UIAnimation _uiAnim)
    {
        uis.Add(_name, _uiAnim);
    }

    public UIAnimation getUI(string _name)
    {
        if (uis[_name] == null)
        {
            Debug.Log("UIないよ");
        }
        return uis[_name];
    }

    public void showUI(string _name)
    {
        if (uis[_name] == null)
        {
            Debug.Log("UIないよ");
        }
        else
        {
            uis[_name].gameObject.SetActive(true);
        }
    }

    public void hideUI(string _name)
    {
        if (uis[_name] == null)
        {
            Debug.Log("UIないよ");
        }
        else
        {
            uis[_name].gameObject.SetActive(false);
        }
    }

    //public void addUiFrash(Vector3 pos, Vector3 size, int _size, string _name, bool _isFrash = false)
    //{
    //    var ui = createNewAnimUI(_name);
    //    ui.sprite =
    //    ui.transform.position = pos;
    //    ui.transform.localScale = size;
    //    ui.transform.SetParent(canv.transform, false);
    //    var uiFrash = ui.gameObject.GetComponent<UIAnimation>();
    //    uiFrash.useFrash = _isFrash;
    //    uis.Add(_name, ui.gameObject);
    //    return ui;
    //}



  
}
