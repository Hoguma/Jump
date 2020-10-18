using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallchange : MonoBehaviour
{
    //이미지 변경 1,
    public int[] stage;
    public Sprite Twostage;
    public Sprite Threestage;
    public Sprite fourstage;
    public Sprite fivestage;
    private SpriteRenderer myRenderer;
    private int stageIm = 1;
    void Start()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 80 || stageIm == 5)
        {
            stageIm = 5;
            myRenderer.sprite = fivestage;
        }
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= stage[2] || stageIm == 4)
        else if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 60 || stageIm == 4)
        {
            stageIm = 4;
            myRenderer.sprite = fourstage;
        }
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= stage[1] || stageIm == 3)
        else if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 40 || stageIm == 3)
        {
            stageIm = 3;
            myRenderer.sprite = Threestage;
        }
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= stage[0] || stageIm == 2)
        else if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 20 || stageIm == 2)
        {
            stageIm = 2;
            myRenderer.sprite = Twostage;
        }
    }
}
