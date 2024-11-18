using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 100f;
    public float currentSpeed = 0f;

    private Rigidbody2D rb;
    public GameObject fishingBubble;
    public GameObject dialogueBubble;
    private Quaternion fixedRotationBubble1;
    private Quaternion fixedRotationBubble2;
    private Quaternion fixedRotationEye;
    public Vector3 bubbleOffset = new(0, 1f, 0);
    public Vector3 EyeOffset = new(0, -1f, 0);

    [SerializeField] bool controlEnabled = true;
    public FishingMinigame fishingMinigame;

    public EyeControl Eye;
    //private bool inSharkZone = false;

    void Start()
    {
        controlEnabled = true;
        rb = GetComponent<Rigidbody2D>();
        if (fishingBubble != null)
        {
            fixedRotationBubble1 = fishingBubble.transform.rotation;
            fishingBubble.SetActive(false);
        }
        if (Eye != null)
        {
            fixedRotationEye = Eye.transform.rotation;
            Eye.SetEyeActive(false);
        }
    }

    public void ToggleDialogueBubble(bool show)
    {
        fishingBubble.SetActive(show);
    }

    public void Update()
    {
        if (fishingBubble != null && fishingBubble.activeSelf)
        {
            fishingBubble.transform.position = transform.position + bubbleOffset;
            fishingBubble.transform.rotation = fixedRotationBubble1;
        }
        if (Eye != null && Eye.isActiveNow)
        {
            Eye.transform.position = transform.position + EyeOffset;
            Eye.transform.rotation = fixedRotationEye;
        }
    }

    void FixedUpdate()
    {
            if (controlEnabled && !fishingMinigame.isFishingActive)
            {
                // Control de velocidad
                if (Input.GetKey(KeyCode.W))
                {
                    currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, -maxSpeed, maxSpeed);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    currentSpeed = Mathf.Clamp(currentSpeed - acceleration * Time.deltaTime, -maxSpeed, maxSpeed);
                }
                else
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * 0.5f);
                }
            }

            if (controlEnabled && !fishingMinigame.isFishingActive) { 
                    // Rotación del barco
                    float rotationInput = 0f;
                if (Input.GetKey(KeyCode.A))
                {
                    rotationInput = 1f;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    rotationInput = -1f;
                }
                transform.Rotate(0, 0, rotationInput * rotationSpeed * Time.deltaTime);
            }
            // Asigna la velocidad directamente al Rigidbody2D para que pueda calcularla
            Vector2 moveDirection = transform.up * currentSpeed;
            rb.velocity = moveDirection;
    }

    public void SetControlEnabled(bool enabled)
    {
        controlEnabled = enabled;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SharkLayer"))
        {
            //inSharkZone = true;
            Eye.SetEyeActive(true);
            Eye.SetEyeState(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SharkLayer"))
        {
            //inSharkZone = false;
            Eye.SetEyeActive(false);
        }
    }
}
