using System;
using UnityEngine;

// ��������� ������� ����� ������
public class AcademyDuelKnight : AgentBase
{
    // ������������� ����� �����������
    protected override void Move(int act)
    {
        // ����� �� �����������, ����� ��������� ���� ������
        this.AddReward(-0.001f);
        base.Move(act);
    }

    // ������������� ����� ��������
    protected override void Rotate(float act)
    {
        // ����� �� �������, ����� ��������� ���� ������
        this.AddReward(-0.001f);
        base.Rotate(act);
    }

    // ������������� ����� �����
    protected override void Attack(int act)
    {
        switch (act)
        {
            case (int)AttackEnum.attack:
                animator.SetTrigger("Attack");
                // ����� �� �����, ����� ��������� ��������������� ������
                this.AddReward(-1f);
                break;
        }
    }

    // ����������� ����� ������ ������
    protected override void Agent_Death()
    {
        // �������� �������� ����� � ������� ���������� �����
        AcademyDuelEnveroment academyDuel = GetComponentInParent<AcademyDuelEnveroment>();
        // ����������� ������� ����� ������� �� ����������� ������
        academyDuel.AddEnemyReward(team, 10f);
        academyDuel.Unregister(this);
        // ��������� ������
        this.EndEpisode();
        // �������� ����� �� �������� ������
        base.Agent_Death();
    }

    // ������� �������������� �������� ������
    private void OnTriggerEnter(Collider other)
    {
        AgentBase agent;
        Projectile projectile;
        int damage;
        switch (other.tag)
        {
            case "Weapon":
                // ���� ����� ���������� � ������� ����� �������� ���
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
                // ���� ����� ���������� � ����������� ��������
                projectile = other.GetComponent<Projectile>();
                damage = (int)Math.Round(-projectile.damage * 0.8);
                // ��������� ���-�� HP � ������
                health.AddHealth(damage);

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


            default:
                agent = other.GetComponent<AgentBase>();

                if (agent && agent.team == team)
                {
                    this.AddReward(-5f);
                }
                return;
        }
    }
}