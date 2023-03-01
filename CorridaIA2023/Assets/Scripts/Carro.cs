using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Carro : MonoBehaviour
{
    NavMeshAgent car;

    public GameObject[] wayPoints;
    public int destiny = 0;
    public int lap = 0;
    public int maxLap = 3;

    public GameObject[] wheels;
    public GameObject[] fWheels;

    public TextMeshProUGUI jpGUI;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<NavMeshAgent>();

        car.SetDestination(wayPoints[destiny].transform.position);

        JpGUI();
    }

    // Update is called once per frame
    void Update()
    {
        //Waypoints
        if (Vector3.Distance(transform.position, wayPoints[destiny].transform.position) < 8)
        {
            RandomizeCarPerformance(destiny);

            destiny++;
            if (destiny > wayPoints.Length - 1)
            {
                destiny = 0;
                lap++;
            }
            car.SetDestination(wayPoints[destiny].transform.position);


            JpGUI();
        }

        //Rodas
        WheelsRotation();
    }

    void RandomizeCarPerformance(int i)
    {
        car.acceleration = Random.Range(8, 16);

        if (wayPoints[i].transform.CompareTag("Lento"))
        {
            car.speed = Random.Range(12, 20);
        }
        if (wayPoints[i].transform.CompareTag("Medio"))
        {
            car.speed = Random.Range(20, 25);
        }
        if (wayPoints[i].transform.CompareTag("Rapido"))
        {
            car.speed = Random.Range(25, 40);
        }
    }

    void WheelsRotation()
    {
        fWheels[0].transform.LookAt(wayPoints[destiny].transform.position);
        fWheels[1].transform.LookAt(wayPoints[destiny].transform.position);

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].transform.Rotate(Time.deltaTime * 200, 0, 0);
        }
    }

    void JpGUI()
    {
        jpGUI.text = $"Lap: {lap} / {maxLap}\n{car.speed} Km/h";
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            if(wayPoints[i].transform.CompareTag("Lento")) Gizmos.color = Color.red;
            if (wayPoints[i].transform.CompareTag("Medio")) Gizmos.color = Color.yellow;
            if (wayPoints[i].transform.CompareTag("Rapido")) Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(wayPoints[i].transform.position, 8);
        }
    }
}
