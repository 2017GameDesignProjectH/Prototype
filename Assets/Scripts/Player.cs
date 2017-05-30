using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    //public PlayerController PlayerController;
    public GameObject bulletCandidate;
    public Image HPBar;
    public float MaxHP;
    public float CurrentHP;

    private Animator animator;
    private float MinimumHitPeriod = 1f;
    private float HitCounter = 0;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        CurrentHP = MaxHP;
    }

    void Update()
    {
        animator.SetFloat("HP", CurrentHP);
        if (CurrentHP > 0 && HitCounter > 0)
        {
            HitCounter -= Time.deltaTime;
        }
        // 把角色的速度值傳給 animator 中的 velocity
        animator.SetFloat("velocity", this.GetComponent<Rigidbody>().velocity.magnitude);
        /*if (PlayerController.getjumpspeed() > 0)
            animator.SetTrigger("jump");*/
    }

    public void Attack()
    {
        GameObject newBullet = GameObject.Instantiate(bulletCandidate);
        BulletScript bullet = newBullet.GetComponent<BulletScript>();
        bullet.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y + 1.5f, this.transform.position.z);
        bullet.transform.rotation = this.transform.rotation;
        bullet.InitAndShoot(this.transform.forward);
    }

    public void Hit(float value)
    {
        if (HitCounter <= 0)
        {
            HitCounter = MinimumHitPeriod;
            CurrentHP -= value;

            animator.SetFloat("HP", CurrentHP);
            animator.SetTrigger("Hit");
            //if (CurrentHP <= 0) { Die(); }
        }

        HPBar.fillAmount = CurrentHP / MaxHP;
    }


}
