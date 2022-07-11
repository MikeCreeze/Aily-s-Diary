using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButShow
{
    ButClose = 0,
    ShowLeftBut = 1,
    ShowRightBut = 2
}

public class FirstComing : MonoBehaviour
{
    public ButShow butShow = ButShow.ButClose;
    public GameObject GuideText;
    public Animator UIAni;
    private bool DoOneLeftBut = true;
    private bool DoOneRightBut = true;


    // Start is called before the first frame update
    void Start()
    {
        if (GrobalClass.FirstComing == true)
        {
            butShow = ButShow.ShowLeftBut;
            Invoke("TextShow",9);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    void TextShow()
    {
        if (butShow == ButShow.ButClose)
        {
            GuideText.GetComponent<Text>().text = "";
            UIAni.SetTrigger("ShowExit");
            DoOneLeftBut = false;
            DoOneRightBut = false;
        }
        else if (butShow == ButShow.ShowLeftBut)
        {
            GuideText.GetComponent<Text>().text = "-点击屏幕右侧- -跳跃收集上半的垃圾- ";
            UIAni.SetTrigger("ShowL");
        }
        else if (butShow == ButShow.ShowRightBut)
        {
            GuideText.GetComponent<Text>().text = "-很好!点击屏幕左侧- -收集下半部分- ";
            UIAni.SetTrigger("ShowR");
        }
       
    }

    public void LeftButClik() {
       
        if (GrobalClass.FirstComing == true&&DoOneLeftBut == true)
        {
            butShow = ButShow.ShowRightBut;
            TextShow();
            
        }

    }
    public void RightButClik()
    {
        if (GrobalClass.FirstComing == true && DoOneRightBut == true)
        {
            butShow = ButShow.ButClose;
            TextShow();
        }
    }
    

}
