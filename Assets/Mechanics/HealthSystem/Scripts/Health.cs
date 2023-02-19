using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Параметр задаёт Максимум HP у агента
    [Header("Player Health")]
    [SerializeField]
    int maxHp = 100; 
    
    // Текущее кол-во HP
    private int  hp = 0; 
    
    // Эвент который уведомляет подписавщиеся на него функции о том что агент умер
    public event Action OnDeath = delegate { };         
   
    // Эвент который уведомляет подписавщиеся на него функции о том что агент получил урон
    public event Action OnHealthChange = delegate { };  
   
    
    private void Awake()
    {
        // При создании объекта заполняем текущее кол-во HP максимальным кол-вом HP
        hp = maxHp;
    }

    // Публичная функция-переменная подсчитывающающая процентное кол-во HP
    // для отображения
    public float pct
    {
        get
        {
            return (float)this.hp / (float)this.maxHp;
        }
    }

    // Публичная функция-переменная ответсвенная за рабоут с текущем кол-во здоровья
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
                // Если HP меньше 0, то сообщаем всем подписчикам, что агент умер
                OnDeath();
            }
            // Сообщаем всем подписчикам, что агент получил урон
            OnHealthChange();

        }
    }

    // Метод проверяющий можно ли добавить HP
    public bool CanAddHealth(int amount)
    {
        return hp < maxHp;
    }

    // Метод добавляющий HP
    public void AddHealth(int amount)
    {
        HealthPoints += amount;
    }

    // Метод упрощающий состояние здоровья до определённых 
    // сегментов для более быстрого обучения агента
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
