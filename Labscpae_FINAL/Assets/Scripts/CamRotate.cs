using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 회전 속력 변수
    public float rotSpeed = 300.0f;

    // 회전 각도 제한
    public float rotLimit = 60.0f;

    float mx = 0;
    float my = 0;

    void Update()
    {
        // 게임 상태가 게임 중 상태가 아니면 업데이트 함수를 종료 (조작X)
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // 사용자의 마우스 입력을 받아 물체를 회전시키고 싶다.
        // 1. 마우스 입력을 받는다.
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;
        my = Mathf.Clamp(my, -rotLimit, rotLimit);

        // 2. 입력받은 값을 이용해서 회전 방향을 결정.
        //Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0);
        //dir.Normalize();

        // 3. 결정된 회전 방향을 물체의 회전 속성에 대입.
        // R = R0 + vt
        //transform.eulerAngles += dir * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(-my, mx, 0);

        // 4. 회전 값 중에서 x축 값을 -90 -90 도 사이로 제한하고 싶다.
        //Vector3 rot = transform.eulerAngles;
        //rot.x = Mathf.Clamp(rot.x, -90.0f, 90.0f);
        //transform.eulerAngles = rot;
    }
}
