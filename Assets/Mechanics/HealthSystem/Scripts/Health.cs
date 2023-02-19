using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // �������� ����� �������� HP � ������
    [Header("Player Health")]
    [SerializeField]
    int maxHp = 100; 
    
    // ������� ���-�� HP
    private int  hp = 0; 
    
    // ����� ������� ���������� ������������� �� ���� ������� � ��� ��� ����� ����
    public event Action OnDeath = delegate { };         
   
    // ����� ������� ���������� ������������� �� ���� ������� � ��� ��� ����� ������� ����
    public event Action OnHealthChange = delegate { };  
   
    
    private void Awake()
    {
        // ��� �������� ������� ��������� ������� ���-�� HP ������������ ���-��� HP
        hp = maxHp;
    }

    // ��������� �������-���������� ����������������� ���������� ���-�� HP
    // ��� �����������
    public float pct
    {
        get
        {
            return (float)this.hp / (float)this.maxHp;
        }
    }

    // ��������� �������-���������� ������������ �� ������ � ������� ���-�� ��������
    public int HealthPoints
    {
        get
        {
            return hp;
        }
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
            
            if (hp <= 0)
            {
                // ���� HP ������ 0, �� �������� ���� �����������, ��� ����� ����
                OnDeath();
            }
            // �������� ���� �����������, ��� ����� ������� ����
            OnHealthChange();

        }
    }

    // ����� ����������� ����� �� �������� HP
    public bool CanAddHealth(int amount)
    {
        return hp < maxHp;
    }

    // ����� ����������� HP
    public void AddHealth(int amount)
    {
        HealthPoints += amount;
    }

    // ����� ���������� ��������� �������� �� ����������� 
    // ��������� ��� ����� �������� �������� ������
    public int StateHealth()
    {
        return (int)HealthPoints / 25;
    }


    private void OnTriggerEnter(Collider other)
    {
        /*
        switch (other.tag)
        {
            case "Medicine":
                if (canHealth && CanAddHealth(25))
                {
                    AddHealth(25);
                    Destroy(other.gameObject);
                }
                return;

            case "Weapon":
                Weapon weapon = other.GetComponent<Weapon>();
                if (weapon.IsAttack)
                {
                    AddHealth(-weapon.Damage);
                }
                return;
        }*/
    }
}
