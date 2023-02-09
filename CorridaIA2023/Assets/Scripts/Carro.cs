using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carro : MonoBehaviour
{
    NavMeshAgent car;

    public GameObject destiny;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<NavMeshAgent>();

        car.SetDestination(destiny.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
