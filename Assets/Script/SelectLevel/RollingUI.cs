using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RollingUI : MonoBehaviour
{



    public static RollingUI Instance;
    public bool SelectLock = false;        //如Item被点击则锁住
    public Text SongNameText;
    public bool CanSelect = false;

    public RectTransform ParentsItem;
    public RectTransform[] ItemRectTransforms;      //存放Item的RectTransform.anchorePosition3D
    public Dictionary<RectTransform, Vector3> ItemDictionary = new Dictionary<RectTransform, Vector3>();      //声明字典（键值对）


    [Header("Circle")]          //声明表头
    public Vector3 CirCenterPos = Vector3.zero;
    private float CirRadius = 800;              //圈半径

    private float OffsetValue_y = 10;          //圈偏移(越在后层越上)

    [Range(0.0f, 0.6f)]          //让以下的值在unity那边能用滑块调整范围
    public float Min_AlphaValue = 0.2f;


    private float SpeedRatio = 6;//速度的系数

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (GrobalClass.LastScene == "Game_Scene")
        {
            AudioManager.Instance.Fade(true, false);   //淡入bgm
            GrobalClass.LastScene = "Default";
        }

        Initialized_All_Items();
        Set_Item_Pos();
        ItemRectTransforms[0].transform.localPosition = new Vector3(ItemRectTransforms[0].transform.localPosition.x, 600, ItemRectTransforms[0].transform.localPosition.z);

        Left_Click();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void Initialized_All_Items()        //初始化item，获取所有item的recttransform组件
    {
        if (ParentsItem == null) { return; }
        var RectsList = ParentsItem.GetComponentsInChildren<RectTransform>();        //获取自身以及孩子组件
        ItemRectTransforms = new RectTransform[RectsList.Length - 1];

        for (int i = 0; i < ItemRectTransforms.Length; i++)
        {
            ItemRectTransforms[i] = RectsList[i + 1];                   //因为Rectslist下标为0为父组件，所以一一对应的是0+1=第1位
        }
    }

    void Set_Item_Pos()
    {
        float angle = 0;
        for (int i = 0; i < ItemRectTransforms.Length; i++)
        {
            angle = i * 360 / ItemRectTransforms.Length;        //计算当前点对应角度
            float radian = (angle / 180) * Mathf.PI;             //通过角度计算弧度
            float sin_Value = CirRadius * Mathf.Sin(radian);        //sin(0)=0,cos(0)=1
            float cos_Value = CirRadius * Mathf.Cos(radian);

            Vector3 TargetPos = CirCenterPos + new Vector3(sin_Value, 0, -cos_Value);//通过圆心点的正余弦值计算在圆上的具体位置

            if (i != 0)
            {
                if (i < ItemRectTransforms.Length / 2)
                {
                    TargetPos.y = OffsetValue_y * i;
                }
                else
                {
                    TargetPos.y = OffsetValue_y * (ItemRectTransforms.Length - i);
                }
            }
            //设置y的偏差

            ItemRectTransforms[i].anchoredPosition3D = TargetPos;//设置Item的位置

            ItemDictionary.Add(ItemRectTransforms[i], TargetPos);           //向字典添加该点位


            SetItemsSiBling();//设置Item显示优先级
            SetItemsAlphaValue(); //设置透明度

        }



    }
    //=================================================================================================
    private Coroutine currentCoroutine;
    //=================================右转部分
    public void Right_Click()
    {
        StartCoroutine(MoveRight());

    }
    IEnumerator MoveRight()
    {
        if (currentCoroutine != null)
        {
            yield return currentCoroutine;//等待该携程执行完毕才执行接下来的内容
        }
        //当上一个current携程执行完毕，则不return，执行接下来的内容
        Vector3 FirstPos = ItemDictionary[ItemRectTransforms[0]];   //词典类可以直接获取到键值对中的键
        for (int i = 0; i < ItemRectTransforms.Length; i++)
        {
            Vector3 nextPos = ItemRectTransforms[(i + 1) % (ItemRectTransforms.Length)].anchoredPosition3D;        //取余的写法能将末尾的下一位指向0号位
            if (i == ItemRectTransforms.Length - 1)     //如果i等于item组最后一位的时候
            {
                nextPos = FirstPos;
            }

            currentCoroutine = StartCoroutine(MoveToTargetPos(ItemRectTransforms[i], nextPos));    //给current赋值，开启携程(基于多线程，会单独执行)，结束current清空
        }
    }

    //=================================左转部分
    public void Left_Click()
    {
        StartCoroutine(MoveLeft());
    }
    IEnumerator MoveLeft()
    {
        if (currentCoroutine != null)
        {
            yield return currentCoroutine;//等待该携程执行完毕才执行接下来的内容
        }
        //当上一个current携程执行完毕，则不return，执行接下来的内容
        Vector3 FirstPos = ItemDictionary[ItemRectTransforms[ItemRectTransforms.Length - 1]];   //词典类可以直接获取到键值对中的键
        for (int i = ItemRectTransforms.Length - 1; i >= 0; i--)
        {
            Vector3 nextPos = ItemRectTransforms[(i + ItemRectTransforms.Length - 1) % (ItemRectTransforms.Length)].anchoredPosition3D;        //取余的写法能将末尾的下一位指向0号位(反向移动则i加上整个item组长度减1)
            if (i == 0)     //如果i等于item组最后一位的时候
            {
                nextPos = FirstPos;
            }
            currentCoroutine = StartCoroutine(MoveToTargetPos(ItemRectTransforms[i], nextPos));    //给current赋值，开启携程(基于多线程，会单独执行)，结束current清空
        }
    }
    IEnumerator MoveToTargetPos(RectTransform rectTran, Vector3 targetPos)      //移动到targetPos位置
    {

        ItemDictionary[rectTran] = targetPos;       //更新字典
        SetItemsSiBling();//更新Item显示优先级
        float speed_Value = (targetPos - rectTran.anchoredPosition3D).magnitude;     //该速度保证了每一个携程都在同一时间(帧)完成
        while (rectTran.anchoredPosition3D != targetPos)               //执行一次后若没到targetpos，则return回来，继续执行移动，直到到达了targetpos
        {
            CanSelect = false;
            rectTran.anchoredPosition3D = Vector3.MoveTowards(rectTran.anchoredPosition3D, targetPos, speed_Value * SpeedRatio * Time.deltaTime);
            yield return null; //中断指令，让函数等待（写null为这一帧不执行该迭代函数）
        }
        //============================移动完成后执行以下代码

        SetItemsAlphaValue(); //更新透明度

        //Debug.Log("到达目标点");
        CanSelect = true;

    }
    //----------------------------------------------------------------------------------------------------
    void SetItemsSiBling()       //排序优先级
    {

        Dictionary<RectTransform, int> orderDic = new Dictionary<RectTransform, int>();
        for (int i = 0; i < ItemDictionary.Count; i++)
        {
            float max_Value = float.MinValue;
            RectTransform ThisRect = new RectTransform();

            foreach (var dic in ItemDictionary)
            {
                if (!orderDic.ContainsKey(dic.Key))
                {

                    if (dic.Value.z > max_Value)
                    {
                        max_Value = dic.Value.z;
                        ThisRect = dic.Key;         //遍历出Z值最大的rectTransform
                        GrobalClass.FirstItem = dic.Key;            //更新获取最前面的Item
                    }
                }
            }

            orderDic.Add(ThisRect, i);
        }
        foreach (var order in orderDic)
        {
            order.Key.SetSiblingIndex(order.Value);
        }
        SongNameText.text = GrobalClass.FirstItem.name;
    }

    void SetItemsAlphaValue()   //透明度优先级
    {
        float Start_Value = CirCenterPos.z - CirRadius;     //起始位置
        foreach (var dic in ItemDictionary)
        {
            float aplhaValue = 1 - Mathf.Abs(dic.Value.z - Start_Value) / (2 * CirRadius) * (1f - Min_AlphaValue);
            var rawImage = dic.Key.GetComponent<RawImage>();
            if (rawImage)
            {
                rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, aplhaValue);
            }
        }
    }



    IEnumerator ChangeingScale(RectTransform rectTran, Vector3 targetScale)
    {
        while (rectTran.transform.localScale != targetScale)               //执行一次后若没到targetpos，则return回来，继续执行移动，直到到达了targetpos
        {
            rectTran.transform.localScale = new Vector3(rectTran.transform.localScale.x + 0.1f * Time.deltaTime, rectTran.transform.localScale.y + 0.1f * Time.deltaTime, 1);
            yield return null; //中断指令，让函数等待（写null为这一帧不执行该迭代函数）
        }
    }
    public void BackToMain()
    {
        GrobalClass.LoadToScene(0);

    }
}

