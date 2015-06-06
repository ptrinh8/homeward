using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets
{
	/// <summary>
	/// Combobox with icons.
	/// </summary>
	[AddComponentMenu("UI/ComboboxIcons", 230)]
	public class ComboboxIcons : ComboboxCustom<ListViewIcons,ListViewIconsItemComponent,ListViewIconsItemDescription>
	{
		/// <summary>
		/// Start this instance.
		/// </summary>
		public override void Start()
		{
			base.Start();
		}

		/// <summary>
		/// Updates the current component with selected item.
		/// </summary>
		protected override void UpdateCurrent()
		{
			Current.SetData(ListView.SelectedItem);

			base.HideList();
		}

		#if UNITY_EDITOR
		[UnityEditor.MenuItem("GameObject/UI/ComboboxIcons")]
		static void CreateObject()
		{
			Utilites.CreateObject("Assets/UIWidgets/Prefabs/ComboboxIcons.prefab");
		}
		#endif
	}
}