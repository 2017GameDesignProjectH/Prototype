using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlashScript : MonoBehaviour {

    public float FlyingSpeed;
    public float LifeTime;
    public float damageValue;
    public GameObject explosion;
    //public AudioSource slashAudio;

    public void InitAndShoot(Vector3 Direction)
    {
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = Direction * FlyingSpeed;
        Invoke("KillYourself", LifeTime);
        //int i = Random.Range(0, 2);
        //i *= 90;
        //this.transform.rotation = Quaternion.Euler(this.transform.rotation.x - 15f,this.transform.rotation.y, this.transform.rotation.z);
        //this.transform.DORotate(new Vector3(0, this.transform.rotation.y, this.transform.rotation.z), 2.5f);
    }

    public void KillYourself()
    {
        GameObject.Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SendMessage("Hit", damageValue);
        //explosion.gameObject.transform.parent = null;
        //explosion.gameObject.SetActive(true);
        //slashAudio.pitch = Random.Range(0.8f, 1);

        KillYourself();
    }

    void Update()
    {

    }

}
