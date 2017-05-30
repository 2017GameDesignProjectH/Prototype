using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CircleSlashScript : MonoBehaviour {

    public float LifeTime;
    public float damageValue;
    private ParticleSystem Circle;
    private float radius;
    //public GameObject explosion;
    //public AudioSource slashAudio;

    void Start()
    {
        Circle = GetComponent<ParticleSystem>();
        radius = 0;
    }

    public void Init()
    {
        Invoke("KillYourself", LifeTime);
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
        radius += 15*Time.deltaTime;
        ParticleSystem.ShapeModule shapeModule = Circle.shape;
        shapeModule.radius = radius;
    }
}
