using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                return null;
            return _instance;
        }
    }

    [SerializeField]
    [Tooltip("Title 터치방지 오브젝트")] private GameObject UiPanel;
    public GameObject player;
    public GameObject platformPrefab;
    private GameObject myPlat;
    [Header("Wall")]
    public GameObject wall;
    [Header("Score")]
    public Text FScore;
    public Image stagePanel = null;
    public float Scopos;
    public float FScore1;
    public int fade = 0;
    public bool isPanel = true;

    public bool isFaded = false;
    private void Awake()
    {
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
    void Start()
    {
        //for(int i = 0; i<3; i++)
        //{
        //    myPlat = (GameObject)Instantiate(platformPrefab, new Vector2(Random.Range(-3f, 3f), player.transform.position.y + (2 + Random.Range(0.5f, 1f))), Quaternion.identity);
        //}
    }
    private void Update()
    {
        Scopos = player.transform.position.y;
        FScore1 = (int)Scopos;
        FScore.text = FScore1.ToString() + "m";

        if (FScore1 == 20 && FScore1 >= 1 && !isFaded && fade == 0)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 1;
        }
        else if (FScore1 == 40 && FScore1 >= 1 && !isFaded && fade == 1)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 2;
        }
        else if (FScore1 == 60 && FScore1 >= 1 && !isFaded && fade == 2)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 3;
        }
        else if (FScore1 == 80 && FScore1 >= 1 && !isFaded && fade == 3)
        {
            StartCoroutine(FadeIn(0.3f));
            fade = 4;
        }
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

    public void PanelChange()
    {
        isPanel = !isPanel;
        UiPanel.SetActive(isPanel);
    }

    public bool panel()
    {
        return isPanel;
    }
}
