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

   
    public void playAnim(string _name)
    {
        uis[_name].animState = UIAnimation.AnimState.PLAY;
    }

    public void pauseAnim(string _name)
    {
        uis[_name].animState = UIAnimation.AnimState.PAUSE;
    }


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

  public void loadUIAnimation(UIAnimationItem[] items)
    {
        foreach (var i in items)
        {
            loadUIAnimation(i.name,  i.obj);
        }
    }
  
}

[System.Serializable]
 public class UIAnimationItem 
{
    public string name;
    public UIAnimation obj;
}