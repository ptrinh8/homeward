using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UIWidgets;

public class TestListViewPerformance : MonoBehaviour {
	[SerializeField]
	ListView lv;

	[SerializeField]
	ListViewIcons lvi;

	public void Test10 () {
		lv.Strings = Enumerable.Range(1, 10).Select(x => x.ToString("00")).ToList();
	}

	public void Test100 () {
		lv.Strings = Enumerable.Range(1, 100).Select(x => x.ToString("000")).ToList();
	}

	public void Test1000 () {
		lv.Strings = Enumerable.Range(1, 1000).Select(x => x.ToString("0000")).ToList();
	}

	public void Testi1000 () {
		var items = Enumerable.Range(1, 1000).Select(x => x.ToString("0000")).Select(x => new ListViewIconsItemDescription() {
			Name = x
		}).ToList();
		lvi.Items = items;
	}

}
