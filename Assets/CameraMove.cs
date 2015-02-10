﻿using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public int lockMidVector; // 1, 2 or 3 - decides which camera code we use
	public float camSpeed; // The speed the camera moves at
	public float moveDistance; // The distance the camera should be from the midVector before moving
	public float camSizeDivider; // The number we divide the distance between the characters by (which we then then add to the orthographic camera size)
	public GameObject background; // The background object

	private GameObject bbw; // BBW
	private GameObject lrrh; // LRRH
	private Vector3 midVector; // The Vector between LRRH and BBW
	private float charDistance; // The distance between LRRH and BBW
	private float origCamSize; // The starting size of the Camera
	private Vector3 camVector; // The Vector we use to apply camSpeed
	private float screenQuart; // The three-quarter point of the screen
	private bool moveUp; // Should the camera be moving up?
	private float newPosY; // New Y position for the camera to move to

	// Use this for initialization
	void Start () {
		// Find the char game objects
		bbw = GameObject.Find ("BBW");
		lrrh = GameObject.Find ("LRRH");

		// Find the midVector
		midVector = new Vector3 ((bbw.transform.position.x+lrrh.transform.position.x)/2, (bbw.transform.position.y+lrrh.transform.position.y)/2, gameObject.transform.position.z);

		// Store the original size of the camera
		origCamSize = gameObject.camera.orthographicSize;

		// Create the camVector using camSpeed
		camVector = new Vector3 (0, camSpeed, 0);

		// Start the camera at the midVector
		gameObject.transform.position = midVector;

	}
	
	// Update is called once per frame
	void Update () 
	{

		// Find BBW and LRRH each update, just in case they've switched
		bbw = GameObject.Find ("BBW");
		lrrh = GameObject.Find ("LRRH");

		// Find the mid vector between the two
		midVector = new Vector3 ((bbw.transform.position.x+lrrh.transform.position.x)/2, (bbw.transform.position.y+lrrh.transform.position.y)/2, gameObject.transform.position.z);

		// Find the distance between the two
		charDistance = Vector3.Distance (bbw.transform.position, lrrh.transform.position);

		switch (lockMidVector) 
		{
		case 1:
			// ONE: Moves up or down depending on whether the characters are outside of a tolerance area
			CameraOne();
			break;
		case 2:
			// TWO: Moves 1:1 with the MidVector
			CameraTwo();
			break;
		case 3:
			// THREE: Moves 1:1 with BBW
			CameraThree();
			break;
		case 4:
			// FOUR: Readjusts to the centre of the screen after both characters reach the top quarter of the screen
			CameraFourandFive(4);
			break;
		case 5:
			// FIVE: Readjusts so that the chars are in the bottom quarter of the camera's view once they reach the top quarter
			CameraFourandFive(5);
			break;
		default:
			// DEFAULT: Use Camera 2 by default
			CameraTwo ();
			break;
		}
	}

	void CameraOne()
	{
		// Adjust cam size based on charDistance
		gameObject.camera.orthographicSize = origCamSize + (charDistance / camSizeDivider);
		
		// If the distance between the Camera and the mid vector on the y-axis is below -moveDistance, move the camera down
		if ((midVector.y - gameObject.transform.position.y) < -moveDistance) {
			gameObject.transform.position -= camVector*Time.deltaTime;
		}
		// If the distance between the Camera and the mid vector on the y-axis is above moveDistance, move the camera up
		if ((midVector.y - gameObject.transform.position.y) > moveDistance) {
			gameObject.transform.position += camVector*Time.deltaTime;
		}
		
		// Move the camera along with the mid vector on the x-axis
		gameObject.transform.position = new Vector3 (midVector.x, gameObject.transform.position.y, gameObject.transform.position.z);
		
		// Move the background in sync with the camera
		background.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, background.transform.position.z);
	}
	
	void CameraTwo() 
	{
		// Adjust cam size based on charDistance
		gameObject.camera.orthographicSize = origCamSize + (charDistance / camSizeDivider);
		
		// Move Camera in sync with the mid vector
		gameObject.transform.position = new Vector3 (midVector.x, midVector.y, gameObject.transform.position.z);
		
		// Move the background in sync with the camera
		background.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, background.transform.position.z);
	}
	
	void CameraThree ()
	{
		// Adjust cam size based on charDistance
		gameObject.camera.orthographicSize = origCamSize + (charDistance / camSizeDivider);
		
		// Move camera in sync with BBW
		gameObject.transform.position = new Vector3 (bbw.transform.position.x, bbw.transform.position.y, gameObject.transform.position.z);
		
		// Move background in sync with the camera
		background.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, background.transform.position.z);
	}
	
	void CameraFourandFive (int choice) 
	{
		// Find the start of the top quarter of the screen (y-axis)
		screenQuart = gameObject.transform.position.y + (gameObject.camera.orthographicSize/2);

		// If both chars are in the top quarter, and the camera isn't currently moving already, move up
		if (bbw.transform.position.y > screenQuart && lrrh.transform.position.y > screenQuart && moveUp == false)
		{		
			// Cam 4: centre on where the chars are
			if (choice == 4) 
			{
				newPosY = midVector.y;
			}
			// Cam 5: Put the chars in the bottom quarter of the screen
			if (choice == 5)
			{
				newPosY = midVector.y + (gameObject.camera.orthographicSize/2);
			}
			moveUp = true;
		}
		
		if (moveUp == true)
		{
			// If we're at the point we need to be, stop moving
			if (gameObject.transform.position.y >= newPosY)
			{
				moveUp = false;
			}
			// Move up at the speed of camVector
			else
			{
				gameObject.transform.position += camVector*Time.deltaTime;
			}
		}
		
		// Move the background in sync with the camera
		background.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, background.transform.position.z);
	}
	
}
