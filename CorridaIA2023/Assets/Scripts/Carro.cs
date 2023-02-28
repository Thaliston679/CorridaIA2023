using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carro : MonoBehaviour
{
    NavMeshAgent car;

    public GameObject[] wayPoints;
    public int destiny = 0;

    private float randomPerformance = 5;

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
        if(Vector3.Distance(transform.position, wayPoints[destiny].transform.position) < 3)
        {
            destiny++;
            if (destiny > wayPoints.Length - 1) destiny = 0;

            car.SetDestination(wayPoints[destiny].transform.position);
        }

        //Velocidade e aceleração aleatória
        if (randomPerformance >= 0) randomPerformance -= Time.deltaTime;
        if (randomPerformance <= 0)
        {
            RandomizeCarPerformance();
        }
        //--
    }

    void RandomizeCarPerformance()
    {
        randomPerformance = 5;
        car.acceleration = Random.Range(4, 8);
        car.speed = Random.Range(10, 17);
    }
}
