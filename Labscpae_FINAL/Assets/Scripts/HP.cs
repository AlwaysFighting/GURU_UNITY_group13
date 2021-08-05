using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    // 오디오 소스 컴포넌트 변수
    AudioSource HP_Increase;

    // 플레이어 게임 오브젝트
    GameObject player;

    void Start()
    {
        // player 검색
        player = GameObject.Find("Player");

        // 오디오 소스 컴포넌트를 가져오기
        HP_Increase = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            // 아이템이 사라지게 한다.
            Destroy(gameObject, 1);

            // 외부 PlayerMove 에서 HP 증가 함수 호출
            PlayerMove HP = player.GetComponent<PlayerMove>();
            HP.heal();

            // 효과음 재생
            HP_Increase.Play();
        }
    }
}