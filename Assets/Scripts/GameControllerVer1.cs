using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameControllerVer1 : MonoBehaviour {

    public GameObject player;
    public GameObject target;
    private GameObject temp;
    private bool flag;

    // Use this for initialization
    void Start () {
        temp = Instantiate(target);
        flag = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            flag = true;
            temp.SetActive(false);
            GameObject.Destroy(temp);
            temp = Instantiate(target);
            temp.transform.position = new Vector3(player.transform.position.x,player.transform.position.y ,player.transform.position.z);
            //temp.transform.rotation = player.transform.rotation;
            temp.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (flag == true)
            {
                temp.SetActive(false);
                //player.transform.position = temp.transform.position;
                //player.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y - 3, temp.transform.position.z);
                player.transform.DOMove(new Vector3(temp.transform.position.x, temp.transform.position.y , temp.transform.position.z), 0.1f);
                //GameObject.Destroy(temp);
            }
        }
    }
}
