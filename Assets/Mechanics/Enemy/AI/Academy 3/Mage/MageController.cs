using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MageController : AgentBase
{
    [Header("Mage Parameters")]
    [SerializeField]
    protected Projectile projectile;
    [SerializeField]
    protected GameObject stick;

    void CastProjectile()
    {
        Projectile spwnProjectile = Instantiate(projectile, stick.transform.position, transform.localRotation);
        spwnProjectile.agent = this.GetComponent<AgentBase>();
    }

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
                health.AddHealth(-projectile.damage);
                return;
        }
    }
}
