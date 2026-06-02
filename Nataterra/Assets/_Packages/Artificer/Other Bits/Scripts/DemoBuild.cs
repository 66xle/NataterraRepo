using UnityEngine;
using TMPro;

namespace Artifice
{
	public class DemoBuild : MonoBehaviour
	{
		public Artificer		build;						// The Artificer object we are controlling
		public TextMeshProUGUI	label;						// Button text label so we can change the wording
		public bool				showUnbuilt		= false;	// Enable the showing on unbuilt parts of the object
		public bool				includePlacing	= false;	// Enable the showing off bits being placed
		public Material			unbuiltMat;					// Material for unbuilt elements
		public Material			placingMat;					// Material for the unbuilt but being placed elements
		public float			addAmount		= 1.0f;		// Amount per frame of elements added for part build
		public float			buildSteps		= 0.1f;		// How many steps it takes to build the object (0.1 is 10 steps for example)
		[Range(0.0f, 1.0f)]
		public float			buildLevel		= 0.0f;		// The amount to instantly build the object to

		void Update()
		{
			// Change the button text depending on the state
			if ( build && label )
			{
				if ( build.ReadyToBuild() )
					label.text = "Build";
				else
				{
					if ( build.ReadyToDismantle() )
						label.text = "Dismantle";
					else
					{
						if ( build.buildMode == BuildMode.Pause )
							label.text = "Continue";
						else
							label.text = "Working";
					}
				}
			}

			if ( showUnbuilt && unbuiltMat )
				build?.ShowUnBuiltElements(unbuiltMat, includePlacing, placingMat);
			else
			{
				if ( includePlacing )
				{
					build?.ShowPlacing(placingMat);
				}
			}
				
		}

		// Called from the GUI Button. Build, Dismantle or Continue based on the current build mode
		public void Build()
		{
			if ( build )
			{
				if ( build.ReadyToBuild() )
					build.StartBuild();
				else
				{
					if ( build.ReadyToDismantle() )
						build.StartDismantle();
					else
					{
						if ( build.buildMode == BuildMode.Pause )
							build.PauseBuild(false);
					}
				}
			}
		}

		// Called from the GUI Button. Do a part build of the object
		public void PartBuild()
		{
			if ( build )
			{
				if ( build.ReadyToBuild() )
					build.StartPartBuild(buildSteps, addAmount);
				else
				{
					if ( build.IsBuilding() )
					{
						build.BuildParts(buildSteps, addAmount);
					}
				}
			}
		}

		// Called from the GUI Toggle. Toggles the show unbuilt option
		public void ShowUnbuilt(bool val)
		{
			showUnbuilt = val;
		}

		// Called from the GUI Toggle. Toggles the show placing option
		public void ShowPlacing(bool val)
		{
			includePlacing = val;
		}

		// Called from the GUI Slider. Set the instant build amount
		public void SetBuiltLevel(float a)
		{
			buildLevel = a;
		}

		// Called from the GUI Button. Instantly build the object to the buildLevel
		public void SetBuilt()
		{
			build.SetBuiltLevel(buildLevel);
		}
	}
}