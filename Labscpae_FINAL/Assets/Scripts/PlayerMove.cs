using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // �ӷ� ����
    public float moveSpeed = 7.0f;

    // ĳ���Ϳ��� �߷�(���� �̵�)�� �����ϰ� �ʹ�.

    // �߷º���
    public float gravitiy = -20.0f;

    // ������
    public float jumpPower = 10;

    // �ִ� ���� Ƚ�� 
    public int maxJump = 2;

    // ���� ���� Ƚ��
    int jumpCount = 0;

    // ���� �ӵ� ���� 
    float yVelocity = 0;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    // ü�� ����
    public int hp = 50;

    // �ִ� ü��
    public int maxHp = 50;

    // �����̴� UI
    public Slider hpSlider;

    // ����Ʈ UI ������Ʈ
    public GameObject hitEffect;

    // �ִϸ����� ������Ʈ ����
    Animator anim;

    // ����� �ҽ� ������Ʈ ����
    AudioSource theAudio;

    // HP �ؽ�Ʈ ����
    public Text hpText;

    private static int HPValue;

    void Start()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ�� �޾ƿ´�.
        cc = GetComponent<CharacterController>();

        // ü�� ���� �ʱ�ȭ
        hp = maxHp;

        // �ڽ� ������Ʈ�� �ִϸ����� ������Ʈ�� �����´�.
        anim = GetComponentInChildren<Animator>();

        // ����� �ҽ� ������Ʈ�� ��������
        theAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // �����̴� value �� ü�� ������ �����Ѵ�. 
        hpSlider.value = (float)hp / (float)maxHp;

        // ���� ���°� ���� �� ���°� �ƴϸ� ������Ʈ �Լ��� ���� (����X)
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �̵� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        // �̵� ���� ������ ũ�� ���� �ִϸ������� �̵� ���� Ʈ���� �����Ѵ�.
        anim.SetFloat("MoveDirection", dir.magnitude);

        // �̵� ���� (���� ��ǥ)�� ī�޶��� ������ �������� (���� ��ǥ) ��ȯ�Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);

        // ����, �÷��̾� ���� �����Ͽ��ٸ� ���� ���� Ƚ���� 0���� �ʱ�ȭ�Ѵ�. 
        //characterController.collisionFlags : ĳ���� ��Ʈ�ѷ� �浹ü�� �浹 ���� üũ
        // ���� �ӵ� ��(�߷�)�� �ٽ� 0���� �ʱ�ȭ�Ѵ�. 

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            jumpCount = 0;
            yVelocity = 0;
        }

        // ����, ���� Ű�� �����ٸ�, �������� �����ӵ��� �����Ѵ�. (�߷��� �ݴ�Ǵ� ��)
        // ��, ���� ���� Ƚ���� �ִ� ���� Ƚ���� �Ѿ�� �ʾƾ� �Ѵ�. 
        if (Input.GetButtonDown("Jump") && jumpCount < maxJump)
        {
            jumpCount++;
            yVelocity = jumpPower;
        }

        // ĳ������ �����ӵ�(�߷�)�� �����Ѵ�. 
        yVelocity += gravitiy * Time.deltaTime; // ��� �߷� ���.
        dir.y = yVelocity;

        // �̵� �������� �÷��̾ �̵���Ų��.
        //transform.position += dir * moveSpeed * Time.deltaTime;
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    // �÷��̾� �ǰ� �Լ�
    public void OnDamage(int value)
    {
        hp -= value;
        HPValue = hp - value;
        hpText.text = "  " + HPValue;

        if (hp < 0)
        {
            hp = 0;
        }
        // hp �� 0 ���� ū ��쿡�� �ǰ� ����Ʈ �ڷ�ƾ ����
        else
        {
            StartCoroutine(HitEffect());
        }
    }

    IEnumerator HitEffect()
    {
        // 1. ����Ʈ�� �Ҵ�.(Ȱ��ȭ��Ų��.)
        hitEffect.SetActive(true);

        // 2. 0.3�ʸ� ��ٸ���.
        yield return new WaitForSeconds(0.3f);

        // 3. ����Ʈ�� ��Ȱ��ȭ��Ų��.
        hitEffect.SetActive(false);
    }

    // �����ۿ� �ε����� �������� ������� �ϰ� �ʹ�.
    // �������� ������鼭 hp�� +20 ��ŭ ȸ���ǰ� �ϰ�ʹ�.
    // �����ۿ� �ε��� �� ȿ������ ���� �ʹ�.

    /*private void OnCollisionEnter(Collision other)
    {
        
            // �������� ������� �Ѵ�.
            Destroy(other.gameObject);

            // hp 20��ŭ ȸ��
            heal();

            // ȿ���� ���
            theAudio.Play();
        
    }*/

    // hp ȸ�� �Լ�
    public void heal()
    {
        if(hp <= 30)
        {
            hp += 20;
            print("HP ����!");
            hpText.text = "  " + hp;
        }
        else
        {
            hp = maxHp;
            print("HP ����!");
            hpText.text = "  " + hp;
        }
        
    }
}