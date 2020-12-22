using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceBackground : MonoBehaviour
{
    public GameObject Up;
    public GameObject CameraDown;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Up.transform.position.y <= CameraDown.transform.position.y)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + (9990f*2), transform.position.z);
        }
    }
}
