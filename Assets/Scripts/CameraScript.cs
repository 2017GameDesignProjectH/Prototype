using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraScript : MonoBehaviour
{
    // Public Parameter
    public GameObject Ring;
    public Transform Player;
    public Transform Boss;
    public float focaldistance;
    public float BossPlayerDistance;
    public float boundary;
    public float currentY = 40f;
    public float currentX = 0f;
    public float minY;
    public float maxY;
    public float minfocalY;
    public float maxfocalY;
    public float AimDistance;
    public float AimFixY;

    //Private Parameter
    private Transform camTransform;
    private Camera cam;
    private float Recordfocaldistance;
    private bool Locked = true;
    private bool AimMode = false;
    private float LerpTime = 1;


    // Use this for initialization
    void Start()
    {
        camTransform = this.transform;
        cam = Camera.main;
        Recordfocaldistance = focaldistance;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            Locked = !Locked;
            //StartCoroutine(TransformCamera());
            //LerpTime = 0.05f;
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.Q))
        {
            SetAim(!AimMode);
            LerpTime = 0.05f;
        }
        currentX += Input.GetAxis("JLHorizontal");
        currentY += Input.GetAxis("IKVertical");
        currentY = Mathf.Clamp(currentY, minY, maxY);
    }

    public void SetAim (bool mode)
    {
        AimMode = mode;
    }

    private void LateUpdate()
    {

        BossPlayerDistance = (Boss.position - Player.position).magnitude;
        if (AimMode)
        {
            //currentY = Mathf.Clamp(BossPlayerDistance/3 + 1f, minY, maxY);
            focaldistance = AimDistance;
            Vector3 direction = (Boss.position - Player.position).normalized;
            Vector3 fixY = new Vector3(0, AimFixY, 0);
            direction.y = 0;

            //camTransform.position = Player.position - direction * focaldistance + fixY;
            
            float temp = (Player.position - direction * focaldistance + fixY).y;
            camTransform.position = Vector3.Lerp(camTransform.position, Player.position - direction * focaldistance + fixY, LerpTime);
            if (LerpTime < 1) {
                LerpTime += 0.05f;
            }
            camTransform.position = new Vector3(camTransform.position.x, temp, camTransform.position.z);
            
            camTransform.LookAt(Boss.position);
        }
        else if (!Locked)
        {
            focaldistance = Recordfocaldistance;
            Vector3 direction = new Vector3(0, 0, -focaldistance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

            camTransform.position = Player.position + rotation * direction;
            /*
            float temp = (Player.position + rotation * direction).y;
            camTransform.position = Vector3.Lerp(camTransform.position, Player.position + rotation * direction, LerpTime);
            if (LerpTime < 1)
            {
                LerpTime += 0.05f;
            }
            camTransform.position = new Vector3(camTransform.position.x, temp, camTransform.position.z);
            */
            camTransform.LookAt(Player.position);
        }
        else
        {
            //currentY = Mathf.Clamp(BossPlayerDistance/3 + 1f, minY, maxY);
            focaldistance = Mathf.Clamp(BossPlayerDistance/3+1f, 0, maxfocalY);
            Vector3 direction = (Boss.position - Player.position).normalized;
            Vector3 fixY = new Vector3(0, Mathf.Clamp(BossPlayerDistance/5, minY, maxY), 0);
            direction.y = 0;
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

            camTransform.position = Player.position - direction*focaldistance + fixY;
            /*
            float temp = (Player.position - direction * focaldistance + fixY).y;
            camTransform.position = Vector3.Lerp(camTransform.position, Player.position - direction * focaldistance + fixY, LerpTime);
            if (LerpTime < 1) {
                LerpTime += 0.05f;
            }
            camTransform.position = new Vector3(camTransform.position.x, temp, camTransform.position.z);
            */
            camTransform.LookAt(Boss.position);
        }
    }
}
