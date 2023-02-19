using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

// Указываем для данного класса требуемые компоненты
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class AgentBase : Agent
{
    // Задаём параметр скорости агента
    [Header("Movment Parameters")]
    [SerializeField]
    float speed = 5f;

    // Основные внешние компоненты
    protected Animator animator;
    protected Health health;
    protected Rigidbody controller;

    // Числовой идентификатор команды
    public Team team;
   
    protected virtual void Awake()
    {
        // Инициализируем основные необходимые компоененты
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        controller = GetComponent<Rigidbody>();

        // Подписываемся на уведомление о событие смерть агента
        health.OnDeath += Agent_Death;
    }

    // Функция которая выполняется в случае смерти агента, 
    // в базовом варианте идёт простое уничтожение объекты со сцены
    protected virtual void Agent_Death()
    {
        Destroy(gameObject);
    }

    // Перегружаемая функция сбора входных наблюдений
    public override void CollectObservations(VectorSensor sensor)
    {
        // Сначала передаём состояние здоровья агента, затем идентификатор команды
        sensor.AddObservation(health.StateHealth());
        sensor.AddObservation((int)team);
    }

    // Перегружаемая функция получения выходных действий
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Получаем дискретные действия для движения
        Move(actionBuffers.DiscreteActions[0]);
        // Получаем продолжительные действия для поворота
        Rotate(actionBuffers.ContinuousActions[0]);
        // Получаем дискретные действия для атаки
        Attack(actionBuffers.DiscreteActions[1]);
    }

    // Функция ответственная за движение агента
    protected virtual void Move(int act)
    {
        Vector3 movmentVector = Vector3.zero;
        // Просто Switch который преобразуем значение от 1 до 4 
        // в указание куда двигаться агенту, формируя вектор движения
        switch (act)
        {
            case (int)MovementEnum.forward:
                movmentVector = transform.forward;
                break;
            case (int)MovementEnum.backward:
                movmentVector = -transform.forward;
                break;
            case (int)MovementEnum.right:
                movmentVector = transform.right;
                break;
            case (int)MovementEnum.left:
                movmentVector = -transform.right;
                break;
            default:
                return;
        }
        // Используя физический компонент придаём импульс агенту 
        // по сформированному вектору движения умноженный на скорость агента
        controller.AddForce(movmentVector * speed,
            ForceMode.Impulse);
    }
    // Функция ответственная за поворот агента
    protected virtual void Rotate(float act)
    {
        // Данная функция скалируя значение от -1 до +1 в 10 раз
        Vector3 rotateDir = Vector3.zero;
        rotateDir.y += act * 10;
        // Разворачиваем агента
        transform.Rotate(rotateDir);
    }

    // Функция ответсвенная за атаку агента
    protected virtual void Attack(int act)
    {
        switch (act)
        {
            // Если поступло от нейросети действие атаки, то запускаем анимацию атаки
            case (int)AttackEnum.attack:
                animator.SetTrigger("Attack");
                break;
        }
    }
}
