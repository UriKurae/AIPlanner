using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("MyActions/Seek")]
[Help("Seek behaviour.")]
public class Seek : BasePrimitiveAction
{
    [InParam("game object")]
    [Help("Game object to add the component, if no assigned the component is added to the game object of this behavior")]
    public GameObject targetGameobject;

    [InParam("Target")]
    [Help("Game object rob")]
    public GameObject target;

    public override TaskStatus OnUpdate()
    {
        Moves moves = targetGameobject.GetComponent<Moves>();

        moves.Seek(target.transform.position);

        return TaskStatus.COMPLETED;
    }
}