Flexible Camera
v1.0, December 04, 2017
by BattleForge (contact: mr.richard.kleber@googlemail.com)
Made in Unity version: "2017.2.0f3"



Contents of the asset:
FlexCamCore.cs			-- The main script which should be placed on the camera.
FlexCamAspect.cs		-- Alters the zooming based on the video aspect.
FlexCamRestrictor.cs	-- Allows setting boundaries to the camera position.
3 Demo scenes			-- These should represent most asset functions with standard configurations.
3 Camera prefabs		-- Provide quick basic setups for cameras.



About the asset:
This asset contains a main script and two complementary scripts for use for a camera. It is designed to be fully
operational with a top-down or sideway view, and to be well compatible with mobile games. The main purpose of
the scripts is to ensure a very smooth functioning of the camera at all times, even in face of ruthless usage.

Under the line all the scripts focus on altering the "transition" component of a given camera via the Update
method. So deactivating the main script (FlexCam.cs), and therefore preventing its Update functions to be
executed, all of its actions are stopped. This is important to note, because it means other assets which may
interact with cameras in other ways than moving or rotating them should remain compatible. If you intend to
move or rotate the camera with other scripts or assets, make sure to deactivate the FlexCam script.
Reactivating the script will cause a smooth transition from its current position to its intended position above
or near the player.



Quick start:
There are two ways:
- Use one of the camera prefabs. Assign the player GameObject to the 'Player' setting in the FlexCam script.
- Add the script "FlexCam" to the camera. Assign the player GameObject to the 'Player' setting in the
     FlexCam script.

At this point the camera should be working as intended. Make sure the camera is not parented with the player
GameObject in any way or vice versa. Also make sure that no other script is moving or rotating the camera while
it is active. Make sure the camera is facing the right direction - use the Offset parameter in the settings to
change that.

If you want a certain tilt or want the camera to face another direction, use the Initiate method. In order to
reference to any methods, either use 'FlexCam.Main.Method (parameter);' or use own references to the
according instance of this script: 'reference.Initialize (parameter);' The tilt will use the 'LookAt' function
to tilt the camera on an offset from the target, and works like a snapshot.



Methods and Features:
ZoomOut:      The camera will use the speed of the Player object to zoom out, increasing the vision at higher
			    speed. Remember, all features can be activated and deactivated at will, even in runtime, which
				will keep the camera smooth.

MoveAhead:    The camera will use the speed of the Player object to move ahead, granting vision over the region
				the Player is moving towards.

ManualZoom:   Adds additional factors for custom zoom. Intended for manual use via external scripts. Allows
				both, a general zoom factor and a scaling for all other functions to adjust to the given custom
				zoom.

CursorFollow: Causes the cursor to move the camera relative to the screen position. An example script has been
				added to showcase possible user scripts interacting with the FlexCam scripts.

LockOn:       Allows adding GameObjects as LockOn targets. As such, while they are within a given range, the
				camera will shift between the player and the targets in range. If the range plus a buffer range
				is left, the LockOn is broken. Adding a temporary LockOn target will remove all other temporary
				ones. Removing the locked-on GameObjects will remove them as LockOn targets. Cannot LockOn to
				the Player. The LockOn behaviors are the following:
				- One Time (temporary): If the target is out of range, the LockOn ends.
				- Returning (temporary): Resumes LockOn when re-entering the range.
				- Permanent: This behaves like the Returning type. Multiple ones can exist at once.

Shaking:      Causes a shake effect based on given parameters. Recommended parameters range from 1-5. The
				algorithm ensures that shake effects are added properly without causing extreme behavior. Works
				while the script is set not to follow. Does not work while the script is disabled though.
				
Activation:   Deactivates or Activates the camera following of the script (not the enable status of the script!),
                allowing the camera to smoothly come to a halt or to recover its position above or near the
				player. Use this if you want to incorporate other assets or scripts to move or rotate the camera.
				Hint: Can be used with Player death. Either by removing the Player GameObject or by actively
				using the deactivation method.

Initiation:   This is mostly needed to set a fixed tilt to the camera. It uses the basic LookAt method for this.
				In case you want to set your hands on camera rotation, feel free to do so. In that case however
				you should continously execute the SetCameraOrthogonal () method to prevent jumpy behavior.



Settings:
All parameters are subject to be editable, even in runtime. It is intended to keep the camera smooth and
operational while doing so. Some radical, mostly unexpected changes however could cause issues. But I would
consider them bugs, so feel free to report them to me. Be reminded though that the Restrictor is merely a small
addition, which may be 



Code and Compatibility:
Most methods and properties are set to be 'virtual', so that you can create own scripts which inherit from this
one to change them. For example you can overwrite the property 'TimeFlow' to alter the usage of time within this
script. It could use the resulting time modifier of another script for example.
As this script is mostly based on algorithms, there should be little to no compatibility issues with varying
Unity versions.



Additional Scripts:
Aspect Script - This is a small script to help keeping the view constant horizontally with various aspect
ratios. In other words: No matter how wide or high the screen is set to, the horizontal view will show the same
length of an area. This may be relevant for mobile games.



Restriction Script:
Allows you to define the boundaries for the final position of the camera. Use the various boolean settings to
determine which limitations you want to set. Possible restrictions are either minimum and maximum coordinates,
a collider or multiple colliders. It can get jumpy though as it transitions the camera between two colliders.
It will always jump to the closest collider. Consider this a hard-cap for the camera position.



Future of this Asset:
I will fix all known bugs and provide support as far as I can. I will seek to ensure the promises of this asset
are fulfilled. The asset is meant to be complete as it is, or at least compatible with other assets and to be
highly adjustable and flexible.
Future improvements of this asset depend on how well it sells itself. If it's worth it, I will gladly invest
time to expand its features. There is indeed potential for more, which however do not seem to be fundamentally
necessary. Another consideration, in the case of some success, is the addition of new camera assets for other
styles (first person, third person, strategy view, etc). I think this is a reasonable and common stance, but I
just wanted to point it out.



Feel free to contact me if you need assistance, have any questions or would like to report bugs.
With best regards,
- BattleForge.




