using System.Linq;
using UnityEngine;

// ��������� ��� ������� ������ ��������� ����������
[RequireComponent(typeof(Health))]
public class Target : MonoBehaviour
{
    // ����������� ����������
    protected Health health;
    AcademyTargetEnviroment env;

    // �������� ������������� �������
    [SerializeField]
    Team team;

    protected virtual void Awake()
    {
        // �������������� �������� ����������� �����������
        health = GetComponent<Health>();
        // ������������� �� ����������� � ������� ������ ������
        health.OnDeath += Agent_Death;
        // �������� ��������� ����� �� ������������ ��������
        env = GetComponentInParent<AcademyTargetEnviroment>();
    }

    // ������� ������� ����������� � ������ ������ ������
    private void Agent_Death()
    {
        // ���� ������������ ������ �������, �� ����������� �����
        if (env.Agents.First().GetComponent<AgentBase>().team == team )
        {
            env.AddEnemyReward(-10f);
        }
        else
        {
            // ����� �������������
            env.AddEnemyReward(10f);
        }
        // ������� ������ �� �����
        env.TargetRemove(gameObject);
    }

    // ������� �������������� �������� ������
    private void OnTriggerEnter(Collider other)
    {
        AgentBase agent;

        switch (other.tag)
        {
            case "Weapon":
                // ���� ������ ����������� � ������� ����� �������� ���
                Weapon weapon = other.GetComponent<Weapon>();
                if (weapon.IsAttack)
                {
                    // ��������� ���-�� HP � ������
                    health.AddHealth(-weapon.Damage);
                    // �������� ������, ������� ������� ������
                    agent = weapon.GetComponentInParent<AgentBase>();
                    if (agent.team == team)
                    {
                        // ���� ����������� ������, �� �������� ��������
                        agent.AddReward(-10f);
                    }
                    else
                    {
                        // ����� ������������� ����������
                        agent.AddReward(5f);
                    }
                }
                return;
            case "EnemyProjectile":
            case "Projectile":
                // ���� ������ ����������� � ����������� ��������
                Projectile projectile = other.GetComponent<Projectile>();
                // ��������� ���-�� HP � ������
                health.AddHealth(-projectile.damage);

                if (projectile.agent.team == team)
                {
                    // ���� ����������� ������, �� �������� ��������
                    projectile.agent.AddReward(-25f - projectile.curDistance * 5);
                }
                else
                {
                    // ����� ������������� ����������
                    projectile.agent.AddReward(10f + projectile.curDistance * 5);
                }
                return;
        }
    }
}
