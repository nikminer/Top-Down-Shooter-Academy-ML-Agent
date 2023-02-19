using UnityEngine;
// ��������� ������� ����� ������
public class AcademyAttackMage : AgentBase
{
    // ��������� ����
    [Header("Mage Parameters")]
    // ���� ��� ������������ �������
    [SerializeField]
    protected Projectile projectile;
    // ���� ��� ���������� ����������� ������
    [SerializeField]
    protected GameObject stick;

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

    // ������� ��������� ����������� �� ������ �� �������� 
    void CastProjectile()
    {
        Projectile spwnProjectile = Instantiate(projectile, stick.transform.position, transform.localRotation);
        spwnProjectile.agent = this.GetComponent<AgentBase>();
    }
}
