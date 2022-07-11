using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*====场景刷新管理====
* 用于:
*1.背景随机建筑刷新
*2.气氛道具出入场
=====================*/
public class GameAreaEdit : MonoBehaviour
{
    public static GameAreaEdit Instance;

    public GameObject PerfabParents_ST;
    public GameObject PerfabParents_ND;
    [Header("预制体")]
    public GameObject[] Element_Perfab;
    private int RandomKey;

    GameObject Perfab_Temp;

    [Header("马路图")]
    [SerializeField]        //强制序列化私有域，在列表中显示成员
    private Sprite[] IGround;
    public GameObject[] Ground;        //马路组件本身，有两个(前后)

    [Header("背景城市图")]
    [SerializeField]
    private Sprite[] ICity;
    public GameObject[] City;        //马路组件本身，有两个(前后)

    //特殊元素生成限制
    private bool Lock_BusStop = false;
    private bool Lock_Metro = false;


    public GameObject AtmosphereObject;
    private Animator Atm_Control;
    public GameObject Plane;        //飞机组件暂时为单独调用

    public GameObject SkyBoxObject;
    private Animator SB_Control;
    private bool SkyState = true;      //天空状态，真为白天

    // Start is called before the first frame update
    void Awake()
    {
        Atm_Control = AtmosphereObject.GetComponent<Animator>();
        SB_Control = SkyBoxObject.GetComponent<Animator>();


        Ground[0].GetComponent<SpriteRenderer>().sprite = IGround[GrobalClass.AreaSkin];
        Ground[1].GetComponent<SpriteRenderer>().sprite = IGround[GrobalClass.AreaSkin];

        City[0].GetComponent<SpriteRenderer>().sprite = ICity[GrobalClass.AreaSkin*2];
        City[1].GetComponent<SpriteRenderer>().sprite = ICity[GrobalClass.AreaSkin*2];
        
        City[0].transform.Find("背景-夜1").GetComponent<SpriteRenderer>().sprite = ICity[GrobalClass.AreaSkin * 2 + 1];
        City[1].transform.Find("背景-夜2").GetComponent<SpriteRenderer>().sprite = ICity[GrobalClass.AreaSkin * 2 + 1];
        Instance = this;
    }
 
           // Update is called once per frame
        void Update()
    {

    }
    public void SummonRandomPerfab_ST()
    {
        Lock_BusStop = false;
        Lock_Metro = false;
        foreach (Transform child in PerfabParents_ST.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
        
        RandomKey = Random.Range(0, 7);
            //Debug.Log(RandomKey);
            switch (RandomKey)
            {
                case 0:
                    if (Lock_Metro == false)
                    {
                        Lock_Metro = true;
                        Perfab_Temp = Instantiate(Element_Perfab[GrobalClass.AreaSkin * 5 + 0], PerfabParents_ST.transform, false);
                        Perfab_Temp.transform.Translate(Random.Range(0 + i * 10, 5 + i * 10), 0, 0);
                    }
                    break;
                case 1:
                case 2:
                case 3:
                    //路灯：刷新必间距相等

                    Perfab_Temp = Instantiate(Element_Perfab[GrobalClass.AreaSkin * 5 + 2], PerfabParents_ST.transform, false);
                    Perfab_Temp.transform.Translate(i * 10, 0, 0);
                    break;
                case 4:
                case 5:
                    //带草路灯：刷新必间距相等，草丛位置偏移翻转随机
                    Perfab_Temp = Instantiate(Element_Perfab[GrobalClass.AreaSkin * 5 + 3], PerfabParents_ST.transform, false);
                    Perfab_Temp.transform.Translate(i * 10, 0, 0);
                    switch (GrobalClass.AreaSkin)
                    {
                        case 0:
                         
                            Perfab_Temp.transform.Find("Grass").gameObject.transform.Translate(Random.Range(-1.5f, 1.6f), 0, 0);
                            if (Random.Range(0, 3) == 0)
                            {
                                Perfab_Temp.transform.Find("Grass").gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            }
                            break;

                    }
                    break;

              
                case 6: //杂草：刷新随机性更强
                    Perfab_Temp = Instantiate(Element_Perfab[GrobalClass.AreaSkin * 5 + 4], PerfabParents_ST.transform, false);
                    Perfab_Temp.transform.Translate(Random.Range(0 + i * 10, 3 + i * 15), 0, 0);
                    switch (GrobalClass.AreaSkin)
                    {
                        case 0:
                           
                            Perfab_Temp.transform.Find("Grass-1").gameObject.transform.Translate(Random.Range(-1.5f, 1.6f), 0, 0);
                            break;

                    }
                    break;
                    

              

            }
        }



    }
    public void SummonRandomPerfab_ND()
    {
        Lock_BusStop = false;
        Lock_Metro = false;
        foreach (Transform child in PerfabParents_ND.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            RandomKey = Random.Range(0, 7);
            //Debug.Log(RandomKey);
            switch (RandomKey)
            {
                case 1:
                    //车站：一仓位仅能刷新一次，减少重复
                    if (Lock_BusStop == false)
                    {
                        Lock_BusStop = true;
                        Perfab_Temp = Instantiate(Element_Perfab[GrobalClass.AreaSkin * 5 + 1], PerfabParents_ND.transform, false);
                        Perfab_Temp.transform.Translate(Random.Range(0 + i * 10, 5 + i * 10), 0, 0);
                    }
                    break;
                case 0: 
                case 2:
                case 3:
                    //路灯：刷新必间距相等
                    Perfab_Temp = Instantiate(Element_Perfab[GrobalClass.AreaSkin * 5 + 2], PerfabParents_ND.transform, false);
                    Perfab_Temp.transform.Translate(i * 10, 0, 0);
                    break;
                   
                case 4:
                case 5:
                    //带草路灯：刷新必间距相等，草丛位置偏移翻转随机
                    Perfab_Temp = Instantiate(Element_Perfab[GrobalClass.AreaSkin * 5 + 3], PerfabParents_ND.transform, false);
                    Perfab_Temp.transform.Translate(i * 10, 0, 0);
                    switch (GrobalClass.AreaSkin)
                    {
                        case 0:
                           
                            Perfab_Temp.transform.Find("Grass").gameObject.transform.Translate(Random.Range(-1.5f, 1.6f), 0, 0);
                            if (Random.Range(0, 3) == 0)
                            {
                                Perfab_Temp.transform.Find("Grass").gameObject.GetComponent<SpriteRenderer>().flipX = true;
                            }
                            break;

                    }
                  
                    break;
               
                case 6: //杂草：刷新随机性更强
                    Perfab_Temp = Instantiate(Element_Perfab[GrobalClass.AreaSkin * 5 + 4], PerfabParents_ND.transform, false);
                    Perfab_Temp.transform.Translate(Random.Range(0 + i * 10, 3 + i * 15), 0, 0);
                    switch (GrobalClass.AreaSkin)
                    {
                        case 0:
                          
                            Perfab_Temp.transform.Find("Grass-1").gameObject.transform.Translate(Random.Range(-1.5f, 1.6f), 0, 0);
                            break;

                    }
                   
                    break;



            }
        }
    }

    public void ChangeTime()
    {
        if(SkyState)
        {
            SkyState = false;
            SB_Control.SetTrigger("Night");
        }
        else
        {
            SkyState = true;
            SB_Control.SetTrigger("Day");
        }
       
    }


    public void Sun_In()
    {
        Atm_Control.SetTrigger("Sun_In");
    }

    public void Moon_In()
    {
        Atm_Control.SetTrigger("Moon_In");
    }
    public void Moon_Star_In()
    {
        Atm_Control.SetTrigger("Moon_Star_In");
    }

    public void Plane_In()
    {
        Plane.GetComponent<Animator>().SetTrigger("F01");
    }

    public void Clear_All()
    {
        Atm_Control.SetTrigger("Clear");
    }
}


