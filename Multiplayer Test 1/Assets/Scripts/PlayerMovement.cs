using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private PlayerControl inputActions;
    private InputAction move;
    private InputAction mouseLook;

    private Rigidbody rb;

    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector3 forceDir = Vector3.zero;

    [SerializeField] private Camera playerCam;


    // Set up new input system
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Search for rigidbody to apply
        // forces to instead of serializing for prefab
        rb = this.GetComponent<Rigidbody>();

        // Enable input methods
        inputActions = new PlayerControl();
        move = inputActions.MovementControls.Move;
        mouseLook = inputActions.MovementControls.MouseLook;
        inputActions.MovementControls.Enable();
    }


    // Calculate movement at fixed rate per second
    private void FixedUpdate()
    {
        // Add to total force to apply to player dependent on WASD movement
        forceDir += move.ReadValue<Vector2>().x * GetCameraRight(playerCam) * movementForce;
        forceDir += move.ReadValue<Vector2>().y * GetCameraForward(playerCam) * movementForce;

        rb.AddForce(forceDir, ForceMode.Impulse);
        forceDir = Vector3.zero;

        // Clamp player speed so that application of force doesn't stack up
        // & speed doesn't drop when camera is not parallel to floor
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();
    }


    // Rotate player body relative to camera, dependent on WASD input
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }


    // Get velocity modifier for moving
    // perpendicular to cam
    private Vector3 GetCameraRight(Camera playerCam)
    {
        Vector3 right = playerCam.transform.right;
        right.y = 0;
        return right.normalized;
    }


    // Get velocity modifier for moving
    // directly towards or away from cam
    private Vector3 GetCameraForward(Camera playerCam)
    {
        Vector3 forward = playerCam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
}
