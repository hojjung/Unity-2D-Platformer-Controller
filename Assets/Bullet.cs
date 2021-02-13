using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 150;

    public float BulletSpeed = 10.0f;


    public void Shoot(Transform gunFirePointTrans)
    {
        gameObject.SetActive(false);
        transform.position = gunFirePointTrans.position;
        transform.rotation = gunFirePointTrans.rotation;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Trigger");
        Health Hitten = collision.gameObject.GetComponent<Health>();

        if(!Hitten)
        {
            return;
        }

        print("HittenFound");

        if (Hitten.teamID == ETeamID.MonsterTeam)
        {
            Hitten.ApplyDamage(Damage);

            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.position += transform.right * BulletSpeed * Time.deltaTime;
    }
}
