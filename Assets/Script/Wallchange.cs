using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallchange : MonoBehaviour
{
    //이미지 변경 1,
    public Sprite Twostage;
    public Sprite Threestage;
    private SpriteRenderer myRenderer;
    private int stage = 1;
    void Start()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 20)
        {
            myRenderer.sprite = Twostage;
        }
        if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 40)
        {
            myRenderer.sprite = Threestage;
        }
    }
}
