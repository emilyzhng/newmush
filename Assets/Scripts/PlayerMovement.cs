using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    private float movementX;
    private float movementY;

    public static float moveSpeed = 120;
    public float jumpForce = 13;
    public bool jreq;
    public float gravity;

    public GameObject button;
    public GameObject canvas;
    public static GameObject currentmush;
    public GameObject SubmitButton;
    public GameObject InteractButton;
    public SFX sfx;
    public GameObject InspectButton;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Gets the Animator

        moveSpeed = 15;
        jreq = false;
        rb.useGravity = false;
        gravity = -50;
        jumpForce = 20;
        button.SetActive(false);
    }

    public void StepSFX()
    {
        sfx.FootStep();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * movementY + camRight * movementX;

        rb.linearVelocity = new Vector3(
            move.x * moveSpeed,
            rb.linearVelocity.y,
            move.z * moveSpeed
        );

        // Rotate toward movement direction
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.fixedDeltaTime
            );
        }

        // Update Animator Speed parameter
        float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        animator.SetFloat("Speed", speed);

        rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);

        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.up * gravity / 2, ForceMode.Acceleration);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jreq)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                jumpForce,
                rb.linearVelocity.z
            );

            jreq = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            jreq = true;
        }

        if (other.CompareTag("mushroom"))
        {
            Debug.Log("hello enter premise");
            currentmush = other.gameObject;
            Debug.Log("Stored mushroom: " + currentmush.name);
            button.SetActive(true);
        }

        if (other.CompareTag("home"))
        {
            SubmitButton.SetActive(true);
        }

        if (other.CompareTag("interact"))
        {
            InteractButton.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Debug.Log("Grounded!");
            jreq = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            jreq = false;
        }
    }

    public void DestroyMush()
    {
        Debug.Log("Current mush is: " + currentmush);

        if (currentmush != null)
        {
            Destroy(currentmush);
            currentmush = null;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("mushroom"))
        {
            button.SetActive(false);
            currentmush = null;
        }

        if (other.CompareTag("interact"))
        {
            InspectButton.SetActive(false);
        }

        if (other.CompareTag("home"))
        {
            SubmitButton.SetActive(false);
        }
    }
}