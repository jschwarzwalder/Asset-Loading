//This script scales the camera by a specified amount. This can be used in 
//VR to change the perceived size of objects in a scene by scaling the 
//player's virtual IPD (InterPupillary Distance)

using UnityEngine;

public class CameraScaler : MonoBehaviour 
{
	public float scaleSpeed = 5f;	//How fast the scaling should occur
	public float scaleAmount = 20f;	//The amount to scale the camera to
	public Transform camController;	//Reference to the transform of the camera (or its control rig)

	bool isScaling;			//Is the camera currently being scaled?
	bool isScaledUp;		//Is the camera currently scaled up (making the world miniature)?
	Vector3 originalScale;	//The original scale of the camera
	Vector3 maxScale;		//The maximum scale of the camera
	Vector3 targetScale;	//The scale the camera should be changed to


	void Start()
	{
		//Record the current (original) camera scale
		originalScale = camController.localScale;
		//Set the max scale to the desired values
		maxScale = new Vector3 (scaleAmount, scaleAmount, scaleAmount);
	}

	void Update()
	{
		//If we aren't currently scaling the camera, leave
		if (!isScaling)
			return;

		//Use Lerp to scale the camera over time
		camController.localScale = Vector3.Lerp (camController.localScale, targetScale, scaleSpeed * Time.deltaTime);

		//If the camera is close enough to the target, stop scaling (prevents slow micro-adjustments)
		if (Vector3.Distance (camController.localScale, targetScale) <= .0001f)
			isScaling = false;
	}

	void OnTriggerEnter(Collider other)
	{
		//When the player enters the trigger area, start scaling
		isScaling = true;

		//If the camera is already scaled up, scale down to the regular size. Otherwise, scale up
		if (isScaledUp)
			targetScale = originalScale;
		else
			targetScale = maxScale;

		//Toggle whether the camera is currently scaled up or not
		isScaledUp = !isScaledUp;
	}
}
