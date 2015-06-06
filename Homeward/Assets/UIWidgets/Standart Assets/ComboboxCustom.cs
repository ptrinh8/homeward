using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets
{

	/// <summary>
	/// Base class for custom combobox.
	/// </summary>
	public class ComboboxCustom<TListViewCustom,TComponent,TItem> : MonoBehaviour
			where TListViewCustom : ListViewCustom<TComponent,TItem>
			where TComponent : ListViewItem
	{
		/// <summary>
		/// Custom Combobox event.
		/// </summary>
		public class ComboboxCustomEvent : UnityEvent<int,TItem>
		{
			
		}

		[SerializeField]
		TListViewCustom listView;
		
		/// <summary>
		/// Gets or sets the ListView.
		/// </summary>
		/// <value>ListView component.</value>
		public TListViewCustom ListView {
			get {
				return listView;
			}
			set {
				if (listView!=null)
				{
					listView.OnSelectObject.RemoveListener(SetCurrent);
					listView.OnSelectObject.RemoveListener(onSelectCallback);
					listView.OnFocusOut.RemoveListener(onFocusHideList);
				}
				listView = value;
				if (listView!=null)
				{
					listParent = listView.transform.parent;
					listView.OnSelectObject.AddListener(SetCurrent);
					listView.OnSelectObject.AddListener(onSelectCallback);
					listView.OnFocusOut.AddListener(onFocusHideList);
				}
			}
		}
		
		[SerializeField]
		Button toggleButton;
		
		/// <summary>
		/// Gets or sets the toggle button.
		/// </summary>
		/// <value>The toggle button.</value>
		public Button ToggleButton {
			get {
				return toggleButton;
			}
			set {
				if (toggleButton!=null)
				{
					toggleButton.onClick.RemoveListener(ToggleList);
				}
				toggleButton = value;
				if (toggleButton!=null)
				{
					toggleButton.onClick.AddListener(ToggleList);
				}
			}
		}

		[SerializeField]
		TComponent current;

		/// <summary>
		/// Gets or sets the current component.
		/// </summary>
		/// <value>The current.</value>
		public TComponent Current {
			get {
				return current;
			}
			set {
				current = value;
			}
		}
		
		/// <summary>
		/// OnSelect event.
		/// </summary>
		public ComboboxCustomEvent OnSelect = new ComboboxCustomEvent();
		UnityAction<int> onSelectCallback;

		Transform listCanvas;
		Transform listParent;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public virtual void Start()
		{
			onSelectCallback = (index) => OnSelect.Invoke(index, listView.Items[index]);

			ToggleButton = toggleButton;

			ListView = listView;

			if (listView!=null)
			{
				listCanvas = Utilites.FindCanvas(listParent);

				listView.gameObject.SetActive(true);
				listView.Start();
				if ((listView.SelectedIndex==-1) && (listView.Items.Count > 0))
				{
					listView.SelectedIndex = 0;
				}
				if (listView.SelectedIndex!=-1)
				{
					UpdateCurrent();
				}
				listView.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Clear listview and selected item.
		/// </summary>
		public virtual void Clear()
		{
			listView.Clear();
			UpdateCurrent();
		}

		/// <summary>
		/// Toggles the list visibility.
		/// </summary>
		public void ToggleList()
		{
			if (listView==null)
			{
				return ;
			}

			if (listView.gameObject.activeSelf)
			{
				HideList();
			}
			else
			{
				ShowList();
			}
		}
		
		/// <summary>
		/// Shows the list.
		/// </summary>
		public void ShowList()
		{
			if (listView==null)
			{
				return ;
			}
			if (listCanvas!=null)
			{
				listParent = listView.transform.parent;
				listView.transform.SetParent(listCanvas);
			}
			listView.gameObject.SetActive(true);
			EventSystem.current.SetSelectedGameObject(listView.gameObject);
		}
		
		/// <summary>
		/// Hides the list.
		/// </summary>
		public void HideList()
		{
			if (listView==null)
			{
				return ;
			}
			listView.gameObject.SetActive(false);
			if (listCanvas!=null)
			{
				listView.transform.SetParent(listParent);
			}
		}

		void onFocusHideList(BaseEventData eventData)
		{
			var ev = eventData as PointerEventData;
			if (ev!=null)
			{
				//don't hide if click on toggle button
				if ((ev.pointerEnter!=null) && (ev.pointerEnter.Equals(toggleButton.gameObject)))
				{
					return ;
				}
			}

			HideList();
		}

		void SetCurrent(int index)
		{
			UpdateCurrent();
		}

		/// <summary>
		/// Updates the current component.
		/// </summary>
		protected virtual void UpdateCurrent()
		{
			HideList();
		}
	}
}