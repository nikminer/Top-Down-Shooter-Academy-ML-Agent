using UnityEngine;
// Наследуем базовый класс агента
public class AcademyAttackMage : AgentBase
{
    // Параметры Мага
    [Header("Mage Parameters")]
    // Поле для метательного объекта
    [SerializeField]
    protected Projectile projectile;
    // Поле где спавинится метательный объект
    [SerializeField]
    protected GameObject stick;

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

    // Функция создающая проджектайл по вызову из анимации 
    void CastProjectile()
    {
        Projectile spwnProjectile = Instantiate(projectile, stick.transform.position, transform.localRotation);
        spwnProjectile.agent = this.GetComponent<AgentBase>();
    }
}
