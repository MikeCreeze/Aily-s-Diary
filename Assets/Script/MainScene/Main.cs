using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Main : MonoBehaviour
{
    Vector3 BackGroundPosition;
    Vector3 CloseGroundPosition;
    Vector3 FogGroundPosition;

    public Sprite[] FarImage;
    public GameObject FarGround;
    
    private Animator Ani_Ctrl;
    private bool Enter_Lock=false;
    void Start()
    {
        if(GrobalClass.LastScene!="CG_End")
        {
            VariableSave.Instance.LoadGame();
        }
     
        if (GrobalClass.If_Game_End)         //大结局更换远景
        {
            FarGround.GetComponent<SpriteRenderer>().sprite = FarImage[1];
            transform.Find("中景-烟").gameObject.SetActive(false);
            transform.Find("大标题").gameObject.SetActive(true);
        }
        else
        {
            FarGround.GetComponent<SpriteRenderer>().sprite = FarImage[0];
            transform.Find("中景-烟").gameObject.SetActive(true);
            transform.Find("大标题").gameObject.SetActive(false); 

        }
        
        Ani_Ctrl = gameObject.GetComponent<Animator>();
        BackGroundPosition = transform.Find("远景").Find("本体").transform.position;
        CloseGroundPosition = transform.Find("近景").Find("本体").transform.position;
        FogGroundPosition = transform.Find("中景-烟").Find("本体").transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!Enter_Lock)
        {
            transform.Find("远景").Find("本体").transform.position = BackGroundPosition + Input.mousePosition * 0.00005f;
            transform.Find("近景").Find("本体").transform.position = CloseGroundPosition + Input.mousePosition * 0.0001f;
            transform.Find("中景-烟").Find("本体").transform.position = FogGroundPosition + Input.mousePosition * 0.00005f;
        }
         
    }
   
    public void StartGame()
    {
        VariableSave.Instance.LoadGame();
       
  
        if (!Enter_Lock)
        {
            transform.Find("远景").Find("本体").transform.position = BackGroundPosition;
              transform.Find("近景").Find("本体").transform.position=CloseGroundPosition;
            transform.Find("中景-烟").Find("本体").transform.position = FogGroundPosition;
            Enter_Lock = true;
            if (GrobalClass.FirstComing)
            {
                AudioManager.Instance.Fade(false, false);
            }
            Ani_Ctrl.SetTrigger("Start");
           if(GrobalClass.If_Game_End)
            {
                transform.Find("大标题").gameObject.GetComponent<Animator>().SetTrigger("Leave");
            }
         
        }
        

    }

    public void Load()
    {
      
        if (GrobalClass.FirstComing)
        {
            GrobalClass.LoadToScene(3);
        }
        else
        {
            GrobalClass.LoadToScene(5);         //改为立体书
        }
      
    }
    public void ReSet()
    {

      
        GrobalClass.FirstComing = true;
        GrobalClass.If_Game_End = false;
        VariableSave.Instance.Start_SaveGame();
       


    }
}
