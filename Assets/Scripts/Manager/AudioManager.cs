using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AudioManager : MonoBehaviour
{

    static private AudioManager instance = null;
    static public AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("AudioManager");
                var _instans = obj.AddComponent<AudioManager>();
                instance = _instans;
                instance.bgmAudioSource = obj.AddComponent<AudioSource>();
            }
            return instance;
        }
    }
    void Awake()
    {
        //instance = this;
    }

    [SerializeField]
    AudioSource bgmAudioSource = null;

    private Dictionary<string, AudioClip> seAudioClips = new Dictionary<string, AudioClip>();


    //SEを鳴らす
    public void PlaySE(string se_name, Vector3? playPos = null)
    {
        if (playPos != null)
        {
            //距離によって
            var dis = Vector3.Distance(Camera.main.transform.position, playPos.Value);
            Debug.Log(dis);
            StartCoroutine(Easing.Deyray(dis / 340f, () =>
            {
                var audsou = gameObject.AddComponent<AudioSource>();
                audsou.clip = seAudioClips[se_name];
                audsou.Play();
                StartCoroutine(AudioSourceIns(audsou));
            }));
        }
        else
        {
            var audsou = gameObject.AddComponent<AudioSource>();
            audsou.clip = seAudioClips[se_name];
            audsou.Play();
            StartCoroutine(AudioSourceIns(audsou));
        }

    }
    //PlaySEのAudioClip版
    public void PlaySE(AudioClip _clip, Action _callback = null)
    {
        var audsou = gameObject.AddComponent<AudioSource>();
        audsou.clip = _clip;
        audsou.Play();
        StartCoroutine(AudioSourceIns(audsou, _callback));
    }

    //BGMを鳴らす
    public void PlayBGM(string se_name)
    {
        bgmAudioSource.clip = Resources.Load<AudioClip>("Audio/BGM/" + se_name);
        bgmAudioSource.Play();
    }

    //BGMを止める
    public void StopBGM(string se_name)
    {
        bgmAudioSource.Stop();
    }

    public void setBGMLoop(bool _loop)
    {
        bgmAudioSource.loop = _loop;
    }

    //なり終わったら消す
    IEnumerator AudioSourceIns(AudioSource au)
    {
        while (au.isPlaying)
        {
            yield return null;
        }
        Destroy(au);
    }
    IEnumerator AudioSourceIns(AudioSource au, Action callback)
    {
        while (au.isPlaying)
        {
            yield return null;
        }
        if (callback != null)
        {
            callback();
        }
        Destroy(au);
    }

    //ファイルを読み込みます
    public void Load(string filename)
    {
        var audios = Resources.LoadAll<AudioClip>(filename);
        foreach (var a in audios)
        {
            //Debug.Log("効果音  " + a.name);
            seAudioClips.Add(a.name, a);
        }
    }



    void Start()
    {
        //Load("Audio/SE");
        LoadSeList("Test","Audio/SE/Test");
    }
    void Update()
    {

    }




    //seをランダムに連続して鳴らすための
    Dictionary<string, List<AudioClip>> seList = new Dictionary<string, List<AudioClip>>();

    Dictionary<string, List<int>> stockSeIndex = new Dictionary<string, List<int>>();

    //seのインデックスを追加する
    public void addSeIndex(string _key)
    {
        //stockSeIndex[_key].Add();
        if (!stockSeIndex.ContainsKey(_key))
        {
            stockSeIndex.Add(_key, new List<int>());
        }
        var _seList = seList[_key];
        stockSeIndex[_key].Add(UnityEngine.Random.Range(0, _seList.Count - 1));

    }
    //Soundのリストを名前を付けて登録する
    public void LoadSeList(string _key, string _filename)
    {
        seList.Add(_key,
            new List<AudioClip>(
            Resources.LoadAll<AudioClip>(_filename))
            );
    }
    //SEListを鳴らす
    public void StopSeList()
    {
        isSeLoop = false;
    }

    bool isSeLoop = true;
    public void PlaySeList(string _key)
    {
        StartCoroutine(PlaySeListCol2(_key));

    }
    public void PlaySeList2(string _key)
    {
        StartCoroutine(PlaySeListCol2(_key));
    }

    //selistをループで鳴らすコルーチン
    IEnumerator PlaySeListCol(string _key)
    {
        var _seList = seList[_key];
        if (_seList.Count == 0)
            yield break;

        while (true)
        {
            var audsou = gameObject.AddComponent<AudioSource>();
            audsou.clip = _seList[UnityEngine.Random.Range(0, _seList.Count - 1)];
            audsou.Play();
            while (audsou.isPlaying)
            {
                if (!isSeLoop)
                {
                    isSeLoop = true;
                    yield break;
                }

                yield return null;

            }
        }

    }
    IEnumerator PlaySeListCol2(string _key)
    {
        var _seList = seList[_key];
        if (_seList.Count == 0)
            yield break;

        var audsou = gameObject.AddComponent<AudioSource>();
        var stockSeIndexs = stockSeIndex[_key];
        for (int i = 0; i < stockSeIndexs.Count; i++)
        {
            audsou.clip = _seList[stockSeIndexs[i]];
            audsou.Play();
            while (audsou.isPlaying)
            {
                if (!isSeLoop)
                {
                    isSeLoop = true;
                    yield break;
                }
                yield return null;
            }
        }
        StartCoroutine(AudioSourceIns(audsou));
        yield break;

        //while (true)
        //{
        //    var audsou = gameObject.AddComponent<AudioSource>();
        //    audsou.clip = _seList[UnityEngine.Random.Range(0, _seList.Count - 1)];
        //    audsou.Play();
        //    while (audsou.isPlaying)
        //    {
        //        if (!isSeLoop)
        //        {
        //            isSeLoop = true;
        //            yield break;
        //        }

        //        yield return null;

        //    }
        //}

    }


    //でバック用の出力
    public void ShowSeNames()
    {
        foreach (var a in seAudioClips)
        {
            Debug.Log(a.Key);
        }
    }

    public Dictionary<string, AudioSource> loopSources = new Dictionary<string, AudioSource>();
    public void PlaySELoop(string source_name, string se_name, float vol = 1.0f)
    {
        var audsou = gameObject.AddComponent<AudioSource>();
        //ディクショナリーに登録しておく
        loopSources.Add(source_name, audsou);

        audsou.clip = seAudioClips[se_name];
        audsou.loop = true;
        vol = Mathf.Clamp(vol, 0.0f, 1.0f);
        audsou.volume = vol;
        audsou.Play();
    }
    public void StopSELoop(string source_name)
    {
        if (loopSources.ContainsKey(source_name))
        {
            var sou = loopSources[source_name];
            sou.Stop();
            loopSources.Remove(source_name);
            Destroy(sou);
        }
    }


}