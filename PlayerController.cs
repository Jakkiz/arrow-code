using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public GamePadState state;
    public GamePadState prevState;
    Rigidbody2D rigidBody;
    public GameObject rollParticle;
    public GameObject deathParticle;
    public GameObject poweredUpEffect;
    public GameObject aimStick;
    public Image littleArrow;
    public GameObject playerAnimator;
    private Fire fireScript;
    private CameraShakeController cameraShake;
    public const float StandardSpeed = 73f;
    public const float ChargingSpeed = 35f;
    float speed;
    float speedRotation = 12f;
    float moveHor;
    float moveVert;
    float aimHor;
    float aimVert;
    float angle;
    int powerArrowCount = 0;
    public bool canMove = true;
    public bool canRoll = true;
    public bool isRolling = false;
    Vector3 aimDir = new Vector3();
    public Vector3 dir = new Vector3();
    public int numberController;
    public Color32 color;           //color of the player
    public bool isDead = false;
    public bool isPushed = false;
    public bool isStunned = false;
    public bool isPoweredUp;
    public bool hasGravityArrow;
    bool canAim = true;
    float timer = 0f;
    float resTimer = 0f;
    float stunTimer = 0f;
    float pushTimer = 0f;
    float powerUpTimer = 0f;
    float tempTimer = 0f;
    GameObject dodgeParticle;
    GameObject[] powerUpParticleList;
    private GameObject gravityArrow = null;
    public int score;

    public InputManager MyManager;
    public GameMasterScript GameMaster;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShakeController>();
        GameMaster = GameObject.Find("Game Master").GetComponent<GameMasterScript>(); // CHANGE GAMEOBJ.FIND
        playerAnimator.GetComponent<SpriteRenderer>().color = color;
        littleArrow.color = color;
        fireScript = GetComponentInChildren<Fire>();
        rigidBody = GetComponent<Rigidbody2D>();
        speed = StandardSpeed;
        isPoweredUp = false;
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            PushStunHandler();
            if (!isStunned)
            {
                if(isPoweredUp && !hasGravityArrow)
                {
                    GetComponentInChildren<WeaponCharge>().weaponChargeValue = 0;
                    powerUpHandler();
                }
                if((hasGravityArrow || gravityArrow != null) && !isPoweredUp)
                {
                    GetComponentInChildren<WeaponCharge>().weaponChargeValue = 0;
                    GravityArrowHandler();
                }
                Movment();
                Aim();
                if(!isPushed)
                {
                    Roll();
                }
            }
            else
            {
                rigidBody.velocity = (dir * 0f);
                GetComponentInChildren<WeaponCharge>().weaponChargeValue = 0;
            }
        }
    }

    void Update()
    {
        Pause(); // pauses the game when you click start

        if (isDead)
        {
            HandleDeath();
        }
        if (isRolling)
        {
            GameObject particle = (GameObject)Instantiate(rollParticle, transform.position, transform.rotation);
            Destroy(particle, 0.4f);
        }
    }

    void HandleDeath()
    {
        playerAnimator.GetComponent<SpriteRenderer>().enabled = false;
        //aimStick.GetComponent<SpriteRenderer>().enabled = false;
        GetComponentInParent<BoxCollider2D>().enabled = false;
        gameObject.transform.position = new Vector3(300, 300, 0); // MOVE PLAYERS FAR AWAY NEEDS TO BE CHANGED
        Destroy(GetComponentInChildren<WeaponCharge>().MyArrow.gameObject, 1f);
        GetComponentInChildren<WeaponCharge>().weaponChargeValue = 0;
        isRolling = false;
    }

    public void HandleRespawn(int number)
    {
        Destroy(GetComponentInChildren<WeaponCharge>().MyArrow.gameObject);
        canRoll = true;
        isPoweredUp = false;
        playerAnimator.GetComponent<SpriteRenderer>().enabled = true;
        //aimStick.GetComponent<SpriteRenderer>().enabled = true;
        GetComponentInParent<BoxCollider2D>().enabled = true;
        gameObject.transform.position = GameObject.Find("SpawnPoint" + number).transform.position;
        isDead = false;
    }
    void Movment()
    {
        if (canMove && !isPushed)
        {
            moveHor = state.ThumbSticks.Left.X;
            moveVert = state.ThumbSticks.Left.Y;
            dir = new Vector3(moveHor, moveVert, 0f);
            rigidBody.AddForce(dir * speed);
        }
    }

    void Pause()
    {
        if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed)
        {
            GameMaster.PauseGame();
        }
    }

    void Aim()
    {
        if (canAim)
        {
            Quaternion oldRot = aimStick.transform.rotation;
            aimHor = state.ThumbSticks.Right.X;
            aimVert = -state.ThumbSticks.Right.Y;

            aimDir = new Vector3(aimHor, aimVert, 0f);
            if (aimHor != 0 || aimVert != 0)
            {
                angle = Mathf.Atan2(aimDir.x, aimDir.y) * 180 / Mathf.PI;
                Quaternion newRot = Quaternion.Euler(0, 0, angle - 90);
                aimStick.transform.rotation = Quaternion.Slerp(oldRot, newRot, Time.deltaTime * speedRotation);
            }
        }
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void Slow()
    {
        float slowTimer = 0;
        speed = ChargingSpeed;
        while (slowTimer < 0.2f)
        {
            canRoll = false;
            slowTimer = slowTimer + Time.deltaTime;
        }
        speed = StandardSpeed;

    }

    public void SetHalfSpeed()
    {
            speed = ChargingSpeed;
    }

    public void SetNormalSpeed()
    {
        speed = StandardSpeed;
    }

    public int ControllerNumber
    {
        set { numberController = value; }
        get { return numberController; }
    }

    public void Kill()
    {
        if(!isDead)
        {
            GameObject GMRef = GameObject.FindGameObjectWithTag("GameMaster");
            GMRef.GetComponent<GameMasterScript>().deadCount++;
            GameObject deathParticlex = (GameObject)Instantiate(deathParticle, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(deathParticlex, 2f);
            isDead = true;
            cameraShake.ShakeTheCamera();
            resTimer = 0;
        }
    }

    public void Roll()
    {
        timer += Time.deltaTime;
        if(state.Triggers.Left != 00 && canRoll == true)
        {
            float chargeValue = GetComponentInChildren<WeaponCharge>().weaponChargeValue;
            if (rigidBody.velocity.magnitude > 1 && chargeValue == 0)
            {
                timer = 0;
                canMove = false;
                canRoll = false;
                isRolling = true;
                rigidBody.velocity = (dir * 40f);
                Debug.Log("I rolled ");
            }
        }
        if (isRolling)
        {
            rigidBody.velocity = (dir * 10f);
        }
        if (timer > 0.4f)
        {
            isRolling = false;
        }
        if (timer > 0.55f)
        {
            canMove = true;
        }

        if (timer > 3f)
        {
            canRoll = true;
            timer = 0;
        }       
    }

    void powerUpHandler()
    {
        if (!isPoweredUp)
        {
            powerUpTimer = 0;
        }
        GameObject powerUpParticle = (GameObject)Instantiate(poweredUpEffect, transform.position, transform.rotation);
        Destroy(powerUpParticle, 1f);
        powerUpTimer += Time.deltaTime;
        GetComponentInChildren<WeaponCharge>().weaponChargeValue = 0;
        GetComponentInChildren<WeaponCharge>().UpdateSlider();
        if (isPoweredUp && powerArrowCount < 3 && powerUpTimer < 5)
        {
            if(state.Triggers.Right != 0 && tempTimer + 0.3f < powerUpTimer)
            {
                fireScript.FirePowerUp(powerArrowCount + 1);
                powerArrowCount = powerArrowCount + 1;
                tempTimer = powerUpTimer;
            }
        }
        else
        {
            isPoweredUp = false;
            powerArrowCount = 0;
            powerUpTimer = 0;
            tempTimer = 0;
        }
    }

    void GravityArrowHandler()
    {
        powerUpTimer += Time.deltaTime;
        if (hasGravityArrow && powerUpTimer < 4f && gravityArrow == null)
        {
            GameObject powerUpParticle = (GameObject)Instantiate(poweredUpEffect, transform.position, transform.rotation);
            Destroy(powerUpParticle, 1f);
            GetComponentInChildren<WeaponCharge>().weaponChargeValue = 0;
            if (state.Triggers.Right != 0 )
            {
                gravityArrow = fireScript.FireGravityArrow();
                powerUpTimer = 0;
                hasGravityArrow = false;
            }
        }
        else
        {
            if(hasGravityArrow)
            {
                gravityArrow = null;
                powerUpTimer = 0;
                hasGravityArrow = false;
                return;
            }
            if (powerUpTimer < 2f && powerUpTimer > 0.3f && gravityArrow != null)
            {
                GetComponentInChildren<WeaponCharge>().weaponChargeValue = 0;
                if (state.Triggers.Right != 0)
                {
                    gravityArrow.GetComponentInChildren<triggerScript>().Explode(numberController);
                    tempTimer = powerUpTimer;
                    gravityArrow = null;
                    powerUpTimer = 0;
                }
            }
        }
    }

    void PushStunHandler()
    {
        if (!isStunned)
        {
            stunTimer = 0;
        }
        if (!isPushed)
        {
            pushTimer = 0;
        }
        else
        {
            GetComponentInChildren<WeaponCharge>().weaponChargeValue = 0;
        }
        pushTimer += Time.deltaTime;
        stunTimer += Time.deltaTime;
        if (isStunned)
        {
            canMove = false;
            canAim = false;
        }
        if (stunTimer > 1.3f)
        {
            isStunned = false;
            canMove = true;
            canAim = true;
        }
        if (pushTimer > 0.5f)
        {
            isPushed = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (numberController != other.GetComponentInParent<PlayerController>().ControllerNumber)
            {
                if (isRolling)
                {
                    isRolling = false;
                    other.GetComponentInParent<PlayerController>().isPushed = true;
                    other.GetComponentInParent<Rigidbody2D>().velocity = (dir * 30f);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Arrow")
        {
            Physics2D.IgnoreCollision(coll.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    public void testDebug()
    {
        Debug.Log("Log from player " + numberController);
    }


    //void OnGUI()
    //{
    //    string text = "Use left stick to turn the cube, hold A to change color\n";
    //    text += string.Format("IsConnected {0} Packet #{1}\n", state.IsConnected, state.PacketNumber);
    //    text += string.Format("\tTriggers {0} {1}\n", state.Triggers.Left, state.Triggers.Right);
    //    text += string.Format("\tD-Pad {0} {1} {2} {3}\n", state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
    //    text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", state.Buttons.Start, state.Buttons.Back, state.Buttons.Guide);
    //    text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", state.Buttons.LeftStick, state.Buttons.RightStick, state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
    //    text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
    //    text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
    //    GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);
    //}
}