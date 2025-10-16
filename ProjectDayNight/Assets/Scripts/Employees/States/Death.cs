using UnityEngine;

public class Death : State
{
    Employee employee;
    [SerializeField] SpriteRenderer empSprite;
    [SerializeField] GameObject bloodSprite;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] float groundOffset = -0.5f;
    const string deathSound = "Death";
    void Awake()
    {
        employee = (Employee)stateMachine;
    }
    public override void OnExit()
    {
        return;
    }

    public override void OnStart()
    {
        Debug.Log("Employee has died");
        employee.StateName = EmployeeState.Dead;
        EmployeeStatusUI.Instance.UpdateUI();
        deathParticles.Play();
        parent.transform.rotation = Quaternion.Euler(
            parent.transform.eulerAngles.x,
            parent.transform.eulerAngles.y,
            90f
        );
        parent.transform.position += new Vector3(0, groundOffset, 0);
        empSprite.sortingLayerName = "Floor";
        empSprite.sortingOrder = 1;
        employee.StopMoving();
        employee.GetComponent<EmployeeMotionManager>().enabled = false;
        employee.GetComponent<CharacterAnimationManager>().enabled = false;
        bloodSprite.SetActive(true);
        AudioManager.Instance.PlayOneShot(deathSound);
        parent.GetComponent<Collider2D>().enabled = false;
        employee.enabled = false;
    }

    public override void OnUpdate()
    {
        return;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
