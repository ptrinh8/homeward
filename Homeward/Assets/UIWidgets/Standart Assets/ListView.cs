using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UIWidgets
{
	/// <summary>
	/// ListView sources.
	/// </summary>
	public enum ListViewSources {
		/// <summary>
		/// The list. Use strings as source for list.
		/// </summary>
		List = 0,
		/// <summary>
		/// The file. Use file as source for list.
		/// </summary>
		File = 1,
	}

	/// <summary>
	/// List view event.
	/// </summary>
	public class ListViewEvent : UnityEvent<int,string>
	{

	}

	/// <summary>
	/// List view item component.
	/// </summary>
	public class ListViewStringComponent : ListViewItem
	{
		/// <summary>
		/// The Text component.
		/// </summary>
		public Text Text;
	}

	[AddComponentMenu("UI/ListView", 250)]
	/// <summary>
	/// List view.
	/// http://ilih.ru/images/unity-assets/UIWidgets/ListView.png
	/// </summary>
	public class ListView : ListViewBase {
		[SerializeField]
		List<string> strings = new List<string>();

		/// <summary>
		/// Gets the strings.
		/// </summary>
		/// <value>The strings.</value>
		public List<string> Strings {
			get {
				return new List<string>(strings);
			}
			set {
				UpdateItems(value);
				if (scrollRect!=null)
				{
					scrollRect.verticalScrollbar.value = 1f;
				}
			}
		}

		/// <summary>
		/// Gets the strings.
		/// </summary>
		/// <value>The strings.</value>
		public new List<string> Items {
			get {
				return new List<string>(strings);
			}
			set {
				UpdateItems(value);
				if (scrollRect!=null)
				{
					scrollRect.verticalScrollbar.value = 1f;
				}
			}
		}

		[SerializeField]
		TextAsset file;

		/// <summary>
		/// Gets or sets the file with strings for ListView. One string per line.
		/// </summary>
		/// <value>The file.</value>
		public TextAsset File {
			get {
				return file;
			}
			set {
				file = value;
				if (file!=null)
				{
					UpdateItems(file);
				}
			}
		}

		/// <summary>
		/// The comments in file start with specified strings.
		/// </summary>
		[SerializeField]
		public List<string> CommentsStartWith = new List<string>(){"#", "//"};

		/// <summary>
		/// The source.
		/// </summary>
		[SerializeField]
		public ListViewSources Source = ListViewSources.List;

		/// <summary>
		/// Allow only unique strings.
		/// </summary>
		[SerializeField]
		public bool Unique = true;

		/// <summary>
		/// Allow empty strings.
		/// </summary>
		[SerializeField]
		public bool AllowEmptyItems;

		[SerializeField]
		Color backgroundColor = Color.white;

		[SerializeField]
		Color textColor = Color.black;

		/// <summary>
		/// Default background color.
		/// </summary>
		public Color BackgroundColor {
			get {
				return backgroundColor;
			}
			set {
				backgroundColor = value;
				UpdateColors();
			}
		}

		/// <summary>
		/// Default text color.
		/// </summary>
		public Color TextColor {
			get {
				return textColor;
			}
			set {
				textColor = value;
				UpdateColors();
			}
		}

		/// <summary>
		/// Color of background on pointer over.
		/// </summary>
		[SerializeField]
		public Color HighlightedBackgroundColor = new Color(203, 230, 244, 255);

		/// <summary>
		/// Color of text on pointer text.
		/// </summary>
		[SerializeField]
		public Color HighlightedTextColor = Color.black;

		[SerializeField]
		Color selectedBackgroundColor = new Color(53, 83, 227, 255);

		[SerializeField]
		Color selectedTextColor = Color.black;

		/// <summary>
		/// Background color of selected item.
		/// </summary>
		public Color SelectedBackgroundColor {
			get {
				return selectedBackgroundColor;
			}
			set {
				selectedBackgroundColor = value;
				UpdateColors();
			}
		}

		/// <summary>
		/// Text color of selected item.
		/// </summary>
		public Color SelectedTextColor {
			get {
				return selectedTextColor;
			}
			set {
				selectedTextColor = value;
				UpdateColors();
			}
		}

		/// <summary>
		/// The default item.
		/// </summary>
		[SerializeField]
		public ImageAdvanced DefaultItem;

		//List<ImageAdvanced> itemsImages = new List<ImageAdvanced>();
		//List<Text> itemsText = new List<Text>();
		List<ListViewStringComponent> components = new List<ListViewStringComponent>();

		List<UnityAction<PointerEventData>> callbacksEnter = new List<UnityAction<PointerEventData>>();
		List<UnityAction<PointerEventData>> callbacksExit = new List<UnityAction<PointerEventData>>();

		/// <summary>
		/// Sort items.
		/// </summary>
		[SerializeField]
		public bool Sort = true;

		/// <summary>
		/// Sort function.
		/// </summary>
		public Func<IEnumerable<string>,IEnumerable<string>> SortFunc = items => items.OrderBy(x => x);

		/// <summary>
		/// OnSelect event.
		/// </summary>
		public ListViewEvent OnSelectString = new ListViewEvent();

		/// <summary>
		/// OnDeselect event.
		/// </summary>
		public ListViewEvent OnDeselectString = new ListViewEvent();

		bool start_called = false;

		[SerializeField]
		ScrollRect scrollRect;
		
		/// <summary>
		/// Gets or sets the ScrollRect.
		/// </summary>
		/// <value>The ScrollRect.</value>
		public ScrollRect ScrollRect {
			get {
				return scrollRect;
			}
			set {
				if (scrollRect!=null)
				{
					scrollRect.verticalScrollbar.onValueChanged.RemoveListener(OnScroll);
				}
				scrollRect = value;
				if (scrollRect!=null)
				{
					scrollRect.verticalScrollbar.onValueChanged.AddListener(OnScroll);
				}
			}
		}
		
		/// <summary>
		/// The top filler.
		/// </summary>
		RectTransform topFiller;
		
		/// <summary>
		/// The bottom filler.
		/// </summary>
		RectTransform bottomFiller;
		
		/// <summary>
		/// The height of the DefaultItem.
		/// </summary>
		float itemHeight;
		
		/// <summary>
		/// The height of the ScrollRect.
		/// </summary>
		float scrollHeight;
		
		/// <summary>
		/// The spacing between items.
		/// </summary>
		float spacing;
		
		/// <summary>
		/// Count of visible items.
		/// </summary>
		int visibleItems;
		
		/// <summary>
		/// Count of hidden items by top filler.
		/// </summary>
		int topHiddenItems;
		
		/// <summary>
		/// Count of hidden items by bottom filler.
		/// </summary>
		int bottomHiddenItems;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public override void Start()
		{
			if (start_called)
			{
				return ;
			}
			start_called = true;

			base.Start();

			DestroyGameObjects = false;

			if (DefaultItem==null)
			{
				throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
			}
			if (DefaultItem.GetComponentInChildren<Text>()==null)
			{
				throw new MissingComponentException("DefaultItem don't have child with 'Text' component. Add child with 'Text' component to DefaultItem.");
			}

			var topFillerObj = new GameObject("top filler");
			topFillerObj.transform.SetParent(Container);
			topFiller = topFillerObj.AddComponent<RectTransform>();
			topFiller.SetAsFirstSibling();
			
			var bottomFillerObj = new GameObject("bottom filler");
			bottomFillerObj.transform.SetParent(Container);
			bottomFiller = bottomFillerObj.AddComponent<RectTransform>();
			
			if (CanOptimize())
			{
				ScrollRect = scrollRect;
				
				scrollHeight = scrollRect.GetComponent<RectTransform>().rect.height;
				itemHeight = DefaultItem.GetComponent<RectTransform>().rect.height;
				visibleItems = (int)Math.Ceiling(scrollHeight / itemHeight) + 1;
				var layout = Container.GetComponent<EasyLayout.EasyLayout>();
				spacing = layout.Spacing.y;
				
				var r = scrollRect.gameObject.AddComponent<ResizeListener>();
				r.OnResize.AddListener(Resize);
			}

			UpdateItems();

			DefaultItem.gameObject.SetActive(false);

			OnSelect.AddListener(OnSelectCallback);
			OnDeselect.AddListener(OnDeselectCallback);
		}

		void Resize()
		{
			scrollHeight = scrollRect.GetComponent<RectTransform>().rect.height;
			visibleItems = (int)Math.Ceiling(scrollHeight / itemHeight) + 1;
			UpdateItems();
		}
		
		bool CanOptimize()
		{
			return scrollRect!=null && Container.GetComponent<EasyLayout.EasyLayout>()!=null;
		}

		void OnSelectCallback(int index, ListViewItem item)
		{
			OnSelectString.Invoke(index, strings[index]);
		}

		void OnDeselectCallback(int index, ListViewItem item)
		{
			OnDeselectString.Invoke(index, strings[index]);
		}

		/// <summary>
		/// Updates the items.
		/// </summary>
		public override void UpdateItems()
		{
			if (Source==ListViewSources.List)
			{
				UpdateItems(strings);
			}
			else
			{
				UpdateItems(File);
			}
		}

		/// <summary>
		/// Clear strings list.
		/// </summary>
		public override void Clear()
		{
			strings.Clear();
			UpdateItems(strings);
		}

		/// <summary>
		/// Updates the items.
		/// </summary>
		/// <param name="newFile">File.</param>
		void UpdateItems(TextAsset newFile)
		{
			if (file==null)
			{
				return ;
			}
			var new_items = new List<string>(newFile.text.Split(new string[] {"\r\n", "\r", "\n"}, StringSplitOptions.None));
			UpdateItems(new_items);
		}

		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of added item.</returns>
		public virtual int Add(string item)
		{
			var newStrings = Strings;
			newStrings.Add(item);
			UpdateItems(newStrings);

			var index = strings.FindIndex(x => x==item);

			return index;
		}

		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of removed item.</returns>
		public virtual int Remove(string item)
		{
			var index = strings.FindIndex(x => x==item);
			if (index==-1)
			{
				return index;
			}

			var newStrings = Strings;
			newStrings.Remove(item);
			UpdateItems(newStrings);

			return index;
		}

		void RemoveCallbacks()
		{
			components.ForEach((x, index) => {
				if (x==null)
				{
					return ;
				}
				if (callbacksEnter.Count > index)
				{
					x.onPointerEnter.RemoveListener(callbacksEnter[index]);
				}
				if (callbacksExit.Count > index)
				{
					x.onPointerExit.RemoveListener(callbacksExit[index]);
				}
			});
			callbacksEnter.Clear();
			callbacksExit.Clear();
		}

		void AddCallbacks()
		{
			components.ForEach(AddCallback);
		}

		void AddCallback(ListViewStringComponent component, int index)
		{
			callbacksEnter.Add(ev => ImageOver(component));
			callbacksExit.Add(ev => ImageOut(component));
			
			component.onPointerEnter.AddListener(callbacksEnter[index]);
			component.onPointerExit.AddListener(callbacksExit[index]);
		}

		/// <summary>
		/// Determines if item exists with the specified index.
		/// </summary>
		/// <returns><c>true</c> if item exists with the specified index; otherwise, <c>false</c>.</returns>
		/// <param name="index">Index.</param>
		public override bool IsValid(int index)
		{
			return (index >= 0) && (index < strings.Count);
		}

		void ImageOver(ListViewStringComponent component)
		{
			if (!IsValid(component.Index))
			{
				var message = string.Format("Index must be between 0 and Items.Count ({0})", strings.Count);
				throw new IndexOutOfRangeException(message);
			}

			if (IsSelected(component.Index))
			{
				return ;
			}

			HighlightColoring(component);
		}

		void ImageOut(ListViewStringComponent component)
		{
			if (!IsValid(component.Index))
			{
				var message = string.Format("Index must be between 0 and Items.Count ({0})", strings.Count);
				throw new IndexOutOfRangeException(message);
			}

			if (IsSelected(component.Index))
			{
				return ;
			}

			DefaultColoring(component);
		}

		List<string> FilterItems(IEnumerable<string> newItems)
		{
			var temp = newItems;
			if (Source==ListViewSources.File)
			{
				temp = temp.Select(x => x.Trim());
				if (Unique)
				{
					temp = temp.Distinct();
				}

				if (!AllowEmptyItems)
				{
					temp = temp.Where(x => x!=string.Empty);
				}

				if (CommentsStartWith.Count > 0)
				{
					temp = temp.Where(line => {
						return !CommentsStartWith.Any(comment => line.StartsWith(comment, StringComparison.InvariantCulture));
					});
				}
			}

			if (Sort && SortFunc!=null)
			{
				temp = SortFunc(temp);
			}

			return temp.ToList();
		}

		ListViewStringComponent ComponentTopToBottom()
		{
			var bottom = components.Count - 1;

			var bottomComponent = components[bottom];
			components.RemoveAt(bottom);
			components.Insert(0, bottomComponent);
			bottomComponent.transform.SetAsFirstSibling();

			return bottomComponent;
		}
		
		ListViewStringComponent ComponentBottomToTop()
		{
			var topComponent = components[0];
			components.RemoveAt(0);
			components.Add(topComponent);
			topComponent.transform.SetAsLastSibling();

			return topComponent;
		}
		
		void OnScroll(float value)
		{
			var value_n = 1f - value;
			var listHeight = (strings.Count * (itemHeight + spacing)) - spacing;
			var window = (listHeight * value_n) - (scrollHeight * value_n);
			var newTopHiddenItems = (int)Math.Floor((window + spacing) / (itemHeight + spacing));
			
			var oldTopHiddenItems = topHiddenItems;
			topHiddenItems = Math.Min(newTopHiddenItems, strings.Count - visibleItems);
			bottomHiddenItems = strings.Count - visibleItems - topHiddenItems;
			
			//поменять данные отображаемых элементов
			if (oldTopHiddenItems==topHiddenItems)
			{
				//do nothing
			}
			// optimization on +-1 item scroll
			else if (oldTopHiddenItems==(topHiddenItems + 1))
			{
				var bottomComponent = ComponentTopToBottom();

				bottomComponent.Index = topHiddenItems;
				bottomComponent.Text.text = strings[topHiddenItems];
				Coloring(bottomComponent);
			}
			else if (oldTopHiddenItems==(topHiddenItems - 1))
			{
				var topComponent = ComponentBottomToTop();

				var new_index = topHiddenItems + visibleItems - 1;
				topComponent.Index = new_index;
				topComponent.Text.text = strings[new_index];
				Coloring(topComponent);
			}
			// all other cases
			else
			{
				//!
				var new_indicies = Enumerable.Range(topHiddenItems, visibleItems).ToArray();
				components.ForEach((x, i) => {
					x.Index = new_indicies[i];
					x.Text.text = strings[new_indicies[i]];
					Coloring(x);
				});
			}
			
			UpdateTopFiller();
			UpdateBottomFiller();
		}

		void UpdateItems(List<string> newItems)
		{
			RemoveCallbacks();
			
			newItems = FilterItems(newItems);

			if (CanOptimize())
			{
				visibleItems = (visibleItems < newItems.Count) ? visibleItems : newItems.Count;
			}
			else
			{
				visibleItems = newItems.Count;
			}

			CreateItems();

			var new_selected_indicies = NewSelectedIndicies(newItems);

			strings = newItems;

			var base_items = new List<ListViewItem>();
			components.ForEach(x => base_items.Add(x));
			base.Items = base_items;

			new_selected_indicies.ForEach(x => Select(x));

			components.ForEach((x, i) => {
				x.Index = i;
				x.Text.text = strings[i];

				Coloring(x);
			});

			AddCallbacks();

			topHiddenItems = 0;
			bottomHiddenItems = newItems.Count() - visibleItems;
			
			UpdateTopFiller();
			UpdateBottomFiller();

			if (scrollRect!=null)
			{
				var r = scrollRect.GetComponent<RectTransform>();
				r.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, r.rect.width);
			}
		}

		void UpdateBottomFiller()
		{
			bottomFiller.SetAsLastSibling();
			if (bottomHiddenItems==0)
			{
				bottomFiller.gameObject.SetActive(false);
			}
			else
			{
				bottomFiller.gameObject.SetActive(true);
				bottomFiller.sizeDelta = new Vector2(bottomFiller.sizeDelta.x, bottomHiddenItems * (itemHeight + spacing) - spacing);
			}
		}
		
		void UpdateTopFiller()
		{
			topFiller.SetAsFirstSibling();
			if (topHiddenItems==0)
			{
				topFiller.gameObject.SetActive(false);
			}
			else
			{
				topFiller.gameObject.SetActive(true);
				topFiller.sizeDelta = new Vector2(bottomFiller.sizeDelta.x, topHiddenItems * (itemHeight + spacing) - spacing);
			}
		}

		List<int> NewSelectedIndicies(IList<string> newItems)
		{
			//duplicated items should not be selected more than at start
			var new_items_copy = new List<string>(newItems);

			var selected_items = SelectedIndicies.Select(x => strings[x]);

			selected_items = selected_items.Where(x => {
				var is_valid_item = newItems.Contains(x);
				if (is_valid_item)
				{
					new_items_copy.Remove(x);
				}
				return is_valid_item;
			}).ToList();

			var selected_indicies = new List<int>();

			var selected_items_copy = new List<string>(selected_items);

			int index = 0;
			components.ForEach(component => {
				var is_selected = selected_items_copy.Contains(newItems[index]);

				selected_items_copy.Remove(newItems[index]);

				if (is_selected)
				{
					selected_indicies.Add(index);
				}
				
				index++;
			});

			return selected_indicies;
		}

		void CreateItem()
		{
			var background = Instantiate(DefaultItem) as ImageAdvanced;// Type conversion for Unity 4.6
			background.gameObject.SetActive(true);

			var text = background.GetComponentInChildren<Text>();

			var component = background.GetComponent<ListViewStringComponent>() ?? background.gameObject.AddComponent<ListViewStringComponent>();
			component.Background = background;
			component.Text = text;
			components.Add(component);

			Utilites.FixInstantiated(DefaultItem, background);
		}
		
		void CreateItems()
		{
			//add new ui elements if necessary
			if (visibleItems > components.Count)
			{
				for (var i = components.Count; i < visibleItems; i++)//each (var i in Enumerable.Range(itemsText.Count, newItemsCount - itemsText.Count))
				{
					CreateItem();
				}
			}
			//del existing ui elements if necessary
			if (visibleItems < components.Count)
			{
				for (var i = components.Count - 1; i >= visibleItems; i--)//each (var i in Enumerable.Range(newItemsCount, itemsText.Count - newItemsCount).Reverse())
				{
					DeleteItem(i);
				}
			}
		}

		void DeleteItem(int index)
		{
			Destroy(components[index].gameObject);
			
			components.RemoveAt(index);
		}
		
		void Coloring(ListViewStringComponent component)
		{
			if (component==null)
			{
				return ;
			}

			if (SelectedIndicies.Contains(component.Index))
			{
				SelectColoring(component);
			}
			else
			{
				DefaultColoring(component);
			}
		}
		
		void UpdateColors()
		{
			components.ForEach(Coloring);
		}

		ListViewStringComponent GetComponent(int index)
		{
			return components.Find(x => x.Index==index);
		}

		/// <summary>
		/// Called when item selected.
		/// Use it for change visible style of selected item.
		/// </summary>
		/// <param name="index">Index.</param>
		protected override void SelectItem(int index)
		{
			SelectColoring(GetComponent(index));
		}

		/// <summary>
		/// Called when item deselected.
		/// Use it for change visible style of deselected item.
		/// </summary>
		/// <param name="index">Index.</param>
		protected override void DeselectItem(int index)
		{
			DefaultColoring(GetComponent(index));
		}

		void HighlightColoring(ListViewStringComponent component)
		{
			if (component==null)
			{
				return ;
			}

			component.Background.color = HighlightedBackgroundColor;
			component.Text.color = HighlightedTextColor;
		}

		void SelectColoring(ListViewStringComponent component)
		{
			if (component==null)
			{
				return ;
			}

			component.Background.color = selectedBackgroundColor;
			component.Text.color = selectedTextColor;
		}

		void DefaultColoring(ListViewStringComponent component)
		{
			if (component==null)
			{
				return ;
			}

			component.Background.color = backgroundColor;
			component.Text.color = textColor;
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected override void OnDestroy()
		{
			OnSelect.RemoveListener(OnSelectCallback);
			OnDeselect.RemoveListener(OnDeselectCallback);

			RemoveCallbacks();

			base.OnDestroy();
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem("GameObject/UI/ListView")]
		static void CreateObject()
		{
			Utilites.CreateObject("Assets/UIWidgets/Prefabs/ListView.prefab");
		}
#endif
	}
}