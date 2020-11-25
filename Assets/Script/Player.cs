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
    public int nextMove = 0;
    public int move;
    bool ismove = false;

    //블랙홀
    private GameObject whirlPool;

    //에니메이션 반대
    public SpriteRenderer spriteRenderer;
    float pastpos;
    Animator anim;

    bool isTouch = false;
    private static Player instance = null;

    void Awake()
    {
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
        //Touch();
        Click();
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].transform.position = PointPosition(i * 0.1f);
        }
        //플레이어 중력
        st5Enemy();

        //벽 레이캐스트
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, w_Layer);

        Risk();
    }

    private void FixedUpdate()
    {
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

        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            PlayerPrefs.SetInt("CoinCount", PlayerPrefs.GetInt("CoinCount") + 1);
        }
        if (collision.gameObject.CompareTag("Die"))
        {
            GameManager.instance.isCharDie = true;
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

                ismove = true;
                canJump = true;
                nextMove = 1;
            }
                if (collision.gameObject.CompareTag("floor"))
            {
                ismove = true;
                canJump = true;
                nextMove = 1;
            }
        }
        if(collision.gameObject.CompareTag("wall"))
        {
            nextMove *= -1;
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
            nextMove = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("Ground"))
        {
            ismove = false;
            canJump = false;
            nextMove = 0;
            anim.SetBool("isJump", true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
    }
    void st5Enemy()
    {
        if (GameManager.instance.Enemy == 2)
        {
            if (GameManager.instance.isEnemyRisk == false)
            {
                rigidbody.gravityScale = 1.0f;
            }
        }
    }
    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos;
        if (isWall)
        {
            currentPointPos = (Vector2)transform.position + ((Vector2)clampedForce * t) + 0.683472f * Physics2D.gravity * 6.0f * (t * t);
        }
        else
            currentPointPos = (Vector2)transform.position + ((Vector2)clampedForce * t) + 0.683472f * Physics2D.gravity * 3.0f  * (t * t);
        return currentPointPos;
    }

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

        //Vector3 force = dragStartPos - Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 force = dragStartPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
    }

    void Dragging()
    {
        //Vector3 draggingPos = Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 draggingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        draggingPos.z = 0f;

        line.positionCount = 2;
        line.SetPosition(1, draggingPos);

        //Vector3 force = dragStartPos - Camera.main.ScreenToWorldPoint(touch.position);
        Vector3 force = dragStartPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
        } else
        {
            nextMove = -1;
        }

        //Debug.Log(clampedForce);
        rigidbody.AddForce(clampedForce, ForceMode2D.Impulse);
        pastpos = gameObject.transform.position.x;
        canJump = false;
    }

    void Click()
    {
        if (Input.GetMouseButtonDown(0) && canJump == true)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //클릭 처리
                DragStart();
            }
        }
        if (Input.GetMouseButton(0) && canJump == true)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //클릭 처리
                Dragging();
            }
        }
        if (Input.GetMouseButtonUp(0) && canJump == true)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //클릭 처리
                DragRelease();
                //Debug.Log(rigidbody.velocity.x);
            }
        }
    }

    void Touch()
    {
        if (Input.touchCount > 0 && canJump == true)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                DragStart();
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Dragging();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                DragRelease();
            }
        }
    }

    void CheckStatus()
    {
        #region ISMOVE
        if (ismove)
        {
            if (nextMove == -1)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            Vector2 frontVec = new Vector2(rigidbody.position.x + nextMove * 0.24f, rigidbody.position.y);
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("floor"));

            if (rayHit.collider == null)
            {
                if (nextMove == 1)
                {
                    nextMove = -1;
                }
                else if (nextMove == -1)
                {
                    nextMove = 1;
                }
            }
        }
        else
        {
            if (pastpos > gameObject.transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
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
            rigidbody.velocity = new Vector2(nextMove, rigidbody.velocity.y);
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
            }
            else if (GameManager.instance.Enemy == 1)
            {
                if (GameManager.instance.isEnemyRisk == false)
                {
                    if (GameManager.instance.windRL == 1 && canJump == false)
                        rigidbody.velocity = new Vector2(rigidbody.velocity.x + -0.05f, rigidbody.velocity.y);
                    else
                        rigidbody.velocity = new Vector2(rigidbody.velocity.x + 0.05f, rigidbody.velocity.y);
                }
            }
            else if (GameManager.instance.Enemy == 2)
            {
                if (GameManager.instance.isEnemyRisk == false)
                {
                    rigidbody.mass = 4.5f;
                }
            }
            else if (GameManager.instance.Enemy == 3)
            {
                if (GameManager.instance.isEnemyRisk == false)
                {
                    slidingSpeed = 0.9f;
                }
            }
        }
    }
}
