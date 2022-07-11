using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum NextCG
{
    NextCG1 = 0,
    NextCG2 = 1,
    NextCG3 = 2,
    NextCGOver = 3,
    NextCGEnding = 4
};

public class ControlCG : MonoBehaviour
{
    public NextCG nextCG;//当前CG

    public GameObject nextBtn;//按钮

    public Animator CGAni;

    // Start is called before the first frame update

    public AudioSource CGAudioS;//音频播放器

    public AudioClip[] CGAudioC;//音源
    public AudioClip[] EndingS;//结尾音源

    private bool IsDoone = false;
 
    private int index = -1;

    public GameObject CurtainControl;



    void Start()
    {
        CurtainControl.SetActive(false);
        if (GrobalClass.If_Game_End == false)
        {
            CurtainControl.GetComponent<Animator>().SetTrigger("Wait");
            nextCG = NextCG.NextCG1;
            CGAni.SetTrigger("CG1");
            Invoke("nextAudio", 2);
            AudioManager.Instance.Play(1, true);
        }
        else {
            nextCG = NextCG.NextCGEnding;
            CGAudioS.GetComponent<AudioSource>().clip = EndingS[0];

            Invoke("delayPlayCG4", 1);
        }
        
    }

    private void Update()
    {
        if (nextCG != NextCG.NextCGEnding) {
            AudioIsOver();
        }
        
    }

    public void next()
    {
        nextCG += 1;

        if (nextCG == NextCG.NextCG2)
        {
          

            CGAni.SetTrigger("CG2");

            Invoke("nextAudio", 2);

        }
        else if (nextCG == NextCG.NextCG3)
        {
        

            CGAni.SetTrigger("CG3");

            Invoke("nextAudio", 2);

        }
        else if (nextCG == NextCG.NextCGOver)
        {
            CurtainControl.SetActive(true);
            AudioManager.Instance.Fade(false,true);
      
            Invoke("Load",1f);
            CurtainControl.GetComponent<Animator>().SetTrigger("AppearClose");
        }

    }
    public void Load()
    {

        GrobalClass.SongName = "要牢记";
        GrobalClass.AreaSkin = 0;
        GrobalClass.LoadToScene(2);

    }
    public void LoadToMain()
    {

        VariableSave.Instance.Start_SaveGame();
        GrobalClass.LoadToScene(0);

    }
    public void EndingS2()
    {
        CGAudioS.GetComponent<AudioSource>().clip = EndingS[1];
        CGAudioS.Play();
    }
    public void EndingS3()
    {
        CGAudioS.GetComponent<AudioSource>().clip = EndingS[2];
        CGAudioS.Play();
    }
    void nextAudio()
    {
        index += 1;
        CGAudioS.GetComponent<AudioSource>().clip = CGAudioC[index];
        CGAudioS.Play();
        nextBtn.SetActive(false);
        IsDoone = false;
    }

    void AudioIsOver()
    {
        if (!CGAudioS.isPlaying && IsDoone == false&&index!=-1)
        {
            nextBtn.SetActive(true);
            IsDoone = true;
        }

    }
    void delayPlayCG4()
    {
        CGAni.SetTrigger("CG_Ending");
        CGAudioS.Play();
    }

}
