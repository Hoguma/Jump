using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectWall : MonoBehaviour
{
    public GameObject iceWall;
    public GameObject elecWall;
    // Start is called before the first frame update
    void Start()
    {
        iceWall.gameObject.SetActive(false);
        elecWall.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.CurrentScore <= 3)
        {
            if(GameManager.instance.Enemy == 3)
            {
                iceWall.gameObject.SetActive(true);
            }
            else
            {
                iceWall.gameObject.SetActive(false);
            }
        }
        else if (GameManager.instance.CurrentScore == 4)
        {
            if (GameManager.instance.Enemy == 1)
            {
                elecWall.gameObject.SetActive(true);
            }
            else
            {
                elecWall.gameObject.SetActive(false);
            }
        }
        else
        {
            elecWall.gameObject.SetActive(false);
            iceWall.gameObject.SetActive(false);
        }
    }
}
