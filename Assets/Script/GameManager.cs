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
    public List<GameObject> platforms;
    public GameObject player;

    [Header("Platform")]
    [SerializeField] private GameObject FloorsPre;
    private GameObject FloorsClone;
    private GameObject myPlat;
    private GameObject Ground;
    public GameObject platformPrefab;
    public GameObject GroundPrefab;
    public Vector2 lately;


    [Header("Title")]
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject TitlePanel;
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private Text CoinNum;
    public bool isTitlePanel = true;
    public bool isOptionPanel = false;
    public bool isSoundon = true;
    public bool isSoundFXon = true;
    public bool isVibeon = true;

    [Header("Ingame")]
    [SerializeField] private GameObject IngamePanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject CoinParents;
    public bool isIngamePanel = false;
    public bool isPausePanel = false;
    public int CurrentScore = 0;
    public int CoinCount = 0;
    public int windRL;


    [Header("GameOver")]
    [SerializeField] private GameObject EndPanel;
    public bool isEndPanel = false;

    [Header("Shop")]
    [SerializeField] private GameObject ShopPanel;
    public bool isShopPanel = false;

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
    public bool isGameRunning = false;
    //방해요소 전 위험요소 뜨는 시간 체크
    public bool isEnemyRisk;
    //방해요소
    public GameObject isCloud;
    //방해요소 블랙홀 한개 생성 삭제 시 생성 확인
    public bool isBlackTrue = true;
    private void Awake()
    {
        isBlackTrue = true;

        //처음 방해요소 알림이 뜨지 않으니 거짓
        isEnemyRisk = false;

        for (int i = 0; i < 5; i++)
        {
            platforms.Add(myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-1.73f, 1.73f), player.transform.position.y + (2 * i + Random.Range(1.3f, 1.5f))), Quaternion.identity));
            lately = myPlat.transform.position;
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

        CoinNum.text = PlayerPrefs.GetInt("CoinCount", 0).ToString();
        if (FScore1 >= 450 && FScore1 < 600)
        {
            if (Enemy == 2)
            {
                if (isEnemyRisk == false)
                {
                    isCloud.SetActive(true);
                }
            }
            else
            {
                isCloud.SetActive(false);
            }
        }
        EnemyStart();
        Vector3 pos = Camera.main.WorldToViewportPoint(player.transform.position);
        if (pos.y < -0.001f && !isEndPanel || isCharDie == true)
        {
            isEndPanel = true;
            EndPanel.SetActive(isEndPanel);
            player.SetActive(!isEndPanel);
            MainUI.SetActive(!isEndPanel);
            EScore.text = FScore1.ToString() + "m";
            //Debug.Log(isEndPanel);
        }
        //이벤트 블랙홀 실행 요건
        if (GameManager.instance.isCharDie == true)
        {
            GameManager.instance.isBlackTrue = true;
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

    public void MainUIChange()
    {
        isTitlePanel = !isTitlePanel;
        TitlePanel.SetActive(isTitlePanel);
        isIngamePanel = !isIngamePanel;
        IngamePanel.SetActive(isIngamePanel);

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

        int cf = CoinParents.transform.childCount;
        for (int i = 0; i < cf; i++)
        {
            Destroy(CoinParents.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < 5; i++)
        {
            platforms.Add(myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-1.73f, 1.73f), player.transform.position.y + (2 * i + Random.Range(1.3f, 1.5f))), Quaternion.identity));
            lately = myPlat.transform.position;
        }
        //방해요소
        isGameRunning = true;
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
        MainUI.SetActive(!isEndPanel);
        
    }

    public void EPanelTitle()
    {
        //GameStart();

        isEndPanel = false;
        //MainUIChange();
        isCharDie = false;
        player.transform.position = new Vector3(0, 0, 0);
        player.SetActive(true);
        mainCam.orthographicSize = 2.5f;
        mainCam.transform.position = new Vector3(0, 1f, mainCam.transform.position.z);
        EndPanel.SetActive(isEndPanel);
        MainUI.SetActive(true);
        MainUIChange();
        isGameRunning = false;
        //Debug.Log(isEndPanel);
        Scopos = 0;
    }

    public bool Endpanel()
    {
        return isEndPanel;
    }

    public void Stage()
    {
        if(FScore1 < 150)
        {
            CurrentScore = 1;
            fade = 0;
        }
        if (FScore1 == 150 && FScore1 >= 1 && !isFaded && fade == 0)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 1;
            CurrentScore = 2;
        }
        else if (FScore1 == 300 && FScore1 >= 1 && !isFaded && fade == 1)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 2;
            CurrentScore = 3;
        }
        else if (FScore1 == 450 && FScore1 >= 1 && !isFaded && fade == 2)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 3;
            CurrentScore = 4;
        }
        else if (FScore1 == 600 && FScore1 >= 1 && !isFaded && fade == 3)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 4;
            CurrentScore = 5;
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
        isGameRunning = true;
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
        int cf = CoinParents.transform.childCount;
        for (int i = 0; i < cf; i++)
        {
            Destroy(CoinParents.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            platforms.Add(myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-1.73f, 1.73f), player.transform.position.y + (2 * i + Random.Range(1.3f, 1.5f))), Quaternion.identity));
            lately = myPlat.transform.position;
        }
        Scopos = 0;
        time = 0;
        Enemy = 0;
        isCharDie = false;
    }

    public bool IsGameRunning()
    {
        return isGameRunning;
    }

    public void EnemyStart()
    {
        if (Enemy == 0 && time < 20f)
        {
            time += Time.deltaTime;
        }
        else if (Enemy == 0 && time >= 20f)
        {
            time = 0;
            Enemy = Random.Range(1, 4);
        }

        if (Enemy == 1 || Enemy == 2 || Enemy == 3)
        {
            windRL = Random.Range(0, 1);
            if (time < 15f)
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                Enemy = 0;
            }
            if (time < 5f)
            {
                isEnemyRisk = true;
            }
            else
            {
                isEnemyRisk = false;
            }
        }
    }

    public void Pause()
    {
        if(!isPausePanel)
        {
            isPausePanel = true;
            Time.timeScale = 0.0f;
            PausePanel.SetActive(isPausePanel);
        }
    }

    public void Continue()
    {
        if (isPausePanel)
        {
            isPausePanel = false;
            Time.timeScale = 1.0f;
            PausePanel.SetActive(isPausePanel);
        }
    }

    public void Option()
    {
        if (!isOptionPanel)
        {
            isOptionPanel = true;
            OptionPanel.SetActive(isOptionPanel);
        }
    }

    public void Optionoff()
    {
        if (isOptionPanel)
        {
            isOptionPanel = false;
            OptionPanel.SetActive(isOptionPanel);
        }
    }

    public void SoundOnOff()
    {
        isSoundon = !isSoundon;
    }

    public void SoundFXOnOff()
    {
        isSoundFXon = !isSoundFXon;
    }

    public void VibrationOnOff()
    {
        isVibeon = !isVibeon;
    }

    public void ShopOnOff()
    {
        isShopPanel = !isShopPanel;
        ShopPanel.SetActive(isShopPanel);
        TitlePanel.SetActive(!isShopPanel);
    }
}
