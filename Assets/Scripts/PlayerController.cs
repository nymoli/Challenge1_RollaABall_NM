using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, @RollABallControls.IPlayerActions
{
    public float speed;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public @RollABallControls controls;
    public Vector2 motion;

    private Rigidbody rb;
    private int count;
    private int lives;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        lives = 3;

        SetCountText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
    }

    public void OnEnable()
    {
        if (controls == null)
        {
            controls = new @RollABallControls();

            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        motion = context.ReadValue<Vector2>();
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 18)
        {
            winTextObject.SetActive(true);
        }

        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            loseTextObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(motion.x, 0.0f, motion.y);
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;
            SetCountText();
        }

        if (count == 10)
        {
            transform.position = new Vector3(75.0f, 0.0f, 75.0f);
        }

        if (lives == 0)
        {
            Destroy(gameObject);
        }
    }
}