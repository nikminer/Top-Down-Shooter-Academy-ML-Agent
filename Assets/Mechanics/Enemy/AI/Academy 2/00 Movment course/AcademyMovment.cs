using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;


[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody))]
public class AcademyMovment : Agent
{
    [Header("Agent Parameters")]
    [HideInInspector]
    public Team team;
    [SerializeField]
    public Role role;

    protected Health health;

    [Header("Movment Parameters")]
    [SerializeField]
    float speed = 5f;


    Rigidbody controller;
    AcademyTargetEnviroment env;

    protected virtual void Awake()
    {
        controller = GetComponent<Rigidbody>();
        health = GetComponent<Health>();

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
       // env.AddTeamReward(-0.001f);

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
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
               // env.TargetRemove(other.gameObject, 5f);
                return;
            case "Wall":
                Debug.Log(other.tag);
               // env.AddTeamReward(-10f);
                return;
            case "Knight":
                Debug.Log(other.tag);
                //env.AddTeamReward(-10f);
                return;
            case "Mage":
                Debug.Log(other.tag);
                //env.AddTeamReward(-10f);
                return;
        }
    }
}
