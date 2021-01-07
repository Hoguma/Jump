using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    private Camera cam;

    private bool isMove = true;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        Rect rect = cam.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scaleWidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }
        cam.rect = rect;
    }

    private void Update()
    {
        if (!GameManager.instance.IsGameRunning())
        {
            cam.orthographicSize = 250;
            transform.position = new Vector3(0, 38f, transform.position.z);
            isMove = true;
        }
        else
        {
            if (isMove)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(0, 815f, transform.position.z), 0.03f);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 960f, 0.03f);
                if (transform.position.y >= 814.9f)
                {
                    cam.orthographicSize = 960f;
                    transform.position = new Vector3(0, 815f, transform.position.z);
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
