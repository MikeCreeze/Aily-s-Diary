using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_Atm_Speed : MonoBehaviour
{
    // Start is called before the first frame update

    private bool SetLock = false;

    public GameObject Obj_BackGround;

    public GameObject Obj_Sun;
    public GameObject Obj_Star;
    public GameObject Obj_Moon;
    public GameObject Obj_Plane;

    private Animator Ani_Ground;
    private Animator Ani_Sun;
    private Animator Ani_Star;
    private Animator Ani_Moon;
    private Animator Ani_Plane;

    [Header("播放速度")]
    public float Ground_Speed = 0;  //背景星星等闪烁速度
    public float Sun_Speed=0;
    public float Star_Speed = 0;
    public float Moon_Speed = 0;
    public float Plane_Speed = 0;
    void Start()
    {
        Ani_Ground = Obj_BackGround.GetComponent<Animator>();
        Ani_Sun = Obj_Sun.GetComponent<Animator>();
        Ani_Star =Obj_Star.GetComponent<Animator>();
        Ani_Moon=Obj_Moon.GetComponent<Animator>();
        Ani_Plane=Obj_Plane.GetComponent<Animator>();

        Ani_Ground.speed = 0;
        Ani_Sun.speed = 0;
        Ani_Star.speed = 0;
        Ani_Moon.speed = 0;
        Ani_Plane.speed = 0;
    }
    private void Update()
    {
        if(SetLock==false&&GrobalClass.RealGameStart==true)
        {
            SetLock = true;
            Set_Ani_Speed();
        }

        
    }


    private void Set_Ani_Speed()
    {
        switch (GrobalClass.SongList)
        {
            case 0:                 //萨哈星球
                Ground_Speed = 2;
                Sun_Speed = 0.49f;
                Star_Speed = 0.94f;
                Moon_Speed = 1.94f;
                Plane_Speed = 1;
                break;
            case 1:                 //垃圾分类要记
                Sun_Speed = 0.6f;
                Star_Speed = 1.176f;
                Moon_Speed = 2.34f;
                Plane_Speed = 1f;
                break;
            case 2:
                Ani_Sun.speed = Sun_Speed;
                Ani_Star.speed = Star_Speed;
                Ani_Moon.speed = Moon_Speed;
                Ani_Plane.speed = Plane_Speed;
                break;
            case 3:
                Ani_Sun.speed = Sun_Speed;
                Ani_Star.speed = Star_Speed;
                Ani_Moon.speed = Moon_Speed;
                Ani_Plane.speed = Plane_Speed;
                break;



        }
        Ani_Ground.speed = Ground_Speed;
        Ani_Sun.speed = Sun_Speed;
        Ani_Star.speed = Star_Speed;
        Ani_Moon.speed = Moon_Speed;
        Ani_Plane.speed = Plane_Speed;
        Debug.Log("气氛组速度赋值完毕"+Ground_Speed);
    }
}
