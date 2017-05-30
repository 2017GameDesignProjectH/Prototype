using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController_2 : MonoBehaviour {

	public BossSkill[] bossSkills;
	public AudioSource stageMusic;

	public int skillNum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (bossSkills [skillNum].Time <= stageMusic.time) {
			GameObject obj = (GameObject)Instantiate (Resources.Load ("Prefabs/Skills/" + bossSkills [skillNum].SkillName));
			skillNum = skillNum + 1;
		}
	}

	public void SettingBossSkills(StageInfo stageInfo){
		bossSkills = stageInfo.BossSkills;
		skillNum = 0;
	}

	public void SettingStageMusic(AudioSource music){
		this.stageMusic = music;
	}
}
