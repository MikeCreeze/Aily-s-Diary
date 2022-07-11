using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffect : MonoBehaviour
{
    public GameObject mouseEffect;

    // Update is called once per frame
    void Update()
    {
   
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosOnScreen = Input.mousePosition;
            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);

            GameObject child = Instantiate(mouseEffect,default);
            child.transform.localPosition = new Vector2( mousePosInWorld.x,mousePosInWorld.y);
            child.GetComponent<ParticleSystem>().Play();
        }

       
    }

}
