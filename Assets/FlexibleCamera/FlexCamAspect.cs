using UnityEngine;

namespace FlexibleCamera
{
	public class FlexCamAspect : MonoBehaviour
	{
		[Tooltip ("Does not need to be set! It will automatically try to use the FlexCam script on the same gameObject. Otherwise it will reference the static accessor.")] public FlexCam Cam;
		
		void Awake ()
		{
			if (Cam != null) return;
			Cam = GetComponent<FlexCam> ();
			if (Cam == null) Cam = FlexCam.Main;
			if (Cam == null) enabled = false;
		}

		void OnDisable ()
		{
			Cam.AspectModifier = 1;
		}
		
		void Update ()
		{
			Cam.AspectModifier = 1 / Cam.Cam.aspect;
		}
	}
}
