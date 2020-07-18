using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed;
  public Rigidbody2D myRigidbody;
  public float jumpHeight;
  public bool isGrounded;
  public float decreasePerMinute;
  public int BananaScore = 10;
  public bool isSane;
  public float berserkTime = 20f;
  public float randomInputInterval = 0.5f;
  private Animator myAnim;
  public GameObject treeSpring;

  // Movement Constraints

  private Vector2 screenBounds;
  private float objectWidth;

  // Health and Potassium
  public float playerHealth;
  public float maxHealth;
  public Image healthBar;
  private bool hurt;
  public float potassiumLevel;
  public float maxPotassium;
  public Image barImage;

  // Sounds

  public AudioSource audio;
  public AudioClip jump;
  public AudioClip impact;
  public AudioClip pickup;

  public GameController GM;
  public CameraController camera;
  // Start is called before the first frame update
  void Start()
  {
    // Retrieving Components
    myRigidbody = GetComponent<Rigidbody2D>();
    GM = GameObject.Find("GM").GetComponent<GameController>();
    camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
    myAnim = GetComponent<Animator>();

    // Setting up for movement constraint
    screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;

    // Making the player sane
    isSane = true;
    myAnim.SetBool("isSane", true);
  }

  // Update is called once per frame
  void Update()
  {
    //Basic Movement, handles directional input and orientation of sprite.
    if (Input.GetAxisRaw("Horizontal") > 0f)
    {
      myRigidbody.velocity = new Vector3(moveSpeed, myRigidbody.velocity.y, 0f);
      transform.localScale = new Vector3(0.07f, 0.07f, 0f);
    }
    else if (Input.GetAxisRaw("Horizontal") < 0f)
    {
      myRigidbody.velocity = new Vector3(-moveSpeed, myRigidbody.velocity.y, 0f);
      transform.localScale = new Vector3(-0.07f, 0.07f, 0f);
    }
    else
    {
      myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
    }
    //Jump function
    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpHeight, 0f);
      audio.clip = jump;
      audio.Play();
      isGrounded = false;
    }

    myAnim.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));

    if (potassiumLevel > maxPotassium)
    {
      potassiumLevel = maxPotassium;
    }

    if (!isSane)
    {
      healthBar.fillAmount = playerHealth / maxHealth;
      playerHealth -= Time.deltaTime * decreasePerMinute / 0.05f;

      if (playerHealth < 0) playerHealth = 0;
    }

    if (playerHealth > 0)
    {
      if (potassiumLevel > 0 && isSane)
      {
        potassiumLevel -= Time.deltaTime * decreasePerMinute / 0.05f;

        if (potassiumLevel < 0) potassiumLevel = 0;

        barImage.fillAmount = potassiumLevel / maxPotassium;
      }
      else if (potassiumLevel == 0)
      {
        isSane = false;
        myAnim.SetBool("isSane", false);

        if (!isSane)
        {
          StartCoroutine(EnterBerserkMode());
        }
      }
    }
    else
    {
      KillPlayer();
    }

  }

  // Movement Restrictions on X axis

  void LateUpdate()
  {
    Vector3 viewPos = transform.position;
    viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
    transform.position = viewPos;
  }

  //Makes sure the player can jump again when they hit the ground
  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "ground")
    {
      isGrounded = true;
      audio.clip = impact;
      audio.Play();
    }
    if (other.gameObject.tag == "spikes")
    {
      myRigidbody.velocity = new Vector3(-3.0f, 3.0f, 0f);
      playerHealth -= 5;
      healthBar.fillAmount = playerHealth / maxHealth;
      Debug.Log("Ouch");
    }
    if (other.gameObject.tag == "spring")
    {
      myRigidbody.velocity = new Vector3(0f, 8.0f, 0f);
      treeSpring.GetComponent<Animator>().Play("tree_spring", -1, 0f);
      Debug.Log("Boing");
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    //This is where the picking up banana logic should go
    if (other.tag == "banana")
    {
      GM.AddScore(BananaScore);
      audio.clip = pickup;
      audio.Play();
      if (!isSane)
      {
        potassiumLevel = maxPotassium;
        // Debug.Log("Got lots");
        isSane = true;
        myAnim.SetBool("isSane", true);
        camera.ToggleShaking(false);
        StopAllCoroutines();
      }
      else
      {
        potassiumLevel += 10;
        // Debug.Log("Got Ten");
      }
      GM.availableSpawnPositions.Add(other.gameObject.transform.position);
      GM.availableBananas.Remove(other.gameObject);
      Destroy(other.gameObject);
    }
    else if (other.tag == "deathPlane")
    {
      KillPlayer();
    }
  }

  IEnumerator EnterBerserkMode()
  {
    potassiumLevel = maxPotassium;
    // Debug.Log("Gone Bananas!");
    camera.setShakeTime(berserkTime, .7f);
    camera.ToggleShaking(true);
    for (int i = Mathf.RoundToInt(berserkTime / randomInputInterval); i > 0; i--)
    {
      yield return new WaitForSeconds(randomInputInterval);
      RandomInput();
    }
    // Debug.Log("Okay, calm down...");
    camera.ToggleShaking(false);
    isSane = true;
    myAnim.SetBool("isSane", true);

  }

  void RandomInput()
  {
    int number = Random.Range(1, 7);

    int verticalFactor = 300;
    int horizontalFactor = 2000;

    switch (number)
    {
      case 1:
        myRigidbody.AddForce(Vector2.up * verticalFactor);
        Debug.Log("bumped up");
        break;

      case 2:
        myRigidbody.AddForce(Vector2.left * horizontalFactor);
        Debug.Log("bumped back");
        break;

      case 3:
        myRigidbody.AddForce(Vector2.right * horizontalFactor);
        Debug.Log("bumped forward");
        break;

      case 4:
        myRigidbody.AddForce(Vector2.left * horizontalFactor);
        potassiumLevel -= 10;
        Debug.Log("hurt and bumped back");
        break;

      case 5:
        myRigidbody.AddForce(Vector2.up * verticalFactor);
        potassiumLevel += 10;
        Debug.Log("healed and bumped up");
        break;

      case 6:
        myRigidbody.AddForce(Vector2.down * verticalFactor);
        Debug.Log("bumped down");
        break;

      case 7:
        myRigidbody.AddForce(Vector2.down * verticalFactor);
        potassiumLevel -= 10;
        Debug.Log("hurt and bumped down");
        break;

      default:
        Debug.Log("Not the bananas you're looking for");
        break;
    }
  }

  void KillPlayer()
  {
    StopAllCoroutines();
    camera.ToggleShaking(false);
    GM.GameOver();
    Destroy(this.gameObject);
  }

}
