using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Artifice
{
	//[ExecuteAlways]
	public class EditorHueShift : MonoBehaviour
	{
		public Material targetMaterial;
		[Range(0f, 1f)]
		public float hueSpeed = 0.1f;

		private float hue;
		private double lastTime;

	#if UNITY_EDITOR
		private void OnEnable()
		{
			lastTime = EditorApplication.timeSinceStartup;
			EditorApplication.update += EditorUpdate;
		}

		private void OnDisable()
		{
			EditorApplication.update -= EditorUpdate;
		}

		private void EditorUpdate()
		{
			if (targetMaterial == null)
				return;

			double currentTime = EditorApplication.timeSinceStartup;
			float deltaTime = (float)(currentTime - lastTime);
			lastTime = currentTime;

			// Get current color
			Color color = targetMaterial.color;

			// Convert to HSV
			Color.RGBToHSV(color, out float h, out float s, out float v);

			// Advance hue
			hue = (h + hueSpeed * deltaTime) % 1f;

			// Apply new color
			targetMaterial.color = Color.HSVToRGB(hue, s, v);

			// Force editor repaint for smooth updates
			//SceneView.RepaintAll();
		}
	#endif
	}
}