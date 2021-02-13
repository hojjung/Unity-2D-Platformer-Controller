using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETeamID
{
    PlayerTeam,
    MonsterTeam
}
public class Health : MonoBehaviour
{
    public ETeamID teamID;

    public int InitMaxHealth = 100;

    int CurrentHealth = 0;

    private void Awake()
    {
        
    }

    void Start()
    {
        CurrentHealth = InitMaxHealth;

        GameManager.GetInstance.AddUnit(this);
    }

    public void ApplyDamage(int dmg)
    {
        CurrentHealth -= dmg;

        if(CurrentHealth<=0)
        {
            Death();
        }
    }

    void Death()
    {
        print("Dead");
        GameManager.GetInstance.RemoveUnit(this);
        Destroy(gameObject);
    }


}
