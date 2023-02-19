using UnityEngine;

// Указываем для данного класса требуемые компоненты
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    // Задаём базовый параметр скорости объекта
    public float speed = 7f;
    // Задаём базовый параметр максимальной дитсанции, которой может пролететь объект
    public float maxDistance = 25;
    // Задаём базовый параметр урона, который наносит агенту урон
    public int damage = 5;

    // Счётчик дистанции объекта
    [HideInInspector]
    public float curDistance = 0;

   // Информациия о агенте, который запустил объект
    [HideInInspector]
    public AgentBase agent;

    protected virtual void Awake()
    {
        // Инициализируем основные необходимые компоененты
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        // А затем придаём агенту импульс, чтобы тот летел вперёд
        rigidbody.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    // Физический счётчик, отсчитывает сколько агент пролетел 
    void FixedUpdate()
    {
        curDistance += 1 * Time.deltaTime;
        // Если агент пролетел максимальную дистанцию, то уничтожаем его
        if (maxDistance <= curDistance)
        {
            Destroy(this.gameObject);
        }
    }

    // Триггеры коллизии уничтожающие объект при столкновение с чем-либо
    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        Destroy(this.gameObject);
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
