using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Manager manager;
    public Camera camera;

    public Slider sensSlider;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private bool isGrounded;

    public float speed = 1f;
    public float sensitivity = 100f;
    public float jumpHeight = 5f;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float rollForce = 10f;
    public float rollSpeed = 1f;

    public float jumpGrace = 0.5f;

    public Vector3 airDrag = Vector3.zero;
    public Vector3 groundDrag = Vector3.zero;
    public Vector3 drag
    {
        get { return (isGrounded ? groundDrag : airDrag); }
    }

    private float xRot = 10;
    private bool rolling = false;
    private float lastJump = -100f;

    private Vector3 velocity;

    private bool optionsOpen = false;

    void Start()
    {
        character = GetComponent<CharacterController>();
        manager = Manager.instance;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (velocity.y < 0)
        {
            if (isGrounded)
                velocity.y = -2f;
            else
                velocity += Physics.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        
        if (velocity.y > 0 && !Input.GetButton("Jump"))
        {
            velocity += Physics.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        character.Move(velocity * Time.deltaTime);

        velocity.x /= 1 + drag.x * Time.deltaTime;
        velocity.y /= 1 + drag.y * Time.deltaTime;
        velocity.z /= 1 + drag.z * Time.deltaTime;

        camera.transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        DialogueManager dm = DialogueManager.instance;

        sensitivity = Mathf.Lerp(50, 500, sensSlider.value);

        if (dm.active)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.instance.Display();
            }
        }
        else if (manager.candyOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                manager.candyOpen = false;

                Transform ui = manager.candyUI;
                ui.GetComponent<Animator>().SetTrigger("close");
                StartCoroutine(DisableIn(ui, 2f));
            }
        }
        else if (manager.mainMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(StartSequence());
            }
        }
        else if (optionsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                sensSlider.transform.parent.gameObject.SetActive(false);
                optionsOpen = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else if (!rolling)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                sensSlider.transform.parent.gameObject.SetActive(true);
                optionsOpen = true;
                Cursor.lockState = CursorLockMode.None;
            }

            Look();
            Move();

            if (Input.GetKeyDown(KeyCode.E) && NPC.closeToPlayer.Count != 0)
                Interact();
        }
    }

    IEnumerator StartSequence()
    {
        manager.mainMenuAnim.SetTrigger("Start");

        yield return new WaitForSeconds(4);

        manager.mainMenuOpen = false;
        manager.flopsyStartRule.Trigger();
    }

    void Interact()
    {
        NPC closest = null;

        foreach (NPC npc in NPC.closeToPlayer)
        {
            if (closest == null || npc.PlayerDist() < closest.PlayerDist())
                closest = npc;
        }

        closest.TriggerDialogue();
    }

    void Move()
    {
        // Move Controls
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(Roll());

        Vector3 dir = transform.right * Input.GetAxis("Horizontal") +
                      transform.forward * Input.GetAxis("Vertical");

        Vector3 move = (dir.sqrMagnitude < 1 ? dir : dir.normalized) * speed * Time.deltaTime;

        character.Move(move);

        if (Input.GetButtonDown("Jump"))
            lastJump = Time.time;

        if (Time.time - lastJump < jumpGrace && isGrounded)
        {
            manager.Modify("timesJumped += 1");
            velocity += Vector3.up * Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
            lastJump = -1000;
        }

        // keep in bounds
        Vector2 x = Squish(transform.position);

        if (x.magnitude > manager.bounds && !(transform.position.y < -7f))
        {
            transform.position = Unsquish(x.normalized * manager.bounds, transform.position.y);
        }
    }

    private Vector2 Squish(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    private Vector3 Unsquish(Vector2 v, float h)
    {
        return new Vector3(v.x, h, v.y);
    }

    void Look()
    {
        // Look Controls
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot %= 360;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        transform.Rotate(mouseX * Vector3.up);
    }

    IEnumerator Roll()
    {
        rolling = true;
        float originalXRot = xRot;

        while (xRot - originalXRot < 200)
        {
            xRot += rollSpeed;
            yield return new WaitForSeconds(0.01f);
        }

        while (xRot - originalXRot < 359)
        {
            xRot += rollSpeed * 1f;
            yield return new WaitForSeconds(0.01f);
        }

        velocity += Vector3.Scale(transform.forward + transform.up, rollForce * new Vector3(
                                  (Mathf.Log(1f / (Time.deltaTime + 1)) / -Time.deltaTime),
                                  (Mathf.Log(1f / (Time.deltaTime + 1)) / -Time.deltaTime), 
                                  (Mathf.Log(1f / (Time.deltaTime + 1)) / -Time.deltaTime)));

        xRot -= 360;
        rolling = false;
    }

    public void UpdateSensitivity(float s)
    {
        sensitivity = Mathf.Lerp(50, 500, s);
    }

    IEnumerator DisableIn(Transform t, float time)
    {
        yield return new WaitForSeconds(time);

        t.gameObject.SetActive(false);
    }
}