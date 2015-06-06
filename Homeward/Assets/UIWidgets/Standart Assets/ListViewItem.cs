using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets {
	/// <summary>
	/// ListViewItem.
	/// Item for ListViewBase.
	/// </summary>
	[RequireComponent(typeof(Image))]
	public class ListViewItem : UIBehaviour,
		IPointerClickHandler,
		IPointerEnterHandler, IPointerExitHandler,
		ISubmitHandler, ICancelHandler
	{
		/// <summary>
		/// The index of item in ListView.
		/// </summary>
		public int Index;

		/// <summary>
		/// What to do when the event system send a pointer click Event.
		/// </summary>
		public UnityEvent onClick = new UnityEvent();

		/// <summary>
		/// What to do when the event system send a pointer click Event.
		/// </summary>
		public PointerUnityEvent onPointerClick = new PointerUnityEvent();

		/// <summary>
		/// What to do when the event system send a pointer enter Event.
		/// </summary>
		public PointerUnityEvent onPointerEnter = new PointerUnityEvent();

		/// <summary>
		/// What to do when the event system send a pointer exit Event.
		/// </summary>
		public PointerUnityEvent onPointerExit = new PointerUnityEvent();

		/// <summary>
		/// The background.
		/// </summary>
		[System.NonSerialized]
		public ImageAdvanced Background;

		/// <summary>
		/// Awake this instance.
		/// </summary>
		protected override void Awake()
		{
			Background = GetComponent<ImageAdvanced>();
		}

		/// <summary>
		/// Raises the submit event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnSubmit(BaseEventData eventData)
		{
			onClick.Invoke();
		}

		/// <summary>
		/// Raises the cancel event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnCancel(BaseEventData eventData)
		{

		}

		/// <summary>
		/// Raises the pointer click event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnPointerClick(PointerEventData eventData)
		{
			onPointerClick.Invoke(eventData);
			onClick.Invoke();
		}

		/// <summary>
		/// Raises the pointer enter event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerEnter(PointerEventData eventData)
		{
			onPointerEnter.Invoke(eventData);
		}
		
		/// <summary>
		/// Raises the pointer exit event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerExit(PointerEventData eventData)
		{
			onPointerExit.Invoke(eventData);
		}
	}
}