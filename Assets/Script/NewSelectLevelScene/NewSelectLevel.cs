using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewSelectLevel : MonoBehaviour
{

    [Header("Circle")]
    public Transform ParentsItem;
    public Transform[] ItemTransforms;      //存放Item的Transform.anchorePosition3D
    public Dictionary<Transform, Vector3> ItemDictionary = new Dictionary<Transform, Vector3>();      //声明字典（键值对）

    public GameObject UI_Canvas;
    public int FirstItem=0;
    private int MaxIndex = 5;          //关卡数
    public bool CanTurn=true;      //可否翻页

    public GameObject Btn_OldSelect;
    public GameObject LevelText;
    // Start is called before the first frame update
    void Start()
    {
        if (GrobalClass.LastScene == "Game_Scene")
        {
            AudioManager.Instance.Fade(true,false);   //淡入bgm
            GrobalClass.LastScene = "Default";
        }
       if(GrobalClass.If_Game_End)
        {
            GrobalClass.UnLockLevel = 2;
            Btn_OldSelect.SetActive(true);
        }
      
        Initialized_All_Items();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Initialized_All_Items()        //初始化item，获取所有item的rtransform组件
    {
        int list_Temp = 0;
        if (ParentsItem == null) { return; }
        var RectsList = ParentsItem.GetComponentsInChildren<Transform>();        //获取自身以及孩子组件
        
        ItemTransforms = new Transform[MaxIndex];

        for (int i = 0; i < RectsList.Length-1; i++)
        {

            if (RectsList[i + 1].name!="正面"&& RectsList[i + 1].name != "反面")
            {
                ItemTransforms[list_Temp] = RectsList[i + 1];                   //因为Rectslist下标为0为父组件，所以一一对应的是0+1=第1位
                //Debug.Log(ItemTransforms[list_Temp].name);
                list_Temp++;
            }

        }
        
        FirstItem = 1;
       
    }

    public void Turn_Left()
    {
        if(CanTurn)
        {
            if (FirstItem < MaxIndex-3+GrobalClass.UnLockLevel)
            {
                Debug.Log(GrobalClass.UnLockLevel);
                CanTurn = false;
                Chick.INSTANCE.Animi_Start();
                Debug.Log(ItemTransforms.Length);
                StartCoroutine(RotateToLeft(ItemTransforms[FirstItem], FirstItem));
                FirstItem += 1;
                Invoke("ResetTurn", 1.8F);
                if(FirstItem-1!=3)
                {
                    LevelText.GetComponent<Text>().text = "第" + (FirstItem - 1).ToString() + "关";
                }
                else
                {
                    LevelText.GetComponent<Text>().text = "最终关";
                }
               
              
            }
        }
     
    }

  
    public void Turn_Right()
    {
        if (CanTurn)
        {
           
            if (FirstItem > 2 && FirstItem <= MaxIndex)
            {
                CanTurn = false;
                Chick.INSTANCE.Animi_End();
                StartCoroutine(RotateToRight(ItemTransforms[FirstItem - 1], FirstItem));
                FirstItem -= 1;
                Invoke("ResetTurn", 1.8F);
                if (FirstItem - 1 != 3)
                {
                    LevelText.GetComponent<Text>().text = "第" + (FirstItem - 1).ToString() + "关";
                }
                else
                {
                    LevelText.GetComponent<Text>().text = "最终关";
                }

            }
        }
    }
    private void ResetTurn()
    {
        CanTurn = true;
    }
    IEnumerator RotateToRight(Transform Tran, int TranIndex)
    {
        bool ChangeSort = false;
        while (Tran.localEulerAngles.y > 60)
        {
            Tran.Rotate(new Vector3(0, -5f, 0), Space.Self);
            if (Tran.localEulerAngles.y <= 90 && ChangeSort == false)
            {

                     ChangeSort = true;
              
                    Debug.Log(TranIndex+"单数");
                    Tran.Find("正面").GetComponent<SpriteRenderer>().sortingOrder =50- TranIndex+1 ;
                    Tran.Find("反面").GetComponent<SpriteRenderer>().sortingOrder = 50-TranIndex  ;


            }
            yield return null;
        }
        while (Tran.localEulerAngles.y > 5)
        {

            Tran.Rotate(new Vector3(0, -3f, 0), Space.Self);
            yield return null;
        }
        while (Tran.transform.eulerAngles.y >1)        
        {
           
                Tran.Rotate(new Vector3(0, -0.1F, 0), Space.Self);
                Debug.Log(Tran.name + "打打");
                Debug.Log(Tran.localEulerAngles.y);
                yield return null;
           
        }
  
    

    }
    IEnumerator RotateToLeft(Transform Tran, int TranIndex)
    {
        bool ChangeSort=false;
        while (Tran.localEulerAngles.y < 120) 
        {
            Tran.Rotate(new Vector3(0, 5f, 0), Space.Self);
            if (Tran.localEulerAngles.y >= 90&&ChangeSort==false) 
            {
              
                ChangeSort = true;

                Tran.Find("正面").GetComponent<SpriteRenderer>().sortingOrder = TranIndex-1;
                Tran.Find("反面").GetComponent<SpriteRenderer>().sortingOrder = TranIndex;



            }
            yield return null;
        }
        while (Tran.localEulerAngles.y < 175)              
        {
          
            Tran.Rotate(new Vector3(0, 3f, 0), Space.Self);
            yield return null;
        }
        while (Tran.localEulerAngles.y < 180)               //执行一次后若没到targetpos，则return回来，继续执行移动，直到到达了targetpos
        {
          
            Tran.Rotate(new Vector3(0, 0.1F, 0), Space.Self);
              Debug.Log(Tran.localEulerAngles.y);
            yield return null;
            
        }
      
    }

    public void CheckSelect()
    {
    
        UI_Canvas.GetComponent<Animator>().SetTrigger("BeSelect");
        Debug.Log(FirstItem-1);
        switch(FirstItem-1)
        {
            case 1:
                GrobalClass.nextNum = 0;
                GrobalClass.SongName = "要牢记";
                GrobalClass.AreaSkin = 0;
                Invoke("LoadToTextCG", 4f);
                break;
            case 2:
                GrobalClass.nextNum = 1;
                GrobalClass.SongName = "环保歌";
                GrobalClass.AreaSkin = 1;
                Invoke("LoadToTextCG", 4f);
                break;
            case 3:
                GrobalClass.nextNum = 2;
                GrobalClass.SongName = "骄傲的少年";
                GrobalClass.AreaSkin = 2;
                Invoke("LoadToTextCG", 4f);
                break;

        }
       
    }
    public void LoadToTextCG()
    {
        GrobalClass.LoadToScene(6);
    }
    public void LoadToGame()
    {
        GrobalClass.LoadToScene(2);
    }
    public void BackToMain()
    {
        GrobalClass.LoadToScene(0);
    }
    public void LoadToSongList()
    {
        GrobalClass.LoadToScene(1);
    }
}
