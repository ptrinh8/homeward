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
	/// Custom ListView event.
	/// </summary>
	public class ListViewCustomEvent : UnityEvent<int> {
		
	}

	/// <summary>
	/// Base class for custom ListView.
	/// </summary>
	public class ListViewCustom<TComponent,TItem> : ListViewBase where TComponent : ListViewItem {

		[SerializeField]
		List<TItem> customItems = new List<TItem>();

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>Items.</value>
		new public List<TItem> Items {
			get {
				return new List<TItem>(customItems);
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
		/// The default item.
		/// </summary>
		[SerializeField]
		public TComponent DefaultItem;

		List<TComponent> components = new List<TComponent>();

		List<TComponent> componentsCache = new List<TComponent>();

		List<UnityAction<PointerEventData>> callbacksEnter = new List<UnityAction<PointerEventData>>();

		List<UnityAction<PointerEventData>> callbacksExit = new List<UnityAction<PointerEventData>>();

		/// <summary>
		/// Gets the selected item.
		/// </summary>
		/// <value>The selected item.</value>
		public TItem SelectedItem {
			get {
				if (SelectedIndex==-1)
				{
					return default(TItem);
				}
				return customItems[SelectedIndex];
			}
		}

		/// <summary>
		/// Gets the selected items.
		/// </summary>
		/// <value>The selected items.</value>
		public List<TItem> SelectedItems {
			get {
				if (SelectedIndex==-1)
				{
					return null;
				}
				return SelectedIndicies.ConvertAll(x => customItems[x]);
			}
		}

		/// <summary>
		/// Gets the selected component.
		/// </summary>
		/// <value>The selected component.</value>
		public TComponent SelectedComponent {
			get {
				if (SelectedIndex==-1)
				{
					return null;
				}
				return components[SelectedIndex];
			}
		}

		/// <summary>
		/// Gets the selected components.
		/// </summary>
		/// <value>The selected components.</value>
		public List<TComponent> SelectedComponents {
			get {
				if (SelectedIndex==-1)
				{
					return null;
				}
				return SelectedIndicies.ConvertAll(x => components[x]);
			}
		}

		/// <summary>
		/// Sort function.
		/// </summary>
		public Func<List<TItem>,List<TItem>> SortFunc;
		
		/// <summary>
		/// What to do when the object selected.
		/// </summary>
		public ListViewCustomEvent OnSelectObject = new ListViewCustomEvent();
		
		/// <summary>
		/// What to do when the object deselected.
		/// </summary>
		public ListViewCustomEvent OnDeselectObject = new ListViewCustomEvent();
		
		/// <summary>
		/// What to do when the event system send a pointer enter Event.
		/// </summary>

		public ListViewCustomEvent OnPointerEnterObject = new ListViewCustomEvent();
		
		/// <summary>
		/// What to do when the event system send a pointer exit Event.
		/// </summary>
		public ListViewCustomEvent OnPointerExitObject = new ListViewCustomEvent();
		
		bool start_called = false;

		[SerializeField]
		Color defaultBackgroundColor = Color.white;
		
		[SerializeField]
		Color defaultColor = Color.black;
		
		/// <summary>
		/// Default background color.
		/// </summary>
		public Color DefaultBackgroundColor {
			get {
				return defaultBackgroundColor;
			}
			set {
				defaultBackgroundColor = value;
				UpdateColors();
			}
		}
		
		/// <summary>
		/// Default text color.
		/// </summary>
		public Color DefaultColor {
			get {
				return defaultColor;
			}
			set {
				DefaultColor = value;
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
		public Color HighlightedColor = Color.black;
		
		[SerializeField]
		Color selectedBackgroundColor = new Color(53, 83, 227, 255);
		
		[SerializeField]
		Color selectedColor = Color.black;
		
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
		public Color SelectedColor {
			get {
				return selectedColor;
			}
			set {
				selectedColor = value;
				UpdateColors();
			}
		}

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
				throw new NullReferenceException(String.Format("DefaultItem is null. Set component of type {0} to DefaultItem.", typeof(TComponent).FullName));
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

			customItems = SortItems(customItems);
			UpdateView();

			DefaultItem.gameObject.SetActive(false);

			OnSelect.AddListener(OnSelectCallback);
			OnDeselect.AddListener(OnDeselectCallback);
		}

		void Resize()
		{
			scrollHeight = scrollRect.GetComponent<RectTransform>().rect.height;
			visibleItems = (int)Math.Ceiling(scrollHeight / itemHeight) + 1;
			UpdateView();
		}

		bool CanOptimize()
		{
			return scrollRect!=null && Container.GetComponent<EasyLayout.EasyLayout>()!=null;
		}

		void OnSelectCallback(int index, ListViewItem item)
		{
			OnSelectObject.Invoke(index);

			if (item!=null)
			{
				SelectColoring(item as TComponent);
			}
		}
		
		void OnDeselectCallback(int index, ListViewItem item)
		{
			OnDeselectObject.Invoke(index);

			if (item!=null)
			{
				DefaultColoring(item as TComponent);
			}
		}
		
		void OnPointerEnterCallback(ListViewItem item)
		{
			OnPointerEnterObject.Invoke(item.Index);

			if (!IsSelected(item.Index))
			{
				HighlightColoring(item as TComponent);
			}
		}
		
		void OnPointerExitCallback(ListViewItem item)
		{
			OnPointerExitObject.Invoke(item.Index);

			if (!IsSelected(item.Index))
			{
				DefaultColoring(item as TComponent);
			}
		}
		
		/// <summary>
		/// Updates thitemsms.
		/// </summary>
		public override void UpdateItems()
		{
			UpdateItems(customItems);
		}

		/// <summary>
		/// Clear items of this instance.
		/// </summary>
		public override void Clear()
		{
			customItems.Clear();
			UpdateItems();
		}

		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of added item.</returns>
		public virtual int Add(TItem item)
		{
			var newItems = customItems;
			newItems.Add(item);
			UpdateItems(newItems);
			
			var index = customItems.FindIndex(x => x.Equals(item));
			
			return index;
		}
		
		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <returns>Index of removed TItem.</returns>
		public virtual int Remove(TItem item)
		{
			var index = customItems.FindIndex(x => x.Equals(item));
			if (index==-1)
			{
				return index;
			}

			Remove(index);

			return index;
		}

		/// <summary>
		/// Remove item by specifieitemsex.
		/// </summary>
		/// <returns>Index of removed item.</returns>
		/// <param name="index">Index.</param>
		public virtual void Remove(int index)
		{
			var newItems = customItems;
			newItems.RemoveAt(index);
			UpdateItems(newItems);			
		}

		void RemoveCallback(ListViewItem item, int index)
		{
			if (item==null)
			{
				return ;
			}
			if (callbacksEnter.Count > index)
			{
				item.onPointerEnter.RemoveListener(callbacksEnter[index]);
			}
			if (callbacksExit.Count > index)
			{
				item.onPointerExit.RemoveListener(callbacksExit[index]);
			}
		}

		void RemoveCallbacks()
		{
			base.Items.ForEach(RemoveCallback);
			callbacksEnter.Clear();
			callbacksExit.Clear();
		}
		
		void AddCallbacks()
		{
			base.Items.ForEach(AddCallback);
		}
		
		void AddCallback(ListViewItem item, int index)
		{
			callbacksEnter.Add(ev => OnPointerEnterCallback(item));
			callbacksExit.Add(ev => OnPointerExitCallback(item));
			
			item.onPointerEnter.AddListener(callbacksEnter[callbacksEnter.Count - 1]);
			item.onPointerExit.AddListener(callbacksExit[callbacksExit.Count - 1]);
		}
		
		List<TItem> SortItems(List<TItem> newItems)
		{
			var temp = newItems;
			if (SortFunc!=null)
			{
				temp = SortFunc(temp);
			}
			
			return temp;
		}

		/// <summary>
		/// Sets component data with specified item.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="item">Item.</param>
		protected virtual void SetData(TComponent component, TItem item)
		{
		}

		List<TComponent> GetNewComponents(List<TItem> newItems)
		{
			componentsCache = componentsCache.Where (x => x != null).ToList ();
			var new_components = new List<TComponent>();
			newItems.ForEach ((x, i) =>  {
				if (i >= visibleItems)
				{
					return;
				}

				if (components.Count > 0)
				{
					new_components.Add(components[0]);
					components.RemoveAt(0);
				}
				else if (componentsCache.Count > 0)
				{
					componentsCache[0].gameObject.SetActive(true);
					new_components.Add(componentsCache[0]);
					componentsCache.RemoveAt(0);
				}
				else
				{
					var component = Instantiate(DefaultItem) as TComponent;
					Utilites.FixInstantiated (DefaultItem, component);
					component.gameObject.SetActive(true);
					new_components.Add(component);
				}
			});

			components.ForEach(x => x.gameObject.SetActive(false));
			componentsCache.AddRange(components);
			components.Clear();

			return new_components;
		}
		
		void OnScroll(float value)
		{
			var value_n = 1f - value;
			var listHeight = (customItems.Count * (itemHeight + spacing)) - spacing;
			var window = (listHeight * value_n) - (scrollHeight * value_n);
			var newTopHiddenItems = (int)Math.Floor((window + spacing) / (itemHeight + spacing));

			var oldTopHiddenItems = topHiddenItems;
			topHiddenItems = Math.Min(newTopHiddenItems, customItems.Count - visibleItems);
			bottomHiddenItems = customItems.Count - visibleItems - topHiddenItems;

			//поменять данные отображаемых элементов
			if (oldTopHiddenItems==topHiddenItems)
			{
				//do nothing
			}
			// optimization on +-1 item scroll
			else if (oldTopHiddenItems==(topHiddenItems + 1))
			{
				var bottomComponent = components[components.Count - 1];
				components.RemoveAt(components.Count - 1);
				components.Insert(0, bottomComponent);
				bottomComponent.transform.SetAsFirstSibling();

				bottomComponent.Index = topHiddenItems;
				SetData(bottomComponent, customItems[topHiddenItems]);
				Coloring(bottomComponent);
			}
			else if (oldTopHiddenItems==(topHiddenItems - 1))
			{
				var topComponent = components[0];
				components.RemoveAt(0);
				components.Add(topComponent);
				topComponent.transform.SetAsLastSibling();

				topComponent.Index = topHiddenItems + visibleItems - 1;
				SetData(topComponent, customItems[topHiddenItems + visibleItems - 1]);
				Coloring(topComponent);
			}
			// all other cases
			else
			{
				var new_indicies = Enumerable.Range(topHiddenItems, visibleItems).ToArray();
				components.ForEach((x, i) => {
					x.Index = new_indicies[i];
					SetData(x, customItems[new_indicies[i]]);
					Coloring(x);
				});
			}

			SetTopFiller();
			SetBottomFiller();
		}

		void UpdateView()
		{
			RemoveCallbacks();

			if (CanOptimize())
			{
				visibleItems = (visibleItems < customItems.Count) ? visibleItems : customItems.Count;
			}
			else
			{
				visibleItems = customItems.Count;
			}

			components = GetNewComponents(customItems);

			var base_items = new List<ListViewItem>();
			components.ForEach(x => base_items.Add(x));
			
			base.Items = base_items;
			
			components.ForEach((x, i) => {
				x.Index = i;
				SetData(x, customItems[i]);
				DefaultColoring(x);
			});

			AddCallbacks();
			
			topHiddenItems = 0;
			bottomHiddenItems = customItems.Count() - visibleItems;
			
			SetTopFiller();
			SetBottomFiller();

			#if UNITY_4_6_1
			// force ContentSizeFitter update text width
			scrollRect.verticalScrollbar.value += 0.1f;
			#endif
		}

		void UpdateItems(List<TItem> newItems)
		{
			newItems = SortItems(newItems);

			var selected_indicies = new List<int>();
			SelectedIndicies.ForEach(index => {
				var new_index = newItems.FindIndex(x => x.Equals(customItems[index]));
				if (new_index!=-1)
				{
					selected_indicies.Add(index);
				}
			});

			customItems = newItems;

			UpdateView();

			selected_indicies.ForEach(x => Select(x));
		}

		void SetBottomFiller()
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

		void SetTopFiller()
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

		/// <summary>
		/// Determines if item exists with the specified index.
		/// </summary>
		/// <returns><c>true</c> if item exists with the specified index; otherwise, <c>false</c>.</returns>
		/// <param name="index">Index.</param>
		public override bool IsValid(int index)
		{
			return (index >= 0) && (index < customItems.Count);
		}

		void Coloring(TComponent component)
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

		/// <summary>
		/// Set highlights colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected virtual void HighlightColoring(TComponent component)
		{
			component.Background.color = HighlightedBackgroundColor;
		}

		/// <summary>
		/// Set select colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected virtual void SelectColoring(TComponent component)
		{
			if (component==null)
			{
				return ;
			}

			component.Background.color = SelectedBackgroundColor;
		}

		/// <summary>
		/// Set default colors of specified component.
		/// </summary>
		/// <param name="component">Component.</param>
		protected virtual void DefaultColoring(TComponent component)
		{
			if (component==null)
			{
				return ;
			}

			component.Background.color = DefaultBackgroundColor;
		}

		void UpdateColors()
		{
			components.ForEach(Coloring);
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
	}
}