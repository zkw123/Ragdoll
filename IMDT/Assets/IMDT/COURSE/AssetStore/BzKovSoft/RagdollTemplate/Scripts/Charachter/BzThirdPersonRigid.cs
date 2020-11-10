using UnityEngine;

namespace BzKovSoft.RagdollTemplate.Scripts.Charachter
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public sealed class BzThirdPersonRigid : BzThirdPersonBase
	{
		CapsuleCollider _capsuleCollider;
		Rigidbody _rigidbody;

		protected override void Awake()
		{
			base.Awake();
			_capsuleCollider = GetComponent<CapsuleCollider>();
			_rigidbody = GetComponent<Rigidbody>();

			if (GetComponent<CharacterController>() != null)
				Debug.LogWarning("You do not needed to attach 'CharacterController' to controller with 'Rigidbody'");
		}

		public override void CharacterEnable(bool enable)
		{
			base.CharacterEnable(enable);
			_capsuleCollider.enabled = enable;
			_rigidbody.isKinematic = !enable;
			if (enable)
				_firstAnimatorFrame = true;
		}

		protected override Vector3 PlayerVelocity { get { return _rigidbody.velocity; } }

		protected override void ApplyCapsuleHeight()
		{
			float capsuleY = _animator.GetFloat(_animatorCapsuleY);
			_capsuleCollider.height = capsuleY;
			var c = _capsuleCollider.center;
			c.y = capsuleY / 2f;
			_capsuleCollider.center = c;
		}

#region Ground Check

		/// <summary>
		/// every FixedUpdate _groundChecker resets to false,
		/// and if collision with ground found till next FixedUpdate,
		/// the character on a ground
		/// </summary>
		bool _groundChecker;
		float _jumpStartedTime = -1.0f;
		Vector3 _jumpStartedPos = Vector3.zero;
		bool _jumping = false;
		
		[SerializeField]
		float _ragdollCrashThreshold = 2.0f;

		void ProccessOnCollisionOccured(Collision collision)
		{
			// if collision comes from botton, that means
			// that character on the ground
			float charBottom =
				transform.position.y +
				_capsuleCollider.center.y - _capsuleCollider.height / 2 +
				_capsuleCollider.radius * 0.8f;
				
			
			foreach (ContactPoint contact in collision.contacts)
			{
				if (contact.point.y < charBottom && !contact.otherCollider.transform.IsChildOf(transform))
				{
					_groundChecker = true;
					//Debug.DrawRay(contact.point, contact.normal, Color.blue);
					break;
				}
			}
			
			if (!_groundChecker)
			{
				// hit something in the air, start falling
				_airVelocity = new Vector3(0, -1.0f, 0);
				
				Vector3 crashVelocity = _rigidbody.velocity;
				crashVelocity.y = 0;
				
				Vector3 crashDelta = _jumpStartedPos - transform.position;
				crashDelta.y = 0;
				
				if ((crashDelta.magnitude > 1.0f) && (crashVelocity.magnitude > _ragdollCrashThreshold))
				{
					Debug.LogFormat("ForceRagdoll() crashDelta={0}; crashVelocity={1}", crashDelta.magnitude, crashVelocity.magnitude);
					ForceRagdoll();
				}
			}
		}

		void OnCollisionStay(Collision collision)
		{
			ProccessOnCollisionOccured(collision);
			
		}
		
		void OnCollisionEnter(Collision collision)
		{
			ProccessOnCollisionOccured(collision);
			if (collision.gameObject.name == "SedanCar"|| collision.gameObject.name == "BusVehicle" || collision.gameObject.name == "SportsCar" || collision.gameObject.name == "UtilityVehicle")
			{
				ForceRagdoll();
				Vector3 v = collision.gameObject.GetComponent<LPPV_CarController>()._rgbd.velocity;
				collision.gameObject.GetComponent<LPPV_CarController>()._rgbd.velocity = new Vector3(1.5f * v.x, 1.5f * v.y, 1.5f * v.z);
			}
		}
		
		bool JustBeginJumping(float threshold) { return (Time.time - _jumpStartedTime < threshold); }

		protected override bool PlayerTouchGound()
		{
			if (!_jumping)
			{
				return true;
			}
			
			//Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0.1f, 0), Color.green);
			//Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0, 0.1f), Color.red);
			
			bool grounded = _groundChecker;
			
			if (_jumping && _groundChecker && !JustBeginJumping(0.5f))
			{
				_jumping = false;
			}
			
			_groundChecker = false;
			// if the character is on the ground and
			// half of second was passed, return true
			
			return grounded;// & (_jumpStartedTime + 0.5f < curTime );
		}

#endregion
		protected override void UpdatePlayerPosition(Vector3 deltaPos)
		{
			Vector3 finalVelocity = deltaPos / Time.deltaTime;
			if (!_jumpPressed)
			{
				finalVelocity.y = _rigidbody.velocity.y;
			}
			else
			{
				_jumpStartedTime = Time.time;
				_jumpStartedPos = transform.position;
				_jumping = true;
			}
			_airVelocity = finalVelocity;		// i need this to correctly detect player velocity in air mode
			_rigidbody.velocity = finalVelocity;
		}
	}
}