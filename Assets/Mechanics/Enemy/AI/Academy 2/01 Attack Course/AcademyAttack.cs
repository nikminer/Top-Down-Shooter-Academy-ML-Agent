using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;


[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class AcademyAttack : Agent
{
    [Header("Agent Parameters")]
    [HideInInspector]
    public Team team;
    [SerializeField]
    public Role role;

    [Header("Movment Parameters")]
    [SerializeField]
    float speed = 5f;

    [Header("Mage Parameters")]
    [SerializeField]
    protected Projectile projectile;
    [SerializeField]
    protected GameObject stick;

    protected Animator animator;
    protected Health health;

    Rigidbody controller;
    AcademyTargetEnviroment env;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        controller = GetComponent<Rigidbody>();

        var behaviorParameters = gameObject.GetComponent<BehaviorParameters>();
        if (behaviorParameters.TeamId == (int)Team.Blue)
        {
            team = Team.Blue;
        }
        else
        {
            team = Team.Red;
        }
        env = GetComponentInParent<AcademyTargetEnviroment>();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Move(actionBuffers.DiscreteActions[(int)ActionsEnum.Move]);
        Rotate(actionBuffers.DiscreteActions[(int)ActionsEnum.Rotate]);
        Attack(actionBuffers.DiscreteActions[(int)ActionsEnum.Attack]);
    }

    void Move(int act)
    {
        //env.AddTeamReward(-0.001f);

        Vector3 movmentVector = Vector3.zero;
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

        controller.AddForce(movmentVector * speed,
            ForceMode.Impulse);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((int)role);
        sensor.AddObservation((int)team);
        sensor.AddObservation(health.StateHealth());
    }

    void Rotate(int act)
    {
        //env.AddTeamReward(-0.001f);

        Vector3 rotateDir = Vector3.zero;
        switch (act)
        {
            case (int)RotationEnum.left:
                rotateDir = transform.up;
                break;
            case (int)RotationEnum.right:
                rotateDir = -transform.up * 1f;
                break;
            default:
                return;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 100f);
    }


    protected virtual void Attack(int act)
    {
        switch (act)
        {
            case (int)AttackEnum.attack:
                animator.SetTrigger("Attack");
                //env.AddTeamReward(-1f);
                break;
        }
    }

    void CastProjectile()
    {
        Instantiate(projectile, stick.transform.position, transform.localRotation, transform);
    }
}
