using UnityEngine;

public class Death : State
{
    Employee employee;
    [SerializeField] SpriteRenderer empSprite;
    [SerializeField] GameObject bloodSprite;
    [SerializeField] ParticleSystem deathParticles;
    [Range(-1, 1)][SerializeField] float groundOffset = 0;
    const string deathSound = "Death";

    StatusIconBruno statusIcon;

    void Awake()
    {
        employee = (Employee)stateMachine;
        statusIcon = GetComponentInChildren<StatusIconBruno>();

    }
    public override void OnExit()
    {
        return;
    }

    public override void OnStart()
    {
        Debug.Log("Employee has died");
        statusIcon.Default(); // set icon
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

    private void OnDrawGizmos()
    {
        Vector2 point = parent.transform.position + new Vector3(0, groundOffset, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(point, 0.05f);
    }
}
