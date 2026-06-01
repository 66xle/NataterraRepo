using UnityEngine;

namespace Artifice
{
	[System.Serializable]
	public struct Vector3Range
	{
		public Mode		mode;
		public Vector3	min;
		public Vector3	max;

		public Vector3Range(Vector3 v)
		{
			mode	= Mode.Constant;
			min		= v;
			max		= v;
		}

		public enum Mode
		{
			Constant,
			RandomBetweenTwo
		}

		public Vector3 GetValue(Artificer a)
		{
			if ( mode == Mode.Constant )
				return max;
			else
			{
				Vector3 v;

				v.x = a.RandomRange(min.x, max.x);
				v.y = a.RandomRange(min.y, max.y);
				v.z = a.RandomRange(min.z, max.z);

				return v;
			}
		}

	}
}