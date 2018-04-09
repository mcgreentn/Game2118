using UnityEngine;
using UnityEngine.SceneManagement;
using FlexibleCamera;

namespace FlexibleCameraDemo
{
	public class TestPlayer : MonoBehaviour
	{
		[Header ("Camera Settings")]
		public bool DoRegularShakes = false;
		public float ShakeInterval = 4.0f;

		[Header ("Map Settings")]
		public bool SpawnCubes = false;
		public GameObject CubeContainer;
		public GameObject SmallCube;
		public GameObject MediumCube;
		public GameObject LargeCube;
		public int SpawnSmall = 100;
		public int SpawnMedium = 30;
		public int SpawnLarge = 10;
		public float SpawnRadius = 50f;
		public float SpawnPower = 0.5f;
		public float SpawnHeight = 12;

		[Header ("Scenes")]
		public Scene SceneOne;
		public Scene SceneTwo;

		float sInterval = 0;

		// This script is intended to be condense to reduce the amount of scripts.

		// Initiation for demo.
		void Start ()
		{
			if (SpawnCubes)
			{
				SpawnCube (LargeCube, SpawnLarge);
				SpawnCube (MediumCube, SpawnMedium);
				SpawnCube (SmallCube, SpawnSmall);
			}
		}
		
		void SpawnCube (GameObject cube, int amount)
		{
			if (cube == null || CubeContainer == null || amount <= 0) return;

			for (int i = 0; i < amount; i++)
			{
				Vector3 v = GetRandomPosition (SpawnRadius, SpawnPower, SpawnHeight);
				Instantiate (cube, v, Quaternion.identity, CubeContainer.transform);
			}
		}

		static Vector3 GetRandomPosition (float radius, float p, float height)
		{
			float x = (Mathf.Pow (Random.value, p)) * radius * (Random.value >= 0.5f ? 1 : -1);
			float y = (Random.value + 1) * height;
			float z = (Mathf.Pow (Random.value, p)) * radius * (Random.value >= 0.5f ? 1 : -1);
			return new Vector3 (x, y, z);
		}



		// Some camera effects with the player object.
		void OnCollisionEnter (Collision collision)
		{
			if (collision.impulse.magnitude > 100 && !collision.collider.gameObject.name.Contains ("Wall") && !collision.collider.gameObject.name.Contains ("Platform"))
				FlexCam.Main.Shake (Mathf.Min (collision.impulse.magnitude / 7000, 5)); // Ranges roughly from 100 (small) to 2000 (standard) to 25000 (high).
		}

		void OnMouseDown ()
		{
			ToggleActivation ();
		}

		void Update ()
		{
			if (DoRegularShakes && ShakeInterval > 0) DoShake ();
		}

		void DoShake ()
		{
			sInterval += Time.deltaTime;
			if (sInterval >= ShakeInterval)
			{
				sInterval = sInterval % ShakeInterval;
				FlexCam.Main.Shake (1);
			}
		}



		// Menu elements.
		public void QuitGame ()
		{
#if UNITY_STANDALONE
			Application.Quit ();
#endif

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}

		public void ToggleActivation ()
		{
			if (!FlexCam.Main.CameraState) FlexCam.Main.Activate ();
			else FlexCam.Main.Deactivate ();
		}
	}
}
