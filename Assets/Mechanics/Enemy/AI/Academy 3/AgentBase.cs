using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

// ��������� ��� ������� ������ ��������� ����������
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class AgentBase : Agent
{
    // ����� �������� �������� ������
    [Header("Movment Parameters")]
    [SerializeField]
    float speed = 5f;

    // �������� ������� ����������
    protected Animator animator;
    protected Health health;
    protected Rigidbody controller;

    // �������� ������������� �������
    public Team team;
   
    protected virtual void Awake()
    {
        // �������������� �������� ����������� �����������
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        controller = GetComponent<Rigidbody>();

        // ������������� �� ����������� � ������� ������ ������
        health.OnDeath += Agent_Death;
    }

    // ������� ������� ����������� � ������ ������ ������, 
    // � ������� �������� ��� ������� ����������� ������� �� �����
    protected virtual void Agent_Death()
    {
        Destroy(gameObject);
    }

    // ������������� ������� ����� ������� ����������
    public override void CollectObservations(VectorSensor sensor)
    {
        // ������� ������� ��������� �������� ������, ����� ������������� �������
        sensor.AddObservation(health.StateHealth());
        sensor.AddObservation((int)team);
    }

    // ������������� ������� ��������� �������� ��������
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // �������� ���������� �������� ��� ��������
        Move(actionBuffers.DiscreteActions[0]);
        // �������� ��������������� �������� ��� ��������
        Rotate(actionBuffers.ContinuousActions[0]);
        // �������� ���������� �������� ��� �����
        Attack(actionBuffers.DiscreteActions[1]);
    }

    // ������� ������������� �� �������� ������
    protected virtual void Move(int act)
    {
        Vector3 movmentVector = Vector3.zero;
        // ������ Switch ������� ����������� �������� �� 1 �� 4 
        // � �������� ���� ��������� ������, �������� ������ ��������
        switch (act)
        {
            case (int)MovementEnum.forward:
                movmentVector = transform.forward;
                break;
            case (int)MovementEnum.backward:
                movmentVector = -transform.forward;
                break;
            case (int)MovementEnum.right:
                movmentVector = transform.right;
                break;
            case (int)MovementEnum.left:
                movmentVector = -transform.right;
                break;
            default:
                return;
        }
        // ��������� ���������� ��������� ������ ������� ������ 
        // �� ��������������� ������� �������� ���������� �� �������� ������
        controller.AddForce(movmentVector * speed,
            ForceMode.Impulse);
    }
    // ������� ������������� �� ������� ������
    protected virtual void Rotate(float act)
    {
        // ������ ������� �������� �������� �� -1 �� +1 � 10 ���
        Vector3 rotateDir = Vector3.zero;
        rotateDir.y += act * 10;
        // ������������� ������
        transform.Rotate(rotateDir);
    }

    // ������� ������������ �� ����� ������
    protected virtual void Attack(int act)
    {
        switch (act)
        {
            // ���� �������� �� ��������� �������� �����, �� ��������� �������� �����
            case (int)AttackEnum.attack:
                animator.SetTrigger("Attack");
                break;
        }
    }
}
