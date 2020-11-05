using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmUi : MonoBehaviour
{
    private Image mainImage;
    public Sprite[] sprites;
    private bool isAlpha;
    public float Minus = 0.00001f;
    Color colors;
    // Start is called before the first frame update
    void Start()
    {
        mainImage = GetComponent<Image>();
        colors.a = 0f;
        colors.r = 1f;
        colors.g = 1f;
        colors.b = 1f;
        mainImage.color = colors;
        mainImage.sprite = sprites[0];
        Minus = 0.000000001f;
    }

    // Update is called once per frame
    void Update()
    {
        Minus += 0.000015f;
        colors.a -= Minus;

        switch (GameManager.instance.CurrentScore)
        {
            case 4:
                if (GameManager.instance.Enemy == 1)
                {
                    Alpha();
                    mainImage.sprite = sprites[3];
                    Debug.Log("4렙 번개 교체완료");
                }
                else if (GameManager.instance.Enemy == 2)
                {
                    Alpha();
                    mainImage.sprite = sprites[4];
                    Debug.Log("4렙 안개 교체완료");

                }
                Debug.Log("4렙");

                break;
        }
        if (GameManager.instance.Enemy == 0)
        {
            isAlpha = true;
        }
        mainImage.color = colors;
    }//수프라이트
    void Alpha()
    {
        if (isAlpha == true)
        {
            colors.a = 1f;
            isAlpha = false;
            Minus = 0.000000001f;
        }
    }
}
