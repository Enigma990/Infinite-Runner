using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance = null;
    public static PlayerController Instance { get { return instance; } }

    //Movement Info
    [SerializeField] private float speed = 5f;
    private float speedMultiplier = 100f;
    bool isJumping = false;
    bool isRolling = false;

    //Player Data
    private Rigidbody playerRB = null;
    private Animator playerAnim = null;

    //Touch Control
    private float maxSwipeTime = 0.5f;
    private float maxSwipeDistance = 50f;
    private float swipeLength;

    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;

    private Vector3 startSwipePosition;
    private Vector3 endSwipePosition;

    // Score Data
    [SerializeField] private Text scoreText;

    private int score = 0;
    public int Score { get { return score; } }

    //Death Menu
    [SerializeField] private GameObject deathMenu = null;

    private void Awake()
    {
        instance = this;

        playerRB = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Resetting Gametime and score
        Time.timeScale = 1f;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //Player Forward Movement
        playerRB.velocity = new Vector3(0, playerRB.velocity.y, speedMultiplier * speed * Time.deltaTime);
    }

    void PlayerInput()
    {
//--------------------------Touch Control-------------------------------------------
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    swipeStartTime = Time.time;
                    startSwipePosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    swipeEndTime = Time.time;
                    endSwipePosition = touch.position;

                    swipeTime = (swipeEndTime - swipeStartTime);
                    swipeLength = (endSwipePosition - startSwipePosition).magnitude;

                    if (swipeTime < maxSwipeTime && swipeLength > maxSwipeDistance)
                    {
                        PlayerMovement();
                    }

                    break;
            }
        }
//-------------------------------------------------------------------------------------


//----------------------------Editor Controls------------------------------------------

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping && !isRolling)
        {
            StartCoroutine(Jump());
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !isJumping && !isRolling)
        {
            StartCoroutine(Roll());
            
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
        }
#endif 

//---------------------------------------------------------------------------------------
    }

    public void UpdateSpeed()
    {
        speed += 2;
    }

    void PlayerMovement()
    {
        //Calculating Swipe Distance
        Vector3 distance = endSwipePosition - startSwipePosition;
        float xDistance = Mathf.Abs(distance.x);
        float yDistance = Mathf.Abs(distance.y);

        //Checking if swipe is horizontal or vertical
        if (xDistance > yDistance)
        {
            if (distance.x > 0)
            {
                //MoveRight
                transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
            }
            else if (distance.x < 0)
            {
                // Move Left
                transform.position = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);
            }
        }
        else if (yDistance > xDistance && !isJumping && !isRolling)
        {
            if (distance.y > 0)
            {
                //Jump
                StartCoroutine(Jump());
            }
            else if (distance.y < 0)
            {
                //Roll
                StartCoroutine(Roll());
            }
        }
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0f);
        isJumping = true;
        playerRB.AddForce(transform.up * 5f, ForceMode.Impulse);
        playerAnim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.7f);
        isJumping = false;
    }

    IEnumerator Roll()
    {
        yield return new WaitForSeconds(0f);
        isRolling = true;
        playerAnim.SetTrigger("Roll");
        yield return new WaitForSeconds(0.9f);
        isRolling = false;
    }

    // Collectable Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectables"))
        {
            score += 1;
            other.gameObject.SetActive(false);
            scoreText.text = score.ToString();
        }
    }

    // Obstacles Collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Time.timeScale = 0f;

            deathMenu.SetActive(true);
            deathMenu.GetComponentInChildren<Text>().text = "Score: " + score.ToString();
        }
    }
}
