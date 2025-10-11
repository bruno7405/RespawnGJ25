using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerInteractor))]
public class PlayerInput : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;
    private InputAction moveAction;
    private InputAction interactAction;
    private Vector2 moveVector;

    PlayerMovement playerMovement;
    PlayerInteractor playerInteractior;

    public static bool active = true;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteractior = GetComponent<PlayerInteractor>();

        // Move Action
        moveAction = playerControls.FindActionMap("Player").FindAction("Move");
        moveAction.performed += context => moveVector = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveVector = Vector2.zero;

        // Interact Action
        interactAction = playerControls.FindActionMap("Player").FindAction("Interact");
        interactAction.performed += Interact;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        interactAction.Enable();

        interactAction.performed += Interact;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        interactAction.Disable();

        interactAction.started -= Interact;
    }

    private void Update()
    {
        if (!active) return;
        playerMovement.HandleMovement(moveVector);
    }


    private void Interact(InputAction.CallbackContext context)
    {
        if (!active) return;
        playerInteractior.Interact();
    }
}
