//This script spawns (creates) collectables into the scene. It also is responsible
//for telling our GameManager that the player scored points

using UnityEngine;

public class CollectableSpawner : MonoBehaviour 
{
	public GameObject collectablePrefab;	//What object does this script spawn?

	Transform[] spawnPoints;	//A collection of positions that collectables can be spawned at
	int lastUsedIndex = -1;		//The index of the last spawn point used


	void Start () 
	{
		//Find all of the spawn points (which are nested as children of this game object)
		spawnPoints = GetComponentsInChildren<Transform> ();

		//Spawn our first collectable
		SpawnCollectable ();
	}

	void SpawnCollectable()
	{
		//Pick a random spawn point (which is represented as an index number)
		int i = Random.Range (0, spawnPoints.Length);

		//If the index picked is the same as the last one used, keep picking new ones
		//until we find a different one. We do this so that a collectable doesn't spawn 
		//in the same spot twice, which would give the player multiple points instantly
		while(i == lastUsedIndex && spawnPoints.Length > 1)
			i = Random.Range (0, spawnPoints.Length);

		//Instatiate (create) a collectable at the spawn points position and with it's rotation
		GameObject obj = Instantiate (collectablePrefab, spawnPoints [i].position, spawnPoints [i].rotation) as GameObject;

		//Tell the Collectable script on the spawned object that "this" is the spawner that created it
		obj.GetComponent<Collectable> ().spawner = this;

		//Record the index that we just used
		lastUsedIndex = i;
	}

	//This method is called from the Collectable script when the player
	//picks it up
	public void CollectableTaken()
	{
		//If the GameManager exists, tell it that the player scored a point
		if (GameManager.instance != null) 
			GameManager.instance.PlayerScored ();

		//Since the last collectable was picked up, spawn a new one
		SpawnCollectable ();
	}
}
