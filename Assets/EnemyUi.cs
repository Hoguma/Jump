using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUi : MonoBehaviour
{
    private Image mainImage;
    public Sprite[] sprites;
    Color colors;
    // Start is called before the first frame update
    void Start()
    {
        mainImage = GetComponent<Image>();
        colors.a = 0f;
        colors.r = 0f;
        colors.g = 0f;
        colors.b = 0f;
        mainImage.color = colors;
        mainImage.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        colors.a -= 0.01f;
        colors.r -= 0.01f;
        colors.g -= 0.01f;
        colors.b -= 0.01f;

        switch (GameManager.instance.CurrentScore)
        {
            case 4:
                if(GameManager.instance.Enemy == 1)
                {
                    mainImage.sprite = sprites[4];
                }
                else if(GameManager.instance.Enemy == 2)
                {
                    mainImage.sprite = sprites[3];
                }
                mainImage.color = colors;
                break;
        }

    }//수프라이트
}
