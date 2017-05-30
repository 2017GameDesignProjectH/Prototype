using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewBossScript : MonoBehaviour
{

    //public CollisionListScript AttackSensor;
    public GameObject Player;
    public GameObject DarkMist;
    public GameObject FireBall;
    public GameObject Slash;
    public GameObject CircleSlash;
    public GameObject Laser;
    public GameObject Thunder;
    //public Slider HPBar;
    //public string motion;
    public float MinimumPeriod;
    public float FarDistance;
    public float Distance;
    public float time = 0;
    public AudioSource Music;
    public List<float> SlashTime;
    public List<float> TripleSlashTime;
    public List<float> CircleSlashTime;
    public List<float> LaserTime;
    public List<float> ThunderTime;

    private Rigidbody rigidBody;
    private float state;
    private bool dash;
    private bool IsSlash;
    private bool IsCircleSlash;
    private bool fire;
    private bool timeflag;
    private int timecount;
    private int SlashCount;
    private int TripleSlashCount;
    private int CircleSlashCount;
    private int LaserCount;
    private int ThunderCount;


    // Use this for initialization
    void Start()
    {
        state = 0;
        time = 0;
        timecount = 0;
        SlashCount = 0;
        TripleSlashCount = 0;
        CircleSlashCount = 0;
        LaserCount = 0;
        IsSlash = false;
        IsCircleSlash = false;

        // Play darkmist
        DarkMist.transform.position = this.transform.position;
        DarkMist.SetActive(true);
    }

    public void DoSlash()
    {
        GameObject newSlash = GameObject.Instantiate(Slash);
        newSlash.SetActive(true);
        SlashScript slash = newSlash.GetComponent<SlashScript>();
        slash.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y, this.transform.position.z);
        slash.transform.rotation = this.transform.rotation;
        slash.InitAndShoot(new Vector3(
            (Player.transform.position.x - this.transform.position.x) + Player.GetComponent<Rigidbody>().velocity.x * 0.4f, 0,
            (Player.transform.position.z - this.transform.position.z) + Player.GetComponent<Rigidbody>().velocity.z * 0.4f
            ).normalized);
        IsSlash = false;
    }

    public void DoTripleSlash()
    {
        for (int i = -1; i < 2; i++)
        {
            GameObject newSlash = GameObject.Instantiate(Slash);
            newSlash.SetActive(true);
            SlashScript slash = newSlash.GetComponent<SlashScript>();
            slash.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y, this.transform.position.z);
            slash.transform.rotation = this.transform.rotation;
            slash.InitAndShoot(new Vector3(
                (Player.transform.position.x - this.transform.position.x) + Player.GetComponent<Rigidbody>().velocity.x * 0.6f * i, 0,
                (Player.transform.position.z - this.transform.position.z) + Player.GetComponent<Rigidbody>().velocity.z * 0.6f * i
                ).normalized);
        }
    }

    public void DoCircleSlash()
    {
        //float r = Random.Range(0, 360);
        //this.transform.DORotate(new Vector3(0, r, 0), 0.2f);
        GameObject newCircleSlash = GameObject.Instantiate(CircleSlash);
        newCircleSlash.SetActive(true);
        CircleSlashScript slash = newCircleSlash.GetComponent<CircleSlashScript>();
        slash.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y, this.transform.position.z);
        slash.Init();
    }

    public void DoThunder()
    {
        GameObject newThunder = GameObject.Instantiate(Thunder);
        newThunder.SetActive(true);
        newThunder.transform.position = new Vector3(Player.transform.position.x,
            this.transform.position.y, Player.transform.position.z);
    }

    public void DoLaser()
    {
        Laser.SetActive(true);
        LaserScript laser = Laser.GetComponent<LaserScript>();
        laser.transform.rotation = this.transform.rotation;
        //this.transform.rotation;
        laser.InitAndShoot();
    }

    // Update is called once per frame
    void Update()
    {
        // Adjust lookat
        Vector3 lookAt = Player.transform.position;
        lookAt.y = this.gameObject.transform.position.y;
        this.transform.LookAt(lookAt);
        Distance = (this.transform.position - Player.transform.position).magnitude;

    }
}
