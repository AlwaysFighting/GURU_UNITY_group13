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
    
    // BadEnding������ �ٽ��ϱ� ��ư�� ������ �� MainScene�� �ε�ǰ� �ϱ�
    public void GameRestart_BE()
    {
        SceneManager.LoadScene("MainScene");
    }

    // BadEnding������ �����ϱ� ��ư�� ������ ������ ����ǰ� �ϱ�
    // ���� �����ϱ�
    public void GameQuit_BE()
    {
        // ���ø����̼��� �����Ѵ�.
        Application.Quit();
    }
}
