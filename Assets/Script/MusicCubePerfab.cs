using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCubePerfab : MonoBehaviour
{


    public GameObject ParentObject;     //最外层对象(自移动音乐快-1)
    public bool Cube_Place;  //区分上下
    public string LJ_Type = "空";
    private bool BeJudge = false;
    private bool Enter_Is_Perfect = false;
    private bool Enter_Is_Normal = false;
    private bool Enter_Is_Bad = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (LJ_Type)
        {
            case "可回收":

            case "厨余":

            case "有害":
                ParentObject.transform.Translate(-Time.deltaTime * GrobalClass.CubeSpeed[0], 0, 0);
                break;
            case "陷阱":

            case "奖励":
                ParentObject.transform.Translate(-Time.deltaTime * GrobalClass.CubeSpeed[1], 0, 0);
                break;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (LJ_Type != "陷阱" && LJ_Type != "奖励")
        {
            if (collision.gameObject.tag == "Judge_Perfect" && BeJudge == false)
            {

                Enter_Is_Perfect = true;
                BeJudge = true;
                CheckLevel();

            }
            else if (collision.gameObject.tag == "Judge_Normal" && BeJudge == false)
            {

                Enter_Is_Normal = true;
                BeJudge = true;
                CheckLevel();

            }
            else if (collision.gameObject.tag == "Judge_Bad" && BeJudge == false)
            {
                Enter_Is_Bad = true;
                BeJudge = true;
                CheckLevel();
            }
        }
        else
        {
            if (LJ_Type == "陷阱")
            {
                if (collision.gameObject.tag == "Judge_Bad" && BeJudge == false)
                {
                    Player.Instance.ResetUpHit();
                    Destroy(gameObject.GetComponent<CircleCollider2D>());
                    Invoke("BeDestroy", 5f);
                }
            }
            else if (LJ_Type == "奖励")
            {
                if (collision.gameObject.tag == "Judge_Bad" && BeJudge == false)
                {
                    Player.Instance.ResetUpHit();
                    GrobalClass.Power += 10;
                    if (Cube_Place == true)                    //判断上下粒子播放
                    {
                        GameStart.Instance.ParticlePlay = 1;
                    }
                    else
                    {
                        GameStart.Instance.ParticlePlay = 2;
                    }
                    Destroy(ParentObject);

                }
            }


        }


        if (collision.gameObject.tag == "Judge_Miss" && BeJudge == false)
        {
            if (LJ_Type != "奖励")
            {


                GrobalClass.Combo = 0;
                switch (LJ_Type)
                {
                    case "可回收":
                        GrobalClass.Power -= 1;

                        //Debug.Log("受击:" + LJ_Type);

                        break;
                    case "厨余":
                        GrobalClass.Power -= 1;
                        //Debug.Log("受击:" + LJ_Type);
                        break;
                    case "有害":
                        GrobalClass.Power -= 4;
                        // Debug.Log("受击:" + LJ_Type);
                        break;
                    case "陷阱":
                        GrobalClass.Power -= 10;
                        //Debug.Log("受击:" + LJ_Type);
                        break;

                }
                Player.Instance.SetBeAttacked();
                Player.Instance.ResetUpHit();
            }

        }
        if (collision.gameObject.tag == "Destroy")
        { BeDestroy(); }


    }


    private void CheckLevel()
    {
        Destroy(gameObject.GetComponent<CircleCollider2D>());

        if (Enter_Is_Perfect == true)
        {
            Player.Instance.ResetUpHit();

            ParentObject.transform.Find("棍子").gameObject.GetComponent<Animator>().SetTrigger("BeKick");

            Debug.Log("哇，金色传说！");//打击准度最高


            ScoreCheck(0);

            return;
        }
        else if (Enter_Is_Normal == true)
        {

            Player.Instance.ResetUpHit();

            ParentObject.transform.Find("棍子").gameObject.GetComponent<Animator>().SetTrigger("BeKick");


            Debug.Log("哇，金色普通...");


            ScoreCheck(1);

            return;
        }
        else if (Enter_Is_Bad == true)
        {

            Player.Instance.ResetUpHit();

            ParentObject.transform.Find("棍子").gameObject.GetComponent<Animator>().SetTrigger("BeKick");

            Debug.Log("草，金色垃圾= =");

            ScoreCheck(2);

            return;
        }
    }

    public void ScoreCheck(int i)  //分数&连击计算============
    {
        if (LJ_Type == "可回收")
        {
            GrobalClass.KHS++;
            GrobalClass.Power += 2;
        }
        if (LJ_Type == "厨余")
        {
            GrobalClass.CY++;
        }
        if (LJ_Type == "有害")
        {
            GrobalClass.YH++;
        }

        switch (i)
        {
            case 0:
                GrobalClass.Iscore += 2;
                GrobalClass.Combo++;
                break;
            case 1:
                GrobalClass.Iscore += 1;
                GrobalClass.Combo++;
                break;
            case 2:
                GrobalClass.Combo++;
                break;
        }
        DestroyPrepare();

    }
    public void DestroyPrepare()    //销毁操作
    {

        if (Cube_Place == true)                    //判断上下粒子播放
        {
            GameStart.Instance.ParticlePlay = 1;
        }
        else
        {
            GameStart.Instance.ParticlePlay = 2;
        }

        Invoke("BeDestroy", 0.4f);
    }

    public void BeDestroy()
    {
        if (LJ_Type != "陷阱")
        {
            GrobalClass.AllRob++;

        }

        Destroy(ParentObject);


    }
}
