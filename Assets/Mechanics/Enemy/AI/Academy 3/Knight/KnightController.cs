using System;
using UnityEngine;

public class KnightController : AgentBase
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Weapon":
                Weapon weapon = other.GetComponent<Weapon>();
                if (weapon.IsAttack)
                {
                    health.AddHealth(-weapon.Damage);
                }
                return;

            case "Projectile":
                Projectile projectile = other.GetComponent<Projectile>();

                int damage = (int)Math.Round(-projectile.damage * 0.5);
                health.AddHealth(damage);

                return;
        }
    }
}
