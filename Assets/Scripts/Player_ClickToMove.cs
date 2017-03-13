﻿//This script is used to control the player with click to move mechanics. It is mostly meant 
//for playing the game on mobile devices (without keyboards or controllers)

using UnityEngine;
using UnityEngine.AI;

public class Player_ClickToMove : MonoBehaviour 
{
	public LayerMask whatIsGround;	//A layer mask defining what layers constitute the ground
	public GameObject navMarker;	//A reference to the prefab that is our "Nav Marker"
	public float turnSmoothing = 15f;		//Speed that the player turns

	NavMeshAgent agent;		//A reference to the player's navmesh agent component
	NavMeshHit navHitInfo;	//Where on a navmesh the player is looking
	Animator anim;			//A reference to the player's animator component


	void Start ()
	{
		//Get references to the local navmesh agent and animator
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();

		//Instantiate (create) our navmarker and disable (hide) it
		navMarker = Instantiate (navMarker) as GameObject;
		navMarker.SetActive (false);
	}

	void Update ()
	{
		//If the GameManager exists AND it tells us that the game is over, leave
		if (GameManager.instance != null && GameManager.instance.IsGameOver ()) 
		{
			//Go back to our Idle animation by dropping the speed to 0
			anim.SetFloat ("Speed", 0f);
			return;
		}

		//Otherwise, check for movement...
		CheckForMovement ();
		//...and then update the animations
		UpdateAnimation ();
	}

	void CheckForMovement()
	{
		//Look to see if we pressed "Fire1" (left mouse, screen touch, trigger, etc). 
		//If we did, we need to figure out what we clicked or tapped on in the scene
		if (Input.GetButtonDown ("Fire1")) 
		{
			//Create a ray from the main camera through our mouse's position
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//Declare a variable to store the results of a raycast
			RaycastHit hit;

			//If this ray hits something on the ground layer...
			if (Physics.Raycast(ray, out hit, 1000, whatIsGround))
			{
				//...look at the navmesh to determine if the ray is within 5 units of it (we can only
				//send the player to spots on the navmesh). If it is...
				if (NavMesh.SamplePosition (hit.point, out navHitInfo, 5, NavMesh.AllAreas)) 
				{
					//...tell the navmesh agent to go to that spot...
					agent.SetDestination (navHitInfo.position);
					//...move our navmarker to that spot...
					navMarker.transform.position = navHitInfo.position;
					//...and enable (show it)
					navMarker.SetActive (true);
				} 
			}
		}
	}

	void UpdateAnimation()
	{
		//Record the desired speed of the navmesh agent
		float speed = agent.desiredVelocity.magnitude;

		//Tell the animator how fast the navmesh agent is going
		anim.SetFloat("Speed", speed);

		//If the player if moving...
		if (speed > 0f) 
		{
			//...calculate the angle the player should be facing...
			Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
			//...and rotate over time to face that direction
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		}

		//If we are within our "Stopping Distance" of the destination...
		if (agent.remainingDistance <= agent.stoppingDistance + .1f) 
		{
			//...disable (hide) the nav marker
			navMarker.SetActive (false);
		}
	}
}
