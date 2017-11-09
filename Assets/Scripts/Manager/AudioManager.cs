using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    static private AudioManager instants = null;
    static public AudioManager Instants
    {
        get
        {
            if (instants == null)
            {
                var obj = Instantiate(new GameObject("AudioManager"));
                var _instans = obj.AddComponent<AudioManager>();
                instants = _instans;
            }
            return instants;
        }
    }
    void Awake()
    {
        instants = this;

        
    }

    [SerializeField]
    AudioSource bgmAudioSource = null;

    private Dictionary<string, AudioClip> seAudioClips = new Dictionary<string, AudioClip>();


    //SEを鳴らす
    public void PlaySE(string se_name,Vector3? playPos = null)
    {
        //var dis = Vector3.Distance(Camera.main.transform.position, new Vector3(playPos));

        var audsou = gameObject.AddComponent<AudioSource>();
        audsou.clip = seAudioClips[se_name];
        audsou.Play();
        StartCoroutine(AudioSourceIns(audsou));
        
    }
    
    
    //BGMを鳴らす
    public void PlayBGM(string se_name)
    {
        bgmAudioSource.clip = Resources.Load<AudioClip>("Audio/BGM" + se_name);
        bgmAudioSource.Play();

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
        
        Load("Audio/SE");
    }


}
