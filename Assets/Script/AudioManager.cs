using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;    //单例模式

    private AudioSource audioSource;

    public AudioClip[] audioClips;
    public AudioClip[] audioClips_effect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        if (Instance == null)//如果
        {
            Instance = this;//自身是音频管理器

            //让引擎不在场景切换的时候销毁音频（换图的时候不重新播放音乐啥的）
            DontDestroyOnLoad(gameObject);

            //查找音效播放器组件
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            //如果是多余的就销毁
            Destroy(gameObject);
        }
    }

    public void CheckBG()
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.Play();

        }
    }

    public void Play(int index, bool bgOver = false)
    {
        //如果需要关闭音乐
        if (bgOver)
        {
            audioSource.Stop();
        }
        if (index != 999)      //如果为999则不播放
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();        
        }

    }
    public void PlayShot(int index)
    {
        audioSource.PlayOneShot(audioClips_effect[index]);
    }
    public void SetStartTime(float i)
    {
        audioSource.SetScheduledStartTime(i);
    }
    public void SetValume(float i)
    {
        audioSource.volume = i;
    }
    public void Fade(bool i,bool IfEnd)   //淡入淡出，开为淡入,ifend判断是否淡出后完全结束该播放
    {
        if (i)
        {
            
            StartCoroutine(Fade_In());
        }
        else
        {
            StartCoroutine(Fade_Out(IfEnd));
        }
    }

    IEnumerator Fade_In()
    {
        audioSource.UnPause();
        audioSource.volume = 0;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime*0.5f;

            yield return null;
        }
       
    }
    IEnumerator Fade_Out(bool ifend)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime*0.5f;

            yield return null;
        }

        audioSource.volume = 1;
        if(ifend)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Pause();
        }
        
    }
}
