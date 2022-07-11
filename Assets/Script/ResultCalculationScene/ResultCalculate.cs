using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ItemState//物体的状态
{
    Standby = 0,//
    Falling = 1,//
    Over = 2
};

public class ResultCalculate : MonoBehaviour
{
    //结算板
    public Animator[] animator;


    public GameObject CurtainControl;       //幕布动画控制
    public GameObject goon;
    
    public Text KitchenWasteText;//厨余垃圾值文本
    public Text RecyclablesText;//可回收垃圾值文本
    public Text HarmfulWasteText;//有害垃圾值文本
    public Text EnvironmentalValuesText;//环保值文本
    public Text SectionCarbonText;//节碳量文本
    private int HB = 0;//统计出的环保值
    private int JT = 0;//统计出的节碳量

    private Sequence[] LJequence;

    public GameObject UICanvas;

    private bool BtnLock=false;       //任意键锁，只允许按一次
    //收集箱
    public ItemState itemState = ItemState.Standby;//物体的状态初始化为待命状态

    public GameObject[] ItemObj;//掉落的物体预制体
    int index = 0;//ItemObj数组的下标

    public GameObject ItemsParent;//生成物体的父物体
    public GameObject StartPoint;//生成起始点
   // public float x_Limit = 5.0f;//生成起始点的移动范围（-x_Limit，x_Limit）

    float Settlement;//结算比例
    float RobNum;//实际生成的个数
    float RunTime=10;


    private void Awake()
    {
        
        CurtainControl.GetComponent<Animator>().SetBool("GoDisappear", true);
        InitData();//初始化各类垃圾数据的数值
        SequenceInit();//动画序列初始化
    }
    // Start is called before the first frame update
    void Start()
    {
        itemState = ItemState.Falling;//物体处于下落状态
        
        Invoke("playAni", 0.5f);
        Invoke("DigitalAnimation",1);
        Invoke("NewItem", 0.5f);
        Invoke("Goon",5);
    }

    // Update is called once per frame
    void Update()
    {
      

        var Count = ItemsParent.gameObject.GetComponentsInChildren<Transform>();//找ItemsParent下子物体的个数
        if (Count.Length - 1 == RobNum)//判定是否全部生成
        {
            itemState = ItemState.Over;
        }
    }

    void InitData()//初始化数据
    {
        LJequence = new Sequence[3];

        JT = 2 * GrobalClass.CY + 4 * GrobalClass.KHS + GrobalClass.YH;
        GrobalClass.Iscore += JT;
        GrobalClass.Iscore = GrobalClass.Iscore - (1-GrobalClass.Power*0.01f)*GrobalClass.AllRob;
        Settlement = (GrobalClass.KHS + GrobalClass.CY + GrobalClass.YH) / (float)GrobalClass.AllRob;
        
        RobNum = (int)(35 * Settlement);//
        HB = (int)(100*(Settlement));
    }
    void SequenceInit()//Sequence初始化
    {
        for (int i = 0; i < 3; i++)
        {
            LJequence[i] = DOTween.Sequence();
        }
    }
    void DigitalAnimation()//数字滚动动画序列
    {
        LJequence[0].Append(DOTween.To(delegate (float value)
        {
            var temp = value;
            KitchenWasteText.text =(int)temp + "kg";
        }, 0, GrobalClass.CY, (RobNum / RunTime)));
        LJequence[1].Append(DOTween.To(delegate (float value)
        {
            var temp = value;
            RecyclablesText.text = (int)temp + "kg";
        }, 0, GrobalClass.KHS, (RobNum / RunTime)));
        LJequence[2].Append(DOTween.To(delegate (float value)
        {
            var temp = value;
            HarmfulWasteText.text = (int)temp + "kg";
        }, 0, GrobalClass.YH, (RobNum / RunTime)));

        Invoke("delayS", (RobNum / RunTime));
    }
    void delayS()
    {
        EnvironmentalValuesText.text = HB.ToString() + "%";
        SectionCarbonText.text = GrobalClass.Iscore.ToString() + "kg";
        animator[1].SetTrigger("HBJTAni");

        if (GrobalClass.FirstComing)
        {
            GrobalClass.FirstComing = false;
            AudioManager.Instance.Play(0);
            AudioManager.Instance.SetStartTime(10f);
        }
        AudioManager.Instance.Fade(true, false);

      

    }

    void NewItem()
    {
        for (int i = 0; i < RobNum; i++)
        {
            Invoke("CreateItem", i / RunTime);//慢慢下落
        }

    }

    void CreateItem()
    {
         index = (++index) % 9;//取0-9

         GameObject newItem = Instantiate(ItemObj[index], StartPoint.transform.position, default, ItemsParent.transform);
    }

    public void LoadToSelect()      //任意键继续
    {
        if(BtnLock==false)
        {
            BtnLock = true;
    
        UICanvas.GetComponent<Animator>().SetTrigger("BeSelect");
        goon.GetComponent<Animator>().SetTrigger("Leave");
        Invoke("RealLoad", 1.5f);
        }

    }
    private void RealLoad()
    {
        if (GrobalClass.SongName == "要牢记")
        {
            GrobalClass.UnLockLevel = 1;
        }

            if (GrobalClass.SongName == "环保歌")
        {
            GrobalClass.UnLockLevel = 2;
            VariableSave.Instance.Start_SaveGame();
        }
        if (GrobalClass.SongName=="骄傲的少年")
        {
            GrobalClass.If_Game_End = true;
          
            VariableSave.Instance.Start_SaveGame();
            GrobalClass.LoadToScene(3);
        }
        else
        {
        GrobalClass.LastScene = "Result_Scene";
            VariableSave.Instance.Start_SaveGame();
            GrobalClass.LoadToScene(5);
        }
    }
    void playAni()
    {
        animator[0].SetTrigger("OpenM");
        animator[1].SetTrigger("IsPlay");
    }

    void Goon()
    {
        goon.SetActive(true);
    }
}
