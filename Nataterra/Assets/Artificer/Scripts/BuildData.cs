using System.Collections.Generic;
using UnityEngine;

namespace Artifice
{
	public class BuildData : ScriptableObject
	{
		[HideInInspector]
		public List<MeshElement>	meshes	= new List<MeshElement>();
		[HideInInspector]
		public List<int>			sorted	= new List<int>();
	}
}