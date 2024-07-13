using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FoodCounter : MonoBehaviour
{
    public FoodInventory inventory;
    public GameObject CounterDisplay;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Text foodCounter = CounterDisplay.GetComponent<Text>();
        foodCounter.text = $"{inventory.foodCount}";
    }
}