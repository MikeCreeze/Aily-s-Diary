using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public GameObject Curtain_Transform;
    public static Camera_Control Instance;
    // Start is called before the first frame update
    void Start()
    {

        Instance = this;
       
    }
    public void Coming()
    {
        if (GrobalClass.FirstComing == true)
        {
            gameObject.GetComponent<Animator>().SetTrigger("FirstComing");
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
