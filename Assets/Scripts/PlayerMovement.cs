using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayeryMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Rotation")]
    [SerializeField] private float rotateSpeed = 180f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Rigidbody rb;

    private float moveInput;
    private float rotateInput;

    [SerializeField] private float movingFullnessDrainPerSecond = 1f;

    PlayerController playerController;

    public Transform slowui;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!playerController.enabled)
        {
            animator.SetBool("walk", false);
            return;
        }
        if (playerController.stop)
        {
            animator.SetBool("walk", false);
            return;
        }
        // W/S : 앞뒤 이동
        moveInput = Input.GetAxisRaw("Vertical");

        // A/D : 좌우 회전
        rotateInput = Input.GetAxisRaw("Horizontal");

        if (animator != null)
        {
            bool isWalking = Mathf.Abs(moveInput) > 0.01f;
            animator.SetBool("walk", isWalking);
        }
    }

    public bool slow;

    private void FixedUpdate()
    {
        if (!playerController.enabled)
        {
            return;
        }

        slowui.gameObject.SetActive(slow);

        Move();
        Rotate();
    }

    private void Move()
    {
        if (playerController.stop)
        {
            return;
        }

        Vector3 moveDirection = transform.forward * moveInput;
        Vector3 nextPosition = rb.position + moveDirection * (moveSpeed * (1 - (playerController.CurrentFullness / playerController.MaxFullness) * 0.55f) * (slow?0.5f:1)) * Time.fixedDeltaTime;

        if(moveDirection != Vector3.zero)
        {
            playerController.ChangeFull(-movingFullnessDrainPerSecond * Time.deltaTime);
        }

        rb.MovePosition(nextPosition);
    }

    private void Rotate()
    {
        if (playerController.stop)
        {
            return;
        }
        float rotationAmount = rotateInput * rotateSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);

        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}