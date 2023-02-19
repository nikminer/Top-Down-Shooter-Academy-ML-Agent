using UnityEngine;

// ��������� ��� ������� ������ ��������� ����������
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    // ����� ������� �������� �������� �������
    public float speed = 7f;
    // ����� ������� �������� ������������ ���������, ������� ����� ��������� ������
    public float maxDistance = 25;
    // ����� ������� �������� �����, ������� ������� ������ ����
    public int damage = 5;

    // ������� ��������� �������
    [HideInInspector]
    public float curDistance = 0;

   // ����������� � ������, ������� �������� ������
    [HideInInspector]
    public AgentBase agent;

    protected virtual void Awake()
    {
        // �������������� �������� ����������� �����������
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        // � ����� ������ ������ �������, ����� ��� ����� �����
        rigidbody.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    // ���������� �������, ����������� ������� ����� �������� 
    void FixedUpdate()
    {
        curDistance += 1 * Time.deltaTime;
        // ���� ����� �������� ������������ ���������, �� ���������� ���
        if (maxDistance <= curDistance)
        {
            Destroy(this.gameObject);
        }
    }

    // �������� �������� ������������ ������ ��� ������������ � ���-����
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
