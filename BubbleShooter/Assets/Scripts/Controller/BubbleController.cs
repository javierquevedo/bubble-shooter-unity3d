using UnityEngine;
using System.Collections;

namespace com.javierquevedo{
	public class BubbleController : MonoBehaviour {
		
		/*
		*  Model
		*/
		public Bubble bubble; 
		
		
		/*
		 * View Properties
		 */
		public float leftBorder; // TODO Move to Geometry Delegate
		public float rightBorder; // TODO Move To Geometry Delegate
		public float topBorder; // TODO Move to Geometry Delegate
		
		public float linearSpeed; // units / sec
		public float radius;
		public float angle; // -90 .. 90  The trayectory angle ___\___
		public bool isMoving;
		
		/* Constants */
		private const float _killSpeed = 10.0f;
		
		/*
		 * Delegates
		 */
		
		MotionDetectionDelegate motionDelegate;
		public delegate bool MotionDetectionDelegate(Vector3 position);
		
		CollisionDetectionDelegate collisionDelegate;
		public delegate void CollisionDetectionDelegate(GameObject bubble);
	
		/* Setters and getters */
		public CollisionDetectionDelegate CollisionDelegate{
			set{
			collisionDelegate = value;			
			}
		}
		public MotionDetectionDelegate MotionDelegate{
			set{
				motionDelegate = value;
			}
		}
	
		void Awake(){
			bubble = new Bubble(JQUtils.GetRandomEnum<BubbleColor>());
		}
	
		void Start () {
			
			
			this.GetComponent<Renderer>().material.color = JQUtils.ColorForBubbleColor(bubble.color);		
		}

		void Update () {
			if (isMoving){
				this.transform.Translate(Vector3.right * this.linearSpeed * Mathf.Cos(Mathf.Deg2Rad *this.angle) * Time.deltaTime);
				this.transform.Translate(Vector3.up * this.linearSpeed * Mathf.Sin(Mathf.Deg2Rad *this.angle) * Time.deltaTime);
				if (this.motionDelegate != null){
					if (!this.motionDelegate(this.transform.position)){
						// revert position
						this.transform.Translate(Vector3.left * this.linearSpeed * Mathf.Cos(Mathf.Deg2Rad *this.angle) * Time.deltaTime);	
						this.transform.Translate(Vector3.down * this.linearSpeed * Mathf.Sin(Mathf.Deg2Rad *this.angle) * Time.deltaTime);
						this.isMoving = false;
						if (collisionDelegate != null)
						{
							collisionDelegate(this.gameObject);
						}
					}
					else
					{
					this.updateDirection();
					}
				}	
			}
		}
		
		public void kill(bool explodes){
			StopAllCoroutines();
			Destroy(this.transform.GetComponent<Rigidbody>());
			Destroy(this.transform.GetComponent<Collider>());
			if (explodes)
			{
				StartCoroutine(scaleTo(new Vector3(0,0,0), 0.15f));
			}else
			{
				Vector3 killPosition = new Vector3(this.transform.position.x, 0f, 0f);
				float distance = Vector3.Distance(this.transform.position, killPosition);
				this.moveTo(killPosition, distance/_killSpeed);
			}
		}
		
		public void moveTo(Vector3 destination, float duration){
			StartCoroutine(tweenTo(destination, duration));	
		}
		
		IEnumerator tweenTo(Vector3 destination, float duration){
			float timeThrough = 0.0f;
			Vector3 initialPosition = transform.position;
			while (Vector3.Distance(transform.position, destination) >= 0.05){
				
				timeThrough += Time.deltaTime;
				Vector3 target = Vector3.Lerp(initialPosition, destination, timeThrough / duration);
				transform.position = target;
				yield return null;
			}
			transform.position = destination;
			if (this.GetComponent<Rigidbody>() == null){
				Destroy (this.gameObject);
			}
		}
		
		IEnumerator scaleTo(Vector3 scale, float duration){
			float timeThrough = 0.0f;
			
			Vector3 initialScale = transform.localScale;

			while (transform.localScale.x >= 0.1){
				timeThrough += Time.deltaTime;
				Vector3 target = Vector3.Lerp(initialScale, scale, timeThrough / duration);
				transform.localScale = target;
				yield return null;
			}
			if (this.GetComponent<Rigidbody>() == null){
				Destroy (this.gameObject);
			}
		}
		
		
		void OnTriggerEnter(Collider collider){
			if (this.isMoving){
				this.isMoving = false;
				if (collisionDelegate != null){
					collisionDelegate(this.gameObject);
				}
			}
		}
			
		void updateDirection(){
			
			/*TODO: Piority Medium
			 * Warning, since we are updating after moving, there is a chance that 
			 * we could fall outside of the border if there was not sufficent time between 
			 * two clock ticks. Improvement: Move only until the border coordinate max, and if there is an excess,
			 * move the excess in the opposite direction 
			 * 
			 */
			
			if (this.transform.position.x + this.radius >= this.rightBorder || this.transform.position.x - this.radius <= this.leftBorder){
				this.angle = 180.0f - this.angle;
				if (this.transform.position.x + this.radius >= this.rightBorder)
					this.transform.position = new Vector3(this.rightBorder - this.radius, this.transform.position.y, this.transform.position.z);
				if(this.transform.position.x - this.radius <= this.leftBorder)
					this.transform.position = new Vector3(this.leftBorder + this.radius, this.transform.position.y, this.transform.position.z);
			}
			
			if (this.transform.position.y + this.radius >= this.topBorder){
				this.isMoving = false;
				if (collisionDelegate != null){
					collisionDelegate(this.gameObject);
				}
			}
		}
	}
}