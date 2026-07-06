using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movimiento Basico")]
    [SerializeField] private float speed = 5f;

    [SerializeField] private InputAction moveAction;
    private Rigidbody2D rb;
    private float horizontalInput;

    private void OnEnable()
    {
        moveAction.Enable();

    }
    private void OnDisable()
    {
        moveAction.Disable();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        horizontalInput = moveVector.x;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
    }
}
