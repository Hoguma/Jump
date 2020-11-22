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

        //Minus += 0.000015f;

        switch (GameManager.instance.CurrentScore)
        {
            case 4:
                if (GameManager.instance.Enemy == 1)
                {
                    Alpha();
                    mainImage.sprite = sprites[3];
                }
                else if (GameManager.instance.Enemy == 2)
                {
                    Alpha();
                    mainImage.sprite = sprites[4];
                }
                break;
        }
        if (GameManager.instance.Enemy == 0)
        {
            time = 0.0f;
        }
        mainImage.color = colors;

    }//수프라이트
    void Alpha()
    {
        time += Time.deltaTime / 4f;
        colors.a = Mathf.Lerp(1f, 0f, time);
    }
}
