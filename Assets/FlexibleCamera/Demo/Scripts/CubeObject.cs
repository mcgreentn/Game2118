using UnityEngine;
using FlexibleCamera;

namespace FlexibleCameraDemo
{
	public class CubeObject : MonoBehaviour
	{
		public int ObjType = 0; // 0: None, 1: Cube, 2: BigCube.

		void Start ()
		{
			if (ObjType == 2) FlexCam.Main.AddPermanentLockOn (gameObject);
		}

		void OnMouseDown ()
		{
			if (ObjType == 1) FlexCam.Main.SetLockOn (gameObject);
		}
	}

}
