using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BossController : MonoBehaviour {

    public CollisionListScript AttackSensor;
    public GameObject Player;
    public GameObject DarkMist;
    public GameObject FireBall;
    public GameObject Slash;
    public GameObject CircleSlash;
    public GameObject Arrow;
    public Slider HPBar;
    public string motion;
    public float movespeed;
    public float runspeed;
    public float MinimumHitPeriod;
    public float MaxHP;
    public float CurrentHP;
    public float FarDistance;
    public float Distance;

    private Animator BossAnimatorController;
    private Rigidbody rigidBody;
    private float HitCounter;
    private float state;
    private float time;
    private bool dash;
    private bool IsSlash;
    private bool IsCircleSlash;
    private bool fire;


    // Use this for initialization
    void Start () {
        BossAnimatorController = this.GetComponent<Animator>();
        rigidBody = this.GetComponent<Rigidbody>();
        CurrentHP = MaxHP;
        HitCounter = 0;
        state = 0;
        time = 0;
        motion = "none";
        IsSlash = false;
        IsCircleSlash = false;
    }

    public void AttackPlayer()
    {
        if (IsCircleSlash)
        {
            DoCircleSlash();
        }
        else if (AttackSensor.CollisionObjects.Count > 0)
        {
            Player.SendMessage("Hit", 200);
        }
        else if (IsSlash)
        {
            DoSlash();
        }
    }

    public void DoSlash()
    {
        GameObject newSlash = GameObject.Instantiate(Slash);
        newSlash.SetActive(true);
        SlashScript slash = newSlash.GetComponent<SlashScript>();
        slash.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y + 1.5f, this.transform.position.z);
        slash.transform.rotation = this.transform.rotation;
        slash.InitAndShoot(new Vector3(
            (Player.transform.position.x - this.transform.position.x) + Player.GetComponent<Rigidbody>().velocity.x*0.4f,0,
            (Player.transform.position.z - this.transform.position.z) + Player.GetComponent<Rigidbody>().velocity.z*0.4f
            ).normalized);
        IsSlash = false;
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
        IsCircleSlash = false;
    }

    public void Hit(float value)
    {
        if (time >= 60*Time.deltaTime) {
            if (HitCounter <= 0)
            {
                HitCounter = MinimumHitPeriod;
                CurrentHP -= value;

                BossAnimatorController.SetFloat("HP", CurrentHP);
                BossAnimatorController.SetTrigger("Hit");
                time = 0;
                if (CurrentHP <= 0) {
                    Die();
                    return;
                }

                if (state == 1 && CurrentHP <= 0.5*MaxHP)
                {
                    BossAnimatorController.SetTrigger("roar");
                    DarkMist.transform.position = this.transform.position;
                    DarkMist.SetActive(true);
                    state++;
                    time = 0;
                }

                if (state == 2 && CurrentHP <= 0.2*MaxHP)
                {
                    BossAnimatorController.SetTrigger("roar");
                    DarkMist.transform.position = this.transform.position;
                    DarkMist.SetActive(true);
                    state++;
                    time = 0;
                    dash = true;
                }

            }
        }
    }

    void Die()
    {
        CurrentHP = 0;
        BossAnimatorController.SetTrigger("die");
        state = 100; // stop any other motion
        time = 0;
    }

    IEnumerator DoDash()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 predict = new Vector3(
            Player.transform.position.x + Player.GetComponent<Rigidbody>().velocity.x * 0.4f, Player.transform.position.y,
            Player.transform.position.z + Player.GetComponent<Rigidbody>().velocity.z * 0.4f
            );
        predict.y = this.gameObject.transform.position.y;
        this.transform.LookAt(predict);
        this.transform.DOMove(predict - this.transform.forward.normalized*2, 0.25f);
    }

    IEnumerator DoTP()
    {
        yield return new WaitForSeconds(0.1f);
        this.transform.position = Player.transform.position + this.transform.forward.normalized * 10;
        //this.transform.DOMove(Player.transform.position + this.transform.forward.normalized*5, 0.1f);
        Vector3 lookAt = Player.transform.position;
        lookAt.y = this.gameObject.transform.position.y;
        this.transform.LookAt(lookAt);
        StartCoroutine(DoDash());
    }

    IEnumerator DoRound()
    {
        yield return new WaitForSeconds(0.1f);
        for(float i = 0; i < 360; i += 30)
        {
            GameObject newArrow = GameObject.Instantiate(Arrow);
            newArrow.transform.SetParent(Player.transform);
            newArrow.SetActive(true);
            ArrowScript arrow = newArrow.GetComponent<ArrowScript>();
            arrow.Init(i);
            yield return new WaitForSeconds(1.5f);
        }

    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;

        if (CurrentHP > 0 && HitCounter > 0)
        {
            HitCounter -= Time.deltaTime;
        }

        if (state == 0)
        {
            BossAnimatorController.SetTrigger("roar");
            state++;
            time = 0;
        }

        if (state == 1 && time>=120*Time.deltaTime)
        {
            Vector3 lookAt = Player.transform.position;
            lookAt.y = this.gameObject.transform.position.y;
            this.transform.LookAt(lookAt);
            Distance = (this.transform.position - Player.transform.position).magnitude;
            int random;

            //近距離(已在攻擊距離內)
            if (AttackSensor.CollisionObjects.Count > 0) 
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                random = Random.Range(0, 5);
                if (random != 3)
                {
                    motion = "furious";
                    BossAnimatorController.SetTrigger("furious");
                    time = -50*Time.deltaTime;
                }
                else
                {
                    motion = "circle slash";
                    BossAnimatorController.SetTrigger("slash");
                    IsCircleSlash = true;
                    time = 0;
                }
            }

            //遠距離
            else if (Distance >= FarDistance) 
            {
                random = Random.Range(0, 300);
                if (random == 77)
                {
                    motion = "dash";
                    BossAnimatorController.SetTrigger("dash");
                    StartCoroutine(DoDash());
                    time = 0;
                }

                else if (random == 66)
                {
                    motion = "slash";
                    BossAnimatorController.SetTrigger("slash");
                    IsSlash = true;
                    time = 60*Time.deltaTime;
                }

                else
                {
                    if (motion != "walk" && motion != "run") motion = "walk";
                    if (motion == "walk")
                    {
                        BossAnimatorController.SetBool("walk", true);
                        rigidBody.velocity = this.transform.forward * movespeed;
                        random = Random.Range(0, 180);
                        if (random == 77)
                            motion = "run";
                    }
                    else if (motion == "run")
                    {
                        BossAnimatorController.SetBool("walk", false);
                        BossAnimatorController.SetBool("run", true);
                        rigidBody.velocity = this.transform.forward * runspeed;
                    }
                }
            }

            //中距離(走路or跑步)
            else
            {
                if (motion != "walk" && motion != "run") motion = "walk";
                if (motion == "walk")
                {
                    BossAnimatorController.SetBool("walk", true);
                    rigidBody.velocity = this.transform.forward * movespeed;
                    random = Random.Range(0, 180);
                    if (random == 77)
                        motion = "run";
                }
                else if (motion == "run")
                {
                    BossAnimatorController.SetBool("walk", false);
                    BossAnimatorController.SetBool("run", true);
                    rigidBody.velocity = this.transform.forward * runspeed;
                }
            }

        }  //end of state 1



        if (state == 2 && time>=120 * Time.deltaTime)
        {
            Vector3 lookAt = Player.transform.position;
            lookAt.y = this.gameObject.transform.position.y;
            this.transform.LookAt(lookAt);
            Distance = (this.transform.position - Player.transform.position).magnitude;
            int random;

            //近距離(已在攻擊距離內)
            if (AttackSensor.CollisionObjects.Count > 0)
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                random = Random.Range(0, 5);
                if (random != 3)
                {
                    motion = "furious";
                    BossAnimatorController.SetTrigger("furious");
                    time = -50 * Time.deltaTime;
                }
                else
                {
                    motion = "circle slash";
                    BossAnimatorController.SetTrigger("slash");
                    IsCircleSlash = true;
                    time = 0;
                }
            }

            //遠距離
            else if (Distance >= FarDistance)
            {
                random = Random.Range(0, 300);
                if (random == 77)
                {
                    motion = "dash";
                    BossAnimatorController.SetTrigger("dash");
                    StartCoroutine(DoDash());
                    time = 0;
                }

                else if (random == 66)
                {
                    motion = "slash";
                    BossAnimatorController.SetTrigger("slash");
                    IsSlash = true;
                    time = 60 * Time.deltaTime;
                }

                else if (random == 99)
                {
                    motion = "tp";
                    BossAnimatorController.SetTrigger("tp");
                    StartCoroutine(DoTP());
                    time = 100 * Time.deltaTime;
                }

                else if (random == 111)
                {
                    motion = "round";
                    BossAnimatorController.SetTrigger("roar");
                    StartCoroutine(DoRound());
                    time = 0;
                }

                else
                {
                    if (motion != "walk" && motion != "run") motion = "walk";
                    if (motion == "walk")
                    {
                        BossAnimatorController.SetBool("walk", true);
                        rigidBody.velocity = this.transform.forward * movespeed;
                        random = Random.Range(0, 180);
                        if (random == 77)
                            motion = "run";
                    }
                    else if (motion == "run")
                    {
                        BossAnimatorController.SetBool("walk", false);
                        BossAnimatorController.SetBool("run", true);
                        rigidBody.velocity = this.transform.forward * runspeed;
                    }
                }
            }

            //中距離(走路or跑步)
            else
            {
                if (motion != "run") motion = "run";
                BossAnimatorController.SetBool("walk", false);
                BossAnimatorController.SetBool("run", true);
                rigidBody.velocity = this.transform.forward * runspeed;
            }
        } //end of state 2

        if (state == 3 && time >= 150 * Time.deltaTime)
        {
            int i = Random.Range(0, 1000);
            if (i % 1000 == 777)
                dash = true;
            if (i % 1000 == 666)
                fire = true;

            Vector3 lookAt = Player.transform.position;
            if (dash == true)
            {
                lookAt.y = this.gameObject.transform.position.y;
                this.transform.LookAt(lookAt);
                this.transform.position = Player.transform.position + this.transform.forward.normalized * 3;
                //this.transform.DOMove(Player.transform.position + this.transform.forward.normalized*5, 0.1f);
                dash = false;
            }

            if (fire == true)
            {
                GameObject FireBall_s = GameObject.Instantiate(FireBall);
                FireBall_s.transform.position = Player.transform.position;
                FireBall_s.SetActive(true);
                fire = false;
            }

            lookAt = Player.transform.position;
            lookAt.y = this.gameObject.transform.position.y;
            this.transform.LookAt(lookAt);
            BossAnimatorController.SetBool("run", true);

            if (AttackSensor.CollisionObjects.Count > 0)
            {
                BossAnimatorController.SetBool("attack", true);
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                BossAnimatorController.SetBool("attack", false);
                rigidBody.velocity = this.transform.forward * movespeed * 2;
            }
        } //end of state 3

        //edit HP bar
        HPBar.value = CurrentHP / MaxHP;



    }
}
