// ��������� ������� ����� ������
public class AcademyAttackKnight : AgentBase
{
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

}
