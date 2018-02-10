using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AudioManager : MonoBehaviour
{

    static private AudioManager instance = null;
    static GameObject objectBase;
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
                objectBase = new GameObject();
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
    //SE用のAudioSourceを削除用に保存する用
    private Dictionary<string, AudioSource> seAudioSources = new Dictionary<string, AudioSource>();


    //SEを鳴らす
    public AudioSource PlaySE(string se_name, Vector3? playPos = null)
    {
        try
        {

            if (se_name == "gaya")
            {
                Debug.Log("gaya ");
            }
            if (playPos != null)
            {
                ////距離によって
                //var dis = Vector3.Distance(Camera.main.transform.position, playPos.Value);
                //Debug.Log(dis);
                //StartCoroutine(Easing.Deyray(dis / 340f, () =>
                //{
                //    var audsou = gameObject.AddComponent<AudioSource>();
                //    audsou.clip = seAudioClips[se_name];
                //    audsou.Play();
                //    StartCoroutine(AudioSourceIns(audsou));
                //}));
                if (!seAudioClips.ContainsKey(se_name))
                {
                    Debug.LogError(string.Format("{0}のサウンドが読み込まれていません", se_name));
                    return null;
                }
                AudioSource audsou = null;
                GameObject obj = Instantiate(objectBase);

                if (!seAudioSources.ContainsKey(se_name))
                {

                    audsou = obj.AddComponent<AudioSource>();
                    audsou.spatialBlend = 1;
                    obj.transform.localPosition = playPos.Value;

                    audsou.clip = seAudioClips[se_name];
                    seAudioSources.Add(se_name, audsou);
                    audsou.Play();
                }
                else
                {
                    if (seAudioSources[se_name] == null)
                    {
                        // obj = Instantiate(new GameObject());

                        audsou = obj.AddComponent<AudioSource>();
                        audsou.spatialBlend = 1;
                        obj.transform.localPosition = playPos.Value;

                        seAudioSources[se_name] = audsou;
                        audsou.clip = seAudioClips[se_name];
                        audsou.Play();
                    }
                }
                StartCoroutine(AudioSourceIns(audsou, () =>
                {

                    if (!seAudioSources.ContainsKey(se_name))
                    {
                        seAudioSources.Remove(se_name);
                    }
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }));
                return audsou;

            }
            else
            {
                if (!seAudioClips.ContainsKey(se_name))
                {
                    Debug.LogError(string.Format("{0}のサウンドが読み込まれていません", se_name));
                    return null;
                }
                AudioSource audsou = null;
                if (!seAudioSources.ContainsKey(se_name))
                {
                    audsou = gameObject.AddComponent<AudioSource>();
                    audsou.clip = seAudioClips[se_name];
                    seAudioSources.Add(se_name, audsou);
                    audsou.Play();
                }
                else
                {
                    if (seAudioSources[se_name] == null)
                    {
                        audsou = gameObject.AddComponent<AudioSource>();
                        seAudioSources[se_name] = audsou;
                        audsou.clip = seAudioClips[se_name];
                        audsou.Play();
                    }
                }
                StartCoroutine(AudioSourceIns(audsou, () =>
                {
                    if (!seAudioSources.ContainsKey(se_name))
                        seAudioSources.Remove(se_name);
                }));
                return audsou;
            }
        }
        catch
        {
            Debug.LogError("SEを鳴らす機能がバグってます  " + se_name); 
        }
        return null;
    }

    public void StopSE(string _key)
    {
        if (seAudioSources.ContainsKey(_key))
        {
            if (seAudioSources[_key] != null)
            {

                seAudioSources[_key].Stop();
            }
        }
    }

    //PlaySEのAudioClip版
    public AudioSource PlaySE(AudioClip _clip, Action _callback = null)
    {
        var audsou = gameObject.AddComponent<AudioSource>();
        audsou.clip = _clip;
        if (!seAudioSources.ContainsKey(_clip.name))
        {

            seAudioSources.Add(_clip.name, audsou);
            audsou.Play();
        }
        StartCoroutine(AudioSourceIns(audsou, _callback));
        return audsou;
    }

    //SEを鳴らす　　curveの曲線でSEを鳴らします
    public AudioSource FadePlaySE(string se_name, AnimationCurve _curve)
    {
        try
        {
            var audsou = gameObject.AddComponent<AudioSource>();
            var clip = seAudioClips[se_name];
            audsou.clip = clip;
            audsou.Play();

            StartCoroutine(
            Utility.TimeCrou(clip.length, (t) =>
            {
                audsou.volume = _curve.Evaluate(t);
            }));
            StartCoroutine(AudioSourceIns(audsou));
            return audsou;
        }
        catch
        {
            Debug.LogError("SEを鳴らす機能がバグってます  " + se_name);
        }
        return null;
    }

    //SEを鳴らす
    public AudioSource FadeOutPlaySE(string se_name, float _time)
    {
        var audsou = gameObject.AddComponent<AudioSource>();
        var clip = seAudioClips[se_name];
        audsou.clip = clip;
        audsou.Play();

        seAudioSources.Add(se_name, audsou);


        StartCoroutine(
        Utility.TimeCrou(_time, (t) =>
        {
            audsou.volume = t;
        }));
        StartCoroutine(AudioSourceIns(audsou, () =>
        {
            seAudioSources.Remove(se_name);
        }));
        return audsou;
    }

    public AudioSource FadeOutSE(string se_name, float _time)
    {
        if (!seAudioSources.ContainsKey(se_name))
            return null;
        var audioSou = seAudioSources[se_name];
        StartCoroutine(
        Utility.TimeCrou(_time, (t) =>
        {
            if (audioSou != null)
                audioSou.volume = 1 - t;
        }, () =>
         {
             if (audioSou != null)
             {
                 if (seAudioSources.ContainsKey(se_name))
                     seAudioSources.Remove(se_name);
                 audioSou.Stop();
             }
         }));
        return audioSou;
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
    //BGMを止める
    public void setBGMLoop(bool _loop)
    {
        bgmAudioSource.loop = _loop;
    }

    //なり終わったら消す
    IEnumerator AudioSourceIns(AudioSource au, Action callback = null)
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

    //なり終わったら消す
    IEnumerator AudioSourceIns(AudioSource au, string _key, Action callback = null)
    {
        while (au.isPlaying)
        {
            yield return null;
        }
        if (callback != null)
        {
            seAudioSources.Remove(_key);
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
            Debug.Log("効果音  " + a.name);
            seAudioClips.Add(a.name, a);
        }
    }



    void Start()
    {
        Load("Audio/SE");
        //LoadSeList("Test","Audio/SE/Test");
        //addSeIndex("Test");
        //addSeIndex("Test");
        //addSeIndex("Test");
        //PlaySeList("Test");

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
            Resources.LoadAll<AudioClip>("Audio/SE/" + _filename))
            );
    }
    //SEListを鳴らす
    public void StopSeList()
    {
        isSeLoop = false;
    }

    bool isSeLoop = true;
    //SElistを鳴らす
    public void PlaySeList(string _key)
    {
        StartCoroutine(PlaySeListCol2(_key));
    }

    //ランダムでSEを鳴らす
    public void PlayRandomSE(string _key)
    {
        //var audsou = gameObject.AddComponent<AudioSource>();
        var _seList = seList[_key];
        PlaySE(_seList[UnityEngine.Random.Range(0, _seList.Count - 1)]);
    }



    //意味がないので未使用
    public void PlaySeList2(string _key)
    {
        StartCoroutine(PlaySeListCol2(_key));
    }

    //selistをループで鳴らすコルーチン(未使用)
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

        var audsou = gameObject.AddComponent<AudioSource>();
        if (!stockSeIndex.ContainsKey(_key))
        {
            stockSeIndex.Add(_key, new List<int>());
        }
        var stockSeIndexs = stockSeIndex[_key];
        //Debug.Log(_key + "取得");
        var _seList = seList[_key];
        while (isSeLoop)
        {
            //Debug.Log(_key + "なるの待機中");
            int i = 0;
            if (stockSeIndexs.Count != 0)
            {
                audsou.clip = _seList[stockSeIndexs[i]];
                audsou.Play();
                stockSeIndexs.Remove(i);
                while (audsou.isPlaying)
                {
                    yield return null;
                }
                i++;
            }
            yield return null;
        }
        stockSeIndexs.Clear();

        isSeLoop = true;
        AudioSourceIns(audsou);
        yield break;


        //for (int i = 0; i < stockSeIndexs.Count; i++)
        //{

        //    audsou.clip = _seList[stockSeIndexs[i]];
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

        //    //PlaySE(_seList[stockSeIndex[]]);
        //}
        //AudioSourceIns(audsou);
        //yield break;


        //    while (true)
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