using UnityEngine;
using System.Collections.Generic;

namespace FlexibleCamera
{
	public class FlexCamRestrictor : MonoBehaviour
	{
		[Tooltip ("Does not need to be set! It will automatically try to use the FlexibleCam script on the same gameObject. Otherwise it will reference the static accessor.")] public FlexCam Cam;

		[Header ("General")]
		[Tooltip ("Used to determine which single dimensions to limit.")] public bool Active = true;

		[Header ("Vector Restrictions")]
		[Tooltip ("Sets the maximum x, y, z values for the camera's target vectors, so that it will not go beyond those values.")] public Vector3 LowerLimit = Vector3.zero;
		[Tooltip ("Sets the maximum x, y, z values for the camera's target vectors, so that it will not go below those values.")] public Vector3 UpperLimit = Vector3.zero;
		[Tooltip ("Used to determine which single dimensions to limit.")] public bool MinX = false;
		[Tooltip ("Used to determine which single dimensions to limit.")] public bool MaxX = false;
		[Tooltip ("Used to determine which single dimensions to limit.")] public bool MinY = false;
		[Tooltip ("Used to determine which single dimensions to limit.")] public bool MaxY = false;
		[Tooltip ("Used to determine which single dimensions to limit.")] public bool MinZ = false;
		[Tooltip ("Used to determine which single dimensions to limit.")] public bool MaxZ = false;

		[Header ("Collider Restrictions")]
		[Tooltip ("Allows setting a collider to define the boundaries of the camera.")] public Collider ColliderLimit = null;
		[Tooltip ("Allows setting multiple collider to define the boundaries of the camera.")] public List<Collider> ColliderLimits = new List<Collider> ();

		protected virtual float VMinX { get { return Mathf.Min (LowerLimit.x, UpperLimit.x); } }
		protected virtual float VMinY { get { return Mathf.Min (LowerLimit.y, UpperLimit.y); } }
		protected virtual float VMinZ { get { return Mathf.Min (LowerLimit.z, UpperLimit.z); } }
		protected virtual float VMaxX { get { return Mathf.Max (LowerLimit.x, UpperLimit.x); } }
		protected virtual float VMaxY { get { return Mathf.Max (LowerLimit.y, UpperLimit.y); } }
		protected virtual float VMaxZ { get { return Mathf.Max (LowerLimit.z, UpperLimit.z); } }

		void Awake ()
		{
			if (Cam != null) return;
			Cam = GetComponent<FlexCam> ();
			if (Cam == null) Cam = FlexCam.Main;
			if (Cam == null) enabled = false;
		}

		public Vector3 ApplyRestrictions (Vector3 vec)
		{
			Vector3 v = new Vector3 (vec.x, vec.y, vec.z);
			if (!Active) return v;

			if (MinX) v.x = Mathf.Max (v.x, VMinX);
			if (MaxX) v.x = Mathf.Min (v.x, VMaxX);
			if (MinY) v.y = Mathf.Max (v.y, VMinY);
			if (MaxY) v.y = Mathf.Min (v.y, VMaxY);
			if (MinZ) v.z = Mathf.Max (v.z, VMinZ);
			if (MaxZ) v.z = Mathf.Min (v.z, VMaxZ);

			if (ColliderLimits.Count > 0)
			{
				List<Collider> L = new List<Collider> (ColliderLimits).FindAll (j => j != null && j.enabled && j.gameObject.activeInHierarchy);
				if (ColliderLimit != null && ColliderLimit.enabled && ColliderLimit.gameObject.activeInHierarchy) L.Add (ColliderLimit);
				if (L.Count > 0)
				{
					float[] distances = new float[L.Count];

					int i = 0;
					foreach (Collider c in L)
					{
						distances[i++] = Vector3.Distance (c.ClosestPoint (v), v);
					}

					v = L[GetLowest (distances)].ClosestPoint (v);
				}
			}
			else if (ColliderLimit != null && ColliderLimit.enabled && ColliderLimit.gameObject.activeInHierarchy)
			{
				Debug.Log ("a");
				v = ColliderLimit.ClosestPoint (v);
			}

			return v;
		}

		int GetLowest (float[] c)
		{
			if (c.Length == 0) return 0;
			int index = 0;
			float val = c[0];

			for (int i = 0; i < c.Length; i++)
			{
				if (c[i] < val)
				{
					index = i;
					val = c[i];
				}
			}

			return index;
		}
	}
}
