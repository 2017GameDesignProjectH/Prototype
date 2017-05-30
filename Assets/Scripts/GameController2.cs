using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;
using DG.Tweening;

public class GameController2 : MonoBehaviour
{
    public BattleDataLoding DataLoading;
    public StageInfo stageInfo;
    public GameObject PlayerController;
    public NewBossScript BossController;
    public GameObject Player;
    public AudioSource Music;
    public float dashspeed;
    public float MinAttackPeriod;
    public float MinDashPeriod;

    private int skillNum;
    private bool flag;
    private float AttackCounter;
    private float DashCounter;

    // Use this for initialization
    void Start()
    {
        skillNum = 0;
        flag = false;
        DataLoading.LoadingStage("PIANO.json");
        stageInfo = DataLoading.stageInfo;
        Music.clip = (AudioClip)Resources.Load("Sounds/" + stageInfo.Music);
        Music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Player
        if (AttackCounter > 0)
        {
            AttackCounter -= Time.deltaTime;
        }

        if (DashCounter > 0)
        {
            DashCounter -= Time.deltaTime;
        }

        Vector3 forward = Player.transform.forward.normalized;
        forward.y = 0;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (AttackCounter <= 0)
            {
                AttackCounter = MinAttackPeriod;
                Player.SendMessage("Attack");
            }
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (DashCounter <= 0)
            {
                DashCounter = MinDashPeriod;
                PlayerController.transform.DOMove(new Vector3(PlayerController.transform.position.x + forward.x * dashspeed,
                    PlayerController.transform.position.y, PlayerController.transform.position.z + forward.z * dashspeed), 0.1f);
            }
        } 


        // Boss
        if (stageInfo.BossSkills[skillNum].Time <= Music.time)
        {
            if(stageInfo.BossSkills[skillNum].SkillName == "Slash_00")
            {
                BossController.DoSlash();
            }

            if (stageInfo.BossSkills[skillNum].SkillName == "TripleSlash_00")
            {
                BossController.DoTripleSlash();
            }

            if (stageInfo.BossSkills[skillNum].SkillName == "CircleSlash_00")
            {
                BossController.DoCircleSlash();
            }

            if (stageInfo.BossSkills[skillNum].SkillName == "Laser_00")
            {
                BossController.DoLaser();
            }

            if (stageInfo.BossSkills[skillNum].SkillName == "Thunder_00")
            {
                BossController.DoThunder();
            }

            skillNum = skillNum + 1;
        }

    }

}
