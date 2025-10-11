using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionAsset playerControls;
    private InputAction moveAction;
    private InputAction interactAction;
    private Vector2 moveVector;

    PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        moveAction = playerControls.FindActionMap("Player").FindAction("Move");
        moveAction.performed += context => moveVector = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveVector = Vector2.zero;

        interactAction = playerControls.FindActionMap("Player").FindAction("Interact");
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
        playerMovement.HandleMovement(moveVector);
    }


    private void Interact(InputAction.CallbackContext context)
    {
        //if (!active) return;
        //playerInteraction.Interact();
    }
}
