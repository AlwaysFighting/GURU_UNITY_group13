using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {

    }
    
    // BadEnding씬에서 다시하기 버튼을 눌렀을 때 MainScene이 로드되게 하기
    public void GameRestart_BE()
    {
        SceneManager.LoadScene("MainScene");
    }

    // BadEnding씬에서 종료하기 버튼을 누르면 게임이 종료되게 하기
    // 게임 종료하기
    public void GameQuit_BE()
    {
        // 어플리케이션을 종료한다.
        Application.Quit();
    }
}
