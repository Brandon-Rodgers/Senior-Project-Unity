using UnityEngine;
using System.Collections;


public class PlayerControl : MonoBehaviour {
	//speed variables
	public float movementSpeed = 1.0f;
	public float mouseSensitivity = 4.0f;
	public float jumpSpeed = 20.0f;
	public float upDownRange = 70.0f;
	//variable to reset gravity
	int gravityReset = 0;
	//Pause menu gameobject
	public GameObject pauseScreen;
	//Death sound
	public AudioClip death;
	private AudioSource deathSound;



	CharacterController characterController;
	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		characterController = GetComponent<CharacterController>();
		//Value Givers
		GameVariables.verticalRotation = 0;
		GameVariables.sprintSpeed = 1;
		GameVariables.lastCollide = GameObject.Find ("FloatingIsland");
		GameVariables.paused = false;
		GameVariables.resume = true;
		pauseScreen.SetActive (false);
		//
		deathSound = this.GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		//Pause Screen Scripts
		if (Input.GetButtonUp("Pause")){
			Time.timeScale = 0f;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			GameVariables.paused = true;
			GameVariables.resume = false;
			pauseScreen.SetActive (true);
		}
		// resume game
		if (!GameVariables.paused && GameVariables.resume) {
			Time.timeScale = 1f;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			GameVariables.resume = false;
		}
		//Full character Movement
		if (!GameVariables.paused && !GameVariables.died && !YouWin.win) {
			//Vertical Rotation
			GameVariables.verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
			GameVariables.verticalRotation = Mathf.Clamp (GameVariables.verticalRotation, -upDownRange, upDownRange);
			Camera.main.transform.localRotation = Quaternion.Euler (GameVariables.verticalRotation, 0, 0);

			//Movement
			if (characterController.isGrounded) {
				// get button imputs
				GameVariables.forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed * GameVariables.sprintSpeed;
				GameVariables.sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed * GameVariables.sprintSpeed;
				//Horizontal Rotation by mouse
				float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
				transform.Rotate (0, rotLeftRight, 0);
			}
			//Sprinting by shift
			if (Input.GetButton ("Sprint") && characterController.isGrounded && Input.GetAxis ("Vertical") > 0) {
				GameVariables.sprintSpeed += ((Mathf.Pow (GameVariables.sprintSpeed, 2)) / 3) * Time.deltaTime;
				GameVariables.sprintSpeed = Mathf.Clamp (GameVariables.sprintSpeed, 0, 5);
			}
			//Stop Sprinting
			if ((!Input.GetButton ("Sprint") && characterController.isGrounded) || (characterController.isGrounded && Input.GetAxis ("Vertical") == 0)) {
				GameVariables.sprintSpeed = 1;
			}
			//Jumping
			if (characterController.isGrounded && Input.GetButtonDown ("Jump")) {
				GameVariables.verticalVelocity = jumpSpeed;
			}
			//set up the movement vector and move
			Vector3 speed = new Vector3 (GameVariables.sideSpeed, GameVariables.verticalVelocity, GameVariables.forwardSpeed);

			speed = transform.rotation * speed;

			characterController.Move (speed * Time.deltaTime);

			//Gravity Reset
			if (characterController.isGrounded && gravityReset == 0) {
				gravityReset = 1;
				GameVariables.verticalVelocity = -0.5f;
			}
			// Gravity
			if (!characterController.isGrounded) {
				GameVariables.verticalVelocity += Physics.gravity.y * Time.deltaTime;
				gravityReset = 0;
			}

		}
		if (GameVariables.deathSound){
			deathSound.PlayOneShot(death);
			GameVariables.deathSound = false;
		}
	}

	void OnTriggerEnter (Collider collider) {
		// track last object collided with
		if (collider.gameObject.name != "Player") { // line added so you cant collide with yourself at high speeds
			GameVariables.lastCollide = collider.gameObject;
		}
		// connect to circle platform
		if (collider.gameObject.name == "ClockwiseRotate1") {
			transform.parent = GameObject.Find ("ClockwiseRotate1").transform;
		}
		if (collider.gameObject.name == "ClockwiseRotate2") {
			transform.parent = GameObject.Find ("ClockwiseRotate2").transform;
		}
		if (collider.gameObject.name == "ClockwiseRotate3") {
			transform.parent = GameObject.Find ("ClockwiseRotate3").transform;
		}
		if (collider.gameObject.name == "ClockwiseRotate4") {
			transform.parent = GameObject.Find ("ClockwiseRotate4").transform;
		}
		if (collider.gameObject.name == "ClockwiseRotate5") {
			transform.parent = GameObject.Find ("ClockwiseRotate5").transform;
		}
	}
}


