using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;

public class Platform : MonoBehaviour
{
    private GameObject target;
    [SerializeField]
    private GameObject CoinPre;
    private GameObject CoinsP;
    [SerializeField]
    private GameObject[] Coins;
    private Collider2D coll;

    public int NumberOfCoin;

    //이미지 변경 1,
    public int[] stage;
    public Sprite Twostage;
    public Sprite Threestage;
    public Sprite fourstage;
    public Sprite fivestage;
    private SpriteRenderer myRenderer;
    private int stageIm = 1;
    private int num;

    private float time = 0f;
    //방해요소
    private bool isCharCk = false;  //캐릭터가 서있나 확인
    public GameObject Risk;         //위험표시
    public GameObject Lightning;    //번개
    public bool  Ck = false;          //한번실행 변수

    private int coinPercent;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider2D>();
        target = FindObjectOfType<Player>().gameObject;
        CoinsP = GameObject.Find("Coins");
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        transform.parent = GameManager.instance.FloorAdd().transform;
        Coins = new GameObject[NumberOfCoin];

        num = GameManager.instance.platforms.Count;
        coinPercent = Random.Range(1, 5);
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

        if (coinPercent == 1 && GameManager.instance.platforms.Count > num)
        {
            GenarateCoin();
            num = 999;
        }

        //1단계 점수 로 땅 바꾸기
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 80 || stageIm == 5)
        if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= stage[3] || stageIm == 5) 
        {
            stageIm = 5;
            myRenderer.sprite = fivestage;
        }
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 60 || stageIm == 4)
        else if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= stage[2] || stageIm == 4) 
        {
            stageIm = 4;
            myRenderer.sprite = fourstage;
            st3Enemy();
        }
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 40 || stageIm == 3)
        else if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= stage[1] || stageIm == 3)
        {
            stageIm = 3;
            myRenderer.sprite = Threestage;
        }
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= 20 || stageIm == 2)
        else if (GameObject.Find("GameManager").GetComponent<GameManager>().FScore1 >= stage[0] || stageIm == 2) 
        {
            stageIm = 2;
            myRenderer.sprite = Twostage;

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            isCharCk = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            isCharCk = false;
        }
    }
    void st3Enemy()
    {
        if(isCharCk)
        {
            Color color = myRenderer.color;
            color.a -= Time.deltaTime * 0.5f;
            myRenderer.color = color;
            if (myRenderer.color.a <= 0.0f)
                Destroy(gameObject);
        }
        if (GameManager.instance.Enemy == 0)
        {
            Ck = false;
        }
        else if (GameManager.instance.Enemy == 1)
        {
            if (GameManager.instance.isEnemyRisk == false)
            {
                if (isCharCk == true && Ck == false)
                {
                    Instantiate(Risk, new Vector3(transform.position.x, transform.position.y + 1.37f, 0), Quaternion.identity);
                    Invoke("LightningG", 1.5f);
                    Ck = true;
                }
            }
        }
    }
    void LightningG()
    {
        Instantiate(Lightning, new Vector3(transform.position.x, transform.position.y + 1.931f, 0), Quaternion.identity);
    }

    void GenarateCoin()
    {
        //for (int i = 0; i < NumberOfCoin; i++)
        //{
        //    Coins[i] = Instantiate(CoinPre, transform.position, Quaternion.identity, CoinsP.transform);
        //}
        //for (int i = 0; i < Coins.Length; i++)
        //{
        //    Coins[i].transform.position = CoinPosition(i * 0.1f);
        //}

        Instantiate(CoinPre,new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity, CoinsP.transform);

    }

    Vector2 CoinPosition(float t)
    {
        Vector2 currentPointPos;
        currentPointPos = new Vector2(transform.position.x, transform.position.y + 3) + (new Vector2(0, 20f) * t) + 1f * Physics2D.gravity * 3.0f * (t * t);
        return currentPointPos;
    }
}
