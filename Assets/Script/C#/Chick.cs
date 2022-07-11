using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : MonoBehaviour
{
    // Start is called before the first frame update
    int yeshu=0;//页数
    public int max_yeshu=8;

    public static Chick INSTANCE;
    void Start()
    {
        INSTANCE = this;
    }
    // Update is called once per frame
    void Update()
    {
      
    }
    public void Animi_Start()
    {
        if (yeshu == 0)//封面时修正
        {
            yeshu += 1;
            GameObject.Find("ALL01").GetComponent<Animator>().SetTrigger("Click");
            GameObject.Find("ALL01").GetComponent<Animator>().SetInteger("123int", yeshu);
            GameObject.Find("ALL01").GetComponent<Animator>().SetBool("open", true);
        
            if (yeshu > max_yeshu)
            { yeshu = max_yeshu; }
        }
        //非封面页
        else
        {
            yeshu += 1;
            GameObject.Find("ALL01").GetComponent<Animator>().SetTrigger("Click");
            GameObject.Find("ALL01").GetComponent<Animator>().SetInteger("123int", yeshu);
            GameObject.Find("ALL01").GetComponent<Animator>().SetBool("open", false);
  

            if (yeshu > max_yeshu)
            { yeshu = max_yeshu; }
        }

    }
    public void Animi_End()
    {
        if (yeshu == max_yeshu)//封面时修正
        {
            yeshu += 1;
            GameObject.Find("ALL01").GetComponent<Animator>().SetTrigger("Click");
            GameObject.Find("ALL01").GetComponent<Animator>().SetInteger("123int", yeshu);
            GameObject.Find("ALL01").GetComponent<Animator>().SetBool("open", true);
        
            if (yeshu < 0)
            { yeshu = 0; }
        }
        else
        {
            yeshu -= 1;
            GameObject.Find("ALL01").GetComponent<Animator>().SetTrigger("Click");
            GameObject.Find("ALL01").GetComponent<Animator>().SetInteger("123int", yeshu);
            GameObject.Find("ALL01").GetComponent<Animator>().SetBool("open", false);
      
            if (yeshu < 0)
            { yeshu = 0; }
        }
    }
    public void Animi_Out()
    {
        GameObject.Find("ALL01").GetComponent<Animator>().SetBool("open", true);
    }
}
