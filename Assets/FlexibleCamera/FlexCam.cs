using UnityEngine;
using System.Collections.Generic;

namespace FlexibleCamera
{
	public class FlexCam : MonoBehaviour
	{

// --- External Settings -----------------------------------------------------------------------------------------------------------------------------

		[Header ("Basic")]
		[Tooltip ("Assign the Player GameObject to this. Or leave it empty and use the Initialize method later.")] public GameObject PlayerObject;
		[Tooltip ("Sets the camera to a specific offset relative to the Player.")] public Vector3 CameraOffset = new Vector3 (0, 10.0f, 0);
		[Tooltip ("Sets a given camera rotation, enabling a tilted view or to rotate it for an entirely different view type.\n\n"
			+ "In order to tilt the camera a little, set CameraBaseRotation for example to 80, 0, 0 and the CameraOffset to 0, 14, -3.")] public Vector3 CameraBaseRotation = new Vector3 (90, 0, 0);
		[Tooltip ("A reference to the camera. It is automatically set if it is on the same GameObject as this script.")] public Camera Cam = null;
		[Tooltip ("Used to define the restrictions of the camera movement. It is automatically set if it is on the same GameObject as this script.")] public FlexCamRestrictor Restrictor = null;
		[Tooltip ("Toggles the camera restrictions on or off. Does nothing if no Restriction script is set.")] public bool RestrictionActive = true;

		[Header ("General")]
		[Tooltip ("Toggles the camera movements on or off. When toggling it off with this, it will smoothy come to a halt. When toggling the script off, it will stop rapidly."
			+ "Either way it will resume the camera work smoothly. While this is off, the camera will continue to perform the CameraShake effect.")] public bool GeneralActive = true;
		[Tooltip ("Use this to scale MoveAhead and Zooming effects with other scripts, for example for varying maximum speeds.")] public float MovementScale = 1.0f;
		[Tooltip ("Time in seconds the camera takes to refocus after a reactivation. Don't set this to 0!")] public float TransitionTime = 1.5f;
		[Tooltip ("The curve to determine the camera pan after a reactivation.")] public AnimationCurve ActivationCurve = AnimationCurve.EaseInOut (0, 0, 1, 1);
		[Tooltip ("Sets the maximum movement speed of the camera transition for MoveAhead and ZoomOut.")] public float MovementSpeed = 8.0f;
		[Tooltip ("Sets the acceleration of the camera transition for MoveAhead and ZoomOut.")] public float Acceleration = 8.0f;
		[Tooltip ("Defines the distance at which the breaking effect occurs for MoveAhead and ZoomOut. Extreme speed and acceleration values need a higher breaking range value.")] public float BreakingRange = 2.0f;

		[Header ("ZoomOut")]
		[Tooltip ("Toggles the Zooming function on or off.\n\nExplanation: Zooms out based on the speed of the player.")] public bool ZoomOutActive = true;
		[Tooltip ("Defines the zooming strength based on the speed.")] public float ZoomOutMagnitude = 1.4f;
		[Tooltip ("Sets a limitation for how far ZoomOut may move the camera. If this is zero or less, it does nothing.")] public float ZoomOutCap = 0;

		[Header ("MoveAhead")]
		[Tooltip ("Toggles the MoveAhead function on or off.\n\nExplanation: Scrolls in the direction of player movement based on player speed.")] public bool MoveAheadActive = true;
		[Tooltip ("Defines the directions and their strengths of the camera moving ahead when the player is moving.\n\n"
			+ "Use negative numbers to let the camera follow instead.")] public Vector3 MoveAheadVector = new Vector3 (2.0f, 0.8f, 2.0f);
		[Tooltip ("Defines the MoveAhead factor.")] public float MoveAheadMagnitude = 1.0f;
		[Tooltip ("Sets a limitation for how far MoveAhead may move the camera. If this is zero or less, it does nothing.")] public float MoveAheadCap = 0;

		[Header ("ManualZoom")]
		[Tooltip ("Toggles the ManualZoom function on or off.\n\nExplanation: Allows zooming the total camera via external scripts or by using these parameter.")] public bool ManualZoomActive = true;
		[Tooltip ("Defines the zooming factor for ManualZoom and scales all other functions.")] public float ManualZoomFactor = 1.0f;
		[Tooltip ("Defines only the base zooming factor for ManualZoom.")] public float ManualZoomOnly = 1.0f;
		[Tooltip ("Defines the scaling factor for all other functions.")] public float ManualZoomScale = 1.0f;

		[Header ("LockOn")]
		[Tooltip ("Toggles the LockOn function on or off.\n\nExplanation: Allows you to choose a target with the LockOnTo method to "
			+ "keep the camera center between that target and the player. An explicit zooming function will apply which relatively "
			+ "zooms in and out based on the distance.")] public bool LockonActive = true;
		[Tooltip ("Defines the range at which LockOn starts if it was previously out of range.")] public float LockonRange = 8.0f;
		[Tooltip ("Defines the additional relative range at which Lockon stops if it was previously within range.")] public float LockonBufferMod = 0.25f;
		[Tooltip ("Defines the target camera position between the player (0.0) and the target (1.0). Use 0.5 for right in between.")] public float LockonBaseLerp = 0.5f;
		[Tooltip ("Defines the zoom in and out factor based on given Lockon ranges by the given factor. 0.0 means no zoom effect.")] public float LockonZoomFactor = 0.3f;
		[Tooltip ("Sets the maximum movement speed of the camera transition for LockOn.")] public float LockonMovementSpeed = 4.0f;
		[Tooltip ("Sets the acceleration of the camera transition for LockOn.")] public float LockonAcceleration = 4.0f;
		[Tooltip ("Defines the distance at which the breaking effect occurs for LockOn. Extreme speed and acceleration values need a higher breaking range value.")] public float LockonBreakingRange = 2.0f;

		[Header ("Shake")]
		[Tooltip ("Enables or disables the camera shake function. While disabled, adding shake does nothing.")] public bool ShakeActive = true;
		[Tooltip ("Modifies the relative shake magnitude for each axis. This will be used as a normalized vector.")] public Vector3 ShakeDirections = new Vector3 (1.0f, 0.5f, 1.0f);
		[Tooltip ("Modifies the total shake effect.")] public float ShakeFactor = 1.0f;
		[Tooltip ("Determines the addition ratio of new shake effects.")] [Range (0, 1)] public float ShakeAddModifier = 0.4f;

		[Header ("CursorFollow")]
		[Tooltip ("Enables or disables the camera cursor follow function.\n\nExplanation: The camera follows the cursor position to allow a look around effect.")] public bool CursorFollowActive = true;
		[Tooltip ("Determines the magnitude and the directions in which the camera will move in order to follow the cursor.")] public Vector3 CursorFollowDirection = new Vector3 (1.0f, 1.0f, 1.0f);
		[Tooltip ("Modifies the magnitude of the cursor follow function.")] public float CursorFollowModifier = 3.0f;
		[Tooltip ("Sets the maximum movement speed of the camera transition for CursorFollow.")] public float CursorFollowMovementSpeed = 12.0f;
		[Tooltip ("Sets the acceleration of the camera transition for CursorFollow.")] public float CursorFollowAcceleration = 12.0f;
		[Tooltip ("Defines the distance at which the breaking effect occurs for CursorFollow. Extreme speed and acceleration values need a higher breaking range value.")] public float CursorFollowBreakingRange = 2.0f;

		public static FlexCam Main = null; // In order to easily use the methods of this script, this reference can be used for the first initiated FlexibleCam script.

		protected float _aspectModifier = 1; public float AspectModifier { get { return _aspectModifier; } set { _aspectModifier = value; } }
		protected Vector3 _cameraOrthogonal = new Vector3 (0, 1.0f, 0); public Vector3 CameraOrthogonal { get { return _cameraOrthogonal; } set { _cameraOrthogonal = value; } }
		protected bool _camState = true; public bool CameraState { get { return _camState; } protected set { _camState = GeneralActive = value; } } // This defines the activation status of the script. True: Active, False: Deactivating.
		protected GameObject _player = null; public GameObject Player { get { return _player; } protected set { _player = PlayerObject = value; } } // This is the reference to the player GameObject.
		
		protected virtual Vector3 GetCameraOffset { get { return CameraOffset * AspectModifier; } set { } } // The final camera offset, incorporating the AspectModifier value.
		protected float _timeMod = 1; public virtual float TimeMod { get { return _timeMod; } set { _timeMod = value; } } // Directly scales the rate at which time flows.
		public virtual float TimeFlow { get { return Time.deltaTime * TimeMod; } } // The method how time is used can be overridden via script. It will affect everything related to time in this script.



// --- Internal Basics -------------------------------------------------------------------------------------------------------------------------------
		
		protected float activeTransition = 0;
		protected Vector3 reactivationPosition = new Vector3 ();
		protected Vector3 playerMovement = new Vector3 ();
		protected Vector3 lastPlayerPosition = new Vector3 ();

		protected float shakeIntensity = 0;
		protected float shakeStartDuration = 0;
		protected float shakeRemainingDuration = 0;
		protected Vector3 lastShake = new Vector3 ();

		protected Vector3 movement = new Vector3 ();
		protected Vector3 movementCurrentVector = new Vector3 ();

		protected Vector3 cameraMovement = new Vector3 ();
		protected Vector3 lastCameraPosition = new Vector3 ();
		protected Vector3 remainingCameraMovement = new Vector3 ();

		protected List<LockOnTarget> lockOnTargets = new List<LockOnTarget> ();
		protected Vector3 lockOnMovement = new Vector3 ();
		protected Vector3 lockOnCurrentVector = new Vector3 ();

		protected Vector3 cursorFollowMovement = new Vector3 ();
		protected Vector3 cursorFollowCurrentVector = new Vector3 ();



		protected virtual void Awake ()
		{
			if (Main == null) Main = this;
			if (Cam == null) Cam = GetComponent<Camera> ();
			if (Cam == null) { GeneralActive = false; enabled = false; Debug.LogError ("FlexCam: No 'camera' component found! Script disabled."); }
			else if (Player == gameObject) { GeneralActive = false; enabled = false; Debug.LogError ("FlexCam: The camera and player GameObjects may not be identical!"); }
			if (Restrictor == null) Restrictor = GetComponent<FlexCamRestrictor> ();
			if (Restrictor == null) RestrictionActive = false;
			if (ActivationCurve == null) ActivationCurve = AnimationCurve.EaseInOut (0, 0, 1, 1);

			Player = PlayerObject;
			CameraState = GeneralActive;
		}

		protected virtual void Start ()
		{
			if (Cam == null) return;

			lastCameraPosition = Cam.transform.position;

			if (Player)
			{
				Initialize (Player);
			}
			else
			{
				lastPlayerPosition = Cam.transform.position - GetCameraOffset;
			}
		}



// --- Utility Methods -------------------------------------------------------------------------------------------------------------------------------
		
		/// <summary> Initializes the camera while also selecting the player GameObject and setting up camera orthogonal. Should be used once. </summary>
		public virtual void Initialize (GameObject player)
		{
			Player = player;

			if (Player != null)
			{
				lastPlayerPosition = Player.transform.position;
				SetupRotation ();
				SetCameraOrthogonal ();
			}
		}

		/// <summary> Initializes the camera while also selecting the player GameObject and setting up camera orthogonal. Should be used once.
		/// Also sets the camera activation status and allows an instant camera transition. </summary>
		public virtual void Initialize (GameObject player, bool activation, bool instant)
		{
			Player = player;

			if (Player != null)
			{
				lastPlayerPosition = Player.transform.position;
				SetupRotation ();
				if (instant)
				{
					SetupPosition ();
					activeTransition = 1;
				}
				SetCameraOrthogonal ();
			}

			SetStatus (activation);
		}

		/// <summary> Sets the camera orthogonal value, which is necessary to cut out jumpy behavior in the direction to the camera.
		/// If you intend to use your own camera rotations, you should execute this method continously via a the same Update function which performs the camera rotation. </summary>
		public virtual void SetCameraOrthogonal ()
		{
			Vector3 v = Cam.transform.forward;
			CameraOrthogonal = Vector3.Normalize (v);
		}

		/// <summary> Sets the camera orthogonal value with a custom Vector3. This is meant for advanced users. </summary>
		public virtual void SetCameraOrthogonal (Vector3 v)
		{
			CameraOrthogonal = Vector3.Normalize (v);
		}

		/// <summary> Smoothly scrolls to the player within the set TransitionTime. </summary>
		public virtual void Activate ()
		{
			ResetValues ();
			SetStatus (true);
		}

		/// <summary> Smoothly scrolls to the player within the set TransitionTime and sets a new player GameObject. </summary>
		public virtual void Activate (GameObject player)
		{
			Player = player;
			ResetValues ();
			SetStatus (true);
		}
		
		/// <summary> Lets the camera keep some momentum to stop it. </summary>
		public virtual void Deactivate ()
		{
			SetStatus (false);
		}
		
		/// <summary> Immediately sets the camera. </summary>
		public virtual void ActivateNow ()
		{
			ResetValues ();
			activeTransition = 1;
			SetStatus (true);
		}

		/// <summary> Immediately sets the camera and sets a new player GameObject. </summary>
		public virtual void ActivateNow (GameObject player)
		{
			Player = player;
			ResetValues ();
			activeTransition = 1;
			SetStatus (true);
		}
		
		/// <summary> Immeadiately stops the camera. </summary>
		public virtual void DeactivateNow ()
		{
			activeTransition = 0;
			SetStatus (false);
		}

		/// <summary> Instantly sets the rotation of the camera. </summary>
		public virtual void SetupRotation ()
		{
			if (Player == null) return;
			Cam.transform.eulerAngles = CameraBaseRotation;
		}

		/// <summary> Instantly sets the initial position of the camera. </summary>
		public virtual void SetupPosition ()
		{
			playerMovement = Vector3.zero;
			movement = Vector3.zero;
			if (Player == null) return;
			Cam.transform.position = Player.transform.position + GetCameraOffset;
		}

		protected virtual void SetStatus (bool state)
		{
			remainingCameraMovement = cameraMovement;
			CameraState = state;
		}



		/// <summary> Adds a new LockOn target. It will be removed once leaving the range. Setting a LockOn will cause previous ones to stop. </summary>
		public virtual void SetLockOn (GameObject target)
		{
			SetTempLockOn (target, false, LockonBaseLerp, LockonRange);
		}

		/// <summary> Adds a new LockOn target.
		/// <para> 'Returning': Determines if the LockOn should either return once again in range or be removed. </para></summary>
		public virtual void SetLockOn (GameObject target, bool returning)
		{
			SetTempLockOn (target, returning, LockonBaseLerp, LockonRange);
		}

		/// <summary> Adds a new LockOn target. Setting a LockOn will cause previous ones to stop.
		/// <para> 'Focus': Determines the target camera position between the player (0.0) and the target (1.0). Overrides the standard setting 'LockonBaseLerp'. </para></summary>
		public virtual void SetLockOn (GameObject target, float range)
		{
			SetTempLockOn (target, false, LockonBaseLerp, range);
		}

		/// <summary> Adds a new LockOn target. Setting a LockOn will cause previous ones to stop.
		/// <para> 'Returning': Determines if the LockOn should either return once again in range or be removed. </para>
		/// <para> 'Range': Determines the leash range. Overrides the standard setting 'LockonRange'. </para></summary>
		public virtual void SetLockOn (GameObject target, float range, bool returning)
		{
			SetTempLockOn (target, returning, LockonBaseLerp, range);
		}

		/// <summary> Adds a new LockOn target. Setting a LockOn will cause previous ones to stop.
		/// <para> 'Returning': Determines if the LockOn should either return once again in range or be removed. </para>
		/// <para> 'Focus': Determines the target camera position between the player (0.0) and the target (1.0). Overrides the standard setting 'LockonBaseLerp'. </para></summary>
		public virtual void SetLockOn (GameObject target, bool returning, float focus)
		{
			SetTempLockOn (target, returning, focus, LockonRange);
		}

		/// <summary> Adds a new LockOn target. Setting a LockOn will cause previous ones to stop.
		/// <para> 'Returning': Determines if the LockOn should either return once again in range or be removed. </para>
		/// <para> 'Range': Determines the leash range. Overrides the standard setting 'LockonRange'. </para>
		/// <para> 'Focus': Determines the target camera position between the player (0.0) and the target (1.0). Overrides the standard setting 'LockonBaseLerp'. </para></summary>
		public virtual void SetLockOn (GameObject target, float range, bool returning, float focus)
		{
			SetTempLockOn (target, returning, focus, range);
		}

		protected virtual void SetTempLockOn (GameObject target, bool returning, float focus, float range)
		{
			if (target == null || target == Player) return;
			LockOnTarget t = new LockOnTarget (target, false, returning, focus, range);
			t.Active = true;
			CheckLockOnTarget (t);
			if (t != null)
			{
				lockOnTargets.Add (t);
				StopOtherTemporaryLockOns (target);
			}
		}

		protected virtual void StopOtherTemporaryLockOns (GameObject target)
		{
			if (target == null) return;
			foreach (LockOnTarget t in lockOnTargets.FindAll (j => !j.Permanent && j.Target != target)) { t.Returning = false; t.Active = false; }
		}

		/// <summary> Stops the current temporary LockOn and removes them. Does not affect permanent LockOns. </summary>
		public virtual void StopLockOn ()
		{
			foreach (LockOnTarget t in lockOnTargets.FindAll (j => !j.Permanent)) { t.Returning = false; t.Active = false; }
		}

		/// <summary> Stops all LockOns including permanent ones and removes them. </summary>
		public virtual void StopAllLockOns ()
		{
			foreach (LockOnTarget t in lockOnTargets) { t.Permanent = false; t.Returning = false; t.Active = false; }
		}

		/// <summary> Adds a permanent Lockon target. It will remain until explicitly deleted. </summary>
		public virtual void AddPermanentLockOn (GameObject target)
		{
			AddPermLockOn (target, LockonBaseLerp, LockonRange);
		}

		/// <summary> Adds a permanent Lockon target. It will remain until explicitly deleted.
		/// <para> 'Range': Determines the leash range. Overrides the standard setting 'LockonRange'. </para></summary>
		public virtual void AddPermanentLockOn (GameObject target, float range)
		{
			AddPermLockOn (target, LockonBaseLerp, range);
		}

		/// <summary> Adds a permanent Lockon target. It will remain until explicitly deleted.
		/// <para> 'Focus': Determines the target camera position between the player (0.0) and the target (1.0). Overrides the standard setting 'LockonBaseLerp'. </para></summary>
		public virtual void AddPermanentLockOn (float focus, GameObject target)
		{
			AddPermLockOn (target, focus, LockonRange);
		}

		/// <summary> Adds a permanent Lockon target. It will remain until explicitly deleted.
		/// <para> 'Range': Determines the leash range. Overrides the standard setting 'LockonRange'. </para>
		/// <para> 'Focus': Determines the target camera position between the player (0.0) and the target (1.0). Overrides the standard setting 'LockonBaseLerp'. </para></summary>
		public virtual void AddPermanentLockOn (GameObject target, float range, float focus)
		{
			AddPermLockOn (target, focus, range);
		}

		protected virtual void AddPermLockOn (GameObject target, float focus, float range)
		{
			if (target == null || target == Player) return;
			LockOnTarget t = new LockOnTarget (target, true, true, focus, range);
			CheckLockOnTarget (t);
			if (t != null) lockOnTargets.Add (t);
		}

		/// <summary> Removes a permanent LockOn target. Does nothing if it's not a permanent LockOn target. </summary>
		public virtual void RemovePermanentLockOn (GameObject target)
		{
			if (target == null) return;
			LockOnTarget t = lockOnTargets.Find (j => j.Permanent && j.Target == target);
			CheckLockOnTarget (t);
			if (t != null) lockOnTargets.Remove (t);
		}

		protected virtual void CheckLockOnTarget (LockOnTarget t)
		{
			if (t == null || Player == null) return;
			if (t.Target == null) { if (lockOnTargets.Contains (t)) lockOnTargets.Remove (t); return; }

			if (t.Active)
			{
				if (Vector3.Distance (Player.transform.position, t.TargetPosition) > t.Range * GetManualScaleMod () * (1 + LockonBufferMod)) t.Active = false;
			}
			else
			{
				if (!t.Returning) lockOnTargets.Remove (t);
				else if (t.Returning && Vector3.Distance (Player.transform.position, t.TargetPosition) <= t.Range * GetManualScaleMod ()) t.Active = true;
			}
		}



		/// <summary> Adds camera shake. Duration is calculated automatically. Info: 10 intensity equals 1 unity distance as radius. Recommended intensity values are around 1 to 5. </summary>
		public virtual void Shake (float intensity)
		{
			DoShake (intensity, Mathf.Pow (intensity, 0.5f) * 0.5f);
		}
		
		/// <summary> Adds camera shake with set duration. </summary>
		public virtual void Shake (float intensity, float duration)
		{
			DoShake (intensity, duration);
		}

		protected virtual void DoShake (float intensity, float duration)
		{
			if (!ShakeActive) return;

			if (shakeStartDuration > 0 && shakeRemainingDuration > 0) shakeIntensity = Mathf.Max (shakeIntensity * (Mathf.Pow (shakeRemainingDuration / shakeStartDuration, 0.5f)), intensity)
				+ Mathf.Min (shakeIntensity * (Mathf.Pow (shakeRemainingDuration / shakeStartDuration, 0.5f)), intensity) * ShakeAddModifier;
			else shakeIntensity = intensity;
			shakeStartDuration = shakeRemainingDuration = Mathf.Max (shakeRemainingDuration, duration);
		}



// --- Camera work -----------------------------------------------------------------------------------------------------------------------------------
		
		protected virtual void OnEnable ()
		{
			ResetValues ();
		}

		public virtual void ResetValues ()
		{
			activeTransition = 0;
			shakeIntensity = 0;
			shakeStartDuration = 0;
			shakeRemainingDuration = 0;
			lastShake = Vector3.zero;
			playerMovement = Vector3.zero;
			cameraMovement = Vector3.zero;
			movement = Vector3.zero;
			movementCurrentVector = Vector3.zero;
			if (Player) lastPlayerPosition = Player.transform.position;
			else lastPlayerPosition = Cam.transform.position - CameraOffset;
			lastCameraPosition = Cam.transform.position;
			reactivationPosition = Cam.transform.position;
			lockOnMovement = Vector3.zero;
			lockOnCurrentVector = Vector3.zero;
			cursorFollowMovement = Vector3.zero;
			cursorFollowCurrentVector = Vector3.zero;
		}



		protected virtual void FixedUpdate ()
		{
			CheckActivationChanges ();

			if (TransitionTime > 0)
			{
				if (CameraState && Player != null)
				{
					if (activeTransition < 1) activeTransition = Mathf.Min (activeTransition + (TimeFlow / TransitionTime), 1);
				}
				else
				{
					if (activeTransition > 0) activeTransition = Mathf.Max (activeTransition - (TimeFlow / TransitionTime), 0);
				}
			}

			if (CameraState)
			{
				foreach (LockOnTarget t in new List<LockOnTarget> (lockOnTargets)) CheckLockOnTarget (t);

				if (Player != null)
				{
					playerMovement = Player.transform.position - lastPlayerPosition;
					lastPlayerPosition = Player.transform.position;
				}
			}

			DoCameraMovement ();
			DoMovement ();
			DoLockOnMovement ();
			DoCursorFollowMovement ();
		}

		protected void CheckActivationChanges ()
		{
			bool b = false;

			if (CameraState != GeneralActive)
			{
				_camState = GeneralActive;
				b = true;
			}

			if (Player != PlayerObject)
			{
				_player = PlayerObject;
				b = true;
			}

			if (b) ResetValues ();
		}

		protected virtual void DoCameraMovement ()
		{
			if (CameraState && Player != null)
			{
				remainingCameraMovement = cameraMovement = Cam.transform.position - lastCameraPosition;
			}
			else
			{
				cameraMovement = remainingCameraMovement * activeTransition;
			}

			lastCameraPosition = Cam.transform.position;
		}

		protected virtual void DoMovement ()
		{
			if (!CameraState) return;

			Vector3 target = GetMoveAheadVector () + GetZoomOutVector () + GetManualZoomVector ();
			Vector3 d = target - movementCurrentVector;
			float m = (1 + Mathf.Max (Mathf.Clamp (BreakingRange / d.magnitude, 1, Acceleration * MovementSpeed)));
			movement = Vector3.MoveTowards (movement, Vector3.Normalize (d) * MovementSpeed * (Mathf.Min (d.magnitude / BreakingRange, 1)), Acceleration * TimeFlow * m);
			movementCurrentVector = movementCurrentVector + movement * TimeFlow;
		}

		protected virtual void DoLockOnMovement ()
		{
			if (!CameraState) return;

			Vector3 target = CutOrthogonal (GetLockOnTargetVector ());
			Vector3 zoom = Vector3.zero;

			if (target != Vector3.zero)
			{
				float x = -1 + 2 * Mathf.Min (2 * target.magnitude / (LockonRange * GetManualScaleMod ()), 1);
				float e;
				if (x >= 0) e = LockonZoomFactor * x;
				else e = -(1 - (1 / (1 + (-LockonZoomFactor * x))));
				zoom = GetCameraOffset * e * GetManualScaleMod ();
			}

			Vector3 d = target - lockOnCurrentVector + zoom;
			float m = (1 + Mathf.Max (Mathf.Clamp (LockonBreakingRange / d.magnitude, 1, LockonAcceleration * LockonMovementSpeed)));
			lockOnMovement = Vector3.MoveTowards (lockOnMovement, Vector3.Normalize (d) * LockonMovementSpeed * (Mathf.Min (d.magnitude / LockonBreakingRange, 1)), LockonAcceleration * TimeFlow * m);
			lockOnCurrentVector = lockOnCurrentVector + lockOnMovement * TimeFlow;
		}

		protected virtual void DoCursorFollowMovement ()
		{
			if (!CameraState) return;

			Vector3 target = CutOrthogonal (GetCursorFollowTarget () * GetManualScaleMod ());
			Vector3 d = target - cursorFollowCurrentVector;
			float m = (1 + Mathf.Max (Mathf.Clamp (CursorFollowBreakingRange / d.magnitude, 1, CursorFollowAcceleration * CursorFollowMovementSpeed)));
			cursorFollowMovement = Vector3.MoveTowards (cursorFollowMovement, Vector3.Normalize (d) * CursorFollowMovementSpeed * (Mathf.Min (d.magnitude / CursorFollowBreakingRange, 1)), CursorFollowAcceleration * TimeFlow * m);
			cursorFollowCurrentVector = cursorFollowCurrentVector + cursorFollowMovement * TimeFlow;
		}
		


		protected virtual void LateUpdate ()
		{
			if (!CameraState || Player == null)
			{
				Vector3 shake = Shaking ();
				Cam.transform.position = Cam.transform.position + RemainingMove () + shake - lastShake;
				lastShake = shake;
			}
			else
			{
				Cam.transform.position = SetRestrictions (Vector3.Lerp (reactivationPosition, Player.transform.position + GetCameraOffset, ActivationCurve.Evaluate (activeTransition))
					+ MoveAheadZoomOut () + CursorFollow () + LockOn () + Shaking ());
			}
		}
		
		protected virtual Vector3 MoveAheadZoomOut ()
		{
			return movementCurrentVector;
		}
		
		protected virtual Vector3 LockOn ()
		{
			return lockOnCurrentVector;
		}

		protected virtual Vector3 CursorFollow ()
		{
			return cursorFollowCurrentVector;
		}

		protected virtual Vector3 Shaking ()
		{
			if (shakeRemainingDuration > 0 && shakeStartDuration > 0)
			{
				shakeRemainingDuration = Mathf.Max (shakeRemainingDuration - TimeFlow, 0);
				return CreateRandomVector3 () * shakeIntensity * ShakeFactor * GetManualScaleMod () * (Mathf.Pow (shakeRemainingDuration / shakeStartDuration, 0.5f)) / 10;
			}
			else return Vector3.zero;
		}

		protected virtual Vector3 RemainingMove ()
		{
			return cameraMovement;
		}


		
		protected virtual float GetManualZoomMod ()
		{
			if (ManualZoomActive)
			{
				return ManualZoomFactor * ManualZoomOnly;
			}
			else return 1;
		}

		protected virtual float GetManualScaleMod ()
		{
			if (ManualZoomActive)
			{
				return ManualZoomFactor * ManualZoomScale;
			}
			else return 1;
		}

		protected virtual Vector3 GetManualZoomVector ()
		{
			if (CameraState && ManualZoomActive)
			{
				return GetCameraOffset * (GetManualZoomMod () - 1);
			}
			else return Vector3.zero;
		}

		protected virtual Vector3 GetZoomOutVector ()
		{
			if (CameraState && ZoomOutActive)
			{
				Vector3 v = GetCameraOffset * ZoomOutMagnitude * CutOrthogonal (playerMovement).magnitude * MovementScale * GetManualScaleMod ();
				if (ZoomOutCap > 0) return Vector3.ClampMagnitude (v, ZoomOutCap);
				else return v;
			}
			else return Vector3.zero;
		}

		protected virtual Vector3 GetMoveAheadVector ()
		{
			if (CameraState && MoveAheadActive)
			{
				Vector3 v = Vector3.Scale (MoveAheadVector, playerMovement) * 10 * MoveAheadMagnitude * MovementScale * GetManualScaleMod ();
				if (MoveAheadCap > 0) return Vector3.ClampMagnitude (v, MoveAheadCap);
				else return v;
			}
			else return Vector3.zero;
		}
		
		public Vector3 GetLockOnTargetVector ()
		{
			if (CameraState && Player != null && LockonActive)
			{
				Vector3 v = Vector3.zero;
				int c = 0;

				foreach (LockOnTarget a in lockOnTargets.FindAll (j => j.Active))
				{
					v += (a.TargetPosition - Player.transform.position) * a.Focus;
					c++;
				}

				if (c == 0) return Vector3.zero;
				return v / c;
			}
			else return Vector3.zero;
		}

		/// <summary> Returns a Vector2 of the current mouse position. Its x and y values range from -1 to 1. </summary>
		public Vector2 GetMouseVector ()
		{
			Vector2 v = Input.mousePosition;
			v.x = 2 * (Mathf.Clamp (v.x / Cam.pixelWidth, 0, 1) - 0.5f);
			v.y = 2 * (Mathf.Clamp (v.y / Cam.pixelHeight, 0, 1) - 0.5f);
			return v;
		}
		
		protected virtual Vector3 GetCursorFollowTarget ()
		{
			if (CameraState && CursorFollowActive)
			{
				Vector2 mousePos = GetMouseVector ();
				Vector3 v = CutOrthogonal (new Vector3 (mousePos.x, mousePos.y, mousePos.y));
				v = Vector3.Scale (v * CursorFollowModifier, CursorFollowDirection);
				return v;
			}
			else return Vector3.zero;
		}

		/// <summary> Modifiers vectors to be capped based on the settings in FlexibleCamRestrictor. </summary>
		protected virtual Vector3 SetRestrictions (Vector3 vec)
		{
			if (RestrictionActive && Restrictor != null) return Restrictor.ApplyRestrictions (vec);
			return vec;
		}



		/// <summary> Creates a random normalized (1.0 length) Vector3 with a random angle, modified by ShakeDirections. </summary>
		protected virtual Vector3 CreateRandomVector3 ()
		{
			return new Vector3 ((0.5f - Random.value) * ShakeDirections.x, (0.5f - Random.value) * ShakeDirections.y, (0.5f - Random.value) * ShakeDirections.z).normalized;
		}
		
		/// <summary> Using this prevents the camera from zooming wildly when the player jumps or moves in the direction of the camera. Applies the CameraOrthogonal setting. </summary>
		protected virtual Vector3 CutOrthogonal (Vector3 v)
		{
			return Vector3.Scale (v, Vector3.one - new Vector3 (Mathf.Abs (CameraOrthogonal.x), Mathf.Abs (CameraOrthogonal.y), Mathf.Abs (CameraOrthogonal.z)));
		}

		protected class LockOnTarget
		{
			GameObject _target; public GameObject Target { get { return _target; } set { _target = value; } }
			bool _permanent; public bool Permanent { get { return _permanent; } set { _permanent = value; } }
			bool _returning; public bool Returning { get { return _returning; } set { _returning = value; } }
			bool _active = false; public bool Active { get { return _active; } set { _active = value; } }
			float _focus = 0; public float Focus { get { return _focus; } set { _focus = value; } }
			float _range = 0; public float Range { get { return _range; } set { _range = value; } }

			public Vector3 TargetPosition { get { return Target.transform.position; } }

			public LockOnTarget (GameObject target, bool permanent, bool returning, float focus, float range)
			{
				Target = target;
				Permanent = permanent;
				Returning = returning;
				Focus = focus;
				Range = range;
			}
		}
	}
}
