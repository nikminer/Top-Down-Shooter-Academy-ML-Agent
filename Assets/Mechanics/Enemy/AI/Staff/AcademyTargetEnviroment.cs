using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class AcademyTargetEnviroment : MonoBehaviour
{
    // ������ �������� ����� ������������ ���-�� �����
    [Tooltip("Max Environment Steps")]
    public int MaxEnvironmentSteps = 25000;

    // ������ ������� ������� ������ ���������� �����
    [SerializeField]
    GameObject[] Targets;
    // ������ ������� ������� ����� ����
    [SerializeField]
    public GameObject[] Agents;

    // ������ ������� �������
    List<GameObject> TargetsList = new List<GameObject>();

    // ������ ����������� ��� ��������� ������� �����
    protected BoxCollider BoxCollider;
    // ���������� ������ ������������� ���-�� �����
    protected int ResetTimer;
    // ������ ������ ��� ����� � ������ ������������ ������
    protected SimpleMultiAgentGroup agentGroup = new SimpleMultiAgentGroup();

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

        if (TargetsList.Count > 0)
        {
            // ���� ��� �������� ������ ������� �� � ������� ������
            foreach (var i in TargetsList)
            {
                Destroy(i.gameObject);
            }
        }
        TargetsList.Clear();

        if (transform.childCount > 0)
        {
            // ������� ����� �� ������� � ������ ��������
            foreach (var i in GetComponentsInChildren<Transform>())
            {
                if (i.tag == "Player" || i.tag == "Knight" || i.tag == "Mage" || i.tag == "Red Mage" || i.tag == "Red Knight")
                {
                    var agent = GetComponent<AgentBase>();
                    if (agent)
                    {
                        agent.EndEpisode();
                        // ��������� ������ � ������� ������ �� ������
                        agentGroup.UnregisterAgent(agent);
                    }
                    Destroy(i.gameObject);
                }
            }
        }
        
        // ��������� ������
        foreach (GameObject target in Targets)
        {
            var spawnedTarget = Instantiate(target, transform, false);
            spawnedTarget.transform.position = GetRandomSpawnPos();
            TargetsList.Add(spawnedTarget);
        }
        // ��������� �������
        foreach (GameObject prefab in Agents)
        {
            var agent = Instantiate(prefab, transform, false);
            agent.transform.position = GetRandomSpawnPos();
            // ������������ ������ � ������
            agentGroup.RegisterAgent(agent.GetComponent<AgentBase>());
        }
    }

    public void AddEnemyReward(float reward)
    {
        // ������������� ������ �������
        agentGroup.AddGroupReward(reward);
    }


    public void Awake()
    {
        // �������� ������ ��� �������� ������� �����
        BoxCollider = GetComponent<BoxCollider>();
        // ������������� �����
        ResetScene();
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
        
        if (transform.childCount > 0)
        {
            foreach (var i in GetComponentsInChildren<Transform>())
            {
                if (i.tag == "Red Knight" || i.tag == "Red Mage" || i.tag == "Knight" || i.tag == "Mage")
                {
                    // ���� ����� ����� �� ������� �����, �� ������������� �����
                    if (i.position.y <= BoxCollider.transform.position.y-10)
                        ResetScene();
                }
            }
        }
    }

    // ����� �������� ������
    public void TargetRemove(GameObject target)
    {
        TargetsList.Remove(target);
        Destroy(target.gameObject);
    }
}
