using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 속력 변수
    public float moveSpeed = 7.0f;

    // 캐릭터에게 중력(수직 이동)을 적용하고 싶다.

    // 중력변수
    public float gravitiy = -20.0f;

    // 점프력
    public float jumpPower = 10;

    // 최대 점프 횟수 
    public int maxJump = 2;

    // 현재 점프 횟수
    int jumpCount = 0;

    // 수직 속도 변수 
    float yVelocity = 0;

    // 캐릭터 컨트롤러 변수
    CharacterController cc;

    // 체력 변수
    public int hp = 50;

    // 최대 체력
    public int maxHp = 50;

    // 슬라이더 UI
    public Slider hpSlider;

    // 이펙트 UI 오브젝트
    public GameObject hitEffect;

    // 애니메이터 컴포넌트 변수
    Animator anim;

    // 오디오 소스 컴포넌트 변수
    AudioSource theAudio;

    // HP 텍스트 변수
    public Text hpText;

    private static int HPValue;

    void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트를 받아온다.
        cc = GetComponent<CharacterController>();

        // 체력 변수 초기화
        hp = maxHp;

        // 자식 오브젝트의 애니메이터 컴포넌트를 가져온다.
        anim = GetComponentInChildren<Animator>();

        // 오디오 소스 컴포넌트를 가져오기
        theAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 슬라이더 value 를 체력 비율로 적용한다. 
        hpSlider.value = (float)hp / (float)maxHp;

        // 게임 상태가 게임 중 상태가 아니면 업데이트 함수를 종료 (조작X)
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // 이동 방향 벡터의 크기 값을 애니메이터의 이동 블렌드 트리에 전달한다.
        anim.SetFloat("MoveDirection", dir.magnitude);

        // 이동 방향 (월드 좌표)을 카메라의 방향을 기준으로 (로컬 좌표) 전환한다.
        dir = Camera.main.transform.TransformDirection(dir);

        // 만일, 플레이억 땅에 착지하였다면 현재 점프 횟수를 0으로 초기화한다. 
        //characterController.collisionFlags : 캐릭터 컨트롤러 충돌체의 충돌 부위 체크
        // 수직 속도 값(중력)을 다시 0으로 초기화한다. 

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            jumpCount = 0;
            yVelocity = 0;
        }

        // 만일, 점프 키를 누른다면, 점프력을 수직속도로 적용한다. (중력의 반대되는 힘)
        // 단, 현재 점프 횟수가 최대 점프 횟수를 넘어가지 않아야 한다. 
        if (Input.GetButtonDown("Jump") && jumpCount < maxJump)
        {
            jumpCount++;
            yVelocity = jumpPower;
        }

        // 캐릭터의 수직속도(중력)을 적용한다. 
        yVelocity += gravitiy * Time.deltaTime; // 계속 중력 계산.
        dir.y = yVelocity;

        // 이동 방향으로 플레이어를 이동시킨다.
        //transform.position += dir * moveSpeed * Time.deltaTime;
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // 플레이어 피격 함수
    public void OnDamage(int value)
    {
        hp -= value;
        HPValue = hp - value;
        hpText.text = "  " + HPValue;

        if (hp < 0)
        {
            hp = 0;
        }
        // hp 가 0 보다 큰 경우에는 피격 이펙트 코루틴 실행
        else
        {
            StartCoroutine(HitEffect());
        }
    }

    IEnumerator HitEffect()
    {
        // 1. 이펙트를 켠다.(활성화시킨다.)
        hitEffect.SetActive(true);

        // 2. 0.3초를 기다린다.
        yield return new WaitForSeconds(0.3f);

        // 3. 이펙트를 비활성화시킨다.
        hitEffect.SetActive(false);
    }

    // 아이템에 부딪히면 아이템이 사라지게 하고 싶다.
    // 아이템이 사라지면서 hp가 +20 만큼 회복되게 하고싶다.
    // 아이템에 부딪힐 때 효과음을 내고 싶다.

    /*private void OnCollisionEnter(Collision other)
    {
        
            // 아이템이 사라지게 한다.
            Destroy(other.gameObject);

            // hp 20만큼 회복
            heal();

            // 효과음 재생
            theAudio.Play();
        
    }*/

    // hp 회복 함수
    public void heal()
    {
        if(hp <= 30)
        {
            hp += 20;
            print("HP 증가!");
            hpText.text = "  " + hp;
        }
        else
        {
            hp = maxHp;
            print("HP 증가!");
            hpText.text = "  " + hp;
        }
        
    }
}