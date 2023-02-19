using System;
using UnityEngine;

// Наследуем базовый класс агента
public class AcademyDuelMage : AgentBase
{
    [SerializeField]
    protected Projectile projectile;
    [SerializeField]
    protected GameObject stick;

    // Перегруженный метод перемещения
    protected override void Move(int act)
    {
        // Штраф за перемещения, чтобы упростить игру игроку
        this.AddReward(-0.001f);
        // Вызываем метод из базового класса
        base.Move(act);
    }

    // Перегруженный метод поворота
    protected override void Rotate(float act)
    {
        // Штраф за поворот, чтобы упростить игру игроку
        this.AddReward(-0.001f);
        // Вызываем метод из базового класса
        base.Rotate(act);
    }

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

    // Перегружаем метод смерти агента
    protected override void Agent_Death()
    {
        // Получаем экземляр среды в которой расположен агент
        AcademyDuelEnveroment academyDuel = GetComponentInParent<AcademyDuelEnveroment>();
        // Присваиваем команде врага награду за уничтожение агента
        academyDuel.AddEnemyReward(team, 10f);
        academyDuel.Unregister(this);
        // Завершаем эпизод
        this.EndEpisode();
        // Вызываем метод из базового класса
        base.Agent_Death();
    }

    // Функция создающая проджектайл по вызову из анимации 
    void CastProjectile()
    {
        Projectile spwnProjectile = Instantiate(projectile, stick.transform.position, transform.localRotation);
        spwnProjectile.agent = this.GetComponent<AgentBase>();
    }

    // Функция обрабатывающая коллизию агента
    private void OnTriggerEnter(Collider other)
    {
        AgentBase agent;
        int damage;
        Projectile projectile;
        switch (other.tag)
        {
            case "Weapon":
                // Если агент столкнулся с оружием бойца ближнего боя
                Weapon weapon = other.GetComponent<Weapon>();
                if (weapon.IsAttack)
                {
                    // Уменьшаем кол-во HP у агента
                    health.AddHealth(-weapon.Damage);
                    // Получаем агента, который таковал агента
                    agent = weapon.GetComponentInParent<AgentBase>();
                    if (agent.team == team)
                    {
                        // Если атаковавший союзнк, то штрафуем союзника
                        agent.AddReward(-10f);
                    }
                    else
                    {
                        // Иначе вознаграждаем противника
                        agent.AddReward(5f);
                    }
                }
                return;

            case "EnemyProjectile":
            case "Projectile":
                // Если агент столкнулся с метательным объектом
                projectile = other.GetComponent<Projectile>();
                damage = (int)Math.Round(-projectile.damage * 0.5);
                // Уменьшаем кол-во HP у агента
                health.AddHealth(damage);

                if (projectile.agent.team == team)
                {
                    // Если атаковавший союзнк, то штрафуем союзника
                    Debug.Log("frendly fire");
                    projectile.agent.AddReward(-25f - projectile.curDistance * 5);
                }
                else
                {
                    // Иначе вознаграждаем противника
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
