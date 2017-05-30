using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class BattleDataLoding : MonoBehaviour {

	public StageInfo stageInfo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void LoadingStage(string stageName){
		GetStageInfo (stageName);
	}

	public void GetStageInfo(string stageName){
		string DPath = Application.dataPath;
		int num = DPath.LastIndexOf ("/");
		DPath = DPath.Substring (0, num);
		string stagePath = DPath + "/StageInfo/" + stageName;
		if (File.Exists (stagePath)) {
			StageDataSplit (stagePath);
		} else {
			Debug.Log ("No such file!");
		}
	}

	public void StageDataSplit(string stagePath){
		string jsonString;
		JsonData jsonData;
		jsonString = File.ReadAllText (stagePath);
		jsonData = JsonMapper.ToObject (jsonString);
		stageInfo.StageName = (string)jsonData ["name"];
		stageInfo.Music = (string)jsonData ["music"];
		Debug.Log (jsonData ["BossSkills"].Count);
		stageInfo.BossSkills = new BossSkill[jsonData ["BossSkills"].Count];
		for (int i = 0; i < jsonData ["BossSkills"].Count; i++) {
			stageInfo.BossSkills [i] = new BossSkill ((float)(double)jsonData ["BossSkills"] [i] ["time"], (string)jsonData ["BossSkills"] [i] ["skill"]);
		}
	}

}

[System.Serializable]
public class StageInfo{
	public StageInfo (){}
	public StageInfo(string stageName, string music){
		StageName = stageName;
		Music = music;
	}
	public string StageName;
	public string Music;
	public BossSkill[] BossSkills;
}

[System.Serializable]
public class BossSkill{
	public BossSkill(){}
	public BossSkill(float time, string skillName){
		Time = time;
		SkillName = skillName;
	}
	public float Time;
	public string SkillName;
}