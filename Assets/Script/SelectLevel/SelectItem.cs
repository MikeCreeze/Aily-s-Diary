using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectItem : MonoBehaviour
{


    public static SelectItem Instance;


    public GameObject CurtainControl;       //幕布动画控制

    public string SongName;
    public GameObject SelectUI; //已选中歌曲后展开的画布
    public GameObject MainUI;   //菜单UI，包含选歌左右钮等
    public bool CheckIfQuit = false;

  
    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Item_BeSelect()
    {
        if(gameObject.name==GrobalClass.FirstItem.name&& RollingUI.Instance.SelectLock == false&&RollingUI.Instance.CanSelect)
        {
           MainUI.SetActive(false);
            SelectUI.SetActive(true);
            RollingUI.Instance.SelectLock = true;
            //transform.localScale = new Vector3(3F, 3F, 1F);
           gameObject.GetComponent<Animator>().SetTrigger("BeSelect");

        }
      
    }

   
    public void CheckToStart()
    {
        CurtainControl.GetComponent<Animator>().SetTrigger("CurtainQuickClose");

        GrobalClass.SongName = GrobalClass.FirstItem.name;
        Debug.Log(GrobalClass.FirstItem.name);

        AudioManager.Instance.Fade(false, false);
        Invoke("RealStartLoad", 1.0f);

    }
    
    private void RealStartLoad()
    {
     
        GrobalClass.LoadToScene(2);
    }


    public void Item_QuitSelect()
    {
      
        if (RollingUI.Instance.SelectLock == true)
        {
            GrobalClass.FirstItem.GetComponent<Animator>().SetTrigger("BeQuit");
            RollingUI.Instance.SelectLock = false;
     
            SelectUI.SetActive(false);
            MainUI.SetActive(true);
          
        }
    }

    public void CheckQuit()
    {
        if(GrobalClass.FirstItem.name!=gameObject.name)
        {
            GrobalClass.FirstItem.GetComponent<Animator>().SetTrigger("BeQuit");
          
        }
    }
}
