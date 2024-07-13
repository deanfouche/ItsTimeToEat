using System.Collections;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    public float maxHungerLevel = 100f;
    public float hungerLevel = 0f;
    public float hungerIncrement = 5f;
    public float hungerRate = 4f;
    private float _nextHungerTick = 0f;
    [SerializeField]
    private HungerMeter _hungerMeter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CalculateHunger();
    }

    public void CalculateHunger()
    {
        // Increase hunger at regular intervals
        if (Time.time > _nextHungerTick)
        {
            _nextHungerTick = Time.time + hungerRate;
            hungerLevel += hungerIncrement;
            _hungerMeter.SetHunger(hungerLevel);
            Debug.Log($"Player hunger level = {hungerLevel}");
        }
    }
}