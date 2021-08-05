using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ���� ���� ������ ���
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    // ���� ���º���
    public GameState gState;

    // UI �ؽ�Ʈ ���� ����
    public Text stateLabel;

    // �÷��̾� ���� ������Ʈ ����
    GameObject player;

    // �÷��̾� move ���۳�Ʈ ����
    PlayerMove playerM;

    // �̱���
    public static GameManager gm;

    // �ɼ� �޴� UI ������Ʈ
    public GameObject optionUI;

    private void Awake()
    {
        if (gm == null)
           gm = this;
    }

    void Start()
    {
        // �ʱ� ���� ���´� �غ� ���·� �����Ѵ�.
        gState = GameState.Ready;

        // ���� ���� �ڷ�ƾ �Լ��� �����Ѵ�.
        StartCoroutine(GameStart());

        // �÷��̾� ������Ʈ�� �˻�
        player = GameObject.Find("Player");

        playerM = player.GetComponent<PlayerMove>();


    }
    IEnumerator GameStart()
    {
        // 5�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(6.0f);

        // Ready... ��� ������ ǥ���Ѵ�.
        stateLabel.text = "���̸� ���ؼ� �̷θ� ����������!";

        // Ready ������ ������ ���������� ǥ���Ѵ�.
        stateLabel.color = new Color32(255, 0, 0, 255);

        // 2�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(2.5f);

        // GO! ��� ������ �����Ѵ�. 
        stateLabel.text = "GO!";

        // GO! ������ ������ ���������� ǥ���Ѵ�.
        stateLabel.color = new Color32(255, 0, 0, 255);

        // 0.5�ʰ� ����Ѵ�. 
        yield return new WaitForSeconds(0.5f);

        // Go ������ �����.
        stateLabel.text = "";

        // ���� ���¸� �غ� ���¿��� ���� ���·� ��ȯ�Ѵ�. 
        gState = GameState.Run;
    }

    void Update()
    {
        // ����, �÷��̾� hp�� 0���Ϸ� �������ٸ� badending������ ��ȯ�Ѵ�.
        if (playerM.hp <= 0)
        {
            SceneManager.LoadScene("BadEnding");
        }
    }

    // �ɼ� �޴� �ѱ�
    public void OpenOptionWindow()
    {
        // ���� ���¸� pause�� �����Ѵ�.
        gState = GameState.Pause;

        // �ð��� �����.
        Time.timeScale = 0;

        // �ɼ� �޴� â�� Ȱ��ȭ�Ѵ�.
        optionUI.SetActive(true);
    }

    // �ɼ� �޴� ����(����ϱ�)
    public void CloseOptionWindow()
    {
        // ���� ���¸� run ���·� �����Ѵ�.
        gState = GameState.Run;

        // �ð��� 1��� �ǵ�����.
        Time.timeScale = 1.0f;

        // �ɼ� �޴� â�� ��Ȱ��ȭ�Ѵ�.
        optionUI.SetActive(false);
    }

    // ���� ������ϱ�(���� �� �ٽ� �ε�)
    public void GameRestart()
    {
        // �ð��� 1��� �ǵ�����.
        Time.timeScale = 1.0f;

        // ���� ���� �ٽ� �ε��Ѵ�.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ���� �����ϱ�
    public void GameQuit()
    {
        // ���ø����̼��� �����Ѵ�.
        Application.Quit();
    }
}
