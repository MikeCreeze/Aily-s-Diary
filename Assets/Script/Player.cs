using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TheJudgementPoint
{
    public bool IfKeeping;
    public bool IfPress;

    public TheJudgementPoint()
    {
        IfKeeping = false;
        IfPress = false;    //判长按条反馈给此变量，以此判定按键应不应该保持长按状态
    }
}
public class Player : MonoBehaviour
{

    private Animator PlayerAni;
    public bool IfGameStart;
    private bool StartLock;     //开始锁，用于update中开始与结束仅执行一次的判断

    public GameObject JudgmentPointManager;
    public static Player Instance;

    public TheJudgementPoint UpPoint = new TheJudgementPoint();

    public TheJudgementPoint DownPoint = new TheJudgementPoint();

    public bool NextUpWaiting = false;

    float rtime = 0;

    public void KeepCheck()      //检测保持长按的按键是否“真的”被按下（配合Invloke在短暂等待后执行该判断，等待期间为JudgementPoint决定IfPress是否打开，打开则进入长按模式）
    {
        if (UpPoint.IfPress == false)
        {

            UpPoint.IfKeeping = false;
        }
        else
        {

            PlayerAni.SetBool("UpKeep", true);   //判定点动画
        }


        if (DownPoint.IfPress == false)
        {

            DownPoint.IfKeeping = false;
        }
        else
        {
            PlayerAni.SetBool("DownKeep", true);   //判定点动画
        }



    }
    public void ResetUpHit()        //落地重置
    {
        NextUpWaiting = false;
    }
    private void Awake()
    {
        Instance = this;

        if (PlayerAni == null)
        {
            PlayerAni = this.gameObject.GetComponent<Animator>();
        }

    }
    void Update()
    {
        rtime += Time.deltaTime*0.5f;
        if(rtime>1)
        {
            rtime = 0;
        }

        Color A = Color.HSVToRGB(rtime, 0.5F, 1,true);
        JudgmentPointManager.transform.Find("Up_Judgment").GetComponent<SpriteRenderer>().color = A;
        JudgmentPointManager.transform.Find("Down_Judgment").GetComponent<SpriteRenderer>().color = A;
        if (IfGameStart)
        {
            if(StartLock==false)
            {
                StartLock = true;
                PlayerAni.SetBool("Start", true);      //播放玩家模型入场动画
            }

            if (Input.GetKeyDown(KeyCode.S))
            {

                UpHit();
              


               
            }
            if (Input.GetKeyUp(KeyCode.S) || UpPoint.IfKeeping == false)
            {
                UpHitCancle();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {

                DownHit();

            }
            if (Input.GetKeyUp(KeyCode.C) || DownPoint.IfKeeping == false)
            {
                DownHitCancle();
            }

        }
        else if(IfGameStart==false&&StartLock==true&&GrobalClass.Power>0)
        {
            StartLock = false;
            PlayerAni.SetBool("Start", false);
            PlayerAni.SetTrigger("EndTrigger");
        }
    }

    //=====
    void DownHit()
    {
        JudgmentPointManager.GetComponent<Animator>().SetBool("Down", true);
        JudgmentPointManager.transform.Find("Down_Judgment").Find("Down_Judge").gameObject.SetActive(true);      //启用判定点，这个起实质作用
        JudgmentPointManager.transform.Find("Down_Judgment").Find("Down_Judge").Find("Down_Judge_Perfect").gameObject.SetActive(true);
        JudgmentPointManager.transform.Find("Down_Judgment").Find("Down_Judge").Find("Down_Judge_Normal").gameObject.SetActive(true);
        Invoke("SetActiveTrue_Down_Judge_Bad", 0.01f);      //万不得已不触发极差，故加个延时
        PlayerAni.SetBool("DownHit", true);

        DownPoint.IfKeeping = true;
        Invoke("KeepCheck", 0.05f);
    }
    void UpHit()
    {
        if (NextUpWaiting == false)
        {


            NextUpWaiting = true;
           // Debug.Log("起跳");

            JudgmentPointManager.GetComponent<Animator>().SetBool("Up", true);   //判定点动画
            JudgmentPointManager.transform.Find("Up_Judgment").Find("Up_Judge").gameObject.SetActive(true);      //启用判定点，这个起实质作用
            JudgmentPointManager.transform.Find("Up_Judgment").Find("Up_Judge").Find("Up_Judge_Perfect").gameObject.SetActive(true);
            JudgmentPointManager.transform.Find("Up_Judgment").Find("Up_Judge").Find("Up_Judge_Normal").gameObject.SetActive(true);
            Invoke("SetActiveTrue_Up_Judge_Bad", 0.01f);      //万不得已不触发极差，故加个延时
            PlayerAni.SetBool("UpHit", true);

            UpPoint.IfKeeping = true;
            Invoke("KeepCheck", 0.05f);
        }
    }

    void SetActiveTrue_Up_Judge_Bad()
    {
        JudgmentPointManager.transform.Find("Up_Judgment").Find("Up_Judge").Find("Up_Judge_Bad").gameObject.SetActive(true);
    }
    void SetActiveTrue_Down_Judge_Bad()
    {
        JudgmentPointManager.transform.Find("Down_Judgment").Find("Down_Judge").Find("Down_Judge_Bad").gameObject.SetActive(true);
    }

    void SetActiveFalse_Up_Judge_Bad()
    {
        JudgmentPointManager.transform.Find("Up_Judgment").Find("Up_Judge").Find("Up_Judge_Bad").gameObject.SetActive(false);
    }
    void SetActiveFalse_Down_Judge_Bad()
    {
        JudgmentPointManager.transform.Find("Down_Judgment").Find("Down_Judge").Find("Down_Judge_Bad").gameObject.SetActive(false);
    }
    
    
    void DownHitCancle()
    {
        JudgmentPointManager.GetComponent<Animator>().SetBool("Down", false);
        JudgmentPointManager.transform.Find("Down_Judgment").Find("Down_Judge").gameObject.SetActive(false);
        Invoke("SetActiveFalse_Down_Judge_Bad", 0.01f);      //万不得已不触发极差，故加个延时
        JudgmentPointManager.transform.Find("Down_Judgment").Find("Down_Judge").Find("Down_Judge_Normal").gameObject.SetActive(false);
        JudgmentPointManager.transform.Find("Down_Judgment").Find("Down_Judge").Find("Down_Judge_Perfect").gameObject.SetActive(false);

        PlayerAni.SetBool("DownHit", false);
        PlayerAni.SetBool("DownKeep", false);   //判定点动画
    }
    void UpHitCancle()
    {
        JudgmentPointManager.GetComponent<Animator>().SetBool("Up", false);
        JudgmentPointManager.transform.Find("Up_Judgment").Find("Up_Judge").gameObject.SetActive(false);
        Invoke("SetActiveFalse_Up_Judge_Bad", 0.01f);      //万不得已不触发极差，故加个延时(优先加分)
        JudgmentPointManager.transform.Find("Up_Judgment").Find("Up_Judge").Find("Up_Judge_Normal").gameObject.SetActive(false);
        JudgmentPointManager.transform.Find("Up_Judgment").Find("Up_Judge").Find("Up_Judge_Perfect").gameObject.SetActive(false);


        PlayerAni.SetBool("UpHit", false);
        PlayerAni.SetBool("UpKeep", false);   //判定点动画
    }
    public void PhoneBottomEvent(int I)
    {
        if (IfGameStart)
        {
            if (I == 1)
            {
                UpHit();
            }
            else if (I == 2)
            {
                UpHitCancle();
            }
            else if (I == 3)
            {
                DownHit();
            }
            else if (I == 4)
            {
                DownHitCancle();
            }
        }

    }

    public void PowerOff()
    {
       
            PlayerAni.SetTrigger("PowerOff");
        PlayerAni.SetBool("Start",false);
        IfGameStart = false;
    }

  
    public void SetSliderParticleUp()       
    {
        GameStart.Instance.ParticlePlay = 1;
    }
    public void SetSliderParticleDown()
    {
        GameStart.Instance.ParticlePlay = 2;
    }
    public void SetBeAttacked()
    {
        PlayerAni.SetTrigger("BeAttacked");
    }
}


