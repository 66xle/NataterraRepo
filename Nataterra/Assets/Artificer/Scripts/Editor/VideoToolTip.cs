using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

#if false
namespace Artifice
{
	internal class VideoTooltip : EditorWindow
	{
		public string	VideoClipFileURI;
		GameObject		tempGO;
		VideoClip		clip;
		VideoPlayer		player;
		Texture			currentRT;
		Texture2D		DefaultTexture;
		public static readonly string DefaultVideoLoading = "DefaultVideoLoading";

		void OnGUI()
		{
			if ( clip == null )
			{
				if ( player == null )
				{
					tempGO = new GameObject("VideoWindowHelp");
					tempGO.hideFlags = HideFlags.DontSave;

					player = tempGO.AddComponent<VideoPlayer>();

					player.url = VideoClipFileURI;
					player.isLooping = true;

					player.prepareCompleted += PlayerOnprepareCompleted;
					player.Prepare();
				}
			}

			if ( currentRT == null )
			{
				if ( DefaultTexture == null )
					DefaultTexture = Resources.Load<Texture2D>(DefaultVideoLoading);
			}

			EditorGUI.DrawPreviewTexture(new Rect(0, 0, position.width, position.height), currentRT == null ? DefaultTexture : currentRT);
		}

		void Update()
		{
			Repaint();
		}

		private void PlayerOnprepareCompleted(VideoPlayer source)
		{
			player.Play();
			currentRT = source.texture;
		}

		public static void SafeDestroy(params UnityEngine.Object[] components)
		{
			if ( !Application.isPlaying )
			{
				foreach ( var component in components )
				{
					if ( component ) DestroyImmediate(component);
				}

			}
			else
			{
				foreach ( var component in components )
				{
					if ( component ) Destroy(component);
				}
			}
		}

		void OnDisable()
		{
			if ( player != null )
			{
				player.Stop();
				player.targetTexture = null;
			}

			SafeDestroy(tempGO);
			if ( DefaultTexture != null )
				Resources.UnloadAsset(DefaultTexture);
		}
	}
}
#endif