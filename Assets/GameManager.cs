using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public int Enemy;
    public bool isCharDie = false;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                return null;
            return _instance;
        }
    }

    [Header("GameObject")]
    [SerializeField] private Camera mainCam;
    public GameObject player;

    [Header("Platform")]
    [SerializeField] private GameObject FloorsPre;
    private GameObject FloorsClone;
    private GameObject myPlat;
    private GameObject Ground;
    public GameObject platformPrefab;
    public GameObject GroundPrefab;


    [Header("Title")]
    [SerializeField] private GameObject TitlePanel;
    public bool isTitlePanel = true;

    [Header("GameOver")]
    [SerializeField] private GameObject EndPanel;
    public bool isEndPanel = false;


    [Header("Wall")]
    [SerializeField] private GameObject Walls;
    public GameObject wall;

    [Header("Score")]
    public float time = 0.0f;
    public Text FScore;
    public Text EScore;
    public Image stagePanel = null;
    public float Scopos;
    public float FScore1;
    public int fade = 0;

    public bool isFaded = false;
    //방해요소
    public GameObject isCloud;
    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-1.73f, 1.73f), player.transform.position.y + (2 * i + Random.Range(1.3f, 1.5f))), Quaternion.identity);
        }
        FloorsClone = FloorsPre;
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        Instantiate(wall, new Vector3(0, 0, 0), transform.rotation);
        Instantiate(wall, new Vector3(0, 10, 0), transform.rotation);
    }

    private void Update()
    {
        if (FScore1 >= 450 && FScore1 < 600)
        {
            if (GameManager.instance.Enemy == 2)
            {
                isCloud.SetActive(true);
            }
            else
            {
                isCloud.SetActive(false);
            }
        }
        EnemyStart();
        Vector3 pos = Camera.main.WorldToViewportPoint(player.transform.position);
        if (pos.y < -0.1f && !isEndPanel || isCharDie == true)
        {
            isEndPanel = true;
            EndPanel.SetActive(isEndPanel);
            player.SetActive(!isEndPanel);
            EScore.text = FScore1.ToString() + "m";
            Debug.Log(isEndPanel);
        }

        if (Scopos < player.transform.position.y)
            Scopos = player.transform.position.y;

        FScore1 = (int)Scopos;
        FScore.text = FScore1.ToString() + "m";


        Stage();
    }
    IEnumerator FadeIn(float t)
    {
        float p = 0f;
        isFaded = true;
        stagePanel.color = new Color(stagePanel.color.r, stagePanel.color.g, stagePanel.color.b, p);
        yield return null;

        while (p <= t)
        {
            p += Time.deltaTime;
            stagePanel.color = new Color(stagePanel.color.r, stagePanel.color.g, stagePanel.color.b, p / 1f);
            yield return null;
        }
        isFaded = false;
        StartCoroutine(FadeOut(t));
    }

    IEnumerator FadeOut(float t)
    {
        float p = 1f;
        isFaded = true;
        stagePanel.color = new Color(stagePanel.color.r, stagePanel.color.g, stagePanel.color.b, p);
        yield return null;

        while (p >= 0f)
        {
            p -= Time.deltaTime;
            stagePanel.color = new Color(stagePanel.color.r, stagePanel.color.g, stagePanel.color.b, p / 1f);
            yield return null;
        }
        isFaded = false;
    }

    public void TPanelChange()
    {
        isTitlePanel = !isTitlePanel;
        TitlePanel.SetActive(isTitlePanel);
        //방해요소
        time = 0;
        Enemy = 0;
        isCharDie = false;
    }

    public bool Titlepanel()
    {
        return isTitlePanel;
    }

    public void EPanelChange()
    {
        GameStart();
        isEndPanel = false;
        isCharDie = false;
        EndPanel.SetActive(isEndPanel);
        Scopos = 0;

        time = 0;
        Enemy = 0;
        isCharDie = false;
    }

    public void EPanelTitle()
    {
        GameStart();
        isEndPanel = false;
        TPanelChange();
        EndPanel.SetActive(isEndPanel);
        Scopos = 0;
    }

    public bool Endpanel()
    {
        return isEndPanel;
    }

    public void Stage()
    {
        if (FScore1 == 150 && FScore1 >= 1 && !isFaded && fade == 0)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 1;
        }
        else if (FScore1 == 300 && FScore1 >= 1 && !isFaded && fade == 1)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 2;
        }
        else if (FScore1 == 450 && FScore1 >= 1 && !isFaded && fade == 2)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 3;
        }
        else if (FScore1 == 600 && FScore1 >= 1 && !isFaded && fade == 3)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 4;
        }
    }

    public GameObject FloorAdd()
    {
        return FloorsPre;
    }

    public GameObject WallAdd()
    {
        return Walls;
    }

    public void GameStart()
    {
        Instantiate(GroundPrefab);
        player.transform.position = new Vector3(0, 0, 0);
        player.SetActive(true);
        mainCam.transform.position = new Vector3(0, 4.5f, mainCam.transform.position.z);

        int nw = Walls.transform.childCount;
        for (int i = 0; i < nw; i++)
        {
            Destroy(Walls.transform.GetChild(i).gameObject);
        }
        Instantiate(wall, new Vector3(0, 0, 0), transform.rotation);
        Instantiate(wall, new Vector3(0, 10, 0), transform.rotation);
        int nf = FloorsPre.transform.childCount;
        for (int i = 0; i < nf; i++)
        {
            Destroy(FloorsPre.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 6; i++)
        {
            myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-1.73f, 1.73f), player.transform.position.y + (2 * i + Random.Range(1.3f, 1.5f))), Quaternion.identity);
        }
    }
    public void EnemyStart()
    {
        if (GameManager.instance.Enemy == 0 && time < 20f)
        {
            time += Time.deltaTime;
        }
        else if (GameManager.instance.Enemy == 0 && time >= 20f)
        {
            time = 0;
            GameManager.instance.Enemy = Random.Range(1, 5);
        }
        if (GameManager.instance.Enemy == 1)
        {
            if (time < 10f)
            {
                Debug.Log("1실행");
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                GameManager.instance.Enemy = 0;
            }
        }
        else if (GameManager.instance.Enemy == 2)
        {
            if (time < 10f)
            {
                Debug.Log("2실행");

                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                GameManager.instance.Enemy = 0;
            }
        }
        else if (GameManager.instance.Enemy == 3)
        {
            if (time < 10f)
            {
                Debug.Log("3실행");
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                GameManager.instance.Enemy = 0;
            }
        }
        else if (GameManager.instance.Enemy == 4)
        {
            if (time < 10f)
            {
                Debug.Log("4실행");
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                GameManager.instance.Enemy = 0;
            }
        }
    }
}
