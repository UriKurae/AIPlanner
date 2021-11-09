using UnityEngine;
using Pada1.BBCore;
using Pada1.BBCore.Framework;

[Condition("MyConditions/Is Cop far?")]
[Help("Checks whether Cop is near the Treasure.")]
public class CopFar : ConditionBase
{
    public override bool Check()
    {
        GameObject cop = GameObject.Find("Cop");
        GameObject treasure = GameObject.Find("Treasure");

        return Vector3.Distance(cop.transform.position, treasure.transform.position) > 2.0f;
    }
}