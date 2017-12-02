using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

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

    [SerializeField]
    public List<GameObject> Texts = new List<GameObject>();


    [SerializeField]
    Canvas canv;

    [SerializeField]
    GameObject textPrefab;


    public Text addText(Vector3 pos, Vector3 size,string _name, string _text , bool _isFrash = false)
    {

        var text = createNewText(_name);
        text.text = _text;
        Texts.Add(textPrefab);
        text.transform.position = pos;
        text.transform.localScale = size;
        text.transform.SetParent(canv.transform,false);
        var textFrash = text.gameObject.GetComponent<TextFrash>();
        textFrash.useFrash = _isFrash;
        return text;

    }

    public Text createNewText(string _name)
    {

        var obj = Instantiate(textPrefab);

        var text = obj.GetComponent<Text>();

        obj.name = _name;



        return text;
        
    }

    //public GameObject frashText;
    //public GameObject stopText;

    //public void instantiateText(bool isFrash, Vector3 pos , Vector3 size)
    //{
    //    if (isFrash == true) {
    //        GameObject.Instantiate(frashText);
    //    }
    //    if (isFrash == false)
    //    {
    //        GameObject.Instantiate(stopText);
    //    }
    //}
   
}
