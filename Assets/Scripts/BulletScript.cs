using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public float FlyingSpeed;
    public float LifeTime;

    public void InitAndShoot(Vector3 Direction)
    {
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();

        rigidbody.velocity = Direction * FlyingSpeed;

        Invoke("KillYourself", LifeTime);

    }

    public void KillYourself()
    {
        GameObject.Destroy(this.gameObject);
    }

    public float damage;

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SendMessage("Hit", damage);
        KillYourself();
    }

}

