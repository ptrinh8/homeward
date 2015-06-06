using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets {
	public class ListViewCustomSampleComponent : ListViewItem {
		[SerializeField]
		public Image Icon;

		[SerializeField]
		public Text Text;

		[SerializeField]
		public Progressbar Progressbar;

		public void SetData(ListViewCustomSampleItemDescription item)
		{
			if (item==null)
			{
				Icon.sprite = null;
				Text.text = string.Empty;
				Progressbar.Value = 0;
			}
			else
			{
				Icon.sprite = item.Icon;
				Text.text = item.Name;
				Progressbar.Value = item.Progress;
			}

			Icon.SetNativeSize();
			//set transparent color if no icon
			Icon.color = (item.Icon==null) ? new Color(0, 0, 0, 0) : Color.white;
		}
	}
}