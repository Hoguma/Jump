using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public GameObject CameraPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = CameraPos.transform.position;
        if (GameManager.instance.FScore1 >= 0 && GameManager.instance.FScore1 < 150)
        {//-7.7
           gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 16.4f, gameObject.transform.position.z);
        }
        else if (GameManager.instance.FScore1 >= 150 && GameManager.instance.FScore1 < 300)
        {//-7.7
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4.65f, gameObject.transform.position.z);
        }
        else if (GameManager.instance.FScore1 >= 300)
        {//-7.7
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + -5.7f, gameObject.transform.position.z);
        }
    }
}
