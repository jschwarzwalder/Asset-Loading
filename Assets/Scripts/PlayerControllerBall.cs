using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControllerBall : MonoBehaviour {

	[SerializeField] private float speed = 0.0f;
	[SerializeField] private Text countText; 
	[SerializeField] private Text winText;

	private Rigidbody rb;
	private int count;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		countText.text = "Count: " + count.ToString ();
		winText.text = "";
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("PickUp"))
		{
			
			other.gameObject.SetActive (false);
			count++;
			countText.text = "Count: " + count.ToString ();
			if (count >= 15) {
				winText.text = "You Win";
			}
		}
	}
}