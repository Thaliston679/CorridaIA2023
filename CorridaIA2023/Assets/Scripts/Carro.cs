using UnityEngine;
using UnityEngine.AI;

public class Carro : MonoBehaviour
{
    NavMeshAgent car;

    public GameObject[] wayPoints;
    public int destiny = 0;
    public int lap = 0;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<NavMeshAgent>();

        car.SetDestination(wayPoints[destiny].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Waypoints
        if (Vector3.Distance(transform.position, wayPoints[destiny].transform.position) < 5)
        {
            RandomizeCarPerformance(destiny);

            destiny++;
            if (destiny > wayPoints.Length - 1)
            {
                destiny = 0;
                lap++;
            }
            car.SetDestination(wayPoints[destiny].transform.position);
        }
    }

    void RandomizeCarPerformance(int i)
    {
        if (wayPoints[i].transform.CompareTag("Lento"))
        {
            car.acceleration = Random.Range(4, 10);
            car.speed = Random.Range(8, 12);
        }
        if (wayPoints[i].transform.CompareTag("Lento"))
        {
            car.acceleration = Random.Range(10, 16);
            car.speed = Random.Range(12, 16);
        }
        if (wayPoints[i].transform.CompareTag("Lento"))
        {
            car.acceleration = Random.Range(16, 24);
            car.speed = Random.Range(16, 20);
        }
        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            if(wayPoints[i].transform.CompareTag("Lento")) Gizmos.color = Color.red;
            if (wayPoints[i].transform.CompareTag("Medio")) Gizmos.color = Color.yellow;
            if (wayPoints[i].transform.CompareTag("Rapido")) Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(wayPoints[i].transform.position, 5);
        }
    }
}
