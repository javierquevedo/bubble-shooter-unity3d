using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(LineRenderer))]
public class BubbleShooterController : MonoBehaviour {
	
	public bool isAiming;

	private float rotationSpeed = 50.0f;
	private float maxLeftAngle = 85.0f;
	private float maxRightAngle = 275.0f;
	
	void Start () {
		isAiming = true;
	}
	
	void Update () {
		if (isAiming){
			float mouseAxisX = Input.GetAxis("Mouse X");
			transform.Rotate(Vector3.back * mouseAxisX * this.rotationSpeed * Time.deltaTime);
			if (transform.eulerAngles.z > this.maxLeftAngle && transform.eulerAngles.z < 180.0 ){
				transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y, maxLeftAngle);
			}
			if (transform.eulerAngles.z < this.maxRightAngle && transform.eulerAngles.z > 180.0 ){
				transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y, maxRightAngle);
			}
		}
	}
}
