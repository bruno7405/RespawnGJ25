using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAttack))]
public class PlayerInput : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;
    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction attackAction;
    private InputAction mapAction;

    private Vector2 moveVector;

    PlayerMovement playerMovement;
    PlayerInteractor playerInteractor;
    PlayerAttack playerAttack;

    public static bool active = true;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteractor = GetComponentInChildren<PlayerInteractor>();
        playerAttack = GetComponent<PlayerAttack>();

        // Move Action
        moveAction = playerControls.FindActionMap("Player").FindAction("Move");
        moveAction.performed += context => moveVector = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveVector = Vector2.zero;

        // Interact Action
        interactAction = playerControls.FindActionMap("Player").FindAction("Interact");

        // Attack Action
        attackAction = playerControls.FindActionMap("Player").FindAction("Attack");

        // Map Action
        mapAction = playerControls.FindActionMap("Player").FindAction("Map");


    }

    private void OnEnable()
    {
        moveAction.Enable();
        interactAction.Enable();
        attackAction.Enable();
        mapAction.Enable();


        interactAction.performed += Interact;
        attackAction.performed += Attack;
        mapAction.performed += ToggleMap;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        interactAction.Disable();
        attackAction.Disable();
        mapAction.Disable();

        interactAction.started -= Interact;
        attackAction.started -= Attack;
        mapAction.started -= ToggleMap;
    }

    private void Update()
    {
        if (!active) return;
        playerMovement.HandleMovement(moveVector);
    }


    private void Interact(InputAction.CallbackContext context)
    {
        if (!active) return;
        playerInteractor.Interact();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!active) return;
        playerAttack.Attack();
    }

    private void ToggleMap(InputAction.CallbackContext context)
    {
        if (!active) return;
        MinimapManager.Instance.Toggle();
    }
}
