using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour {

    public JumpSensor myJumpDetect;
    public GameObject myPlayer;
    public GameObject myCamera;
    public GameObject cameraCollisionBox;
    public Animator PlayerAnimator;
    public float pushSpeed;
    public float moveSpeed;
    public float jumpSpeed;
    public float playerGravity;
    public float k;

    //Private Parameter
    private float timer;
    private float jumpTimer;
    private bool isJumping;
    private bool isAddGravity = false;

    private void Start()
    {

    }

    public float getjumpspeed()
    {
        if (isJumping)
            return 1f;
        else
            return 0f;
    }

    void Update()
    {

        //Debug.Log("Can Jump?" + myJumpDetect.IsCanJump());
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //Top Down Camera 
        //Vector3 forward = this.transform.forward;
        //Third Person Follow Camera
        Vector3 forward = myCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        //Vector3 right = this.transform.right;
        //Third Person Follow Camera
        Vector3 right = myCamera.transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 distance = (myCamera.transform.position - myPlayer.transform.position);
        distance.y = 0; ;


        myPlayer.GetComponent<Rigidbody>().AddForce(Vector3.down * playerGravity);


        if (vertical != 0)
        {
            if (myJumpDetect.IsCanJump()) { myPlayer.GetComponent<Rigidbody>().AddForce(forward * vertical * pushSpeed); }
            else { myPlayer.GetComponent<Rigidbody>().AddForce(forward * vertical * pushSpeed * 0.1f); }

            float rotate = Mathf.Atan2(myPlayer.GetComponent<Rigidbody>().velocity.x, myPlayer.GetComponent<Rigidbody>().velocity.z);
            myPlayer.transform.rotation = Quaternion.Euler(0, rotate / Mathf.PI * 180, 0);
        }

        if (horizontal != 0)
        {
            if (myJumpDetect.IsCanJump()) { myPlayer.GetComponent<Rigidbody>().AddForce(right * horizontal * pushSpeed); }
            else { myPlayer.GetComponent<Rigidbody>().AddForce(right * horizontal * pushSpeed * 0.1f); }

            float rotate = Mathf.Atan2(myPlayer.GetComponent<Rigidbody>().velocity.x, myPlayer.GetComponent<Rigidbody>().velocity.z);
            myPlayer.transform.rotation = Quaternion.Euler(0, rotate / Mathf.PI * 180, 0);
        }

        /* 
         if ((vertical != 0 || horizontal != 0) && myJumpDetect.IsCanJump())
         {
             float rotate = Mathf.Atan2(myPlayer.GetComponent<Rigidbody>().velocity.x, myPlayer.GetComponent<Rigidbody>().velocity.z);
             myPlayer.transform.rotation = Quaternion.Euler(0, rotate / Mathf.PI * 180, 0);
         }

         */

        if (myPlayer.GetComponent<Rigidbody>().velocity.magnitude > moveSpeed)
        {
            myPlayer.GetComponent<Rigidbody>().velocity = myPlayer.GetComponent<Rigidbody>().velocity / myPlayer.GetComponent<Rigidbody>().velocity.magnitude * moveSpeed;
        }

        if (vertical == 0 && horizontal == 0 && myJumpDetect.IsCanJump())
        {
            myPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Space) && myJumpDetect.IsCanJump())
        {
            myPlayer.GetComponent<Rigidbody>().velocity = new Vector3(myPlayer.GetComponent<Rigidbody>().velocity.x * k, jumpSpeed, myPlayer.GetComponent<Rigidbody>().velocity.z * k);
            isJumping = true;
            PlayerAnimator.SetFloat("jumpspeed", 1);
        }
        else
        {
            isJumping = false;
            PlayerAnimator.SetFloat("jumpspeed", 0);
        }

        if (isJumping && !myJumpDetect.IsCanJump())
        {
            isAddGravity = true;
            int add = 1;
            jumpTimer += add * Time.deltaTime;

            // if(jumpTimer > 0.994f)
            // {
            //     playerGravity = 10;
            // }            
        }

        if (isAddGravity && myJumpDetect.IsCanJump())
        {
            //Debug.Log("Here");
            isAddGravity = false;
            playerGravity = 20.47f;
            isJumping = false;
            jumpTimer = 0;
        }

    }
}
