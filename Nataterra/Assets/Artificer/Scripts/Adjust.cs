using System.Collections.Generic;
using UnityEngine;

namespace Artifice
{
	[System.Serializable]
	public class AdjustVolume
	{
		public Bounds						box;
		public float						radius		= 1.0f;
		public VolumeCheck					volumeCheck	= VolumeCheck.Center;
		public VolumeShape					volumeShape	= VolumeShape.Box;

		public AdjustVolume()
		{
			radius		= 1.0f;
			volumeCheck	= VolumeCheck.Center;
			box.size	= Vector3.one;
			volumeShape	= VolumeShape.Box;
		}
	}

	[AddComponentMenu("Artificer/Adjust")]
	public class Adjust : MonoBehaviour
	{
		[Multiline()]
		public string						selection;
		[Multiline()]
		public string						moveSelection;
		public Overridable<int>				moveTo;
		public AdjustMode					adjustMode			= AdjustMode.Selection;
		public List<AdjustVolume>			volumes				= new List<AdjustVolume>();
		public float						priority			= 1.0f;
#if UNITY_6000_1_OR_NEWER
		public Color						color				= Color.orange;
#else
		public Color						color				= new Color(1f, 0.6470588f, 0f, 1f);
#endif
		public SplitOptions					options;
		public AdjustShow					showElements		= AdjustShow.None;
		List<int>							currentSelection	= new List<int>();

		int	builtFrame;
		List<MeshElement>	elements = new List<MeshElement>();

		public void UpdateSelection()
		{
			currentSelection = GetSelection();
		}

		public void AddElement(MeshElement me)
		{
			if ( !elements.Contains(me) )
				elements.Add(me);
		}

		public bool InVolume(MeshElement me, Transform tm, out float _priority)
		{
			if ( Time.frameCount != builtFrame )
			{
				builtFrame = Time.frameCount;
				elements.Clear();
			}

			_priority = float.MaxValue;

			if ( !enabled )
				return false;

			if ( adjustMode == AdjustMode.Volume )
			{
				for ( int i = 0; i < volumes.Count; i++ )
				{
					AdjustVolume vol = volumes[i];
					if ( vol.volumeCheck == VolumeCheck.Center )
					{
						Vector3 lpos = transform.InverseTransformPoint(tm.TransformPoint(me.center));
						Vector3 p = lpos - vol.box.center;	//boxOffset;
						if ( vol.volumeShape == VolumeShape.Box )
						{
							if ( p.x > -(vol.box.size.x * 0.5f) && p.x < (vol.box.size.x * 0.5f) && p.y > -(vol.box.size.y * 0.5f) && p.y < (vol.box.size.y * 0.5f) && p.z > -(vol.box.size.z * 0.5f) && p.z < (vol.box.size.z * 0.5f) )
							{
								_priority = priority;
								return true;
							}
						}
						else
						{
							if ( p.sqrMagnitude < vol.radius * vol.radius )
							{
								_priority = priority;
								return true;
							}
						}
					}

					if ( vol.volumeCheck == VolumeCheck.Bounds )
					{
						Vector3 min = me.tm1.MultiplyPoint(me.bounds.min);
						Vector3 max = me.tm1.MultiplyPoint(me.bounds.max);

						min = transform.InverseTransformPoint(min);
						max = transform.InverseTransformPoint(max);
						if ( vol.volumeShape == VolumeShape.Box )
						{
							if ( vol.box.Contains(min) && vol.box.Contains(max) )
							{
								_priority = priority;
								return true;
							}
						}
						else
						{
							Vector3 lpos = transform.InverseTransformPoint(tm.TransformPoint(me.center));
							Vector3 p = lpos - vol.box.center;	//boxOffset;

							float r2 = vol.radius * vol.radius;
							if ( (lpos - min - p).sqrMagnitude < r2 && (lpos - max - p).sqrMagnitude < r2  )
							{
								_priority = priority;
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		public List<int> GetSelection()
		{
			return GetSelection(selection);
		}

		public List<int> GetMoveSelection()
		{
			return GetSelection(moveSelection);
		}

		static public List<int> GetSelection(string selection)
		{
			List<int> result = new List<int>();
			if ( string.IsNullOrWhiteSpace(selection) )
				return result;

			selection = selection.Replace('\n', ' ');

			string[] tokens = selection.Split(new char[] { ' ', ',' }, System.StringSplitOptions.RemoveEmptyEntries);

			foreach ( string token in tokens )
			{
				if ( token[0] == '!' )
				{
					string tok = token.TrimStart('!');
					if ( tok.Contains("-") )
					{
						string[] parts = tok.Split('-');
						if ( parts.Length == 2 && int.TryParse(parts[0], out int start) && int.TryParse(parts[1], out int end) )
						{
							if ( end >= start )
							{
								for ( int i = start; i <= end; i++ )
									result.Remove(i);
							}
							else
								result.Remove(start);
						}
						else
						{
							if ( int.TryParse(parts[0], out int s1) )
								result.Remove(s1);
						}

					}
					else
					{
						if ( int.TryParse(tok, out int value) )
							result.Remove(value);
					}
				}
				else
				{
					if ( token.Contains("-") )
					{
						string[] parts = token.Split('-');
						if ( parts.Length == 2 && int.TryParse(parts[0], out int start) && int.TryParse(parts[1], out int end) )
						{
							if ( end >= start )
							{
								for ( int i = start; i <= end; i++ )
									result.Add(i);
							}
							else
								result.Add(start);
						}
						else
						{
							if ( int.TryParse(parts[0], out int s1) )
								result.Add(s1);
						}
					}
					else
					{
						if ( int.TryParse(token, out int value) )
							result.Add(value);
					}
				}
			}

			return result;
		}

		public List<MeshElement> GetElements()
		{
			return elements;
		}
	}
}