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
    public void PlaySE(AudioClip _clip, Action _callback)
    {
        var audsou = gameObject.AddComponent<AudioSource>();
        audsou.clip = _clip;
        audsou.Play();
        StartCoroutine(AudioSourceIns(audsou, _callback));
    }
    //BGMを鳴らす
    public void PlayBGM(string se_name)
    {
        bgmAudioSource.clip = Resources.Load<AudioClip>("Audio/BGM" + se_name);
        bgmAudioSource.Play();
    }
    public void StopBGM(string se_name)
    {
        bgmAudioSource.Stop();
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


    void Start()
    {
        Load("Audio/SE");
        SeListLoad("Test","Audio/SE/Test");


    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(seListPlayCol("Test"));
        }
    }

    public void setBGMLoop(bool _loop)
    {
        bgmAudioSource.loop = _loop;
    }


    //seをランダムに連続して鳴らすための
    Dictionary<string, List<AudioClip>> seList = new Dictionary<string, List<AudioClip>>();
    //Soundのリストを名前を付けて登録する
    public void SeListLoad(string _key, string _filename)
    {
        seList.Add(_key,
            new List<AudioClip>(
            Resources.LoadAll<AudioClip>(_filename))
            );
    }

    public void PlaySeList(string _key)
    {
        StartCoroutine(seListPlayCol(_key));
    }
    bool isSeLoop = true;
    public void StopSeList()
    {
        isSeLoop = false;
    }
    //selistをループで鳴らす
    IEnumerator seListPlayCol(string _key)
    {
        var _seList = seList[_key];
        if (_seList.Count == 0)
            yield break;

        while (true)
        {
            var audsou = gameObject.AddComponent<AudioSource>();
            audsou.clip = _seList[UnityEngine.Random.Range(0, _seList.Count-1)];
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

    //でバック用の出力
    public void ShowSeNames()
    {
        foreach (var a in seAudioClips)
        {
            Debug.Log(a.Key);
        }
    }

}