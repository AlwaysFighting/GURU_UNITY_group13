using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // ���콺 ��Ŭ���� �ϸ�. �ü� �������� ����ź�� �߻��ϰڴ�.
    // �ʿ� ��� : �߻��� ��ġ, �߻��� ��, ����ź ������Ʈ

    // ����ź ������Ʈ
    public GameObject bombFactory;

    // �߻��� ��ġ
    public Transform firePosition;

    // �߻��� ��
    public float throwPower = 10.0f;

    // �Ѿ� ����Ʈ ���� ������Ʈ
    public GameObject bulletEffect;

    // ��ƼŬ �ý��� ����
    ParticleSystem ps;

    // ����� �ҽ� ������Ʈ ����
    AudioSource aSource;

    // �Ѿ� ���ݷ�
    public int attackPower = 2;

    // �ִϸ����� ������Ʈ
    Animator anim;

    // ���� ��� ���
    enum WeaponMode
    {
        Normal
    }

    WeaponMode wMode;

    // ���� ��� �ؽ�Ʈ
    public Text weaponText;

    // �ѱ� ����Ʈ �迭
    public GameObject[] eff_Flash;

    void Start()
    {
        // ��ƼŬ �ý��� ������Ʈ�� ��������
        ps = bulletEffect.GetComponent<ParticleSystem>();

        // ����� �ҽ� ������Ʈ�� ��������
        aSource = GetComponent<AudioSource>();

        // �ڽ� ������Ʈ���� �ִϸ����� ��������
        anim = GetComponentInChildren<Animator>();

        // �⺻ ���� ���� �Ϲ� ���
        wMode = WeaponMode.Normal;
        weaponText.text = "Normal";
    }
    
    void Update()
    {
        // ���� ���°� ���� �� ���°� �ƴϸ� ������Ʈ �Լ��� ���� (����X)
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        // ����, ���콺 ��Ŭ���� �Ѵٸ�...
        if (Input.GetMouseButtonDown(0))
        {
            // 1. ���̸� ����
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 2. ���̿� �ε��� ����� ������ ������ ����
            RaycastHit hitInfo = new RaycastHit();

            // 3. ���̸� �߻��ؼ� �ε��� ����� �ִٸ�...
            if(Physics.Raycast(ray, out hitInfo))
            {
                // ����, �ε��� ����� ���̾ enemy���..
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(attackPower);
                }

                // �ε��� ��ġ�� �Ѿ� ����Ʈ ������Ʈ�� ��ġ��Ų��.
                bulletEffect.transform.position = hitInfo.point;

                // �Ѿ� ����Ʈ�� ������ �ε��� ������Ʈ�� ǥ���� ���� ����(��� ����) �� ��ġ��Ų��.
                bulletEffect.transform.forward = hitInfo.normal;

                // �Ѿ� ����Ʈ�� �÷����Ѵ�.
                ps.Play();
            }

            // �� �Ҹ��� �÷����Ѵ�
            aSource.Play();

            // ����, ��Ʈ Ʈ���� MoveDirection �Ķ������ ���� 0 �϶�...
            if(anim.GetFloat("MoveDirection") == 0)
            {
                // �� �߻� �ִϸ��̼��� �÷����Ѵ�.
                anim.SetTrigger("Attack");
            }

            // �ѱ� ����Ʈ �ڷ�ƾ �Լ��� �����Ѵ�.
            StartCoroutine(ShootEffect(0.1f));
        }
        
        
        // ����, ���콺 ��Ŭ���� �Ѵٸ�...
        if(Input.GetMouseButtonDown(1))
        {
            // ����, ���� ��尡 ��� ����� ����ź�� ��ô
            switch(wMode)
            {
                case WeaponMode.Normal:
                    // ����ź�� �����Ѵ�.
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.position;

                    // ����ź�� ������ �ٵ� ������Ʈ�� �޾ƿ��ڴ�.
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    // �ü� �������� �߻�
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    break;
            }
        }
    }

    // �ѱ� ����Ʈ �ڷ�ƾ �Լ�
    IEnumerator ShootEffect(float duration)
    {
        // �ټ����� ����Ʈ ������Ʈ �߿��� �����ϰ� 1���� ����.
        int num = Random.Range(0, eff_Flash.Length - 1);

        // ���õ� ������Ʈ�� Ȱ��ȭ��Ų��.
        eff_Flash[num].SetActive(true);

        // �����ð�(duration)���� ��ٸ���.
        yield return new WaitForSeconds(duration);

        // Ȱ��ȭ�� ������Ʈ�� �ٽ� ��Ȱ��ȭ ��Ų��.
        eff_Flash[num].SetActive(false);

    }
}
