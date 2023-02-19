using System.Linq;
using UnityEngine;

// Указываем для данного класса требуемые компоненты
[RequireComponent(typeof(Health))]
public class Target : MonoBehaviour
{
    // Необходимые компоненты
    protected Health health;
    AcademyTargetEnviroment env;

    // Числовой идентификатор команды
    [SerializeField]
    Team team;

    protected virtual void Awake()
    {
        // Инициализируем основные необходимые компоененты
        health = GetComponent<Health>();
        // Подписываемся на уведомление о событие смерть агента
        health.OnDeath += Agent_Death;
        // Получаем компонент среды из родительских объектов
        env = GetComponentInParent<AcademyTargetEnviroment>();
    }

    // Функция которая выполняется в случае смерти агента
    private void Agent_Death()
    {
        // Если уничтоженная мишень союзная, то накладываем штраф
        if (env.Agents.First().GetComponent<AgentBase>().team == team )
        {
            env.AddEnemyReward(-10f);
        }
        else
        {
            // Иначе вознаграждаем
            env.AddEnemyReward(10f);
        }
        // Удаляем мешень со сцены
        env.TargetRemove(gameObject);
    }

    // Функция обрабатывающая коллизию мишени
    private void OnTriggerEnter(Collider other)
    {
        AgentBase agent;

        switch (other.tag)
        {
            case "Weapon":
                // Если мишень столкнулась с оружием бойца ближнего боя
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
                // Если мишень столкнулася с метательным объектом
                Projectile projectile = other.GetComponent<Projectile>();
                // Уменьшаем кол-во HP у агента
                health.AddHealth(-projectile.damage);

                if (projectile.agent.team == team)
                {
                    // Если атаковавший союзнк, то штрафуем союзника
                    projectile.agent.AddReward(-25f - projectile.curDistance * 5);
                }
                else
                {
                    // Иначе вознаграждаем противника
                    projectile.agent.AddReward(10f + projectile.curDistance * 5);
                }
                return;
        }
    }
}
