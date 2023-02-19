using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class AcademyTargetEnviroment : MonoBehaviour
{
    // Данный параметр задаёт максимальное кол-во шагов
    [Tooltip("Max Environment Steps")]
    public int MaxEnvironmentSteps = 25000;

    // Список мишеней который должен уничтожить агент
    [SerializeField]
    GameObject[] Targets;
    // Список агентов которые могут быть
    [SerializeField]
    public GameObject[] Agents;

    // Список Текущих мишений
    List<GameObject> TargetsList = new List<GameObject>();

    // объект необходимый для получения размера среды
    protected BoxCollider BoxCollider;
    // Внутренний таймер отсчитывающий кол-во шагов
    protected int ResetTimer;
    // Группа агента для учёта и выдачи коллективных наград
    protected SimpleMultiAgentGroup agentGroup = new SimpleMultiAgentGroup();

    // Метод для получения случайной позиции внутри среды
    public Vector3 GetRandomSpawnPos()
    {
        Vector3 point = new Vector3(
            Random.Range(BoxCollider.bounds.min.x, BoxCollider.bounds.max.x),
            Random.Range(BoxCollider.bounds.min.y, BoxCollider.bounds.max.y),
            Random.Range(BoxCollider.bounds.min.z, BoxCollider.bounds.max.z)
        );
        return point;
    }

    // Метод презагрузки среды
    public void ResetScene()
    {
        // Сбрасываем таймер
        ResetTimer = 0;

        if (TargetsList.Count > 0)
        {
            // Если ещё остались мишени удаляем их и очишаем список
            foreach (var i in TargetsList)
            {
                Destroy(i.gameObject);
            }
        }
        TargetsList.Clear();

        if (transform.childCount > 0)
        {
            // Очишаем среду от агентов и прочих объектов
            foreach (var i in GetComponentsInChildren<Transform>())
            {
                if (i.tag == "Player" || i.tag == "Knight" || i.tag == "Mage" || i.tag == "Red Mage" || i.tag == "Red Knight")
                {
                    var agent = GetComponent<AgentBase>();
                    if (agent)
                    {
                        agent.EndEpisode();
                        // Завершаем эпизод и убираем агента из группы
                        agentGroup.UnregisterAgent(agent);
                    }
                    Destroy(i.gameObject);
                }
            }
        }
        
        // Размещаем мешени
        foreach (GameObject target in Targets)
        {
            var spawnedTarget = Instantiate(target, transform, false);
            spawnedTarget.transform.position = GetRandomSpawnPos();
            TargetsList.Add(spawnedTarget);
        }
        // Размещаем агентов
        foreach (GameObject prefab in Agents)
        {
            var agent = Instantiate(prefab, transform, false);
            agent.transform.position = GetRandomSpawnPos();
            // Регистрируем агента в группе
            agentGroup.RegisterAgent(agent.GetComponent<AgentBase>());
        }
    }

    public void AddEnemyReward(float reward)
    {
        // Вознаграждаем группу агентов
        agentGroup.AddGroupReward(reward);
    }


    public void Awake()
    {
        // Получаем объект для получени размера среды
        BoxCollider = GetComponent<BoxCollider>();
        // Перезагружаем среду
        ResetScene();
    }


    public void FixedUpdate()
    {
        //  Таймер
        ResetTimer += 1;
        if (ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            // Если таймер перевалил за определённое значение, то перезагружаем среду
            ResetScene();
        }
        
        if (transform.childCount > 0)
        {
            foreach (var i in GetComponentsInChildren<Transform>())
            {
                if (i.tag == "Red Knight" || i.tag == "Red Mage" || i.tag == "Knight" || i.tag == "Mage")
                {
                    // Если агент вышел за пределы среды, то перезагружаем среду
                    if (i.position.y <= BoxCollider.transform.position.y-10)
                        ResetScene();
                }
            }
        }
    }

    // Метод удаления мешени
    public void TargetRemove(GameObject target)
    {
        TargetsList.Remove(target);
        Destroy(target.gameObject);
    }
}
