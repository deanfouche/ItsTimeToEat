using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hunger = 0f;
    public float hungerIncrement = 5f;
    public float hungerRate = 4f;
    private float _nextHungerTick = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CalculateHunger();
    }

    void CalculateHunger()
    {
        // Increase hunger at regular intervals
        if (Time.time > _nextHungerTick)
        {
            _nextHungerTick = Time.time + hungerRate;
            hunger += hungerIncrement;
        }
    }
}
