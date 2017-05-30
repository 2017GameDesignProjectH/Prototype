using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowScript : MonoBehaviour
{
    public float LifeTime;
    public float damageValue;
    public GameObject explosion;
    //public AudioSource slashAudio;
    private float ShootTime;

    public void Init(float i)
    {
        ShootTime = 12f-i/30f;
        this.transform.position = new Vector3(
            this.transform.parent.transform.position.x + Mathf.Cos(i)*5.0f,
            this.transform.parent.transform.position.y + 2.0f,
            this.transform.parent.transform.position.z + Mathf.Sin(i)*5.0f
        );
        this.transform.LookAt(this.transform.parent);
        Invoke("Shoot", ShootTime);
    }

    public void Shoot()
    {


        //Invoke("KillYourself", LifeTime);
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
    }

    void Update()
    {

    }
}
