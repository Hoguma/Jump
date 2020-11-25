using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    private Camera cam;

    private bool isMove = true;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!GameManager.instance.IsGameRunning())
        {
            cam.orthographicSize = 2.5f;
            transform.position = new Vector3(0, 1f, transform.position.z);
            isMove = true;
        }
        else
        {
            if (isMove)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(0, 4.5f, transform.position.z), 0.03f);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, 0.03f);
                if (transform.position.y >= 4.49f)
                {
                    cam.orthographicSize = 5f;
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
