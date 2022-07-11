using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum whereLevel { 
    LevelNull = 0,
    LevelOne = 1,
    LevelTwo = 2
 
};

public class Instructions : MonoBehaviour
{
    whereLevel nextLevel = whereLevel.LevelNull;//将要切换的关卡

    public Text instructionsText;//介绍的文字
    public string words;//实时显示的文本
    
    public float timeinterval = 0.3f;//打字时间间隔,越小越快
    public bool isActive = false;
    public float timer;//计时器
    public int currentPos;//当前打字的位置

    public Text tips;//提示文本
    public GameObject nextBut;//继续按钮
    public Animator butAni;//按钮动画

    public GameObject Curtain_Ani;
    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        nextBut.SetActive(false);//继续按钮隐藏

        nextLevel = nextLevel + GrobalClass.nextNum;
        ShowText();

        timer = 0;
        isActive = true;
        words = instructionsText.text;
        instructionsText.text = "";
        tips.text = "";
        Curtain_Ani.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        OnStartWriter();
    }

    void OnStartWriter() {
        if (isActive)
        {
          
            timer += 0.05f;
            if (timer >= timeinterval)
            {
                timer = 0;
                currentPos++;
                instructionsText.text = words.Substring(0,currentPos);
                if (currentPos >= words.Length)
                {
                    OnFinish();
                }
            }
        }
    }
    void OnFinish() {
        isActive = false;
        timer = 0;
        currentPos = 0;
        instructionsText.text = words;

        if (nextLevel == whereLevel.LevelOne)
        {
            tips.text = "能源系统:绿宝运行需消耗能源，被杂物击中损失更多";
        }
        else if (nextLevel == whereLevel.LevelTwo)
        {
            tips.text = "回收系统：可回收垃圾能够为绿宝补充能源\n大魔王：大魔王存在期间绿宝将能发射飞弹，一定时间内未击败,绿宝将承受高额伤害";
        }
        else
        {
            tips.text = "音乐块打击：屏幕左侧为下打击，右侧为上打击。";
        }
       

        nextBut.SetActive(true);
        butAni.SetTrigger("ButFadeIn");
    }

    void ShowText() {
        if (nextLevel == whereLevel.LevelOne)
        {
            instructionsText.text = "6月14日  星期一 天气：小雨   \n我将我的想法告诉了爸爸妈妈，听了他们的话我才意识到要想让绿宝运行起来，就必须要消耗能源。而保护环境也不仅仅只是收集污染物这么简单，收集完后对相应的垃圾进行整理回收才是最重要的...";
        }
        else if (nextLevel == whereLevel.LevelTwo)
        {
            instructionsText.text = "6月16日  星期日 天气：晴    \n在查阅了许多资料后，我发现根本就没有什么垃圾，它们只是被放错了地方的资源。\n像厨余垃圾通过生物处理，可转化为化肥，旧报纸回收后可制成环保铅笔......这就给它搭载上回收装置。下一站，就往污染源——工业区进发！";
        }
        else
        {
            instructionsText.text = "6月16日  星期日 天气：晴    \n忙碌了三天，终于将环保机器人的设计图弄出来了，它的外观像一条龙，可以收集垃圾。我把它取名为“绿宝”，希望它能为我们的家园带来一片绿色，保护我们的家园！";
        }
       

    }

    public void ButClik() {
        Curtain_Ani.SetActive(true);
        Curtain_Ani.GetComponent<Animator>().SetTrigger("AppearClose");
        AudioManager.Instance.Fade(false, false);

        Invoke("LoadToGame", 3F);
    }
    public void LoadToGame()
    {
        GrobalClass.LoadToScene(2);
    }
  
}
