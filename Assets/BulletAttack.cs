using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    public ETeamID attackTargetID;

    public int bulletPoolCount = 15;

    public GameObject prefabBullet;

    public Transform gunTrans;

    public Transform gunFirePointTrans;

    public float ShootCoolTime = 1.2f;

    float coolTimeCounter = 0.0f;

    Bullet[] bulletPool;

    int poolIndex = 0;

    private void Start()
    {
        CreateBulletPool();
    }

    void CreateBulletPool()
    {
        bulletPool = new Bullet[bulletPoolCount];

        int i = 0;

        while(i<bulletPoolCount)
        {
            Bullet BulletMade =  Instantiate(prefabBullet).GetComponent<Bullet>();

            bulletPool[i] = BulletMade;

            i++;
        }
    }


    void Update()
    {
        coolTimeCounter += Time.deltaTime;

        Health FoundTarget =  GameManager.GetInstance.GetNearest(attackTargetID, transform);

        if(!FoundTarget)
        {
            return;
        }

        GunRotateForTarget(FoundTarget.transform);

        TryShootBullet();
    }

    void GunRotateForTarget(Transform target)
    {
        Vector3 GunPos = gunTrans.position;

        Vector3 TargetPos = target.position;

        Vector3 AimDir =  (TargetPos - GunPos).normalized;

        float Angle = Mathf.Atan2(AimDir.y, AimDir.x) * Mathf.Rad2Deg;

        gunTrans.eulerAngles = new Vector3(0, 0, Angle);
    }

    void TryShootBullet()
    {
        if(coolTimeCounter<ShootCoolTime)
        {
            return;
        }

        print("Shoot");

        coolTimeCounter = 0.0f;

        bulletPool[poolIndex].Shoot(gunFirePointTrans);

        poolIndex++;

        if(poolIndex>=bulletPoolCount)
        {
            poolIndex = 0;
        }
    }
}
