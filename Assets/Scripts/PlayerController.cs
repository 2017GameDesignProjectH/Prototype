using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;
    public GameObject cameraCollisionBox;
    public CollisionCounter cameraProbe;
    public CollisionCounter probe;
    public Animator PlayerAnimator;

    public JumpSensor JumpSensor;
    public float jumpspeed;
    public float movespeed;
    public float Maxdistance;
    public float verticalRatio;
    public bool Aim = false;

    private bool isHoriMove;
    private bool isVertiMove;
    private Vector3 horiVelocity;
    private Vector3 vertiVelocity;

    //FOR velocity in jump
    private float YVelocity;
    //FOR Continue Movement
    private float timer;

    public float getjumpspeed()
    {
        return YVelocity;
    }

    void CalculateHeight()
    {
        Vector3 originVector = mainCamera.transform.position;
        Vector3 localVector = mainCamera.transform.localPosition;

        // REMOVE Y to caculate XZ magnitude
        originVector.y = player.transform.position.y;

        float y = Maxdistance - (originVector - player.transform.position).magnitude;
        localVector.y = y;
        mainCamera.transform.localPosition = localVector * verticalRatio;
    }

    void Draw()
    {
        if (this.isHoriMove && this.isVertiMove)
        {
            player.GetComponent<Rigidbody>().velocity = this.horiVelocity + this.vertiVelocity;
        }
        else if (this.isHoriMove)
        {
            player.GetComponent<Rigidbody>().velocity = this.horiVelocity;
        }
        else if (this.isVertiMove)
        {
            player.GetComponent<Rigidbody>().velocity = this.vertiVelocity;
        }

        if (this.isHoriMove || this.isVertiMove)
        {
            // Caculate the player's degree from velocity
            float rotate = Mathf.Atan2(player.GetComponent<Rigidbody>().velocity.x, player.GetComponent<Rigidbody>().velocity.z);
            player.transform.rotation = Quaternion.Euler(0, rotate / Mathf.PI * 180, 0);

            // Let player move continue a little bit
            this.timer = 0.1f;
        }
        if (this.timer >= 0)
        {
            this.timer -= Time.deltaTime;
        }
        else
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        // Reset Y axis velocity to origin
        player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, this.YVelocity, player.GetComponent<Rigidbody>().velocity.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Q))
        {
            Aim = !Aim;
        }
        if (Aim)
        {
            player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            return;
        }
        // GET INPUT AXIS
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        // GET DIRECTION
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        //Calibrate Y AXIS 
        forward.y = 0;
        right.y = 0;
        // GET DIRECTION UNIT VECTOR
        forward.Normalize();
        right.Normalize();

        Vector3 distance = (mainCamera.transform.position - player.transform.position);
        distance.y = 0;

        //RESET ROTATION 
        this.isHoriMove = false;
        this.isVertiMove = false;
        this.horiVelocity = Vector3.zero;
        this.vertiVelocity = Vector3.zero;

        this.YVelocity = player.GetComponent<Rigidbody>().velocity.y;

        // check jump
        if ( (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button1) ) && JumpSensor.IsCanJump())
        {
            YVelocity = jumpspeed;
            //PlayerAnimator.SetTrigger("jump");
            
            PlayerAnimator.SetFloat("jumpspeed", 1);
        } else {
            PlayerAnimator.SetFloat("jumpspeed", 0);
            
        }

        if (vertical > 0)
        {
            this.vertiVelocity = forward * movespeed;
            //Camera chases the player
            if (distance.magnitude > Maxdistance)
            {
                float originY = cameraCollisionBox.transform.position.y;
                Vector3 newPosition = player.transform.position + distance.normalized * Maxdistance;
                newPosition.y = originY;
                cameraCollisionBox.transform.position = newPosition;
            }

            CalculateHeight();
            isVertiMove = true;

        }
        else if (vertical < 0)
        {
            this.vertiVelocity = -forward * movespeed;
            if (cameraProbe.counter <= 0)
            {
                //Camera chases the player
                float originY = cameraCollisionBox.transform.position.y;
                Vector3 newPosition = player.transform.position + distance.normalized * Maxdistance;
                newPosition.y = originY;
                cameraCollisionBox.transform.position = newPosition;
            }
            else
            {
                //Camera move higher to see the player
                CalculateHeight();
            }

            isVertiMove = true;
        }

        if (horizontal > 0)
        {
            if (probe.counter > 0)
            {
                cameraCollisionBox.transform.position += -right * movespeed * Time.deltaTime;
            }
            this.horiVelocity = right * movespeed;
            isHoriMove = true;
        }
        else if (horizontal < 0)
        {
            if (probe.counter > 0)
            {
                cameraCollisionBox.transform.position += right * movespeed * Time.deltaTime;
            }
            this.horiVelocity = -right * movespeed;
            isHoriMove = true;
        }

        // perform the result on charactor
        Draw();

    }
}