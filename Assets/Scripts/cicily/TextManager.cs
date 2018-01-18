using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{

    static private TextManager instance;
    static public TextManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Create TextManager Instance");
                var obj = new GameObject("TextManager");
                var _instance = obj.AddComponent<TextManager>();
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
    public Dictionary<string,GameObject> texts = new Dictionary<string,GameObject>();

    [SerializeField]
    Canvas canv;

    [SerializeField]
    GameObject textPrefab;


    public Text addTextFrash(Vector3 pos, Vector3 size, string _name, string _text, bool _isFrash = false)
    {

        var text = createNewTextFrash(_name);
        text.text = _text;
        text.transform.position = pos;
        text.transform.localScale = size;
        text.transform.SetParent(canv.transform, false);
        var textFrash = text.gameObject.GetComponent<TextFrash>();
        textFrash.useFrash = _isFrash;
        texts.Add(_name, text.gameObject);
        return text;

    }

    public Text createNewTextFrash(string _name)
    {

        var obj = Instantiate(textPrefab);
        var text = obj.GetComponent<Text>();
        obj.name = _name;

        return text;

    }

    public void SetPrefab( GameObject obj )
    {
        textPrefab = obj;
    }

    public void SetCanvas(Canvas c)
    {
        canv = c;
    }

}
