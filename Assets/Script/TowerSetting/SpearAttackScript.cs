using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttackScript : MonoBehaviour
{
    public List<GameObject> enemySpawnList; // 생성된 적 배열값

    private float towerRange; //타워 사거리
    public int towerRangeX = 20; // 사거리 설정값(곱해주는 값)
    private GameObject target; // 공격 타겟    

    public int nowenemyCount = 0;// 총 생산횟수

    public int NormalDamage = 0; // 공격력
    public int ArmorPearce = 1; // 방어무시 공격력

    private float timer = 0.0f;
    private float waitingTime = 5;
    private float attackTimer = 0.0f;
    private float attackDelay = 0.1f;

    LineRenderer lineRenderer;

    LayerMask layermask;

    void Start()
    {
        target = null;
        enemySpawnList = new List<GameObject>(); // 배열 선언  
        towerRange = new Vector3(1, 1, 1).magnitude * towerRangeX; // 타워 사거리 계산    
        layermask = LayerMask.GetMask("Enemy");

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.SetColors(Color.red, Color.red);
        lineRenderer.SetWidth(0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        enemySpawnList = GameObject.Find("Controller").GetComponent<EnemySpawn>().enemyList; // 적 생성 배열값 받아옴
        nowenemyCount = GameObject.Find("Controller").GetComponent<EnemySpawn>().nowEnemyCount; // 현재 적 숫자 받아오기        
        if (target == null)
        {
            Distance(); // 공격함수 실행
        }
        if (attackTimer > attackDelay)
        {
            RayzerAttack();
            attackTimer = 0;
        }

        if (timer > waitingTime)
        {
            Destroy(gameObject);
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
    }

    void Distance() //사거리 내의 오브젝트를 찾고 타겟설정
    {
        for (int i = 0; i < nowenemyCount; i++)
        {
            if (towerRange >= (enemySpawnList[i].transform.position - transform.position).magnitude)
            {
                target = enemySpawnList[i];
                break;
            }
        }
    }

    void RayzerAttack()
    {        
        RaycastHit hit;
        Vector3 dir = target.transform.position - transform.position;
        if (Physics.Raycast(transform.position, dir, out hit, layermask))
        {
            if(hit.collider.gameObject.tag == "Enemy")
                hit.collider.gameObject.GetComponent<NormalEnemy>().Hit(NormalDamage, ArmorPearce);
            else if (hit.collider.gameObject.tag == "SpeedEnemy")
                hit.collider.gameObject.GetComponent<SpeedEnemy>().Hit(NormalDamage, ArmorPearce);
            else if (hit.collider.gameObject.tag == "BigEnemy")
                hit.collider.gameObject.GetComponent<BigEnemy>().Hit(NormalDamage, ArmorPearce);
        }
        Debug.DrawRay(transform.position, dir, Color.red);
    }
}

