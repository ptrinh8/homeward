// ==================================================================================
// <file="TextureGenerator.cs" product="Homeward">
// <date>2015-01-11</date>
// ==================================================================================

#region Header Files

using UnityEngine;
using System.Collections;

#endregion

public static class TextureGenerator 
{
	public static Texture2D CreateTexture(Color color)
	{
		Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		tex.SetPixel (0, 0, color);
		tex.Apply ();
		return tex;
	}
}
