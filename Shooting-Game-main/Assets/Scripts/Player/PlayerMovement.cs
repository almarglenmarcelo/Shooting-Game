using UnityEngine;

//[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float speed = 6f;
	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidBody;
	int floorMask; //palatandaan kung san nakatutok yung mouse
	float camRayLength = 100f; //gaano kahaba yung ray cast


#if UNITY_ANDROID
	public Canvas joystickCanvas;
	public VariableJoystick variableJoystickMovement;
	public VariableJoystick variableJoystickRotation;
	public bool canFire = false;
#endif

	void Awake()
    {



		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		playerRigidBody = GetComponent<Rigidbody>();

    }


    private void Start()
    {
#if UNITY_ANDROID
		joystickCanvas.gameObject.SetActive(true);
#endif
	}


	void FixedUpdate()
    {
#if UNITY_ANDROID
		float h = variableJoystickMovement.Horizontal;
		float v = variableJoystickMovement.Vertical;
#else

		float h = Input.GetAxisRaw("Horizontal"); //can have a value of 0f or 1f, if both keys are pressed it can give a 0f value
		float v = Input.GetAxisRaw("Vertical");
#endif

		Move(h, v);
		Turning();
		Animating(h, v);
    }


	void Move(float h, float v)
    {
		movement.Set(h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidBody.MovePosition(transform.position + movement); //connects and moves the rigid body along with the player


    }


	void Turning() 
	{

#if UNITY_ANDROID
		if(variableJoystickRotation.Horizontal != 0 || variableJoystickRotation.Vertical != 0)
        {
			transform.eulerAngles = new Vector3(0, Mathf.Atan2(variableJoystickRotation.Horizontal, variableJoystickRotation.Vertical) * 180 / Mathf.PI, 0);
			canFire = true;
        }
		else
        {
			canFire = false;
        }
#else

		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); // position ng mouse
		RaycastHit floorHit; //kung san currently tumatama yung Camera Ray

		if (Physics.Raycast(camRay, out floorHit, camRayLength ,floorMask)) { //saan dapat nakatutok ung character
		Vector3 playerToMouse = floorHit.point - transform.position;
		playerToMouse.y = 0f; // ind dpat gumagalaw yung y axis

		//storing a rotation using quaternion
		Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
		playerRigidBody.MoveRotation(newRotation);

		}

#endif

	}


	void Animating(float h, float v) { 
		bool isWalking = h!=0f || v != 0f;
		anim.SetBool("IsWalking", isWalking);
	}

}