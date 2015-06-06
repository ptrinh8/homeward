using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

public class TestListView : MonoBehaviour {
	public ListView listView;
	public ListView listView2;
	
	// Use this for initialization
	void Start()
	{
		var button = GetComponent<Button>();
		button.onClick.AddListener(Click);
	}
	
	void Click()
	{
		listView.Add("Added from script");
		listView.Remove("Caster");
	}
	
}
