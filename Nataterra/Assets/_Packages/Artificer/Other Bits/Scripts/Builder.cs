using UnityEngine;

namespace Artifice
{
	public class Builder : MonoBehaviour
	{
		public Artificer	builder;
		public KeyCode		buildKey		= KeyCode.B;
		public KeyCode		dismantleKey	= KeyCode.Space;
		public GameObject	hide;
		public Artificer	dismantle;

		void Start()
		{
		}

		public void DoUpdate()
		{
			if ( builder )
			{
				if ( builder.ReadyToBuild() )
				{
					if ( Input.GetKeyDown(buildKey) )
					{
						builder.buildMode = BuildMode.Build;
						if ( hide )
						{
							hide.SetActive(false);
						}
					}
				}

				if ( builder.ReadyToDismantle() )
				{
					if ( Input.GetKeyDown(dismantleKey) )
					{
						if ( dismantle )
						{
							dismantle.StartDismantle();
						}
						else
							builder.StartDismantle();
					}
				}
			}
		}

		public void Build()
		{
			if ( builder.buildMode == BuildMode.None )
			{
				builder.buildMode = BuildMode.Build;
				if ( hide )
				{
					hide.SetActive(false);
				}
			}
			else
			{
				if ( builder.buildMode != BuildMode.Build )
				{
					//builder.buildMode = BuildMode.Dismantle;
					builder.StartDismantle();
				}
			}
		}
	}
}