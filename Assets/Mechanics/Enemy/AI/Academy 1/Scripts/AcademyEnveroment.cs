using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class AcademyEnveroment : MonoBehaviour
{
    [Tooltip("Max Environment Steps")]
    public int MaxEnvironmentSteps = 25000;

    [SerializeField]
    GameObject RedPref;
    [SerializeField]
    GameObject RedPref1;

    [SerializeField]
    GameObject BluePref;
    [SerializeField]
    GameObject BluePref1;

    [SerializeField]
    int maxAgentPerTeam = 1;

    protected SimpleMultiAgentGroup BlueAgentGroup;
    protected SimpleMultiAgentGroup RedAgentGroup;

    public void Awake()
    {
        RedAgentGroup = new SimpleMultiAgentGroup();
        BlueAgentGroup = new SimpleMultiAgentGroup();
        ResetScene();
    }

    protected int ResetTimer;
    public void FixedUpdate()
    {
        ResetTimer += 1;
        if (ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            BlueAgentGroup.AddGroupReward(-1f);
            BlueAgentGroup.GroupEpisodeInterrupted();

            RedAgentGroup.AddGroupReward(-1f);
            RedAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }

        if (RedAgentGroup.GetRegisteredAgents().Count == 0 || BlueAgentGroup.GetRegisteredAgents().Count == 0)
        {
            BlueAgentGroup.EndGroupEpisode();
            RedAgentGroup.EndGroupEpisode();
            ResetScene();
        }

    }

    public Vector3 GetRandomSpawnPos(Vector3 position)
    {
        int count = 0;
        Vector3 randomSpawnPos = Vector3.zero;
        do
        {
            count++;

            var randomPosX = Random.Range(-8f, 8f);
            var randomPosZ = Random.Range(-3f, 3f);

            randomSpawnPos = new Vector3(randomPosX, 0, randomPosZ);
        }
        while (Physics.CheckBox(randomSpawnPos, new Vector3(4f, 0.01f, 4f)) && count < 10);

        return randomSpawnPos;
    }

    public void ResetScene()
    {
        ResetTimer = 0;

        if (transform.childCount > 0 )
        {
            foreach(var i in GetComponentsInChildren<Transform>())
            {
                if (i.tag == "Player" || i.tag == "Knight" || i.tag == "Mage")
                {
                    Destroy(i.gameObject);
                }
            }
        }

        if (RedPref != null)
        {
            for (int i = 0; i < maxAgentPerTeam; i++)
            {
                var agent = Instantiate(RedPref, transform, false);
                agent.transform.localPosition += GetRandomSpawnPos(agent.transform.localPosition);
                RedAgentGroup.RegisterAgent(agent.GetComponent<AgentController>());
            }
        }

        if (RedPref1 != null)
        {
            for (int i = 0; i < maxAgentPerTeam; i++)
            {
                var agent = Instantiate(RedPref1, transform, false);
                agent.transform.localPosition += GetRandomSpawnPos(agent.transform.localPosition);
                RedAgentGroup.RegisterAgent(agent.GetComponent<AgentController>());
            }
        }

        if (BluePref != null)
        {
            for (int i = 0; i < maxAgentPerTeam; i++)
            {
                var agent = Instantiate(BluePref, transform, false);
                agent.transform.localPosition += GetRandomSpawnPos(agent.transform.localPosition);
                BlueAgentGroup.RegisterAgent(agent.GetComponent<AgentController>());
            }
        }

        if (BluePref1 != null)
        {
            for (int i = 0; i < maxAgentPerTeam; i++)
            {
                var agent = Instantiate(BluePref1, transform, false);
                agent.transform.localPosition += GetRandomSpawnPos(agent.transform.localPosition);
                BlueAgentGroup.RegisterAgent(agent.GetComponent<AgentController>());
            }
        }
    }

    public void AddAgentToGroup(AgentController agent)
    {
        switch (agent.team)
        {
            case Team.Blue:
                BlueAgentGroup.RegisterAgent(agent);
                return;
            case Team.Red:
                RedAgentGroup.RegisterAgent(agent);
                return;
        }

    }

    public void AddEnemyTeamReward(Team team, float reward)
    {
        AddTeamReward(team == Team.Blue ? Team.Red : Team.Blue, reward);
    }

    public void AddTeamReward(Team team, float reward)
    {
        switch(team)
        {
            case Team.Blue:
                BlueAgentGroup.AddGroupReward(reward);
                break;
            case Team.Red:
                RedAgentGroup.AddGroupReward(reward);
                break;
        }
    }

}
