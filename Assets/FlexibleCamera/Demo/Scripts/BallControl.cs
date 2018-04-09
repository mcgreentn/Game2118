using UnityEngine;

namespace FlexibleCameraDemo
{
	public class BallControl : MonoBehaviour
	{
		// This script is composed of Unity standard assets and altered for use in the demo.

		[SerializeField] float m_MovePower = 5; // The force added to the ball to move it.
		[SerializeField] bool m_UseTorque = true; // Whether or not to use torque to move the ball.
		[SerializeField] float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.
		[SerializeField] float m_JumpPower = 2; // The force added to the ball when it jumps.
		[SerializeField] float m_GroundRayLength = 5; // The maximum distance the ball can jump.
		
		Rigidbody m_Rigidbody;

		Vector3 move;
		bool jump;

		void Start ()
		{
			m_Rigidbody = GetComponent<Rigidbody> ();
			GetComponent<Rigidbody> ().maxAngularVelocity = m_MaxAngularVelocity;
		}

		public void Move (Vector3 moveDirection, bool jump)
		{
			if (m_UseTorque)
			{
				m_Rigidbody.AddTorque (new Vector3 (moveDirection.z, 0, -moveDirection.x) * m_MovePower);
			}
			else
			{
				m_Rigidbody.AddForce (moveDirection * m_MovePower);
			}

			if (Physics.Raycast (transform.position, -Vector3.up, m_GroundRayLength) && jump)
			{
				m_Rigidbody.AddForce (Vector3.up * m_JumpPower, ForceMode.Impulse);
			}
		}

		void Update ()
		{
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");
			jump = Input.GetButton ("Jump");

			move = (v * Vector3.forward + h * Vector3.right).normalized;
		}

		void FixedUpdate ()
		{
			Move (move, jump);
			jump = false;
		}
	}
}
