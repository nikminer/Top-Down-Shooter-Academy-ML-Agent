using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class AcademyDuelEnveroment : MonoBehaviour
{
    // ������ �������� ����� ������������ ���-�� �����
    [Tooltip("Max Environment Steps")]
    public int MaxEnvironmentSteps = 25000;

    // ������ ������� ������� �������
    [SerializeField]
    GameObject[] RedPrefs;
    // ������ ������� ����� �������
    [SerializeField]
    GameObject[] BluePrefs;

    // ���������� ������ ������������� ���-�� �����
    protected int ResetTimer;
    // ������ ����������� ��� ��������� ������� �����
    protected BoxCollider BoxCollider;

    // ������ ������� ��� ����� � ������ ������������ ������
    protected SimpleMultiAgentGroup RedGroup = new SimpleMultiAgentGroup();
    protected SimpleMultiAgentGroup BlueGroup = new SimpleMultiAgentGroup();
    
    // ����� ��� ��������� ��������� ������� ������ �����
    public Vector3 GetRandomSpawnPos()
    {
        Vector3 point = new Vector3(
            Random.Range(BoxCollider.bounds.min.x, BoxCollider.bounds.max.x),
            Random.Range(BoxCollider.bounds.min.y, BoxCollider.bounds.max.y),
            Random.Range(BoxCollider.bounds.min.z, BoxCollider.bounds.max.z)
        );
        return point;
    }
    
    // ����� ����������� �����
    public void ResetScene()
    {
        // ���������� ������
        ResetTimer = 0;
        // ������� ����� �� ������� � ������ ��������
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
                            // ��������� ������ � ������� ������ �� ������
                            BlueGroup.UnregisterAgent(agent);
                        }
                        else
                        {
                            // ��������� ������ � ������� ������ �� ������
                            RedGroup.UnregisterAgent(agent);
                        }

                       
                    }

                    Destroy(i.gameObject);
                }
            }
        }
        // ��������� ������� �����
        foreach (GameObject prefab in BluePrefs)
        {
            var agent = Instantiate(prefab, transform, false);
            agent.transform.position = GetRandomSpawnPos();
            // ������������ ������ � ������
            BlueGroup.RegisterAgent(agent.GetComponent<AgentBase>());
        }

        foreach (GameObject prefab in RedPrefs)
        {
            var agent = Instantiate(prefab, transform, false);
            agent.transform.position = GetRandomSpawnPos();
            // ������������ ������ � ������
            RedGroup.RegisterAgent(agent.GetComponent<AgentBase>());
        }
    }

    public void Awake()
    {
        // �������� ������ ��� �������� ������� �����
        BoxCollider = GetComponent<BoxCollider>();
        // ������������� �����
        ResetScene();
    }

    // ������������� ������ ������� ����������
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
            // ��������� ������ � ������� ������ �� ������
            BlueGroup.UnregisterAgent(agent);
        }
        else
        {
            // ��������� ������ � ������� ������ �� ������
            RedGroup.UnregisterAgent(agent);
        }
    }


    public void FixedUpdate()
    {
        //  ������
        ResetTimer += 1;

        if (ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            // ���� ������ ��������� �� ����������� ��������, �� ������������� �����
            ResetScene();
        }
        // ���� ��� ������ ����������, �.�. � ����� ������ �� ��������, �� ������������� �����
        if (!GetComponentInChildren<AgentBase>())
        {
            ResetScene();
        }
    }

}
