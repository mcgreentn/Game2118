using UnityEngine;
using FlexibleCamera;

namespace FlexibleCameraDemo
{
	public class FlexCamExample : MonoBehaviour
	{
		// Purpose: Show in a demo how to interact with the main script (FlexCam.cs).
		//   While the cursor is at the edge, gradually increase the CursorFollowModifier, otherwise decrease it back to its original value.

		public float MaxMultiplier = 2.0f; // Sets the maximum based on its base modifier.
		public float Threshold = 0.8f; // The limit at which the mod either increases or decreases.
		public float IncreaseRate = 0.3f; // Relative increase per second.
		public float DecreaseRate = 0.7f; // Relative decrease per second.

		float baseMod = 0; // The base value of the CursorFollowModifier.

		void Start ()
		{
			baseMod = FlexCam.Main.CursorFollowModifier;
		}

		void Update ()
		{
			Vector2 v = FlexCam.Main.GetMouseVector ();
			if (Mathf.Abs (v.x) > Threshold || Mathf.Abs (v.y) > Threshold)
			{
				FlexCam.Main.CursorFollowModifier = Mathf.Min (FlexCam.Main.CursorFollowModifier + ((MaxMultiplier - 1) * baseMod * IncreaseRate * FlexCam.Main.TimeFlow), baseMod * MaxMultiplier);
			}
			else
			{
				FlexCam.Main.CursorFollowModifier = Mathf.Max (FlexCam.Main.CursorFollowModifier - ((MaxMultiplier - 1) * baseMod * DecreaseRate * FlexCam.Main.TimeFlow), baseMod);
			}
		}
	}

}
