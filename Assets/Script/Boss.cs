using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private bool Boss_HasCame=false;      //如果已经进入则不再进入
    private Animator Boss_Ani;
    public static Boss Instance;
    private int Missile_Damage = 30;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Boss_Ani = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Alive()
    {
        if(!Boss_HasCame)
        {
            Boss_Ani.SetBool("Alive", true);
            Boss_HasCame = true;
        }
       
    }
    public void Win()
    {
        if (Boss_HasCame)
        {
            Boss_HasCame = false;
            Boss_Ani.SetTrigger("Win");
            Boss_Ani.SetBool("Alive", false);
        }
    }
    public void SetPlayerHitted()           //在win动画中调用
    {
        Player.Instance.SetBeAttacked();
        GrobalClass.Power -= 40;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile" && Boss_HasCame == true)
        {
            
            if(GrobalClass.BossHealth-Missile_Damage<=0)
            {
                Boss_Ani.SetBool("Alive", false);
                Boss_HasCame = false;
            }
            else
            {
                Boss_Ani.SetTrigger("BeAttack");
          

            }
            GrobalClass.BossHealth -= 10;
            collision.gameObject.transform.Find("Particle").GetComponent<ParticleSystem>().Play();
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        }
    }
  
}
