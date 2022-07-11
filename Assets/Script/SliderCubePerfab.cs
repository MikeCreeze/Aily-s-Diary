using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderCubePerfab : MonoBehaviour
{
    GameObject MusicSlider;
    bool IfBeHit=false; //是否被打击过，若有则中途松开也不断combo
    bool MoveSwitch=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        MusicSlider = gameObject.transform.Find("判定条").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(MoveSwitch)
        { 

        gameObject.transform.Translate(-Time.deltaTime * GrobalClass.CubeSpeed[0], 0, 0);

          
        }
        else
        {
            if (MusicSlider.GetComponent<RectTransform>().rect.width >= 0)
            {
                MusicSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(MusicSlider.GetComponent<RectTransform>().rect.width - Time.deltaTime * GrobalClass.CubeSpeed[0]*1.5f, 3);
               
            }
            else
            {
                GrobalClass.Combo ++;
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Judge_Bad")
        {
           
            if (collision.gameObject.transform.parent.name =="Up_Judge")
            {
                Player.Instance.ResetUpHit();            //重置玩家跳跃权限
                Player.Instance.UpPoint.IfPress = true;  //长按为真
               
            }
            else if (collision.gameObject.transform.parent.name == "Down_Judge")
            {
                Player.Instance.ResetUpHit();            //重置玩家跳跃权限
                Player.Instance.DownPoint.IfPress = true;  //长按为真
            }
            IfBeHit = true;
            MoveSwitch = false;

        }
        
        if (collision.gameObject.tag == "Destroy")
        { Destroy(gameObject); }
        if (collision.gameObject.tag == "Judge_Miss" )
        {
            if(!IfBeHit)
            {
                GrobalClass.Combo = 0;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Judge_Bad")
        {
            if (collision.gameObject.transform.parent.name == "Up_Judge")
            {
                Player.Instance.UpPoint.IfKeeping = false;
                Player.Instance.UpPoint.IfPress = false;
            }
            else if (collision.gameObject.transform.parent.name == "Down_Judge")
            {
                Player.Instance.DownPoint.IfKeeping = false;
                Player.Instance.DownPoint.IfPress = false;
            }
            MoveSwitch = true;
        }
    }
    
    
}
