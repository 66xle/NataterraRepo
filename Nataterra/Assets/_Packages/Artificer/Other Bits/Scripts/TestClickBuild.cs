using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice
{
	public class TestClickBuild : MonoBehaviour
	{
		public Artificer	build;

		void Start()
		{
			build.showingPart = 0;
			build.buildIndex = -1;
		}

		void Update()
		{
        
		}
	}
}