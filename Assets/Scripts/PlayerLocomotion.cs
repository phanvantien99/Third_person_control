using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField]
    InputManager inputManager;

    [SerializeField]
    Vector3 moveDirection;

    [SerializeField]
    Transform cameraObject;

    [SerializeField]
    Rigidbody playerRigidbody;

    [SerializeField]
    float movementSpeed = 7;

    [SerializeField]
    float rotationSpeed = 5;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection =
            targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation =
            Quaternion
                .Slerp(transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
}
