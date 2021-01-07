using GooglePlayGames;
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
    bool isSuccess = false;
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

    [Header("GameObject")]
    [SerializeField] private Camera mainCam;
    public List<GameObject> platforms;
    public GameObject player;
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
    [SerializeField] private GameObject CoinParents;
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

    [Header("Login")]
    [SerializeField] private GameObject LoginPanel;
    public bool isLoginPanel = true;
    public InputField id, pass;

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


        Backend.BMember.LoginWithTheBackendToken((callback) =>
        {
            
        });
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
        
        if (isEnemyCheck == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isEnemyCheck = false;
                Time.timeScale = 1f;
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

            Where where = new Where();
            where.Equal("Name", inDate);
            Backend.GameSchemaInfo.Get("Score", where, 1, callback1 =>
            {
                if (callback1.IsSuccess())
                {
                    if (int.Parse(callback1.Rows()[0]["HighScore"]["N"].ToString()) < FScore1)
                    {
                        Social.ReportScore(FScore1, GPGSIds.leaderboard_ranking, (bool success) => { });
                        bestScoreTxt.text = FScore1.ToString() + "m";
                        Param where1 = new Param();
                        where1.Add("Name", inDate);

                        Param param = new Param();
                        param.Add("HighScore", FScore1);

                        Backend.GameSchemaInfo.Update("Score", where1, param, (callback) =>
                        {
                            if (callback.IsSuccess())
                            {
                                Debug.Log(inDate);
                                Debug.Log("최고기록 갱신");
                            }
                            else
                            {
                                Debug.Log(callback);
                                Debug.Log(inDate);
                            }
                        });
                    }
                }
                else
                    Debug.Log(callback1);
            });
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

        if (isIngamePanel && uichange)
        {
            CoinView.anchoredPosition = new Vector2(CoinView.anchoredPosition.x, CoinView.anchoredPosition.y + 450f);
            uichange = false;
        }
        else if (!isIngamePanel && !uichange)
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
        //if(Random.Range(0, 2) == 0)
        AdmobManager.instance.ShowFrontAd();
    }

    public void EPanelTitle()
    {
        isEndPanel = false;
        isCharDie = false;
        player.transform.position = new Vector3(0, 0, 0);
        player.SetActive(true);
        mainCam.orthographicSize = 960f;
        mainCam.transform.position = new Vector3(0, 0f, mainCam.transform.position.z);
        EndPanel.SetActive(isEndPanel);
        MainUI.SetActive(true);
        MainUIChange();
        isGameRunning = false;
        Scopos = 0;
    }

    public void PPanelTitle()
    {
        isPausePanel = false;
        Time.timeScale = 1.0f;
        player.transform.position = new Vector3(0, 0, 0);
        mainCam.orthographicSize = 960f;
        mainCam.transform.position = new Vector3(0, 0f, mainCam.transform.position.z);
        PausePanel.SetActive(isPausePanel);
        MainUI.SetActive(true);
        MainUIChange();
        isGameRunning = false;
        Scopos = 0;
    }

    public bool Endpanel()
    {
        return isEndPanel;
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

    public void GameStart()
    {
        isGameRunning = true;
        player.transform.position = new Vector3(0, 0, 0);
        player.SetActive(true);
        mainCam.transform.position = new Vector3(0, 815f, mainCam.transform.position.z);

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
            isEnemyCheck = true;
            Time.timeScale = 0.2f;
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

    public void Pause()
    {
        UiClickSound();
        if (!isPausePanel)
        {
            isPausePanel = true;
            Time.timeScale = 0.0f;
            PausePanel.SetActive(isPausePanel);
        }
    }

    public void Continue()
    {
        UiClickSound();
        if (isPausePanel)
        {
            isPausePanel = false;
            Time.timeScale = 1.0f;
            PausePanel.SetActive(isPausePanel);
        }
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

    public void OnClickSignUp()
    {
        Backend.BMember.CustomSignUp(id.text, pass.text, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("회원가입 성공");
                Backend.BMember.CreateNickname(id.text, callback1 =>
                {
                    if (callback1.IsSuccess())
                    {
                        Debug.Log("사용가능한 닉네임입니다.");
                    }
                    else
                    {
                        Debug.Log("중복된 닉네임입니다.");
                    }
                });
            }
            else
            {
                Debug.Log("회원가입 실패");
            }
        });
    }

    public void OnClickLogin()
    {
        Backend.BMember.CustomLogin(id.text, pass.text, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("로그인 성공");
                LoginPanel.SetActive(false);
                bro = Backend.BMember.GetUserInfo();
                inDate = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
                if (bro.IsSuccess())
                {
                    Where param = new Where();
                    param.Equal("Name", inDate);
                    Backend.GameSchemaInfo.Get("Score", param, 1, callback1 =>
                    {
                        if (callback1.IsSuccess())
                        {
                            bestScoreTxt.text = callback1.Rows()[0]["HighScore"]["N"].ToString() + "m";
                            Debug.Log("정보 불러오기 성공");
                        }
                        else
                        {
                            bro = Backend.GameSchemaInfo.Insert("Score");
                            Param param1 = new Param();
                            param1.Add("Name", inDate);
                            Backend.GameSchemaInfo.Update("Score", bro.GetInDate(), param1);
                            Debug.Log("정보 불러오기 실패");
                        }
                    });
                }
                else
                    Debug.Log(bro);
            }
            else
            {
                Debug.Log("로그인 실패");
            }
        });
    }

    private void RankUpdate()
    {
        Backend.GameSchemaInfo.Get("Score", "HighScore", callback =>
        {
            if (callback.IsSuccess())
            {
                Backend.GameInfo.UpdateRTRankTable("Score", "HighScore", callback.GetInDate(), FScore1, callback1 =>
                {
                    if (callback1.IsSuccess())
                    {
                        Debug.Log("랭킹 업데이트 완료");
                    }
                    else
                    {
                        Debug.LogError(int.Parse(callback1.GetStatusCode()));
                    }
                });
            }
            else
            {
                Backend.GameSchemaInfo.Insert("Score", callback3 =>
                {
                    if (callback3.IsSuccess())
                    {
                        Debug.Log("랭킹 정보 입력됨");
                        Backend.GameInfo.UpdateRTRankTable("Score", "HighScore", callback.GetInDate(), FScore1, callback2 =>
                        {
                            if (callback2.IsSuccess())
                            {
                                Debug.Log("랭킹 업데이트 완료");
                            }
                            else
                            {
                                Debug.LogError(int.Parse(callback2.GetStatusCode()));
                            }
                        });
                    }

                });
            }
        });
    }

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

    private void GoogleAuth()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success == false)
                {
                    Debug.Log("구글 로그인 실패");
                    return;
                }

                Debug.Log("구글 로그인 성공");
                Debug.Log("Email : " + PlayGamesPlatform.Instance.GetIdToken());
                Debug.Log("GoogleId : " + ((PlayGamesLocalUser)Social.localUser).Email);
                Debug.Log("UserName : " + Social.localUser.userName);
                Debug.Log("UserName : " + PlayGamesPlatform.Instance.GetUserDisplayName());
            });
        }
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
                    Where param = new Where();
                    param.Equal("Name", inDate);
                    Backend.GameSchemaInfo.Get("Score", param, 1, callback1 =>
                    {
                        if (callback1.IsSuccess())
                        {
                            bestScoreTxt.text = callback1.Rows()[0]["HighScore"]["N"].ToString() + "m";
                            Debug.Log("정보 불러오기 성공");
                        }
                        else
                        {
                            bro = Backend.GameSchemaInfo.Insert("Score");
                            Param param1 = new Param();
                            param1.Add("Name", inDate);
                            Backend.GameSchemaInfo.Update("Score", bro.GetInDate(), param1);
                            Debug.Log("정보 불러오기 실패");
                        }
                    });
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
                            Where param = new Where();
                            param.Equal("Name", inDate);
                            Backend.GameSchemaInfo.Get("Score", param, 1, callback1 =>
                            {
                                if (callback1.IsSuccess())
                                {
                                    bestScoreTxt.text = callback1.Rows()[0]["HighScore"]["N"].ToString() + "m";
                                    Debug.Log("정보 불러오기 성공");
                                }
                                else
                                {
                                    bro = Backend.GameSchemaInfo.Insert("Score");
                                    Param param1 = new Param();
                                    param1.Add("Name", inDate);
                                    Backend.GameSchemaInfo.Update("Score", bro.GetInDate(), param1);
                                    Debug.Log("정보 불러오기 실패");
                                }
                            });
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
}




