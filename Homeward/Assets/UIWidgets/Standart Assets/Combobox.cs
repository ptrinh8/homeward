using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Combobox", 220)]
	/// <summary>
	/// Combobox.
	/// http://ilih.ru/images/unity-assets/UIWidgets/Combobox.png
	/// </summary>
	public class Combobox : MonoBehaviour
	{
		[SerializeField]
		ListView listView;

		/// <summary>
		/// Gets or sets the ListView.
		/// </summary>
		/// <value>ListView component.</value>
		public ListView ListView {
			get {
				return listView;
			}
			set {
				if (listView!=null)
				{
					listView.OnSelectString.RemoveListener(SelectItem);
					listView.OnFocusOut.RemoveListener(onFocusHideList);
				}
				listView = value;
				if (listView!=null)
				{
					listParent = listView.transform.parent;
					listView.OnSelectString.AddListener(SelectItem);
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
		bool editable = true;

		/// <summary>
		/// Gets or sets a value indicating whether this is editable and user can add new items.
		/// </summary>
		/// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
		public bool Editable {
			get {
				return editable;
			}
			set {
				editable = value;
				input.interactable = editable;
			}
		}
		
		InputField input;

		/// <summary>
		/// OnSelect event.
		/// </summary>
		public ListViewEvent OnSelect = new ListViewEvent();

		Transform listCanvas;
		Transform listParent;

		void Start()
		{
			input = GetComponent<InputField>();
			input.onEndEdit.AddListener(InputItem);
			Editable = editable;

			ToggleButton = toggleButton;

			ListView = listView;

			if (listView!=null)
			{
				listCanvas = Utilites.FindCanvas(listParent);

				listView.OnSelectString.AddListener((index, item) => OnSelect.Invoke(index, item));

				listView.gameObject.SetActive(true);
				listView.Start();
				if ((listView.SelectedIndex==-1) && (listView.Strings.Count > 0))
				{
					listView.SelectedIndex = 0;
				}
				if (listView.SelectedIndex!=-1)
				{
					SelectItem(listView.SelectedIndex, listView.Strings[listView.SelectedIndex]);
				}
				listView.gameObject.SetActive(false);
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

		/// <summary>
		/// Clear listview and selected item.
		/// </summary>
		public virtual void Clear()
		{
			listView.Clear();
			input.text = string.Empty;
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

		/// <summary>
		/// Set the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of item.</returns>
		public int Set(string item)
		{
			var index = listView.Add(item);
			listView.Select(index);

			return index;
		}

		void SelectItem(int index, string text)
		{
			input.text = text;

			HideList();
		}

		void InputItem(string item)
		{
			if (!editable)
			{
				return ;
			}
			if (item==string.Empty)
			{
				return ;
			}

			if (!listView.Strings.Contains(item))
			{
				var index = listView.Add(item);
				listView.Select(index);
			}
		}

		void OnDestroy()
		{
			if (input!=null)
			{
				input.onEndEdit.RemoveListener(InputItem);
			}
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem("GameObject/UI/Combobox")]
		static void CreateObject()
		{
			Utilites.CreateObject("Assets/UIWidgets/Prefabs/Combobox.prefab");
		}
#endif

	}
}