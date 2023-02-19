// Наследуем базовый класс агента
public class AcademyAttackKnight : AgentBase
{
    // Перегруженный метод атаки
    protected override void Attack(int act)
    {
        switch (act)
        {
            case (int)AttackEnum.attack:
                animator.SetTrigger("Attack");
                // Штраф за атаку, чтобы уменьшить злоупотребление атакой
                this.AddReward(-1f);
                break;
        }
    }

}
