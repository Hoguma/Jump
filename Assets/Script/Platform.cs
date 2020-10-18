using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;

public class Platform : MonoBehaviour
{
    private GameObject target;
    private Collider2D coll;

    //이미지 변경 1,
    public int[] stage;
    public Sprite Twostage;
    public Sprite Threestage;
    public Sprite fourstage;
    public Sprite fivestage;
    private SpriteRenderer myRenderer;
    private int stageIm = 1;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        target = FindObjectOfType<Player>().gameObject;
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (target.transform.position.y - (0.6371382 * 0.7) > transform.position.y)
        {
            coll.enabled = true;
        }
        else
            coll.enabled = false;

        //1단계 점수 로 땅 바꾸기
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= stage[3] || stageIm == 5)
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
