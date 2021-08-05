using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState // enum : ������ �Լ���, ���� ������ �� ���� ���� ���
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState enemyState;

    // �÷��̾� ���� ������Ʈ
    GameObject player;

    // ���� ���� 
    public float findDistance = 8.0f;

    // ���� ĳ���� ��Ʈ�ѷ�
    CharacterController cc;

    // �̵� �ӵ�
    public float moveSpeed = 4.0f;

    // ���� ���� ����
    public float attackDistance = 8.0f;

    // ���� ���� �ð� ����
    float currentTime = 0;

    // ���� ������ �ð�
    public float attackDelayTime = 2.0f;

    // ���ݷ� ����
    public int attackPower = 2;

    // �ʱ� ��ġ�� ȸ�� ����� ����
    Vector3 originPos;
    Quaternion originRot;

    // �̵� ������ �Ÿ�
    public float moveDistance = 10.0f;

    // �ִ� ü�� ����
    public int maxHp = 5;

    // ���� ü�� ����
    int currentHp;

    // �����̴� ����
    public Slider hpSlider;

    // �ִϸ����� ������Ʈ ����
    Animator anim;


    void Start()
    {
        // �ʱ� ���´� ��� ����(Idle)
        enemyState = EnemyState.Idle;

        // player �˻�
        player = GameObject.Find("Player");

        // ĳ���� ��Ʈ�ѷ� �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ʱ� ��ġ�� ȸ�� �����ϱ�
        originPos = transform.position;
        originRot = transform.rotation;

        // ���� ü�� ����
        currentHp = maxHp;

        // �ڽ� ������Ʈ�� �ִϸ����� ������Ʈ�� ��������
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // ���� üũ�� switch��
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        // hp �����̴� ���� ü�� ������ �����Ѵ�.
        hpSlider.value = (float)currentHp / (float)maxHp;
    }

    // ��� �ൿ �Լ�
    void Idle()
    {
        // ���� player ���� �Ÿ��� ���� ���� �̳����...
        if (Vector3.Distance(player.transform.position, transform.position) <= findDistance)
        {
            // ���¸� �̵� ���·� �����Ѵ�. 
            enemyState = EnemyState.Move;
            print("���� ��ȯ : Idle -> Move");
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        // ���� �̵� �Ÿ� ���̶��...
        if (Vector3.Distance(originPos, transform.position) > moveDistance)
        {
            // ���¸� ���� ���·�..
            enemyState = EnemyState.Return;
            print("���� ��ȯ : Move -> Return");
        }

        // ���� ���� ���� ���̶��..
        if (Vector3.Distance(player.transform.position, transform.position) > attackDistance)
        {
            // �̵� ������ ���Ѵ�.
            Vector3 dir = (player.transform.position - transform.position).normalized;

            // ���� ���� ������ �̵� ����� ��ġ��Ų��.
            transform.forward = dir;

            // ĳ���� ��Ʈ�ѷ��� �̵� ������ �̵��Ѵ�.
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            // ���¸� �������� ����
            enemyState = EnemyState.Attack;
            print("���� ��ȯ : Move -> Attack");
            anim.SetTrigger("MoveToAttackDelay");

            // ���� ��� �ð��� �̸� ����
            currentTime = attackDelayTime;

        }
    }
    void Attack()
    {
        // ����, �÷��̾ ���� ���� �̳����...
        if (Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
        {

            // ����, ���� ��� �ð��� ���� ��� �ð��� �Ѿ�ٸ�...
            if (currentTime >= attackDelayTime)
            {
                currentTime = 0; // �ð� �ʱ�ȭ
                // player�� �����Ѵ�. 
                print("����!");
                anim.SetTrigger("StartAttack");
            }
            else
            {
                // �ð��� �����Ѵ�. 
                currentTime += Time.deltaTime;
            }
        }
        else
        {
            // ���¸� �̵� ���·� ��ȯ��Ų��.
            enemyState = EnemyState.Move;
            print("���� ��ȯ : Attack -> Move");
            anim.SetTrigger("AttackToMove");
        }
    }

    // �÷��̾�� �������� �ִ� �Լ�
    public void HitEvent()
    {
        PlayerMove pm = player.GetComponent<PlayerMove>(); // PlayerMove ��ũ��Ʈ �޾ƿ���
        pm.OnDamage(attackPower);
    }

    void Return()
    {
        // ����, ���� ��ġ�� �������� �ʾҴٸ�, �� �������� �̵��Ѵ�

        Vector3 dist = originPos - transform.position;
        dist.y = 0;

        //if (Vector3.Distance(originPos, transform.position) > 0.1f)
        if (dist.magnitude > 0.1f)
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            Vector3 dir = dist.normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.forward = dir;
        }
        // ���� ��ġ�� �����ϸ�, ��� ���·� ��ȯ�Ѵ�. 
        else
        {
            transform.position = originPos;
            transform.rotation = originRot;

            enemyState = EnemyState.Idle;
            print("���� ��ȯ : Return -> Idle");
            anim.SetTrigger("MoveToIdle");


            // ü���� �ִ�ġ�� ȸ���Ѵ�.
            currentHp = maxHp;
        }
    }

    // ������ ó�� �Լ�
    public void HitEnemy(int value)
    {
        // ����, ���� ���°� �ǰ�, ����, ��� ������ ���� �Լ��� �����Ѵ�. 
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Return
            || enemyState == EnemyState.Die)
        {
            return;
        }

        currentHp -= value;

        // ���� ���� HP�� 0���� ũ�ٸ�...
        if (currentHp > 0)
        {
            // ���¸� �ǰ� ���·� ��ȯ��Ų��. 
            enemyState = EnemyState.Damaged;
            print("���� ��ȯ: Any state -> Damaged");
            anim.SetTrigger("Damaged");
            Damaged();
        }
        // �׷��� �ʴٸ�,
        else
        {
            // ���¸� ��� ���·� ��ȯ��Ų��. 
            enemyState = EnemyState.Die;
            print("���� ��ȯ : Any state -> Die");
            anim.SetTrigger("Die");
            Die();
        }
    }
    void Damaged()
    {
        // �ڷ�ƾ �Լ��� �����Ѵ�.
        StartCoroutine(DamageProcess());
    }

    IEnumerator DamageProcess()
    {
        // 2�ʰ� �����Ѵ�.
        yield return new WaitForSeconds(1.0f);

        // ���¸� �̵� ���·� ��ȯ�Ѵ�. 
        enemyState = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
    }
    void Die()
    {
        // ������ ����� �ڷ�ƾ���� ��� �����Ų��.
        StopAllCoroutines();

        // ��� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // ĳ���� ��Ʈ�ѷ��� ��Ȱ��ȭ�Ѵ�.
        cc.enabled = false;

        // 2�ʰ� ��ٷȴٰ� ��ü�� �����Ѵ�.(Destroy)
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

}