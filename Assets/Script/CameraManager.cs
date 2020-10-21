using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private bool isMove = true;

    private void Start()
    {
    }

    private void Update()
    {
        if (GameManager.instance.panel())
        {
            transform.position = new Vector3(0, 0, transform.position.z);
            isMove = true;
        }
        else
        {
            if (isMove)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(0, 4.5f, transform.position.z), 0.02f);
                if (transform.position.y >= 4.4f)
                {
                    transform.position = new Vector3(0, 4.5f, transform.position.z);
                    isMove = false; 
                }
            }
            
            if (transform.position.y < target.transform.position.y)
            {
                transform.position = new Vector3(0, target.transform.position.y, transform.position.z);
            }
        }
    }
}
