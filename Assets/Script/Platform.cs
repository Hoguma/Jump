using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;

public class Platform : MonoBehaviour
{
    private GameObject target;
    private Collider2D coll;

    //이미지 변경 1,
    public Sprite Onestage;
    private SpriteRenderer myRenderer;
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
        
        if (target.transform.position.y - (0.6371382 * 0.65) > transform.position.y)
        {
            coll.enabled = true;
        }
        else
            coll.enabled = false;

        //1단계 점수 로 땅 바꾸기
        if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 20)
        {
            myRenderer.sprite = Onestage;
        }
    }

}
