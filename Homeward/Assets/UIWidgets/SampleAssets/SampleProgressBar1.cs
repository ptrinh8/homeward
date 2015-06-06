using UnityEngine;
using UnityEngine.UI;
using UIWidgets;

public class SampleProgressBar1 : MonoBehaviour {
	public Progressbar bar;

	// Use this for initialization
	void Start()
	{
		var button = GetComponent<Button>();
		button.onClick.AddListener(Click);
	}
	
	void Click()
	{
		if (bar.IsAnimationRun)
		{
			bar.Stop();
		}
		else
		{
			bar.Animate();
		}
	}

}
