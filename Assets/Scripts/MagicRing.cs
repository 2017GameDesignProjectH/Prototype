using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRing : MonoBehaviour {

	public GameObject basicRing;

	public GameObject signList;
	public GameObject sign;

	public float Timer;
	public float timeOffset;
	public List<float> tempoTime = new List<float> ();
	public int count;

	// Use this for initialization
	void Start () {
		count = 0;
		Timer = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Timer = Timer + Time.fixedDeltaTime;
		basicRing.transform.Rotate (new Vector3 (0, 10, 0) * Time.fixedDeltaTime);
		if (count < tempoTime.Count && tempoTime [count] < Timer + timeOffset) {
			CreateSign ();
			count++;
		}
	}

	void CreateSign(){
		GameObject obj = GameObject.Instantiate (sign);
		obj.transform.parent = signList.transform;
		obj.GetComponent<TempoSign> ().SetTimer (timeOffset);
	}
}
