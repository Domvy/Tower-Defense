using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedEnemy : MonoBehaviour
{
    public float FullHP = 20;
    public int HP = 20;
    public int Armor = 0;
    public Canvas canvasObj; // hp바 표시

    private void Start()
    {
        gameObject.GetComponent<EnemyNavi2>().SendMessage("SpeedSetting", "SpeedEnemy"); // EnemyNavi 스크립트에 몬스터 타입 전송        
    }

    void Update()
    {
        if (HP <= 0)
        {
            Die();
        }

        canvasObj.transform.rotation = Quaternion.Euler(0, 0, -1); // hp바 회전값 고정
        canvasObj.GetComponentInChildren<Slider>().value = HP / FullHP;
    }

    void Die() // 사망시 EnemySpawn 스크립트 함수 호출
    {
        BasicSetting.instance.PlayerMoney += 5;
        GameObject.Find("Controller").GetComponent<EnemySpawn>().Die(gameObject);
    }

    public void Hit(int Damage, int ArmorPearce)
    {
        if (Damage - Armor < 0)
        {
            Armor = Damage;
        }
        HP -= (Damage - Armor) + ArmorPearce;
    }
}
