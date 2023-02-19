using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class AcademyDuelEnveroment : MonoBehaviour
{
    // Данный параметр задаёт максимальное кол-во шагов
    [Tooltip("Max Environment Steps")]
    public int MaxEnvironmentSteps = 25000;

    // Список агентов красной команды
    [SerializeField]
    GameObject[] RedPrefs;
    // Список агентов синей команды
    [SerializeField]
    GameObject[] BluePrefs;

    // Внутренний таймер отсчитывающий кол-во шагов
    protected int ResetTimer;
    // объект необходимый для получения размера среды
    protected BoxCollider BoxCollider;

    // Группы агентов для учёта и выдачи коллективных наград
    protected SimpleMultiAgentGroup RedGroup = new SimpleMultiAgentGroup();
    protected SimpleMultiAgentGroup BlueGroup = new SimpleMultiAgentGroup();
    
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
        // Очишаем среду от агентов и прочих объектов
        if (transform.childCount > 0 )
        {
            foreach(var i in GetComponentsInChildren<Transform>())
            {
                if (i.tag == "Player" || i.tag == "Knight" || i.tag == "Mage" || i.tag == "Red Mage" || i.tag == "Red Knight")
                {
                    var agent = GetComponent<AgentBase>();
                    if (agent)
                    {
                        agent.EndEpisode();
                        if (agent.team == Team.Blue)
                        {
                            // Завершаем эпизод и убираем агента из группы
                            BlueGroup.UnregisterAgent(agent);
                        }
                        else
                        {
                            // Завершаем эпизод и убираем агента из группы
                            RedGroup.UnregisterAgent(agent);
                        }

                       
                    }

                    Destroy(i.gameObject);
                }
            }
        }
        // Размещаем агентов синих
        foreach (GameObject prefab in BluePrefs)
        {
            var agent = Instantiate(prefab, transform, false);
            agent.transform.position = GetRandomSpawnPos();
            // Регистрируем агента в группе
            BlueGroup.RegisterAgent(agent.GetComponent<AgentBase>());
        }

        foreach (GameObject prefab in RedPrefs)
        {
            var agent = Instantiate(prefab, transform, false);
            agent.transform.position = GetRandomSpawnPos();
            // Регистрируем агента в группе
            RedGroup.RegisterAgent(agent.GetComponent<AgentBase>());
        }
    }

    public void Awake()
    {
        // Получаем объект для получени размера среды
        BoxCollider = GetComponent<BoxCollider>();
        // Перезагружаем среду
        ResetScene();
    }

    // Вознаграждаем группу агентов противника
    public void AddEnemyReward(Team team, float reward)
    {
        if (team == Team.Blue)
        {
            RedGroup.AddGroupReward(reward);
        }
        else
        {
            BlueGroup.AddGroupReward(reward);
        }
    }

    public void Unregister(AgentBase agent)
    {
        if (agent.team == Team.Blue)
        {
            // Завершаем эпизод и убираем агента из группы
            BlueGroup.UnregisterAgent(agent);
        }
        else
        {
            // Завершаем эпизод и убираем агента из группы
            RedGroup.UnregisterAgent(agent);
        }
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
        // Если все агенты уничтожены, т.е. в среде никого не осталось, то перезагружаем среду
        if (!GetComponentInChildren<AgentBase>())
        {
            ResetScene();
        }
    }

}
