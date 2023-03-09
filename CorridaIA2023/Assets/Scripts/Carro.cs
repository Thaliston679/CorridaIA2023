using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Carro : MonoBehaviour
{
    NavMeshAgent car;
    public GameObject carObj;

    public GameObject[] wayPoints0;
    public GameObject[] wayPoints1;
    int waypointCount = 0;
    public int destiny = 0;
    public int lap = 0;
    public int maxLap = 3;
    int rota = 0;
    public int ranking;

    public GameObject[] wheels;
    public GameObject[] fWheels;

    public TextMeshProUGUI jpGUI;

    public GameObject cam;
    public Transform[] camPos;
    int camID = 0;
    bool finish = false;

    // Start is called before the first frame update
    void Start()
    {
        rota = Random.Range(0, 2);
        cam = GetComponentInChildren<Camera>().gameObject;
        cam.transform.localPosition = camPos[camID].position;
        cam.transform.localRotation = camPos[camID].rotation;
        car = GetComponent<NavMeshAgent>();

        

        JpGUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (lap > 3 && !finish) Chegada();

        if(rota == 0)
        {
            //Waypoints
            if (Vector3.Distance(transform.position, wayPoints0[destiny].transform.position) < 8)
            {
                RandomizeCarPerformance(destiny);

                destiny++;
                waypointCount++;
                if (destiny > wayPoints0.Length - 1)
                {
                    destiny = 0;
                    rota = Random.Range(0, 2);
                    lap++;
                }
                car.SetDestination(wayPoints0[destiny].transform.position);


                JpGUI();
            }
        }

        if(rota == 1)
        {
            //Waypoints
            if (Vector3.Distance(transform.position, wayPoints1[destiny].transform.position) < 8)
            {
                RandomizeCarPerformance(destiny);

                destiny++;
                waypointCount++;
                if (destiny > wayPoints1.Length - 1)
                {
                    destiny = 0;
                    rota = Random.Range(0, 2);
                    lap++;
                }
                car.SetDestination(wayPoints1[destiny].transform.position);


                JpGUI();
            }
        }

        

        //Rodas
        WheelsRotation();
    }

    void RandomizeCarPerformance(int i)
    {
        car.acceleration = Random.Range(8, 16);

        if(rota == 0)
        {
            if (wayPoints0[i].transform.CompareTag("Lento"))
            {
                car.speed = Random.Range(15, 20);
            }
            if (wayPoints0[i].transform.CompareTag("Medio"))
            {
                car.speed = Random.Range(20, 25);
            }
            if (wayPoints0[i].transform.CompareTag("Rapido"))
            {
                car.speed = Random.Range(25, 35);
            }
        }

        if(rota == 1)
        {
            if (wayPoints1[i].transform.CompareTag("Lento"))
            {
                car.speed = Random.Range(15, 20);
            }
            if (wayPoints1[i].transform.CompareTag("Medio"))
            {
                car.speed = Random.Range(20, 25);
            }
            if (wayPoints1[i].transform.CompareTag("Rapido"))
            {
                car.speed = Random.Range(25, 35);
            }
        }
        
    }

    void WheelsRotation()
    {
        if(rota == 0)
        {
            fWheels[0].transform.LookAt(wayPoints0[destiny].transform.position);
            fWheels[1].transform.LookAt(wayPoints0[destiny].transform.position);
        }

        if(rota == 1)
        {
            fWheels[0].transform.LookAt(wayPoints1[destiny].transform.position);
            fWheels[1].transform.LookAt(wayPoints1[destiny].transform.position);
        }

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].transform.Rotate(Time.deltaTime * 200, 0, 0);
        }
    }

    void JpGUI()
    {
        if(jpGUI != null) jpGUI.text = $"Lap: {lap} / {maxLap}\n{car.speed} Km/h\n{ranking}º Lugar";
    }

    public void CamSwitch()
    {
        camID++;
        if (camID > camPos.Length - 1) camID = 0;
        cam.transform.localPosition = camPos[camID].position;
        cam.transform.localRotation = camPos[camID].rotation;
        Debug.Log(camID);
    }

    public int GetWayPointCount()
    {
        return waypointCount;
    }

    public bool GetFinish()
    {
        return finish;
    }

    public void SetRanking(int rank)
    {
        ranking = rank;
    }

    public void Largada()
    {
        if (rota == 0) car.SetDestination(wayPoints0[destiny].transform.position);
        if (rota == 1) car.SetDestination(wayPoints1[destiny].transform.position);
    }

    public void Chegada()
    {
        finish = true;
        carObj.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < wayPoints0.Length; i++)
        {
            if(wayPoints0[i].transform.CompareTag("Lento")) Gizmos.color = Color.red;
            if (wayPoints0[i].transform.CompareTag("Medio")) Gizmos.color = Color.yellow;
            if (wayPoints0[i].transform.CompareTag("Rapido")) Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(wayPoints0[i].transform.position, 8);
        }

        for (int i = 0; i < wayPoints1.Length; i++)
        {
            if (wayPoints1[i].transform.CompareTag("Lento")) Gizmos.color = Color.red;
            if (wayPoints1[i].transform.CompareTag("Medio")) Gizmos.color = Color.yellow;
            if (wayPoints1[i].transform.CompareTag("Rapido")) Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(wayPoints1[i].transform.position, 8);
        }
    }
}
