using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float power = 0.5f;
    [SerializeField]
    private float maxDrag = 1f;
    [SerializeField]
    private Rigidbody2D rigidbody;
    [SerializeField]
    private LineRenderer line;

    [SerializeField]
    private GameObject PointPre;
    [SerializeField]
    private GameObject[] Points;

    public int numberOfpoints; 
    Vector3 dragStartPos;
    Touch touch;

    Vector3 clampedForce = Vector3.zero;

    //벽 슬라이딩?
    float isRight = 1;
    bool isWall = true;
    bool canJump = true;
    public Transform wallChk;
    public float wallchkDistance;
    public float slidingSpeed;
    public LayerMask w_Layer;

    //좌우 무빙
    bool isFloor = true;
    public int nextMove = 0;
    bool ismove = false;
    bool RiskOnce = true;
    public Transform floorChk;
    public LayerMask f_Layer;
    public float FloorchkDistance;


    //방해요소 이팩트
    private GameObject whirlPool;
    public GameObject Windpre;
    public GameObject Rainpre1;
    public GameObject Risks;

    public GameObject LWindpre;
    public GameObject RWindpre;
    public GameObject Rainpre;
    public GameObject Snowpre;

    //에니메이션 반대
    public SpriteRenderer spriteRenderer;
    float pastpos;
    public Animator anim;

    bool isTouch = false;
    private static Player instance = null;
    Rigidbody2D rigid;
    public float strength = 20f;

    //사운드
    public AudioSource myFx;
    public AudioClip JunpSound;

    void Awake()
    {
        rigid = this.GetComponent<Rigidbody2D>();

        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Player Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        Points = new GameObject[numberOfpoints];
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        for (int i = 0; i < numberOfpoints; i++)
        {
            Points[i] = Instantiate(PointPre, transform.position, Quaternion.identity, transform);
        }
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(false);
        }
    }
    
    
    private void Update()
    {
        CheckStatus();
        if (canJump == true)
        {
            if(GameManager.instance.isEnemyCheck == false)
            {
                //Click();
                Touch();
            }
        }
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].transform.position = PointPosition(i * 0.1f);
        }
        
        //벽 레이캐스트
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, w_Layer);
        //바닥 레이캐스트
        isFloor = Physics2D.Raycast(floorChk.position, Vector2.down, FloorchkDistance, f_Layer);
        Risk();
        
    }

    private void FixedUpdate()
    {
        if (nextMove == 1)
        {
            floorChk.transform.position = new Vector3(gameObject.transform.position.x + 30.4f, gameObject.transform.position.y - 52f, 0);
            //spriteRenderer.flipX = true;

        }
        else if (nextMove == -1)
        {
            floorChk.transform.position = new Vector3(gameObject.transform.position.x - 30.4f, gameObject.transform.position.y - 52f, 0);
            //spriteRenderer.flipX = false;

        }
        #region 주석
        //애니메이션 오류 문제 해결
        //Debug.DrawRay(new Vector2(rigidbody.position.x, rigidbody.position.y - 0.7f), Vector3.down, new Color(0, 1, 0));
        //RaycastHit2D rayHit2 = Physics2D.Raycast(new Vector2(rigidbody.position.x, rigidbody.position.y - 0.7f), Vector3.down, 1, LayerMask.GetMask("floor"));
        #endregion
        Physics();
        if (ismove == true)
        {
            anim.SetBool("isJump", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("effect"))
        {
            GameManager.instance.isCharDie = true;
        }
        //돈 삭제 위치 변경 eatCoin
        if (collision.gameObject.CompareTag("Die"))
        {
            GameManager.instance.isCharDie = true;
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "BlackHole")
        {
            Vector2 pos = other.transform.position;
            float dist = Vector2.Distance(pos, transform.position);
            Vector2 dir = ((Vector2)(transform.position) - pos).normalized;
            rigid.velocity += -1 * ((dir * strength) / (dist + 0.001f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.instance.Titlepanel())
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                ismove = false;
                anim.SetBool("isJump", true);
                nextMove = 0;
                ismove = true;
                canJump = true;
            }
            if (collision.gameObject.CompareTag("floor"))
            {
                ismove = true;
                canJump = true;
            }
        }
        if(collision.gameObject.CompareTag("wall"))
        {
            nextMove *= -1;
        }
        if (GameManager.instance.CurrentScore == 4)
        {
            if (GameManager.instance.Enemy == 1)
            {
                if (collision.gameObject.CompareTag("wall"))
                {
                    GameManager.instance.isCharDie = true;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            canJump = true;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            ismove = false;
            anim.SetBool("isJump", false);
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("wall"))
        {
            ismove = false;
            canJump = false;
            //nextMove = 0;
            anim.SetBool("isJump", true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(floorChk.position, Vector2.down * FloorchkDistance);
    }

    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos;
        if (isWall)
            currentPointPos = (Vector2)transform.position + ((Vector2)clampedForce * t) + Physics2D.gravity * 3456f * (t * t);
        else
            currentPointPos = (Vector2)transform.position + ((Vector2)clampedForce * t) + 1728f * Physics2D.gravity * (t * t);
        //Debug.Log(currentPointPos);
        return currentPointPos;
    }

    #region Drag

    void DragStart()
    {
        isTouch = true;
        ismove = false;

        //dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        dragStartPos.z = 0f;

        line.positionCount = 1;
        line.SetPosition(0, dragStartPos);

        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(true);
        }

        Vector3 force = dragStartPos - dragStartPos;
        clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
    }

    void Dragging()
    {
        //Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        draggingPos.z = 0f;

        line.positionCount = 2;
        line.SetPosition(1, draggingPos);

        Vector3 force = dragStartPos - draggingPos;

        clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
    }

    void DragRelease()
    {
        isTouch = false;
        line.positionCount = 0;

        //Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragReleasePos.z = 0f;

        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(false);
        }

        Vector3 force = dragStartPos - dragReleasePos;
        clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;

        if (clampedForce.x > 0)
        {
            nextMove = 1;
            spriteRenderer.flipX = true;

        }
        else
        {
            nextMove = -1;
            spriteRenderer.flipX = false;

        }

        //Debug.Log(clampedForce);
        rigidbody.AddForce(clampedForce, ForceMode2D.Impulse);
        pastpos = gameObject.transform.position.x;
        canJump = false;
    }

    #endregion

    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //클릭 처리
                isTouch = true;
                DragStart();

            }
        }
        else if (Input.GetMouseButton(0) && isTouch)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //클릭 처리
                Dragging();
            }
        }
        else if (Input.GetMouseButtonUp(0) && isTouch)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //클릭 처리
                DragRelease();
                JumpSound();
            }
        }
        else
            isTouch = false;
    }

    void Touch()
    {
        if (Input.touchCount > 0 && canJump == true)
        {
            touch = Input.GetTouch(0);
            if (!IsPointerOverUIObject(Input.GetTouch(0).position))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    DragStart();
                    isTouch = true;
                }
                else if (touch.phase == TouchPhase.Moved && isTouch)
                {
                    Dragging();
                    isTouch = true;
                }
                else if (touch.phase == TouchPhase.Ended && isTouch)
                {
                    DragRelease();
                    isTouch = false;
                }
                else if (touch.phase == TouchPhase.Canceled && isTouch)
                { 
                    isTouch = false;
                }
            }
        }
    }

    void CheckStatus()
    {
        #region ISMOVE
        if (ismove)
        {
            if(nextMove == 1)
                spriteRenderer.flipX = true;
            else if (nextMove == -1)
                spriteRenderer.flipX = false;

            if (isFloor == false)
            {
                if (nextMove == 1)
                {
                    floorChk.transform.position = new Vector3(transform.position.x - 30.4f, transform.position.y - 52f, 0);
                    nextMove = -1;
                }
                else if (nextMove == -1)
                {
                    floorChk.transform.position = new Vector3(transform.position.x + 30.4f, transform.position.y - 52f, 0);
                    nextMove = 1;
                }
            }
        }
        #endregion

        if (isWall)
            canJump = true;


        anim.SetBool("isRuning", ismove);
    }

    void Physics()
    {
        if (ismove)
        {
            gameObject.transform.position = new Vector3(transform.position.x + (nextMove*2), transform.position.y, 0);
        }
        if (isWall)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * slidingSpeed);
        }
    }

    void Risk()
    {
        if (GameManager.instance.CurrentScore <= 3)
        {
            if (GameManager.instance.Enemy == 0)
            {
                rigidbody.mass = 3;
                slidingSpeed = 0.5f;
                RiskOnce = true;
                LWindpre.SetActive(false);
                RWindpre.SetActive(false);
                Rainpre1.SetActive(false);
                Snowpre.SetActive(false);

            }
            else if (GameManager.instance.Enemy == 1)
            {
                if (GameManager.instance.isEnemyRisk == false)
                {
                    if (GameManager.instance.windRL == 1 && canJump == false)
                    {
                        rigidbody.velocity = new Vector2(rigidbody.velocity.x + -4.8f, rigidbody.velocity.y);
                        RWindpre.SetActive(true);
                        LWindpre.SetActive(false);
                    }
                    else
                    {
                        rigidbody.velocity = new Vector2(rigidbody.velocity.x + 4.8f, rigidbody.velocity.y);
                        LWindpre.SetActive(true);
                        RWindpre.SetActive(false);
                    }
                    //if (RiskOnce)
                    //{ Destroy(Instantiate(Windpre, Risks.transform), 10); RiskOnce = false; }

                }
            }
            else if (GameManager.instance.Enemy == 2)
            {
                if (GameManager.instance.isEnemyRisk == false)
                {
                    Rainpre1.SetActive(true);
                    //if (RiskOnce)
                    //{ Destroy(Instantiate(Rainpre, Risks.transform), 10); RiskOnce = false; }
                    rigidbody.mass = 4.5f;
                }
            }
            else if (GameManager.instance.Enemy == 3)
            {
                if (GameManager.instance.isEnemyRisk == false)
                {
                    Snowpre.SetActive(true);
                    slidingSpeed = 0.9f;
                }
            }
        }
    
        //플레이어 중력
        if (GameManager.instance.CurrentScore == 5)
        {
            rigidbody.gravityScale = 250f;
        }
        else
        {
            rigidbody.gravityScale = 384f;
        }
    }

    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();


        EventSystem.current
        .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    public void JumpSound()
    {
        if (GameManager.instance.isSoundon == true)
        {
            myFx.PlayOneShot(JunpSound);
        }
    }
    
}
