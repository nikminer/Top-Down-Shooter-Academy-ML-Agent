using UnityEngine;

public class AcademyTargeting : AgentBase
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Move(int act)
    {
        this.AddReward(-0.001f);
        base.Move(act);
    }

    protected override void Rotate(float act)
    {
        this.AddReward(-0.001f);
        base.Rotate(act);
    }

    protected override void Attack(int act)
    {
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Red Mage":
                this.AddReward(5f);
                Destroy(other.gameObject);
                return;
            case "Red Knight":
                this.AddReward(5f);
                Destroy(other.gameObject);
                return;
            case "Wall":
                this.AddReward(-40f);
                return;
            case "Knight":
                this.AddReward(5f);
                Destroy(other.gameObject);
                return;
            case "Mage":
                this.AddReward(5f);
                Destroy(other.gameObject);
                return;
        }
    }
}
