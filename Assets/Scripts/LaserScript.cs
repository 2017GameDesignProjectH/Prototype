using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserScript : MonoBehaviour
{
    public float LifeTime;
    public GameObject Player;
    private Vector3 point;
    private bool StartUpdate = false;
    private float rotationx;
    private float rotationy;
    private float rotationz;
    //public AudioSource LaserAudio;

    public void InitAndShoot()
    {
        Invoke("KillYourself", LifeTime);
        //this.transform.rotation = Quaternion.Euler(this.transform.rotation.x - 15f,this.transform.rotation.y, this.transform.rotation.z);
        //this.transform.DORotate(new Vector3(0, -0.000001, 0), 0);
        /*this.transform.DORotate(new Vector3(
            Player.transform.position.x - Player.GetComponent<Rigidbody>().velocity.x * 2f, 0,
            Player.transform.position.z - Player.GetComponent<Rigidbody>().velocity.z * 2f
            ).normalized, 0.01f);*/

        //rotationx = this.transform.rotation.x;
        //rotationy = this.transform.rotation.y;
        //rotationz = this.transform.rotation.z;
        StartUpdate = true;
    }

    public void KillYourself()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (StartUpdate == true)
        {
            this.transform.DORotate(new Vector3(
                Player.transform.position.x + Player.GetComponent<Rigidbody>().velocity.x * 0.2f, 0,
                Player.transform.position.z + Player.GetComponent<Rigidbody>().velocity.z * 0.2f
                ).normalized, LifeTime);
            StartUpdate = false;
        }
    }
}
