using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Carro : MonoBehaviour
{
    NavMeshAgent car;
    public GameObject carObj;
    public GameObject camObj;
    public GameObject targetObj;
    public float smooth;

    public GameObject[] wayPoints0;
    public GameObject[] wayPoints1;
    int waypointCount = 0;
    public int destiny = 0;
    public int lap = 1;
    public int maxLap = 3;
    int rota = 0;
    public int ranking;

    public GameObject[] wheels;
    public GameObject[] fWheels;

    public TextMeshProUGUI jpGUI;

    public GameObject camObjPos;
    public Transform[] camPos;
    int camID = 0;
    bool finish = false;

    public float timeCar = 0;
    public string timeCarString = "00:00:00";

    // Start is called before the first frame update
    void Start()
    {
        rota = Random.Range(0, 2);
        camObjPos.transform.localPosition = camPos[camID].position;
        camObjPos.transform.localRotation = camPos[camID].rotation;
        car = GetComponent<NavMeshAgent>();

        JpGUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(!finish) timeCar += Time.deltaTime * 1000f;

        if (lap > 3 && !finish) Chegada();

        if(rota == 0)
        {
            //Waypoints
            if (Vector3.Distance(transform.position, wayPoints0[destiny].transform.position) < 8)
            {
                if (!finish) RandomizeCarPerformance(destiny);

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
                if(!finish) RandomizeCarPerformance(destiny);

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

        Billboard();
    }

    void Billboard()
    {
        Vector3 smoothedPosition = Vector3.Lerp(camObj.transform.position, camObjPos.transform.position, smooth * Time.deltaTime);
        camObj.transform.position = smoothedPosition;

        Vector3 direction = targetObj.transform.position - camObj.transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            camObj.transform.rotation = Quaternion.Slerp(camObj.transform.rotation, targetRotation, Time.deltaTime * (smooth/3));
            camObj.transform.rotation = targetRotation;
        }
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
            wheels[i].transform.Rotate(Time.deltaTime * 15 * car.speed, 0, 0);
        }
    }

    public void JpGUI()
    {
        if(jpGUI != null) jpGUI.text = $"Lap: {lap} / {maxLap}\n{car.speed} Km/h\n{ranking}º Lugar";
    }

    public void CamSwitch()
    {
        camID++;
        if (camID > camPos.Length - 1) camID = 0;
        camObjPos.transform.localPosition = camPos[camID].position;
        camObjPos.transform.localRotation = camPos[camID].rotation;
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
        timeCar = 0;
        if (rota == 0) car.SetDestination(wayPoints0[destiny].transform.position);
        if (rota == 1) car.SetDestination(wayPoints1[destiny].transform.position);
    }

    public void Chegada()
    {
        finish = true;
        smooth = 0.5f;
        camObjPos.transform.localPosition = camPos[3].position;
        camObjPos.transform.localRotation = camPos[3].rotation;
        int minutes = (int)(timeCar/60000);
        int seconds = (int)((timeCar / 1000) % 60);
        int mileseconds = (int)(timeCar % 1000);
        timeCarString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, mileseconds);
        car.speed = 40;
        car.acceleration = 9999;
        car.angularSpeed = 9999;
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
