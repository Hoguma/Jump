using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll1 : MonoBehaviour
{
    public GameObject cmrPos;
    int scorel;
    float q;
    float w;
    float e;
    float r;
    float t;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = cmrPos.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = cmrPos.transform.position;
        scorel = (int)GameManager.instance.FScore1;

        if (GameManager.instance.FScore1 <= 150)
        {
            q = scorel * -30f;
        }
        else if (GameManager.instance.FScore1 > 150 && GameManager.instance.FScore1 <= 300)
        {
            scorel -= 150;
            w = scorel * -28.666667f;
        }
        else if (GameManager.instance.FScore1 > 300 && GameManager.instance.FScore1 <= 450)
        {
            scorel -= 300;
            e = scorel * -24.39333333333333f;
        }
        else if (GameManager.instance.FScore1 > 450 && GameManager.instance.FScore1 <= 600)
        {
            scorel -= 450;
            r = scorel * -24.38f;
        }
        else if (GameManager.instance.FScore1 > 600)
        {
            scorel -= 600;
            t = scorel * -24.38f;
        }
        if (GameManager.instance.isEndPanel == false)
        {
            Debug.Log("111");

            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + q + w + e + r + t, 0), Time.deltaTime * 900);
        }
        else
        {
            Debug.Log("sex");
            scorel = 0;
            q = 0f;
            w = 0f;
            e = 0f;
            r = 0f;
            t = 0f;
            transform.position = new Vector3(transform.position.x, cmrPos.transform.position.y - 30f, 0f); ;
        }
    }
}
