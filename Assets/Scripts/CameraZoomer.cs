//This script moves the camera by a specified amount. This script is used to demonstrate
//the differences between scaling a VR camera and simply zooming it

using UnityEngine;

public class CameraZoomer : MonoBehaviour 
{
	public float zoomSpeed = 5f;	//How fast the zooming should occur
	public float zoomAmount = 10f;	//The amount to move the camera
	public Transform camController;	//Reference to the transform of the camera (or its control rig)

	bool isZooming;			//Is the camera currently being zoomed?
	bool isZoomed;			//Is the camera currently zoomed out?
	Vector3 originalOffset;	//The original position of the camera
	Vector3 zoomOffset;		//The zoomed position of the camera
	Vector3 targetOffset;	//The position currently being zoomed to


	void Start()
	{
		//Record the original position of the camera
		originalOffset = camController.position;
		//Calculate the zoomed position
		zoomOffset = originalOffset + new Vector3 (0f, zoomAmount, 0f);
	}

	void Update()
	{
		//If we aren't zooming, leave
		if (!isZooming)
			return;

		//Use Lerp to zoom over time
		camController.position = Vector3.Lerp (camController.position, targetOffset, zoomSpeed * Time.deltaTime);

		//If we are close to our target position, stop zooming
		if (Vector3.Distance (camController.position, targetOffset) <= .0001f)
			isZooming = false;
	}

	void OnTriggerEnter(Collider other)
	{
		//When the player enters this trigger area, start zooming
		isZooming = true;

		//If we are currently zoomed out, zoom in. Otherwise, zoom out
		if (isZoomed)
			targetOffset = originalOffset;
		else
			targetOffset = zoomOffset;

		//Toggle whether or not we are zoomed
		isZoomed = !isZoomed;
	}
}
