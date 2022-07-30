using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    PlayerController playerControls;

    AnimatorManager animatorManager;
    float moveAmount;


    [SerializeField]
    Vector2 movementInput;

    [SerializeField]
    Vector2 cameraInput;

    [SerializeField]
    public float cameraInputX;
    [SerializeField]
    public float cameraInputY;

    public float verticalInput;

    public float horizontalInput;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerController();
            playerControls.PlayerMovement.Movement.performed += i =>
                movementInput = i.ReadValue<Vector2>();

            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.Enable();
        }
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimator(0, moveAmount);
    }
}
