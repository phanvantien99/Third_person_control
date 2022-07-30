using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [Header("Target")]
    [SerializeField]
    InputManager inputManager;
    [SerializeField] Transform targetTransform; // the target camera'll follow;
    [SerializeField] Transform cameraTransform; // Actual camera object in the scence (See in Hierachy (*this object -> camera pivot -> MAIN CAMERA*))
    [SerializeField] Transform cameraPivot; // the object camera used to pivot(up/down)

    [Header("Handle Collision")]
    [SerializeField] LayerMask collistionLayers;
    [SerializeField] float cameraCollisionOffset = 0.2f; // How much camera'll jump off of the objects its colliding with
    [SerializeField] float minimumCollisionOffset = 0.2f;
    Vector3 cameraVectorPosition;

    Vector3 cameraVelocity = Vector3.zero;
    float defaultPosition;


    [HideInInspector]
    float lookAngle; // Look up/down

    [HideInInspector]
    float pivotAngle; // look left/right

    [Header("Speed")]
    [SerializeField]
    float cameraSpeed = 0.2f;
    [SerializeField]
    float cameraLookSpeed = 2;
    [SerializeField]
    float cameraPivotSpeed = 2;


    [Header("Limit Y Angle")]
    [SerializeField]
    float minimunPivotAngle = -90;
    [SerializeField]
    float maximunPivotAngle = 90;

    private void Awake()
    {
        inputManager = targetTransform.GetComponent<InputManager>();
        defaultPosition = cameraTransform.position.z;
    }

    public void HandleAllCamera()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }

    void FollowTarget()
    {
        // function for follow camera to target
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraVelocity, cameraSpeed);
        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        lookAngle += inputManager.cameraInputX * cameraLookSpeed;
        pivotAngle -= inputManager.cameraInputY * cameraPivotSpeed;

        pivotAngle = Mathf.Clamp(pivotAngle, minimunPivotAngle, maximunPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    void HandleCameraCollision()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();
        if (Physics.SphereCast
            (cameraPivot.transform.position, 2, direction, out hit, Mathf.Abs(targetPosition), collistionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =-(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
