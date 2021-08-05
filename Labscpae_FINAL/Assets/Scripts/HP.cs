using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    // ����� �ҽ� ������Ʈ ����
    AudioSource HP_Increase;

    // �÷��̾� ���� ������Ʈ
    GameObject player;

    void Start()
    {
        // player �˻�
        player = GameObject.Find("Player");

        // ����� �ҽ� ������Ʈ�� ��������
        HP_Increase = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            // �������� ������� �Ѵ�.
            Destroy(gameObject, 1);

            // �ܺ� PlayerMove ���� HP ���� �Լ� ȣ��
            PlayerMove HP = player.GetComponent<PlayerMove>();
            HP.heal();

            // ȿ���� ���
            HP_Increase.Play();
        }
    }
}