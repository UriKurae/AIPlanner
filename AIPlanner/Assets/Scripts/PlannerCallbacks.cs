using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Generated.Semantic.Traits;

public class PlannerCallbacks : MonoBehaviour
{
    public Moves moves;
    public UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    public Robber trt;
    private float waitTime = 0.5f;

    public IEnumerator Steal(GameObject treasure)
    {
        treasure.GetComponent<Renderer>().enabled = false;
        yield return null;
    }

    public IEnumerator Seek(GameObject treasure, GameObject copGO)
    {
        
        if (anim.GetBool("Walk Forward"))
        {
            anim.SetBool("Walk Forward", false);
        }
        anim.SetBool("Run Forward", true);
        if (treasure)
            Debug.Log("Treasure is active");

        agent.SetDestination(treasure.transform.position);
        while ((Vector3.Distance(treasure.transform.position, transform.position) > 2.0f) &&
               (Vector3.Distance(treasure.transform.position, copGO.transform.position) > 6.0f))
            yield return waitTime * Time.deltaTime;
        
        if (Vector3.Distance(treasure.transform.position, copGO.transform.position) < 6.0f)
        {
            trt.CopAway = false;
        }
        else
        {
            trt.Ready2Steal = true;
        }
    }

    public IEnumerator Wander(GameObject cop, GameObject treasure)
    {
        if (anim.GetBool("Run Forward"))
        {
            anim.SetBool("Run Forward", false);
        }
        anim.SetBool("Walk Forward", true);
        while (Vector3.Distance(treasure.transform.position, cop.transform.position) < 6.0f)
        {
            moves.Wander();
            yield return waitTime * Time.deltaTime;
            
        }
    }

    public IEnumerator Hide(GameObject target)
    {
        if (anim.GetBool("Walk Forward"))
        {
            anim.SetBool("Walk Forward", false);
        }
        anim.SetBool("Run Forward", true);
        while (Vector3.Distance(target.transform.position, transform.position) > 1.0f)
        {
            moves.Hide();
            yield return waitTime * Time.deltaTime;
        }
    }
}
