using UnityEngine;

namespace FlexibleCameraDemo
{
	public class CubeControl : MonoBehaviour
	{
		// This script is composed of Unity standard assets and altered for use in the demo.

		[SerializeField] float m_MovePower = 5; // The force added to the cube to move it.
		[SerializeField] float m_DispositionPower = 5; // The disposition magnitude in case no physics are used for movement.
		[SerializeField] bool m_UsePhysics = true; // Whether to use force to move the cube.
		[SerializeField] float m_JumpPower = 2; // The force added to the cube when it jumps.
		[SerializeField] float m_GroundRayLength = 5; // The maximum distance the cube can jump.

		Rigidbody m_Rigidbody;

		Vector3 move;
		bool jump;

		void Start ()
		{
			m_Rigidbody = GetComponent<Rigidbody> ();
		}

		public void Move (Vector3 moveDirection, bool jump)
		{
			if (m_UsePhysics)
			{
				m_Rigidbody.AddForce (moveDirection * m_MovePower);
			}
			else
			{
				gameObject.transform.position = gameObject.transform.position + moveDirection * m_DispositionPower / 10;
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
