﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace UIWidgets {
	/// <summary>
	/// Draggable handle.
	/// </summary>
	public class DraggableHandle : MonoBehaviour, IDragHandler {
		RectTransform drag;
		Canvas canvas;
		RectTransform canvasRect;

		/// <summary>
		/// Set the specified draggable object.
		/// </summary>
		/// <param name="newDrag">New drag.</param>
		public void Drag(RectTransform newDrag)
		{
			drag = newDrag;
			canvas = Utilites.FindCanvas(transform).GetComponent<Canvas>();
			canvasRect = canvas.GetComponent<RectTransform>();
		}

		/// <summary>
		/// Raises the drag event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnDrag(PointerEventData eventData)
		{
			drag.localPosition += FixPosition(eventData.position) - FixPosition(eventData.position - eventData.delta);
		}

		Vector3 FixPosition(Vector3 screenPosition)
		{
			Vector3 result;
			var canvasSize = canvasRect.sizeDelta;
			Vector2 min = Vector2.zero;
			Vector2 max = canvasSize;

			var isOverlay = canvas.renderMode == RenderMode.ScreenSpaceOverlay;
			var noCamera = canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null;
			if (isOverlay || noCamera)
			{
				result = screenPosition;
			}
			else
			{
				var ray = canvas.worldCamera.ScreenPointToRay(screenPosition);
				var plane = new Plane(canvasRect.forward, canvasRect.position);
				
				float distance;
				plane.Raycast(ray, out distance);

				result = canvasRect.InverseTransformPoint(ray.origin + (ray.direction * distance));
				
				min = - Vector2.Scale(max, canvasRect.pivot);
				max = canvasSize - min;
			}
			
			result.x = Mathf.Clamp(result.x, min.x, max.x);
			result.y = Mathf.Clamp(result.y, min.y, max.y);
			
			return result;
		}
	}
}