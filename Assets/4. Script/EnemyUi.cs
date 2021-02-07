using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUi : MonoBehaviour
{
    public GameObject ga;
    public Image mainImage;
    public Image WordImage;
    public Sprite[] Msprites;
    public Sprite[] Wsprites;
    private bool isAlpha;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        mainImage.sprite = Msprites[0];
        WordImage.sprite = Wsprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        Alpha();

        switch (GameManager.instance.CurrentScore)
        {
            case 1:
            case 2:
            case 3:
                if (GameManager.instance.Enemy == 1)// 바람
                {
                    mainImage.sprite = Msprites[0];
                    WordImage.sprite = Wsprites[0];
                }
                else if (GameManager.instance.Enemy == 2)// 폭우
                {
                    mainImage.sprite = Msprites[1];
                    WordImage.sprite = Wsprites[1];
                }
                else if (GameManager.instance.Enemy == 3)// 폭설
                {
                    mainImage.sprite = Msprites[2];
                    WordImage.sprite = Wsprites[2];
                }
                break;
            case 4:
                if (GameManager.instance.Enemy == 1)//번개
                {
                    mainImage.sprite = Msprites[3];
                    WordImage.sprite = Wsprites[3];
                }
                else if (GameManager.instance.Enemy == 2)//안개
                {
                    mainImage.sprite = Msprites[4];
                    WordImage.sprite = Wsprites[4];
                }
                break;
            case 5:
                if(GameManager.instance.Enemy == 1) //블랙홀
                {
                    mainImage.sprite = Msprites[5];
                    WordImage.sprite = Wsprites[5];
                }
                break;
        }

    }//수프라이트

    void Alpha()
    {
        if(GameManager.instance.isEnemyCheck == false)
        {
            ga.SetActive(false);
        }
        else
        {
            ga.SetActive(true);
            time += Time.unscaledDeltaTime;
            if(time >= 1.5f)
            {
                time = 0;
                GameManager.instance.isEnemyCheck = false;
                Time.timeScale = 1;
            }
        }
    }
}
