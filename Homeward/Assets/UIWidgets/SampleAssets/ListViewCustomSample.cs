using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UIWidgets {

	[System.Serializable]
	public class ListViewCustomSampleItemDescription {
		[SerializeField]
		public Sprite Icon;
		[SerializeField]
		public string Name;
		[SerializeField]
		public int Progress;

		public override int GetHashCode()
		{
			return Icon.GetHashCode() ^ Name.GetHashCode() ^ Progress;
		}

		public override bool Equals(System.Object obj)
		{
			ListViewCustomSampleItemDescription descObj = obj as ListViewCustomSampleItemDescription; 
			if (descObj == null)
				return false;
			return Name==descObj.Name && Progress==descObj.Progress && Icon.Equals(descObj.Icon);
		}
	}

	public class ListViewCustomSample : ListViewCustom<ListViewCustomSampleComponent,ListViewCustomSampleItemDescription> {
		[System.NonSerialized]
		bool start_called2 = false;

		public override void Start()
		{
			if (start_called2)
			{
				return ;
			}
			start_called2 = true;

			SortFunc = (x) => x.OrderBy(y => y.Name).ToList();
			base.Start();
		}

		protected override void SetData(ListViewCustomSampleComponent component, ListViewCustomSampleItemDescription item)
		{
			component.SetData(item);
		}

		protected override void HighlightColoring(ListViewCustomSampleComponent component)
		{
			base.HighlightColoring(component);
			component.Text.color = HighlightedColor;
		}

		protected override void SelectColoring(ListViewCustomSampleComponent component)
		{
			base.SelectColoring(component);
			component.Text.color = SelectedColor;
		}

		protected override void DefaultColoring(ListViewCustomSampleComponent component)
		{
			base.DefaultColoring(component);
			component.Text.color = DefaultColor;
		}
	}
}