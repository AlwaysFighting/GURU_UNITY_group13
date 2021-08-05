using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 게임 상태 열거형 상수
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    // 게임 상태변수
    public GameState gState;

    // UI 텍스트 변수 선언
    public Text stateLabel;

    // 플레이어 게임 오브젝트 변수
    GameObject player;

    // 플레이어 move 컴퍼넌트 변수
    PlayerMove playerM;

    // 싱글턴
    public static GameManager gm;

    // 옵션 메뉴 UI 오브젝트
    public GameObject optionUI;

    private void Awake()
    {
        if (gm == null)
           gm = this;
    }

    void Start()
    {
        // 초기 게임 상태는 준비 상태로 설정한다.
        gState = GameState.Ready;

        // 게임 시작 코루틴 함수를 실행한다.
        StartCoroutine(GameStart());

        // 플레이어 오브젝트를 검색
        player = GameObject.Find("Player");

        playerM = player.GetComponent<PlayerMove>();


    }
    IEnumerator GameStart()
    {
        // 5초간 대기한다.
        yield return new WaitForSeconds(6.0f);

        // Ready... 라는 문구를 표시한다.
        stateLabel.text = "아이를 구해서 미로를 빠져나가자!";

        // Ready 문구의 색상을 빨간색으로 표시한다.
        stateLabel.color = new Color32(255, 0, 0, 255);

        // 2초간 대기한다.
        yield return new WaitForSeconds(2.5f);

        // GO! 라는 문구로 변경한다. 
        stateLabel.text = "GO!";

        // GO! 문구의 색상을 빨간색으로 표시한다.
        stateLabel.color = new Color32(255, 0, 0, 255);

        // 0.5초간 대기한다. 
        yield return new WaitForSeconds(0.5f);

        // Go 문구를 지운다.
        stateLabel.text = "";

        // 게임 상태를 준비 상태에서 실행 상태로 전환한다. 
        gState = GameState.Run;
    }

    void Update()
    {
        // 만일, 플레이어 hp가 0이하로 떨어진다면 badending씬으로 전환한다.
        if (playerM.hp <= 0)
        {
            SceneManager.LoadScene("BadEnding");
        }
    }

    // 옵션 메뉴 켜기
    public void OpenOptionWindow()
    {
        // 게임 상태를 pause로 변경한다.
        gState = GameState.Pause;

        // 시간을 멈춘다.
        Time.timeScale = 0;

        // 옵션 메뉴 창을 활성화한다.
        optionUI.SetActive(true);
    }

    // 옵션 메뉴 끄기(계속하기)
    public void CloseOptionWindow()
    {
        // 게임 상태를 run 상태로 변경한다.
        gState = GameState.Run;

        // 시간을 1배로 되돌린다.
        Time.timeScale = 1.0f;

        // 옵션 메뉴 창을 비활성화한다.
        optionUI.SetActive(false);
    }

    // 게임 재시작하기(현재 씬 다시 로드)
    public void GameRestart()
    {
        // 시간을 1배로 되돌린다.
        Time.timeScale = 1.0f;

        // 현재 씬을 다시 로드한다.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 게임 종료하기
    public void GameQuit()
    {
        // 어플리케이션을 종료한다.
        Application.Quit();
    }
}
