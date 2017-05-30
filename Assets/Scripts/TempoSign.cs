using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoSign : MonoBehaviour {

	public float timer;
	
	// Update is called once per frame
	void FixedUpdate () {
		float x = Mathf.Sqrt (timer)*3;
		float z = timer;
		this.transform.localPosition = new Vector3 (x, 0, z);
		timer = timer - Time.fixedDeltaTime;
		if (timer < 0) {
			Destroy (this.gameObject);
		}
	}

	public void SetTimer(float iniTime){
		timer = iniTime;
	}
}
