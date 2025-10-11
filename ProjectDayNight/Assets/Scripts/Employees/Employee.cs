using Unity.VisualScripting;
using UnityEngine;

public class Employee : StateMachineBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int wage { get; private set; } = 10;
    public int morale { get; private set; } = 100;
    public bool working { get; private set; } = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
