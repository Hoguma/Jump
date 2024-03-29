﻿using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using BackEnd.Tcp;
using UnityEngine.Assertions.Must;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    string inDate;
    BackendReturnObject bro;
    
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

    private int BestScore = 0;

    private bool NoAds;
    [SerializeField] private GameObject NoAdsBtn;

    [Header("GameObject")]
    [SerializeField] private Camera mainCam;
    public List<GameObject> platforms;
    public GameObject player;
    public GameObject DieEffect;
    public RectTransform CoinView;
    Vector2 Coinviewpos;

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
    public GameObject CoinParents;
    public Text bestScoreTxt_in;
    public bool isIngamePanel = false;
    public bool isPausePanel = false;
    public int CurrentScore = 0;
    public int CoinCount = 0;
    public int windRL;


    [Header("GameOver")]
    [SerializeField] private GameObject EndPanel;
    public Text bestScoreTxt;
    public bool isEndPanel = false;

    [Header("Shop")]
    [SerializeField] private GameObject ShopPanel;
    public bool isShopPanel = false;
    private bool[] igotthis_BE = new bool[8];

    [Header("Wall")]
    [SerializeField] private GameObject Walls;
    public GameObject wall;

    [Header("Score")]
    public float time = 0.0f;
    public Text FScore;
    public Text EScore;
    public Image stagePanel = null;
    public float Scopos;
    public int FScore1;
    public int fade = 0;

    public bool isFaded = false;
    public bool isGameRunning = false;
    //방해요소 전 위험요소 뜨는 시간 체크
    public bool isEnemyCheck;
    public bool isEnemyRisk;
    public bool uichange = true;
    //방해요소
    public GameObject isCloud;
    //방해요소 블랙홀 한개 생성 삭제 시 생성 확인
    public bool isBlackTrue = true;
    //사운드
    public AudioSource myFx;
    public AudioClip clickSound;
    public AudioClip charDie;

    
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;


        isBlackTrue = true;

        //처음 방해요소 알림이 뜨지 않으니 거짓
        isEnemyRisk = false;

        for (int i = 0; i < 5; i++)
        {
            platforms.Add(myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-342f, 342f), player.transform.position.y + (384 * i + Random.Range(150f, 250f))), Quaternion.identity));
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
        Instantiate(wall, new Vector3(0, 1200, 0), transform.rotation);

        Coinviewpos = CoinView.anchoredPosition;
        Debug.Log(Coinviewpos);
    }

    private void Start()
    {
        Backend.Initialize(HandleBackendCallBack);
        GoolglePlayInit();
    }

    private void HandleBackendCallBack()
    {
        if (Backend.IsInitialized)
        {
            Debug.Log("서버 초기화 완료" + Backend.Utils.GetGoogleHash());
        }
        else
        {
            Debug.Log("서버 초기화 실패");
        }
    }


    private void Update()
    {
        if(NoAds)
            NoAdsBtn.SetActive(false);
        if (BestScore < FScore1)
        {
            BestScore = FScore1;
            bestScoreTxt_in.text = BestScore.ToString() + "m";
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        //타이틀에서 방해요소 실행 안됨
        if (isTitlePanel == true)
        {
            Enemy = 0;
            time = 0;
        }
        //현 스테이지 변수
        if (FScore1 < 150)
        {
            CurrentScore = 1;
        }
        else if (FScore1 >= 150 && FScore1 < 300)
        {
            CurrentScore = 2;
        }
        else if (FScore1 >= 300 && FScore1 < 450)
        {
            CurrentScore = 3;
        }
        else if (FScore1 >= 450 && FScore1 < 600)
        {
            CurrentScore = 4;
        }
        else if (FScore1 >= 600)
        {
            CurrentScore = 5;
        }
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
            charDieSound();
            Destroy(Instantiate(DieEffect, new Vector2(player.transform.position.x, player.transform.position.y + 50), Quaternion.identity), 0.5f);
            isEndPanel = true;
            EndPanel.SetActive(isEndPanel);
            player.SetActive(!isEndPanel);
            MainUI.SetActive(!isEndPanel);
            int nr = Player.Instance.Risks.transform.childCount;
            for (int i = 0; i < nr; i++)
            {
                Destroy(Player.Instance.Risks.transform.GetChild(i).gameObject);
            }
            EScore.text = FScore1.ToString() + "m";

            if (BestScore <= FScore1)
            {
                BestScore = FScore1;
                Social.ReportScore(BestScore, GPGSIds.leaderboard_ranking, (bool success) => { });
                Debug.Log(BestScore);
                bestScoreTxt.text = BestScore.ToString() + "m";
                UpdateScoreData();
            }
        }
        //이벤트 블랙홀 실행 요건
        if (GameManager.instance.isCharDie == true)
        {
            GameManager.instance.isBlackTrue = true;
        }
        if (Scopos < player.transform.position.y)
            Scopos = player.transform.position.y;

        FScore1 = (int)Scopos / 100;
        FScore.text = FScore1.ToString() + "m";

        if (isIngamePanel && uichange && !isShopPanel)
        {
            CoinView.anchoredPosition = new Vector2(CoinView.anchoredPosition.x, CoinView.anchoredPosition.y + 450f);
            uichange = false;
        }
        else if (!isIngamePanel && !uichange && !isShopPanel)
        {
            CoinView.anchoredPosition = Coinviewpos;
            uichange = true;
        }
        else if (isShopPanel && uichange && !isIngamePanel)
        {
            CoinView.anchoredPosition = new Vector2(CoinView.anchoredPosition.x, CoinView.anchoredPosition.y + 210f);
            uichange = false;
        }
        else if (!isShopPanel && !uichange && !isIngamePanel)
        {
            CoinView.anchoredPosition = Coinviewpos;
            uichange = true;
        }


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
        UiClickSound();

        MPChange();

        IPChange();

        GameReset();

        if(!NoAds)
         AdmobManager.instance.ShowFrontAd();
    }

    public void GameStartbtn()
    {
        MainUIChange();
        AdmobManager.instance.ToggleBannerAd(false);
    }

    public void GameStart()
    {
        player.transform.position = new Vector3(0, 0, 0);
        player.SetActive(true);
        mainCam.transform.position = new Vector3(0, 815f, mainCam.transform.position.z);

        GameReset();
    }

    public bool Titlepanel()
    {
        return isTitlePanel;
    }

    public void EPanelChange()
    {
        GameStart();

        EPChange();

        MainUI.SetActive(true);

        if (!NoAds)
            AdmobManager.instance.ShowFrontAd();
    }

    public void PPanelChange()
    {
        GameStart();

        PPChange();

        if (!NoAds)
            AdmobManager.instance.ShowFrontAd();
    }

    public void EPanelTitle()
    {
        isCharDie = false;

        PlayerPosReset();

        CameraReset();

        EPChange();

        MainUIChange();

        MainUI.SetActive(true);

        AdmobManager.instance.ToggleBannerAd(true);

        isGameRunning = false;
    }

    public void PPanelTitle()
    {
        AdmobManager.instance.ToggleBannerAd(true);
        PPChange();
        Time.timeScale = 1.0f;
        PlayerPosReset();
        CameraReset();
        MainUI.SetActive(true);
        MainUIChange();
        isGameRunning = false;
        Scopos = 0;
    }

    public bool Endpanel()
    {
        return isEndPanel;
    }

    private void GameReset()
    {
        int nw = Walls.transform.childCount;
        for (int i = 0; i < nw; i++)
        {
            Destroy(Walls.transform.GetChild(i).gameObject);
        }
        Instantiate(wall, new Vector3(0, 0, 0), transform.rotation);
        Instantiate(wall, new Vector3(0, 1200, 0), transform.rotation);
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
        int nr = Player.Instance.Risks.transform.childCount;
        for (int i = 0; i < nr; i++)
        {
            Destroy(Player.Instance.Risks.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 5; i++)
        {
            platforms.Add(myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-342f, 342f), player.transform.position.y + (384 * i + Random.Range(150f, 250f))), Quaternion.identity));

            lately = myPlat.transform.position;
        }

        isGameRunning = true;
        isCharDie = false;
        Scopos = 0;
        Enemy = 0;
        time = 0;
    }

    private void MPChange()
    {
        isTitlePanel = !isTitlePanel;
        TitlePanel.SetActive(isTitlePanel);
    }

    private void EPChange()
    {
        isEndPanel = !isEndPanel;
        EndPanel.SetActive(isEndPanel);
    }

    private void PPChange()
    {
        isPausePanel = !isPausePanel;
        PausePanel.SetActive(isPausePanel);
    }

    private void IPChange()
    {
        isIngamePanel = !isIngamePanel;
        IngamePanel.SetActive(isIngamePanel);
    }

    public void Pause()
    {
        UiClickSound();
        if (!isPausePanel)
        {
            PPChange();
            Time.timeScale = 0.0f;
        }
    }

    public void Continue()
    {
        UiClickSound();
        if (isPausePanel)
        {
            PPChange();
            Time.timeScale = 1.0f;
        }
    }

    public void Restart()
    {
        UiClickSound();
        GameStart();
        PPChange();
        Time.timeScale = 1.0f;
    }

    public void Option()
    {
        UiClickSound();
        if (!isOptionPanel)
        {
            isOptionPanel = true;
            OptionPanel.SetActive(isOptionPanel);
        }
    }

    public void Optionoff()
    {
        UiClickSound();
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
        UiClickSound();
        isSoundFXon = !isSoundFXon;
    }

    public void VibrationOnOff()
    {
        UiClickSound();
        isVibeon = !isVibeon;
    }

    public void ShopOnOff()
    {
        UiClickSound();
        isShopPanel = !isShopPanel;
        ShopPanel.SetActive(isShopPanel);
        TitlePanel.SetActive(!isShopPanel);
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
            isEnemyCheck = true;
            Time.timeScale = 0f;
            Enemy = Random.Range(1, 4);
            windRL = Random.Range(0, 2);
        }
        if (isEnemyCheck == false)
        {
            if (Enemy == 1 || Enemy == 2 || Enemy == 3)
            {
                if (time < 11f)
                {
                    time += Time.deltaTime;
                }
                else
                {
                    time = 0;
                    Enemy = 0;
                }
                if (time < 1f)
                {
                    isEnemyRisk = true;
                }
                else
                {
                    isEnemyRisk = false;
                }
            }
        }
    }

    private void CameraReset()
    {
        mainCam.orthographicSize = 960f;
        mainCam.transform.position = new Vector3(0, 0f, mainCam.transform.position.z);
    }

    private void PlayerPosReset()
    {
        player.transform.position = new Vector3(0, 0, 0);
        player.SetActive(true);
    }

    public void Stage()
    {
        if (FScore1 < 150)
        {
            fade = 0;
        }
        if (FScore1 == 150 && FScore1 >= 1 && !isFaded && fade == 0)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 1;
            GameManager.instance.Enemy = 0;
        }
        else if (FScore1 == 300 && FScore1 >= 1 && !isFaded && fade == 1)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 2;
            GameManager.instance.Enemy = 0;
        }
        else if (FScore1 == 450 && FScore1 >= 1 && !isFaded && fade == 2)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 3;
            GameManager.instance.Enemy = 0;
        }
        else if (FScore1 == 600 && FScore1 >= 1 && !isFaded && fade == 3)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 4;
            GameManager.instance.Enemy = 0;
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

    public bool IsGameRunning()
    {
        return isGameRunning;
    }


    #region 서버
    private void GoolglePlayInit()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail()
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();

        GPGSLogin();
    }

    public string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }

    public void GPGSLogin()
    {
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
            if (BRO.IsSuccess())
            {
                Debug.Log("뒤끝 연동 완료");
                bro = Backend.BMember.GetUserInfo();
                inDate = bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();
                if (bro.IsSuccess())
                {
                    GetData();
                    Shop.instance.num = PlayerPrefs.GetInt("SelectedNum", 0);
                    Player.Instance.spriteRenderer.sprite = Shop.instance.character[Shop.instance.num];
                    Player.Instance.anim.runtimeAnimatorController = Shop.instance.animators[Shop.instance.num];
                    Shop.instance.nametag.sprite = Shop.instance.charName[Shop.instance.num];
                }
                else
                    Debug.Log(bro + "812");
            }
            else
                Debug.Log("뒤끝 연동 실패");
            Debug.Log("구글 로그인 성공");
            Debug.Log("Email : " + PlayGamesPlatform.Instance.GetIdToken());
            Debug.Log("GoogleId : " + ((PlayGamesLocalUser)Social.localUser).Email);
            Debug.Log("UserName : " + Social.localUser.userName);
            Debug.Log("UserName : " + PlayGamesPlatform.Instance.GetUserDisplayName());
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                    if (BRO.IsSuccess())
                    {
                        Debug.Log("뒤끝 연동 완료");
                        Debug.Log(BRO);
                        bro = Backend.BMember.GetUserInfo();
                        Debug.Log(bro);
                        inDate = bro.GetReturnValuetoJSON()["row"]["inDate"].ToString();
                        if (bro.IsSuccess())
                        {
                            GetData();
                            Shop.instance.num = PlayerPrefs.GetInt("SelectedNum", 0);
                            Player.Instance.spriteRenderer.sprite = Shop.instance.character[Shop.instance.num];
                            Player.Instance.anim.runtimeAnimatorController = Shop.instance.animators[Shop.instance.num];
                            Shop.instance.nametag.sprite = Shop.instance.charName[Shop.instance.num];
                        }
                        else
                            Debug.Log(bro + "812");
                    }
                    else
                        Debug.Log(BRO);
                    Debug.Log("구글 로그인 성공");
                    Debug.Log("Email : " + PlayGamesPlatform.Instance.GetIdToken());
                    Debug.Log("GoogleId : " + ((PlayGamesLocalUser)Social.localUser).Email);
                    Debug.Log("UserName : " + Social.localUser.userName);
                    Debug.Log("UserName : " + PlayGamesPlatform.Instance.GetUserDisplayName());
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }
    }

    public void OnGpgsLogin()
    {
        BackendReturnObject _bro = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "GPGS");
        if (_bro.IsSuccess())
        {
            Debug.Log("구글 뒤끝 로그인 성공");
        }
        else
        {
            Debug.Log("구글 뒤끝 로그인 실패");
        }
    }

    public void OnUpdateEmail()
    {
        BackendReturnObject _bro = Backend.BMember.UpdateFederationEmail(GetTokens(), FederationType.Google);
        if (_bro.IsSuccess())
        {
            Debug.Log("이메일 주소 저장 성공");
        }
        else
        {
            Debug.Log("이메일 주소 저장 실패");
        }
    }

    public void OnCheckUserAuth()
    {
        BackendReturnObject _bro = Backend.BMember.CheckUserInBackend(GetTokens(), FederationType.Google);
        if (_bro.GetStatusCode() == "200")
        {
            Debug.Log("가입되어있는 계정입니다.");
        }
        else
        {
            Debug.Log("가입되어있지 않은 계정입니다.");
        }
    }

    public void OnChangeCustom2Fed()
    {
        BackendReturnObject _bro = Backend.BMember.ChangeCustomToFederation(GetTokens(), FederationType.Google);
        if (_bro.IsSuccess())
        {
            Debug.Log("구글계정으로 변경 완료");
        }
        else
        {
            Debug.Log("구글계정으로 변경 실패");
        }
    }

    public void ShowLeaderBorad()
    {
        Social.ShowLeaderboardUI();
    }

    public void UiClickSound()
    {
        if (isSoundon == true)
        {
            myFx.PlayOneShot(clickSound);
        }
    }
    public void charDieSound()
    {
        if (isSoundon == true)
        {
            myFx.PlayOneShot(charDie);
        }
    }

    private void OnApplicationQuit()
    {
        UpdateScoreData();
        UpDateCoinData();
    }

    private void UpdateScoreData()
    {
        Where where = new Where();
        where.Equal("Name", inDate);
        Backend.GameSchemaInfo.Get("Score", where, 1, callback1 =>
        {
            if (callback1.IsSuccess())
            {
                Param where1 = new Param();
                where1.Add("Name", inDate);
                Param param = new Param();
                param.Add("HighScore", BestScore);
                Backend.GameSchemaInfo.Update("Score", where1, param, (callback) =>
                {
                    if (callback.IsSuccess())
                        Debug.Log("점수 갱신 완료");
                    else
                        Debug.Log(callback1);
                });
            }
        });
    }

    public void UpDateCoinData()
    {
        Where where = new Where();
        where.Equal("Name", inDate);
        Backend.GameSchemaInfo.Get("Coin", where, 1, callback1 =>
        {
            if (callback1.IsSuccess())
            {
                Param where1 = new Param();
                where1.Add("Name", inDate);
                Param param = new Param();
                param.Add("Coin", PlayerPrefs.GetInt("CoinCount", 0));
                Backend.GameSchemaInfo.Update("Coin", where1, param, (callback) =>
                {
                    if (callback.IsSuccess())
                        Debug.Log("코인 갱신 완료");
                    else
                        Debug.Log(callback1);
                });
            }
        });
    }

    public void UpDateCharData(int i)
    {
        Where where = new Where();
        where.Equal("Name", inDate);
        Backend.GameSchemaInfo.Get("Character", where, 1, callback1 =>
        {
            if (callback1.IsSuccess())
            {
                Param where1 = new Param();
                where1.Add("Name", inDate);
                Param param = new Param();
                if(i == 1)
                {
                    param.Add("Brown", Shop.instance.igotthis[i]);
                }
                else if (i == 2)
                {
                    param.Add("Black", Shop.instance.igotthis[i]);
                }
                else if(i == 3)
                {
                    param.Add("Box", Shop.instance.igotthis[i]);
                }
                else if (i == 4)
                {
                    param.Add("Bulb", Shop.instance.igotthis[i]);
                }
                else if(i == 5)
                {
                    param.Add("Slime", Shop.instance.igotthis[i]);
                }
                else if(i == 6)
                {
                    param.Add("Rainbow", Shop.instance.igotthis[i]);
                }
                else if(i == 7)
                {
                    param.Add("Cyber", Shop.instance.igotthis[i]);
                }

                Backend.GameSchemaInfo.Update("Character", where1, param, (callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        Debug.Log("캐럭터 구매 완료");
                        UpDateCoinData();
                    }
                    else
                        Debug.Log(callback1);
                });
            }
        });
    }

    public void UpDateNoAdsData()
    {
        Where where = new Where();
        where.Equal("Name", inDate);
        Backend.GameSchemaInfo.Get("NoAds", where, 1, callback1 =>
        {
            if (callback1.IsSuccess())
            {
                Param where1 = new Param();
                where1.Add("Name", inDate);
                Param param = new Param();
                param.Add("NoAds", NoAds);
                Backend.GameSchemaInfo.Update("NoAds", where1, param, (callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        Debug.Log("NoAds 갱신 완료");
                        NoAdsBtn.SetActive(false);
                    }
                    else
                        Debug.Log(callback1);
                });
            }
        });
    }

    private void GetData()
    {
        Where param = new Where();
        param.Equal("Name", inDate);
        Backend.GameSchemaInfo.Get("Score", param, 1, callback1 =>
        {
            if (callback1.IsSuccess())
            {
                BestScore = int.Parse(callback1.Rows()[0]["HighScore"]["N"].ToString());
                bestScoreTxt_in.text = BestScore.ToString() + "m";
                bestScoreTxt.text = BestScore.ToString() + "m";
                Debug.Log("정보 불러오기 성공" + BestScore);
            }
            else
            {
                bro = Backend.GameSchemaInfo.Insert("Score");
                Param param1 = new Param();
                param1.Add("Name", inDate);
                Backend.GameSchemaInfo.Update("Score", bro.GetInDate(), param1);
                bestScoreTxt_in.text = "0m";
                Debug.Log("테이블 생성");
            }
        });

        Backend.GameSchemaInfo.Get("Coin", param, 1, callback1 =>
        {
            if (callback1.IsSuccess())
            {
                PlayerPrefs.SetInt("CoinCount", int.Parse(callback1.Rows()[0]["Coin"]["N"].ToString()));
                CoinNum.text = CoinCount.ToString();
                Debug.Log("정보 불러오기 성공" + CoinCount);
            }
            else
            {
                bro = Backend.GameSchemaInfo.Insert("Coin");
                Param param1 = new Param();
                param1.Add("Name", inDate);
                Backend.GameSchemaInfo.Update("Coin", bro.GetInDate(), param1);
                Debug.Log("테이블 생성");
            }
        });

        Backend.GameSchemaInfo.Get("Character", param, 1, callback1 =>
        {
            if (callback1.IsSuccess())
            {
                Debug.Log(bool.Parse(callback1.Rows()[0]["Brown"]["BOOL"].ToString())+  " 아잇");
                igotthis_BE[1] = bool.Parse(callback1.Rows()[0]["Brown"]["BOOL"].ToString());
                igotthis_BE[2] = bool.Parse(callback1.Rows()[0]["Black"]["BOOL"].ToString());
                igotthis_BE[3] = bool.Parse(callback1.Rows()[0]["Box"]["BOOL"].ToString());
                igotthis_BE[4] = bool.Parse(callback1.Rows()[0]["Bulb"]["BOOL"].ToString());
                igotthis_BE[5] = bool.Parse(callback1.Rows()[0]["Slime"]["BOOL"].ToString());
                igotthis_BE[6] = bool.Parse(callback1.Rows()[0]["Rainbow"]["BOOL"].ToString());
                igotthis_BE[7] = bool.Parse(callback1.Rows()[0]["Cyber"]["BOOL"].ToString());

                for(int i = 1; i < 8; i++)
                {
                    Shop.instance.igotthis[i] = igotthis_BE[i];
                    Debug.Log(Shop.instance.igotthis[i]);
                }
            }
            else
            {
                Debug.Log("검색 실패");
                bro = Backend.GameSchemaInfo.Insert("Character");
                Param param1 = new Param();
                param1.Add("Name", inDate);
                Backend.GameSchemaInfo.Update("Character", bro.GetInDate(), param1);
                Debug.Log("테이블 생성");
            }
        });

        Backend.GameSchemaInfo.Get("NoAds", param, 1, callback1 =>
        {
            if (callback1.IsSuccess())
            {
                NoAds = bool.Parse(callback1.Rows()[0]["NoAds"]["BOOL"].ToString());
                //버튼 활성화/비활성화
                if (NoAds)
                    NoAdsBtn.SetActive(false);
                Debug.Log("정보 불러오기 성공" + CoinCount);
            }
            else
            {
                bro = Backend.GameSchemaInfo.Insert("NoAds");
                Param param1 = new Param();
                param1.Add("Name", inDate);
                Backend.GameSchemaInfo.Update("NoAds", bro.GetInDate(), param1);
                Debug.Log("테이블 생성");
            }
        });
    }
    #endregion

    public bool GetNoAds()
    {
        return NoAds;
    }

    public void NoAdsSucsess()
    {
        NoAds = true;
        UpDateNoAdsData();
    }

    public void NoAdsFail()
    {
        Debug.Log("다시시도해주세요");
    }
}




