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
    bool isWall;
    public Transform wallChk;
    public float wallchkDistance;
    public float slidingSpeed;
    public LayerMask w_Layer;

    //좌우 무빙
    public int nextMove = 1;
    public int move;
    bool ismove = false;
    

    //에니메이션 반대
    SpriteRenderer spriteRenderer;
    float pastpos;
    Animator anim;
    private void Start()
    {
        Points = new GameObject[numberOfpoints];
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        for (int i = 0; i < numberOfpoints; i++)
        {
            Points[i] = Instantiate(PointPre, transform.position, Quaternion.identity);
        }
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            ismove = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            ismove = false;
        }
    }
    private void FixedUpdate()
    { 
        //좌우 이동 레이캐스트 땅체크
        //레이캐스트 그림그려줌 scene에서
        Vector2 frontVec = new Vector2(rigidbody.position.x + nextMove * 0.24f, rigidbody.position.y );
        //Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("floor"));

        //애니메이션 오류 문제 해결
        Debug.DrawRay(new Vector2(rigidbody.position.x, rigidbody.position.y - 0.7f), Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit2 = Physics2D.Raycast(new Vector2(rigidbody.position.x, rigidbody.position.y - 0.7f), Vector3.down, 1, LayerMask.GetMask("floor"));

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

        if (rayHit2.collider == null)
        {
            nextMove = 0;
            anim.SetBool("isRuning", false);
        }
        else
        {
            if (nextMove == 0)
            {
                nextMove = 1;
            }
            anim.SetBool("isRuning", true);
        }
        //오늘 애니메이션은 레이캐스트로 변경한다.
        if (ismove == true)
        {
            rigidbody.velocity = new Vector2(nextMove, rigidbody.velocity.y);
        }
        if (isWall)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * slidingSpeed);
        }
    }
    private void Update()
    {
        //에니메이션 반대
        //땅과 공중 비교
        if(ismove == true)
        {
            if (nextMove == -1)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            if(pastpos > gameObject.transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
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
        
        for(int i = 0; i < Points.Length; i++)
        {
            Points[i].transform.position = PointPosition(i * 0.1f);
        }

        //벽 레이캐스트
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, w_Layer);

    }

    void DragStart()
    {
        //dragStartPos = Camera.main.ScreenToWorldPoint(touch.position);
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragStartPos.z = 0f;
        line.positionCount = 1;
        line.SetPosition(0, dragStartPos);
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].SetActive(true);
        }
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
        Vector3 force = dragStartPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
    }
    void DragRelease()
    {
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

        rigidbody.AddForce(clampedForce, ForceMode2D.Impulse);
    }
    private void OnMouseDown()
    {
        DragStart();
    }
    private void OnMouseDrag()
    {
        Dragging();
    }
    private void OnMouseUp()
    {
        pastpos = gameObject.transform.position.x;
        ismove = false;
        DragRelease();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
    }

    Vector2 PointPosition(float t)
    {
        Vector2 currentPointPos = (Vector2)transform.position + ((Vector2)clampedForce * t) + 0.5f * Physics2D.gravity * 4 * (t * t);
        return currentPointPos;
    }
}
