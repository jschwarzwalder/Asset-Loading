//This script is used to make a 3D UI mesh "zoom" in (instead of just appearing, the element
//starts small and then expands to a target size). 

using UnityEngine;
using System.Collections;	//This allows us to use coroutines
using UnityEngine.UI;		//Enable UI items in script

public class VRUIPanel : MonoBehaviour 
{
	public float appearSpeed = 10f;		//The speed that the object will zoom in and out
	public Button preSelectedButton;	//Which UI button will be the first one selected (this is needed for controller support)

	Vector3 initialScale;	//Initial scale of the object
	Vector3 targetScale;	//Target scale of the object


	//This will be called when this game object is enabled
	void OnEnable()
	{
		//Record the initial scale of the object 
		initialScale = transform.localScale;
		//Set the scale to zero, making it invisible
		transform.localScale = Vector3.zero;
		//Set the target scale to its initial scale so it can grow to its original size
		targetScale = initialScale;

		//Start the Zoom coroutine
		StartCoroutine (Zoom ());
	}

	//This will be called when this game object is disabled
	void OnDisable()
	{
		//Record the initial scale of the object 
		initialScale = transform.localScale;
		//Set the target scale to zero so it can shrink all the way down
		targetScale = Vector3.zero;

		//Start the Zoom coroutine
		StartCoroutine (Zoom ());
	}

	//This Coroutine will scale the game object. Coroutines allow us to break instructions
	//up so that they happen over time instead of all at once.
	IEnumerator Zoom()
	{
		//While the game object has not yet reached its target scale...
		while (Vector3.Distance (transform.localScale, targetScale) > .01f) 
		{
			//...scale the game object over time using Lerp...
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, appearSpeed * Time.deltaTime);
			//...and leave or "pause" this code until the next game frame. Then, start back right here
			yield return null;
		}

		//Once scaling is complete, select the designated button
		preSelectedButton.Select ();
	}
}
