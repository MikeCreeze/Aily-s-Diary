using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_bigger : MonoBehaviour
{
    // Start is called before the first frame update
    public float  time = 0.1f;
    public GameObject SelectTrigger;        //选择歌曲的按钮，调用过镜头放大再启用
    public GameObject cannvasUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
   public void Big()
    {
        if (GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView > 54)
        {
            Invoke("Bigger", time);
        }
        cannvasUI.SetActive(true);
        SelectTrigger.SetActive(true);


    }
    void Bigger()
    {
        GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView -= 1;
        Big();
    }
}
