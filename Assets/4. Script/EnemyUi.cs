using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUi : MonoBehaviour
{
    private Image mainImage;
    public Sprite[] sprites;
    private bool isAlpha;
    public float time;
    Color colors;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        mainImage = GetComponent<Image>();
        colors.a = 0f;
        colors.r = 1f;
        colors.g = 1f;
        colors.b = 1f;
        mainImage.color = colors;
        mainImage.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        Alpha();

        //Debug.Log(GameManager.instance.CurrentScore);
        switch (GameManager.instance.CurrentScore)
        {
            case 1:
            case 2:
            case 3:
                if (GameManager.instance.Enemy == 1)// 바람
                {
                    mainImage.sprite = sprites[0];
                }
                else if (GameManager.instance.Enemy == 2)// 폭우
                {
                    mainImage.sprite = sprites[1];
                }
                else if (GameManager.instance.Enemy == 3)// 폭설
                {
                    mainImage.sprite = sprites[2];
                }
                break;
            case 4:
                if (GameManager.instance.Enemy == 1)//번개
                {
                    mainImage.sprite = sprites[3];
                }
                else if (GameManager.instance.Enemy == 2)//안개
                {
                    mainImage.sprite = sprites[4];
                }
                break;
            case 5:
                if(GameManager.instance.Enemy == 1) //블랙홀
                {
                    mainImage.sprite = sprites[5];
                }
                break;
        }
        if (GameManager.instance.Enemy == 0)
        {
            time = 0.0f;
            colors.a = 0f;
        }
        mainImage.color = colors;

    }//수프라이트
    void Alpha()
    {
        //time += Time.deltaTime / 4f;
        //colors.a = Mathf.Lerp(1f, 0f, time);
        if(GameManager.instance.isEnemyCheck == false)
        {
            colors.a = 0f;
        }
        else
        {
            colors.a = 1f;
        }
    }
}
