using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Rob")]
[Help("Rob the GameObject")]
public class Rob : BasePrimitiveAction
{
    [InParam("game object")]
    [Help("Game object to rob")]
    public GameObject targetGameobject;

    public override TaskStatus OnUpdate()
    {
        Moves moves = targetGameobject.GetComponent<Moves>();
        GameObject treasure = GameObject.Find("Robber");
        
        if (Vector3.Distance(targetGameobject.transform.position, treasure.transform.position) < 3.0f)
        {
            GameObject.Destroy(treasure);
            return TaskStatus.COMPLETED;
        }

        return TaskStatus.FAILED;
    }
}