using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMove : MonoBehaviour
{
    private InputAction MoveAction;
    private InputAction SprintAction;
    private InputAction JumpAction;
    [SerializeField]private Transform cameraTransform;
    private CharacterController characterController;

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    private float jumpHeight = 2f;

    private float speed;
    private bool isGrounded;

    private Vector3 velocity;
    private float gravity = -9.81f;

    public Animator animator;
    void Awake()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
        SprintAction = InputSystem.actions.FindAction("Sprint");
        JumpAction = InputSystem.actions.FindAction("Jump");
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        speed = walkSpeed;
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        Vector2 input = MoveAction.ReadValue<Vector2>();
        Vector3 move = (cameraTransform.right * input.x + cameraTransform.forward * input.y).normalized;
        move.y = 0;
        speed = SprintAction.IsPressed() ? runSpeed : walkSpeed;

        if (MoveAction.IsPressed())
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else if (SprintAction.IsPressed() && MoveAction.IsPressed())
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }

        characterController.Move(move * speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        if (JumpAction.WasPressedThisFrame() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
