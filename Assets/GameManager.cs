using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMarmot.Tools;

public class GameManager : Singleton<GameManager>
{
    Dictionary<ETeamID, List<Health>> sceneUnits;

    protected override void Awake()
    {
        base.Awake();

        sceneUnits = new Dictionary<ETeamID, List<Health>>();
    }

    public void AddUnit(Health health)
    {
        if(sceneUnits.ContainsKey(health.teamID))
        {
            sceneUnits[health.teamID].Add(health);

            return;
        }
        List<Health> healthList = new List<Health>();

        healthList.Add(health);

        sceneUnits.Add(health.teamID, healthList);
    }

    public void RemoveUnit(Health health)
    {
        sceneUnits[health.teamID].Remove(health);
    }

    public Health GetNearest(ETeamID id,Transform source)
    {
        if (sceneUnits.ContainsKey(id))
        {
            var List = sceneUnits[id];

            return GetNearSort(List, source);
        }

        return null;
    }

    Health GetNearSort(List<Health> list, Transform src)
    {
        Vector3 SrcPos = src.position;

        Health nearHealth = null;

        float nearDistSqr = float.MaxValue;

        foreach(var AA in list)
        {
            if(AA.transform ==src)
            {
                continue;
            }

            Vector3 offset = AA.transform.position - SrcPos;

            float newDistSqr = offset.sqrMagnitude;

            if(newDistSqr<nearDistSqr)
            {
                nearHealth = AA;

                nearDistSqr = newDistSqr;
            }
        }

        return nearHealth;
    }
}
