using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallchange : MonoBehaviour
{
    //이미지 변경 1,
    public Sprite Onestage;
    private SpriteRenderer myRenderer;
    void Start()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 20)
        {
            myRenderer.sprite = Onestage;
        }
    }
}
