using UnityEngine;
using UnityEngine.AI;

public class MouseMove : MonoBehaviour {
	private NavMeshAgent agent;
	public Animator animCop;
	Vector3 destinationPoint;
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		destinationPoint = this.transform.position;
	}
	
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (camRay, out hit, 100)) {
				agent.destination = hit.point;
				destinationPoint = hit.point;
				animCop.SetBool("Run Forward", true);
			}
		}
		
	}
}
