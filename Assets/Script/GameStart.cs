using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;//注意要using Playables和Timeline这两个

public class GameStart : MonoBehaviour
{
    public static GameStart Instance;
    [SerializeField]        //强制序列化私有域，在列表中显示成员
    private Sprite []ImageGround;
    //===============================================
    public int ParticlePlay = 0;      //是否播放打击粒子，0默认1上2下
    private GameObject Particle_System;
    public GameObject Particle_Perfab;
    //===============================================

    private int i = 0;                  //音乐块组下标
    public GameObject MusicSurface;
    public GameObject Slider_MusicCube;
    public GameObject MusicCube_Perfab_Up;     //音乐块预制体
    public GameObject MusicCube_Perfab_Down;
    public GameObject Missile_Perfab;       //导弹预制体


    public GameObject PowerCube_Perfab; //能源预制体
    public GameObject Trap_Perfab_Up; //陷阱预制体，注意上为需要跳上


    //===============================================

    public GameObject PlayerObj;             //玩家组件（实际链接的是那根棍子）
    public GameObject RandomElementArea;    //随机建筑产生区，控制开始随机
    public GameObject CurtainControl;       //幕布动画控制

    public GameObject Power_Slider;         //能源条的UI
    private bool PowerOffLock;               //能源耗尽锁

    private bool PlayerAppearLock = false;

    public float RunTime;            //运行时间(秒)

    public PlayableDirector TimeController;//创建一个PlayableDirector类型的变量

    public GameObject JudgeGuide_Perfab;//判定点提示(打击，引导等，与判定点功能无关联)

    public bool BossComing = false;
    private bool BossExist = false;     //Boss是否存在

    [Header("时间轴组件")]
    [SerializeField]//在面板里面显示下面的这个数组
    private TimelineAsset[] TimelineList;//声明一个TimeLine资源数组

    [Header("分数与Combo")]
    public Text TScoreText;     //显示分数
    public Text TCombo;          //连击
    public GameObject ComboEffect; 

    bool Pause=false;
    public GameObject PauseBtn;
    public GameObject UICanvas;
    public GameObject PauseUI;

    void InitVar()
    {

        GrobalClass.RealGameStart = false;
        RunTime = 0;

        GrobalClass.Iscore = 0;
        GrobalClass.Combo = 0;
        GrobalClass.Power = 100;
        GrobalClass.KHS = 0;
        GrobalClass.CY = 0;
        GrobalClass.YH = 0;
        GrobalClass.AllRob = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        InitVar();
    
        TimeController = FindObjectOfType<PlayableDirector>();//给PlayableDirector变量赋值
        TCombo.text = GrobalClass.Combo.ToString();
        Invoke("RealStart", 0.1f);
        if(GrobalClass.FirstComing==true)
        {
            Camera_Control.Instance.Coming();
            CurtainControl.GetComponent<Animator>().SetTrigger("FirstComing");
            PauseBtn.SetActive(false);
        }

    }
 
    // Update is called once per frame
    void  FixedUpdate()
    {
        TScoreText.text = "Score:" + GrobalClass.Iscore.ToString(); //展示分数
        
        //Debug.Log(GrobalClass.AllRob);
        Power_Slider.GetComponent<Slider>().value = GrobalClass.Power;
        if(BossComing)
        {

            Boss.Instance.Alive();
          
            Debug.Log("BOSS进入");
            BossComing = false;
            BossExist = true;
        }
        if(BossExist)
        {
            GrobalClass.BossWinTime += Time.deltaTime;
            if(GrobalClass.BossWinTime>5)
            {
                BossExist = false;
                Boss.Instance.Win();
            }
        }
        switch(ParticlePlay)            //创建打击效果(一般为打击到物品执行一次)
        {
            case 1:

                Particle_System = Instantiate(Particle_Perfab, default);
                Particle_System.transform.localPosition = new Vector2(-11.2f, 1.2f);
                Particle_System.GetComponent<ParticleSystem>().Play();

                Instantiate(JudgeGuide_Perfab, new Vector2(-11.2f, 1.64f), default);

                ParticlePlay = 0;

                if (BossExist)
                {
                    Particle_System = Instantiate(Missile_Perfab, default);
                }

                break;
            case 2:

                Particle_System = Instantiate(Particle_Perfab, default);
                Particle_System.transform.localPosition = new Vector2(-11.2f, -3.4f);
                Particle_System.GetComponent<ParticleSystem>().Play();

                Instantiate(JudgeGuide_Perfab, new Vector2(-11.2f, -3.4f), default);

                ParticlePlay = 0;

                if (BossExist)
                {
                    Particle_System = Instantiate(Missile_Perfab, default);
                }
                   
                break;
        }
       
        if (int.Parse (TCombo.text)!= GrobalClass.Combo)
        {
            if(GrobalClass.Combo!=0&& GrobalClass.Combo >=3)
            {

                TCombo.GetComponent<Animator>().SetTrigger("BeChange");     //不是因为断了combo则播放弹跳效果
                
                Particle_System = Instantiate(ComboEffect, default);
                Particle_System.transform.localPosition = new Vector2(0f,6f);
                Particle_System.GetComponent<ParticleSystem>().Play();

            }
            else
            {
                TCombo.color = new Vector4(TCombo.color.r, TCombo.color.g, TCombo.color.b, 0);
            }
            TCombo.text = GrobalClass.Combo.ToString();
        }
        else
        {
            if(GrobalClass.Combo >= 3)      //大于才显示
            {
                TCombo.color = new Vector4(TCombo.color.r, TCombo.color.g, TCombo.color.b, 100);
            }
          
        }
          
        //============================================================ 旧版音乐块入场逻辑，在update中使用，现已用TimeLine代替（仍可用于玩家自定义谱面，暂时保留）
        if (GrobalClass.RealGameStart == true)
        {
            RunTime += Time.deltaTime;
            PowerCheck();


            if (RunTime >= GrobalClass.PlayerAppearTime && PlayerAppearLock == false)     //判断玩家何时入场(TXT中第一项)
            {
                PlayerAppearLock = true;
                Player.Instance.IfGameStart = true;                               //将玩家脚本的游戏开始打开，此时可以控制玩家
            }
         

            if (i < GrobalClass.MuCubeGroundList.Count && RunTime >= GrobalClass.MuCubeGroundList[i].AppearTime)       //判断音乐块的产生
            {


                GameObject child;       //预制体产生临时存储
                if (GrobalClass.MuCubeGroundList[i].AppearPlace ==1)       //如果成员变量为2则刷在下面，否则刷上面(为1的时候)
                {
                    int TE = (int)GrobalClass.MuCubeGroundList[i].CubeType;
                    //Debug.Log(TE);
                    //Debug.Log(GrobalClass.MuCubeGroundList[i].CubeType);
                    switch (TE)
                    {
                        case 1:
                            
                            child = Instantiate(MusicCube_Perfab_Up, new Vector3(24.5f, 16.85f, 0), default, MusicSurface.transform);
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<SpriteRenderer>().sprite = ImageGround[Random.Range(0, 3)];
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().Cube_Place = true;//赋值位置，开为上关为下
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().LJ_Type = "可回收";
                            break;
                        case 2:

                            child = Instantiate(MusicCube_Perfab_Up, new Vector3(24.5f, 16.85f, 0), default, MusicSurface.transform);
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<SpriteRenderer>().sprite = ImageGround[Random.Range(4, 7)];
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().Cube_Place = true;
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().LJ_Type = "厨余";
                            break;
                        case 3:

                            child = Instantiate(MusicCube_Perfab_Up, new Vector3(24.5f, 16.85f, 0), default, MusicSurface.transform);
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<SpriteRenderer>().sprite = ImageGround[Random.Range(8, 10)];
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().Cube_Place = true;
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().LJ_Type = "有害";
                            break;
                        case 4:

                            child = Instantiate(Trap_Perfab_Up, new Vector3(27f, -14f, 0), default, MusicSurface.transform);
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<SpriteRenderer>().sprite = ImageGround[Random.Range(11, 14)];
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().Cube_Place = true;
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().LJ_Type = "陷阱";
                            break;

                        case 5:
                            child = Instantiate(PowerCube_Perfab, new Vector3(27f, 1.65f, 0), default, MusicSurface.transform);
                            child.transform.Find("本体").GetComponent<MusicCubePerfab>().Cube_Place = true;
                            child.transform.Find("本体").GetComponent<MusicCubePerfab>().LJ_Type = "奖励";
                            break;
                        case 10:     //滑块条
                            float TN = (GrobalClass.MuCubeGroundList[i].CubeType - 10) * 1000;
                            GameObject childd = Instantiate(Slider_MusicCube, new Vector3(24.5f, 1.2f, 0), default, MusicSurface.transform);
                            childd.transform.Find("判定条").GetComponent<RectTransform>().sizeDelta = new Vector2(TN, 3);
                            break;

                        default:
                            Debug.Log("产生在 上 没有该类型块!!!"); break;
                             
                               
                           
                    }
                   
                }
                else
                {
                    int TE = (int)GrobalClass.MuCubeGroundList[i].CubeType;

                    switch (TE)
                    {
                        case 1:

                            child = Instantiate(MusicCube_Perfab_Down, new Vector3(24.5f,-18.8f, 0), default, MusicSurface.transform);
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<SpriteRenderer>().sprite = ImageGround[Random.Range(0, 3)];
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().Cube_Place = false;//赋值位置，开为上关为下
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().LJ_Type = "可回收";
                            break;
                        case 2:

                            child = Instantiate(MusicCube_Perfab_Down, new Vector3(24.5f, -18.8f, 0), default, MusicSurface.transform);
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<SpriteRenderer>().sprite = ImageGround[Random.Range(4, 7)];
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().Cube_Place = false;//赋值位置，开为上关为下
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().LJ_Type = "厨余";
                            break;
                        case 3:

                            child = Instantiate(MusicCube_Perfab_Down, new Vector3(24.5f, -18.8f, 0), default, MusicSurface.transform);
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<SpriteRenderer>().sprite = ImageGround[Random.Range(8, 10)];
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().Cube_Place = false;//赋值位置，开为上关为下
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().LJ_Type = "有害";
                            break;


                        //------注意！！！！！！！！！这里应当是躲避上陷阱，但没实装，暂时复制了上块的代码
                        case 4:

                            child = Instantiate(Trap_Perfab_Up, new Vector3(27f, -14f, 0), default, MusicSurface.transform);
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<SpriteRenderer>().sprite = ImageGround[Random.Range(11, 14)];
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().Cube_Place = true;
                            child.transform.Find("棍子").transform.Find("本体").gameObject.GetComponent<MusicCubePerfab>().LJ_Type = "陷阱";
                            break;
                      //----
                        case 5:
                            child = Instantiate(PowerCube_Perfab, new Vector3(27f, -3.45f, 0), default, MusicSurface.transform);
                            child.transform.Find("本体").GetComponent<MusicCubePerfab>().Cube_Place = false;
                            child.transform.Find("本体").GetComponent<MusicCubePerfab>().LJ_Type = "奖励";
                            break;
                        case 10:     //2为滑块条
                                
                            float TN = (GrobalClass.MuCubeGroundList[i].CubeType - 10) * 1000;

                            GameObject childd = Instantiate(Slider_MusicCube, new Vector3(24.5f, -3.46f, 0), default, MusicSurface.transform);
                            childd.transform.Find("判定条").GetComponent<RectTransform>().sizeDelta = new Vector2(TN, 3);
                            break;

                        default:
                            Debug.Log("产生在 下 没有该类型块!!!"); break;
                    }
                    // Debug.Log("上" + (GrobalClass.MuCubeGroundList[i].AppearTime + 1f));
                }
                i += 1;
            }

            if(RunTime>=GrobalClass.SongEndTime)
            {
                CurtainControl.GetComponent<Animator>().SetTrigger("CurtainClose");
                Debug.Log("歌曲结束");
                GrobalClass.RealGameStart = false;
                Player.Instance.IfGameStart = false;
              
                Invoke("LoadToCalculate", 5f);
            }
        }
    }
    public void LoadToCalculate()
    {
        GrobalClass.MuCubeGround = null;
        GrobalClass.MuCubeGroundList.Clear();


        GrobalClass.LoadToScene(4);
    }
    public void PhoneBottomUP_Down()
    {
        Player.Instance.PhoneBottomEvent(1);
    }
    public void PhoneBottomUP_Up()
    {
        Player.Instance.PhoneBottomEvent(2);
    }
    public void PhoneBottomDown_Down()
    {
        Player.Instance.PhoneBottomEvent(3);
    }
    public void PhoneBottomDown_Up()
    {
        Player.Instance.PhoneBottomEvent(4);
    }

    public void CreateSlider_Up(float L)                 //生成滑块条音乐块,L为长度,P为位置
    {

        GameObject childd = Instantiate(Slider_MusicCube, new Vector3(24.5f, 1.2f, 0), default, MusicSurface.transform);
        childd.transform.Find("判定条").GetComponent<RectTransform>().sizeDelta = new Vector2(L, 1);
        Debug.Log("判定条上" + RunTime);
    }

    public void CreateSlider_Down(float L)                 //生成滑块条音乐块,L为长度
    {

        GameObject childd = Instantiate(Slider_MusicCube, new Vector3(24.5f, -3.46f, 0), default, MusicSurface.transform);
        childd.transform.Find("判定条").GetComponent<RectTransform>().sizeDelta = new Vector2(L, 1);
        Debug.Log("判定条下" + RunTime);

    }

    public void SetPause()                  //退出游戏
    {

        if (!Pause)
        {
            
            Pause = true;
            PauseUI.SetActive(true);
            Time.timeScale = 0f;
            TimeController.Pause();
        }

      
    }
    public void SetContinue()
    {
        Pause = false;
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        TimeController.Play();
        
    }
    public void SetQuit()
    {
        CurtainControl.GetComponent<Animator>().SetTrigger("CurtainQuickClose");
        PauseUI.transform.Find("Image").transform.Find("Quit").GetComponent<Button>().enabled = false;
        Time.timeScale = 1f;
        Invoke("RealQuit", 1.5f);
    }
    public void RealQuit()
    {
        GrobalClass.MuCubeGround = null;
        GrobalClass.MuCubeGroundList.Clear();

        GrobalClass.LastScene = "Game_Scene";
        GrobalClass.LoadToScene(5);

    
    }
    public void RealStart()               
    {
        GrobalClass.RealGameStart = true;
        
        RandomElementArea.GetComponent<Animator>().SetTrigger("ElementStart");
        
        TimeController.Play(TimelineList[GrobalClass.SongList]);
 
    }


    private void PowerCheck()
    {
        if(GrobalClass.FirstComing==false)      //非新手教程则实装能源系统
        {
          
            if (Pause == false && Player.Instance.IfGameStart == true)
            {
                UICanvas.GetComponent<Animator>().SetBool("HavePower", true);
           
            }
            if (GrobalClass.Power > 100)
            {
                GrobalClass.Power = 100;
            }
            if (GrobalClass.Power <= 0 && PowerOffLock == false)
            {
                Player.Instance.PowerOff();
                PowerOffLock = true;
                UICanvas.GetComponent<Animator>().SetTrigger("PowerOff");
                
                Invoke("PowerOff_End", 2f);

            }
        }
       
    }
    private void PowerOff_End()
    {
        SetQuit();
    }


    
    public void SetBossIn(int Heal)
    {
        BossComing = true;
        GrobalClass.BossHealth = Heal;
        GrobalClass.BossWinTime = 0;
    }
}

