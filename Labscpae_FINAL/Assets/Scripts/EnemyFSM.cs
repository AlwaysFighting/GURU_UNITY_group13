using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState // enum : 열거형 함수로, 여러 가지를 한 번에 묶는 기능
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    EnemyState enemyState;

    // 플레이어 게임 오브젝트
    GameObject player;

    // 감지 범위 
    public float findDistance = 8.0f;

    // 나의 캐릭터 컨트롤러
    CharacterController cc;

    // 이동 속도
    public float moveSpeed = 4.0f;

    // 공격 가능 범위
    public float attackDistance = 8.0f;

    // 현재 누적 시간 변수
    float currentTime = 0;

    // 공격 딜레이 시간
    public float attackDelayTime = 2.0f;

    // 공격력 변수
    public int attackPower = 2;

    // 초기 위치와 회전 저장용 변수
    Vector3 originPos;
    Quaternion originRot;

    // 이동 가능한 거리
    public float moveDistance = 10.0f;

    // 최대 체력 변수
    public int maxHp = 5;

    // 현재 체력 변수
    int currentHp;

    // 슬라이더 변수
    public Slider hpSlider;

    // 애니메이터 컴포넌트 변수
    Animator anim;


    void Start()
    {
        // 초기 상태는 대기 상태(Idle)
        enemyState = EnemyState.Idle;

        // player 검색
        player = GameObject.Find("Player");

        // 캐릭터 컨트롤러 받아오기
        cc = GetComponent<CharacterController>();

        // 초기 위치와 회전 저장하기
        originPos = transform.position;
        originRot = transform.rotation;

        // 현재 체력 설정
        currentHp = maxHp;

        // 자식 오브젝트의 애니메이터 컴포넌트를 가져오기
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 상태 체크용 switch문
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        // hp 슬라이더 값에 체력 비율을 적용한다.
        hpSlider.value = (float)currentHp / (float)maxHp;
    }

    // 대기 행동 함수
    void Idle()
    {
        // 만일 player 와의 거리가 감지 범위 이내라면...
        if (Vector3.Distance(player.transform.position, transform.position) <= findDistance)
        {
            // 상태를 이동 상태로 변경한다. 
            enemyState = EnemyState.Move;
            print("상태 전환 : Idle -> Move");
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        // 만일 공격 범위 밖이라면..
        if (Vector3.Distance(player.transform.position, transform.position) > attackDistance)
        {
            // 이동 방향을 구한다.
            Vector3 dir = (player.transform.position - transform.position).normalized;

            // 나의 전방 방향을 이동 방향과 일치시킨다.
            transform.forward = dir;

            // 캐릭터 컨트롤러로 이동 방향을 이동한다.
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            // 상태를 공격으로 변경
            enemyState = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");
            anim.SetTrigger("MoveToAttackDelay");

            // 공격 대기 시간을 미리 누적
            currentTime = attackDelayTime;

        }
    }
    void Attack()
    {
        // 만일, 플레이어가 공격 범위 이내라면...
        if (Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
        {

            // 만일, 현재 대기 시간이 공격 대기 시간을 넘어간다면...
            if (currentTime >= attackDelayTime)
            {
                currentTime = 0; // 시간 초기화
                // player를 공격한다. 
                print("공격!");
                anim.SetTrigger("StartAttack");
            }
            else
            {
                // 시간을 누적한다. 
                currentTime += Time.deltaTime;
            }
        }
        else
        {
            // 상태를 이동 상태로 전환시킨다.
            enemyState = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            anim.SetTrigger("AttackToMove");
        }
    }

    // 플레이어에게 데미지를 주는 함수
    public void HitEvent()
    {
        PlayerMove pm = player.GetComponent<PlayerMove>(); // PlayerMove 스크립트 받아오기
        pm.OnDamage(attackPower);
    }

    // 데미지 처리 함수
    public void HitEnemy(int value)
    {
        // 만일, 나의 상태가 피격, 사망 상태일 때는 함수를 종료한다. 
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
        {
            return;
        }

        currentHp -= value;

        // 만일 남은 HP가 0보다 크다면...
        if (currentHp > 0)
        {
            // 상태를 피격 상태로 전환시킨다. 
            enemyState = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");
            anim.SetTrigger("Damaged");
            Damaged();
        }
        // 그렇지 않다면,
        else
        {
            // 상태를 사망 상태로 전환시킨다. 
            enemyState = EnemyState.Die;
            print("상태 전환 : Any state -> Die");
            anim.SetTrigger("Die");
            Die();
        }
    }
    void Damaged()
    {
        // 코루틴 함수를 실행한다.
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        // 2초간 정지한다.
        yield return new WaitForSeconds(1.0f);

        // 상태를 이동 상태로 전환한다. 
        enemyState = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }
    void Die()
    {
        // 기존에 예약된 코루틴들을 모두 종료시킨다.
        StopAllCoroutines();

        // 사망 코루틴을 시작한다.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 컨트롤러를 비활성화한다.
        cc.enabled = false;

        // 2초간 기다렸다가 몸체를 제거한다.(Destroy)
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

}