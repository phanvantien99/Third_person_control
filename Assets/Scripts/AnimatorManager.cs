using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{

    [SerializeField]
    Animator animator;

    [SerializeField]
    int horizontal;

    [SerializeField]
    int vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimator(float horizontalMovement, float verticalMovement)
    {

        //Animation snapping
        float snappedHorizontal = SnappedVertex(horizontalMovement);
        float snappedVertical = SnappedVertex(verticalMovement);

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }


    float SnappedVertex(float vertexMovement)
    {
        if (vertexMovement > 0 && vertexMovement < 0.55f)
            return 0.5f;
        else if (vertexMovement > 0.55f)
            return 1;
        else if (vertexMovement < 0 && vertexMovement > -0.55f)
            return -0.5f;
        else if (vertexMovement < -0.55f)
            return -1;
        return 0;

    }
}
