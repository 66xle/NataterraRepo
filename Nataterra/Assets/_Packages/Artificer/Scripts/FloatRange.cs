using UnityEngine;

namespace Artifice
{
	[System.Serializable]
	public struct FloatRange
	{
		public Mode		mode;
		public float	min;
		public float	max;

		public FloatRange(float v)
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

		public float GetValue(Artificer a)
		{
			if ( mode == Mode.Constant )
				return max;
			else
				return a.RandomRange(min, max);
		}
	}
}