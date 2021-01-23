using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBuff : MonoBehaviour
{
    [Header("EnemyIcon")]
    private Image Icon;
    public Sprite[] sprites;

    void Start()
    {
        Icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Enemy == 0 && GameManager.instance.time < 20f)
        {
            Icon.fillAmount = 0f;
        }
        else if (GameManager.instance.Enemy == 0 && GameManager.instance.time >= 20f)
        {
            Icon.fillAmount = 1f;
        }
        if (GameManager.instance.isEnemyCheck == false)
        {
            if (GameManager.instance.Enemy == 1 || GameManager.instance.Enemy == 2 || GameManager.instance.Enemy == 3)
            {
                if (GameManager.instance.time < 11f)
                {
                    Icon.fillAmount -= 0.09f * Time.deltaTime;
                }
            }
        }

        switch (GameManager.instance.CurrentScore)
        {
            case 1:
            case 2:
            case 3:
                if (GameManager.instance.Enemy == 1)// 바람
                {
                    Icon.sprite = sprites[0];
                }
                else if (GameManager.instance.Enemy == 2)// 폭우
                {
                    Icon.sprite = sprites[1];
                }
                else if (GameManager.instance.Enemy == 3)// 폭설
                {
                    Icon.sprite = sprites[2];
                }
                break;
            case 4:
                if (GameManager.instance.Enemy == 1)//번개
                {
                    Icon.sprite = sprites[3];
                }
                else if (GameManager.instance.Enemy == 2)//안개
                {
                    Icon.sprite = sprites[4];
                }
                break;
            case 5:
                if (GameManager.instance.Enemy == 1) //블랙홀
                {
                    Icon.sprite = sprites[5];
                }
                break;
        }
    }
}
