using System;
using UnityEngine;

// ��������� ������� ����� ������
public class AcademyDuelMage : AgentBase
{
    [SerializeField]
    protected Projectile projectile;
    [SerializeField]
    protected GameObject stick;

    // ������������� ����� �����������
    protected override void Move(int act)
    {
        // ����� �� �����������, ����� ��������� ���� ������
        this.AddReward(-0.001f);
        // �������� ����� �� �������� ������
        base.Move(act);
    }

    // ������������� ����� ��������
    protected override void Rotate(float act)
    {
        // ����� �� �������, ����� ��������� ���� ������
        this.AddReward(-0.001f);
        // �������� ����� �� �������� ������
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

    // ������� ��������� ����������� �� ������ �� �������� 
    void CastProjectile()
    {
        Projectile spwnProjectile = Instantiate(projectile, stick.transform.position, transform.localRotation);
        spwnProjectile.agent = this.GetComponent<AgentBase>();
    }

    // ������� �������������� �������� ������
    private void OnTriggerEnter(Collider other)
    {
        AgentBase agent;
        int damage;
        Projectile projectile;
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
                damage = (int)Math.Round(-projectile.damage * 0.5);
                // ��������� ���-�� HP � ������
                health.AddHealth(damage);

                if (projectile.agent.team == team)
                {
                    // ���� ����������� ������, �� �������� ��������
                    Debug.Log("frendly fire");
                    projectile.agent.AddReward(-25f - projectile.curDistance * 5);
                }
                else
                {
                    // ����� ������������� ����������
                    projectile.agent.AddReward(10f + projectile.curDistance * 5);
                }
                return;
            
            case "Mage":
            case "Player":
            case "Knight":
            case "Red Knight":
                this.AddReward(-40f);
                return;

        }
    }
}
