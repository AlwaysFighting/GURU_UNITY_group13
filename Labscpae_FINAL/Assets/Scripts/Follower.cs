using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    enum FollwerState
    {
        Hide,
        Move,
        Eventstart,
        Idle
    }

    // 나의 캐릭터 컨트롤러
    CharacterController cc;

    FollwerState follwerState;

    Animator anim;

    // 플레이어 게임 오브젝트
    GameObject player;

    // 감지 범위 
    public float findDistance = 2.0f;

    // 이동 속도
    public float moveSpeed = 4.0f;

    public float followDistance = 2.0f;

    // 초기 위치와 회전 저장용 변수
    Vector3 originPos;
    Quaternion originRot;


    void Start()
    {
        // 초기 상태는 대기 상태(Idle)
        follwerState = FollwerState.Hide;

        // player 검색
        player = GameObject.Find("Player");

        // 캐릭터 컨트롤러 받아오기
        cc = GetComponent<CharacterController>();

        // 자식 오브젝트의 애니메이터 컴포넌트를 가져오기
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 상태 체크용 switch문
        switch (follwerState)
        {
            case FollwerState.Hide:
                Hide();
                break;
            case FollwerState.Move:
                Move();
                break;
            case FollwerState.Eventstart:
                Eventstart();
                break;
            case FollwerState.Idle:
                Idle();
                break;
        }

    }
    private void FixedUpdate()
    {
        // 초기 위치와 회전 저장하기
        originPos = player.transform.position;
        originRot = player.transform.rotation;
    }

    void Eventstart()
    {
        follwerState = FollwerState.Idle;
        anim.SetTrigger("StandToIdle");
    }

    void Idle()
    {
        // 만일 player 와의 거리가 감지 밖이라면...
        if (Vector3.Distance(originPos, transform.position) > followDistance)
        {
            // 상태를 이동 상태로 변경한다. 
            follwerState = FollwerState.Move;
            print("상태 전환 : Idle -> Move");
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        // 만일, 이동 거리 안에 있다면... 
        /*if (Vector3.Distance(originPos, transform.position) < followDistance)
        {
            // 상태를 복귀 상태로 전환한다.
            follwerState = FollwerState.Idle;
            print("상태 전환 : Move -> Idle");
        }*/

        // 만일  범위 밖이라면..
        if (Vector3.Distance(originPos, transform.position) >= followDistance)
        {
            // 이동 방향을 구한다.
            Vector3 dir = (originPos - transform.position).normalized;

            // 나의 전방 방향을 이동 방향과 일치시킨다.
            transform.forward = dir;

            // 캐릭터 컨트롤러로 이동 방향을 이동한다.
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // resident 5초 뒤 제거
            Destroy(gameObject, 10);
        }
        else
        {
            follwerState = FollwerState.Idle;
            anim.SetTrigger("MoveToIdle");
            print("상태 전환 : Move -> Idle");
        }

    }

    void Hide()
    {
        // 만일 player 와의 거리가 이내라면...
        if (Vector3.Distance(player.transform.position, transform.position) <= findDistance)
        {
            follwerState = FollwerState.Eventstart;
            print("발견!");
            anim.SetTrigger("HideToStand");
        }
    }
}