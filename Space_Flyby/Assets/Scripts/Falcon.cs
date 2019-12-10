using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falcon : MonoBehaviour {


	//Rear lighting for adjustments
	public Light rear_lighting;


	//Cameras to switch between.
	public Camera main_camera;
	public Camera cockpit_camera;


    //Maximum values for the controllers thrust
    private const float MINIMUM_THRUST_VAL = -1.0f;
    private const float MAXIMUM_THRUST_VAL = 1.0f;

	private const float MAX_REAR_LIGHTING = 10.0f;
	private const float MIN_REAR_LIGHTING = 1.0f;


	//Scale the rotations by this value for better turns.
	private const float MOVE_MULTIPLIER = 1.2f;


    //Max Speed of the craft.
	private const float SPEED_MODIFIER = 20;
	private const float ZOOM_SPEED = 50;




	private bool primary_camera_is_active = true;
	private bool valid_toggle_time = true;

	// Use this for initialization
	void Start () {
		
	}
	

	//Used to scale values from one range to another.
	public float scale(float old_Min, float old_Max, 
						float new_Min, float new_Max, float old_Value){
    	float old_Range = (old_Max - old_Min);
    	float new_Range = (new_Max - new_Min);
    	float new_Value = (((old_Value - old_Min) * new_Range) / old_Range) + new_Min;
    	return new_Value;
	}


	private void Toggle_Camera(){
		if(primary_camera_is_active){
			primary_camera_is_active = false;
			main_camera.enabled = false;
			cockpit_camera.enabled = true;
		}else{
			primary_camera_is_active = true;
			main_camera.enabled = true;
			cockpit_camera.enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {


        //Get all relevant movement params
		float moveX = Input.GetAxis ("Horizontal");
		float moveY = Input.GetAxis ("Vertical");
		//float moveZ = Input.GetAxis ("Diagonal");

        //Get thrust param
		float thrust = Input.GetAxis ("Jump");

        //Apply thrust
		if (thrust < 1) {
			float speed = -(thrust - 1); 
			transform.position += transform.forward * Time.deltaTime * speed * SPEED_MODIFIER;
		}

		//Set intensity of rear lighting
		float light_val = scale(MINIMUM_THRUST_VAL, MAXIMUM_THRUST_VAL, 
								MIN_REAR_LIGHTING, MAX_REAR_LIGHTING, -thrust);
		rear_lighting.intensity = light_val;



		//Logic here for swapping cameras.
		if(Input.GetKeyUp(KeyCode.V) == true){
			valid_toggle_time = true;
		}
		if(Input.GetKeyDown(KeyCode.V) == true && valid_toggle_time == true){
			Toggle_Camera();
			valid_toggle_time = false;
		}


        //Apply rotation
		transform.Rotate (moveY * MOVE_MULTIPLIER, 0, 0);
		transform.Rotate (0, 0, -moveX * MOVE_MULTIPLIER);


	}
}