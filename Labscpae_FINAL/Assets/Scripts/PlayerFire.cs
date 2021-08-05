using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // 마우스 우클릭을 하면. 시선 방향으로 수류탄을 발사하겠다.
    // 필요 요소 : 발사할 위치, 발사할 힘, 수류탄 오브젝트

    // 수류탄 오브젝트
    public GameObject bombFactory;

    // 발사할 위치
    public Transform firePosition;

    // 발사할 힘
    public float throwPower = 10.0f;

    // 총알 이펙트 게임 오브젝트
    public GameObject bulletEffect;

    // 파티클 시스템 변수
    ParticleSystem ps;

    // 오디오 소스 컴포넌트 변수
    AudioSource aSource;

    // 총알 공격력
    public int attackPower = 2;

    // 애니메이터 컴포넌트
    Animator anim;

    // 게임 모드 상수
    enum WeaponMode
    {
        Normal
    }

    WeaponMode wMode;

    // 무기 모드 텍스트
    public Text weaponText;

    // 총구 이펙트 배열
    public GameObject[] eff_Flash;

    void Start()
    {
        // 파티클 시스템 컴포넌트를 가져오기
        ps = bulletEffect.GetComponent<ParticleSystem>();

        // 오디오 소스 컴포넌트를 가져오기
        aSource = GetComponent<AudioSource>();

        // 자식 오브젝트에서 애니메이터 가져오기
        anim = GetComponentInChildren<Animator>();

        // 기본 무기 모드는 일반 모드
        wMode = WeaponMode.Normal;
        weaponText.text = "Normal";
    }
    
    void Update()
    {
        // 게임 상태가 게임 중 상태가 아니면 업데이트 함수를 종료 (조작X)
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        // 만일, 마우스 좌클릭을 한다면...
        if (Input.GetMouseButtonDown(0))
        {
            // 1. 레이를 생성
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 2. 레이에 부딪힌 대상의 정보를 저장할 변수
            RaycastHit hitInfo = new RaycastHit();

            // 3. 레이를 발사해서 부딪힌 대상이 있다면...
            if(Physics.Raycast(ray, out hitInfo))
            {
                // 만일, 부딪힌 대상의 레이어가 enemy라면..
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(attackPower);
                }

                // 부딪힌 위치에 총알 이펙트 오브젝트를 위치시킨다.
                bulletEffect.transform.position = hitInfo.point;

                // 총알 이펙트의 방향을 부딪힌 오브젝트의 표면의 수직 방향(노멀 벡터) 과 일치시킨다.
                bulletEffect.transform.forward = hitInfo.normal;

                // 총알 이펙트를 플레이한다.
                ps.Play();
            }

            // 총 소리를 플레이한다
            aSource.Play();

            // 만약, 블렌트 트리의 MoveDirection 파라메터의 값이 0 일때...
            if(anim.GetFloat("MoveDirection") == 0)
            {
                // 총 발사 애니메이션을 플레이한다.
                anim.SetTrigger("Attack");
            }

            // 총구 이펙트 코루틴 함수를 실행한다.
            StartCoroutine(ShootEffect(0.1f));
        }
        
        
        // 만일, 마우스 우클릭을 한다면...
        if(Input.GetMouseButtonDown(1))
        {
            // 만일, 무기 모드가 노멀 모드라면 수류탄을 투척
            switch(wMode)
            {
                case WeaponMode.Normal:
                    // 수류탄을 생성한다.
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.position;

                    // 수류탄의 리지드 바디 컴포넌트를 받아오겠다.
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    // 시선 방향으로 발사
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
            }
        }
    }

    // 총구 이펙트 코루틴 함수
    IEnumerator ShootEffect(float duration)
    {
        // 다섯개의 이펙트 오브젝트 중에서 랜덤하게 1개를 고른다.
        int num = Random.Range(0, eff_Flash.Length - 1);

        // 선택된 오브젝트를 활성화시킨다.
        eff_Flash[num].SetActive(true);

        // 일정시간(duration)동안 기다린다.
        yield return new WaitForSeconds(duration);

        // 활성화된 오브젝트를 다시 비활성화 시킨다.
        eff_Flash[num].SetActive(false);

    }
}
