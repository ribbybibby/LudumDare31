﻿using UnityEngine;
using System.Collections;

public class WoodSpawn : MonoBehaviour {

	// Edit in Unity
	public int childLimit; // Number of woods that can be spawned and exist at once
	public GameObject woody; // Wood prefab
	public float timer; // Cooldown time
	public float distanceFromPlayers; // Distance from the players at which a spawn can happen

	// Don't edit in Unity
	public int child;

	// Private vars
	private GameObject bbw; // BBW
	private GameObject lrrh; // LRRH
	private float bbwDistance; // BBW distance
	private float lrrhDistance; // LRRH distance
	private bool readyToSpawn; // Has the cooldown ran down?
	
	// Find BBW and LRRH, start the cooldown, set children to 0
	void Start () {
		bbw = GameObject.FindGameObjectWithTag ("BBW");
		lrrh = GameObject.FindGameObjectWithTag ("LRRH");
		StartCoroutine (spawnCooldown());
		child = 0;
	}

	// Fixed Update:
	/* We only spawn an enemy if:
	 * 1. BBW is below or level with the spawn
	 * 2. The number of spawned children is lower than childLimit
	 * 3. The timer has proc'd
	 * 4. BBW and LRRH are distanceFromPlayers away from the spawn 
	 */
	void FixedUpdate () {

		// Find distance from Players
		bbw = GameObject.FindGameObjectWithTag ("BBW");
		lrrh = GameObject.FindGameObjectWithTag ("LRRH");
		bbwDistance = Vector3.Distance(gameObject.transform.position, bbw.gameObject.transform.position);
		lrrhDistance = Vector3.Distance(gameObject.transform.position, lrrh.gameObject.transform.position);

		// Spawn
		if ((gameObject.transform.position.y - bbw.gameObject.transform.position.y) >= -1.5f && child < childLimit 
		    && readyToSpawn == true && bbwDistance > distanceFromPlayers && lrrhDistance > distanceFromPlayers) 
		{
			GameObject newWood = (GameObject)Instantiate(woody, gameObject.transform.position, Quaternion.identity);
			newWood.GetComponent<EnemyReceiver>().spawnParent = gameObject;
			newWood.name = "WoodCutter";
			child++;
			StartCoroutine (spawnCooldown());
		}
	}

	// Cooldown that controls the readyToSpawn bool
	IEnumerator spawnCooldown(){
		readyToSpawn = false;
		float rndNo = Random.Range (1, 5);
		yield return new WaitForSeconds(timer + (timer / rndNo));
		readyToSpawn = true;
	}
}