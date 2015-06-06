using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

namespace UIWidgets
{
	/// <summary>
	/// Utilites.
	/// </summary>
	static public class Utilites {
#if UNITY_EDITOR
		/// <summary>
		/// Creates the object by path to asset prefab.
		/// </summary>
		/// <returns>The created object.</returns>
		/// <param name="path">Path.</param>
		/// <param name="parent">Parent.</param>
		static public GameObject CreateObject(string path, Transform parent=null)
		{
			var prefab = Resources.LoadAssetAtPath<GameObject>(path);
			if (prefab==null)
			{
				throw new ArgumentException(string.Format("Prefab not found at path {0}.", path));
			}

			var go = UnityEngine.Object.Instantiate(prefab) as GameObject;

			var go_parent = parent ?? Selection.activeTransform;
			if ((go_parent==null) || (go_parent.gameObject.GetComponent<RectTransform>()==null))
			{
				go_parent = UnityEngine.Object.FindObjectOfType<Canvas>().transform;
			}

			if (go_parent!=null)
			{
				go.transform.SetParent(go_parent, false);
			}

			go.name = prefab.name;

			var rectTransform = go.GetComponent<RectTransform>();
			if (rectTransform!=null)
			{
				rectTransform.anchoredPosition = new Vector2(0, 0);
			}

			FixInstantiated(prefab, go);

			return go;
		}
		#endif

		/// <summary>
		/// Fixs the instantiated object (in some cases object have wrong position, rotation and scale).
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="instance">Instance.</param>
		static public void FixInstantiated(Component source, Component instance)
		{
			FixInstantiated(source.gameObject, instance.gameObject);
		}

		/// <summary>
		/// Fix the instantiated object (in some cases object have wrong position, rotation and scale).
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="instance">Instance.</param>
		static public void FixInstantiated(GameObject source, GameObject instance)
		{
			var defaultRectTransform = source.GetComponent<RectTransform>();

			var rectTransform = instance.GetComponent<RectTransform>();

			rectTransform.localPosition = defaultRectTransform.localPosition;
			rectTransform.position = defaultRectTransform.position;
			rectTransform.rotation = defaultRectTransform.rotation;
			rectTransform.localScale = defaultRectTransform.localScale;
			rectTransform.anchoredPosition = defaultRectTransform.anchoredPosition;
		}

		/// <summary>
		/// Finds the canvas.
		/// </summary>
		/// <returns>The canvas.</returns>
		/// <param name="currentObject">Current object.</param>
		static public Transform FindCanvas(Transform currentObject)
		{
			var canvas = currentObject.GetComponentInParent<Canvas>();
			if (canvas==null)
			{
				return null;
			}
			return canvas.transform;
		}
	}

	/// <summary>
	/// For each extensions.
	/// </summary>
	public static class ForEachExtensions
	{
		/// <summary>
		/// Foreach with index.
		/// </summary>
		/// <param name="enumerable">Enumerable.</param>
		/// <param name="handler">Handler.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
		{
			int i = 0;
			foreach (T item in enumerable)
			{
				handler(item, i);
				i++;
			}
		}

		/// <summary>
		/// Foreach.
		/// </summary>
		/// <param name="enumerable">Enumerable.</param>
		/// <param name="handler">Handler.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> handler)
		{
			foreach (T item in enumerable)
			{
				handler(item);
			}
		}
	}
}