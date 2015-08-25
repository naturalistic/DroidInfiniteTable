using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace DroidInfiniteTable
{
	[Activity(Label = "BasicTable", MainLauncher = true, Icon = "@drawable/icon")]
	public class InfiniteListActivity : ListActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			var adapter = new ArrayAdapter<DateTime>(this, Android.Resource.Layout.SimpleListItem1, new List<DateTime> ());
			ListView.Adapter = adapter;
			var scrollListener = new EndlessScrollListener (adapter);
			this.ListView.SetOnScrollListener (scrollListener);
			ListView.VerticalScrollBarEnabled = false;
		}
	}

	public class EndlessScrollListener : Java.Lang.Object, AbsListView.IOnScrollListener {

		private const int chunksize = 20;
		private ArrayAdapter<DateTime> adapter;

		public EndlessScrollListener(ArrayAdapter<DateTime> adapter) {
			this.adapter = adapter;
			for (int i = 0; i < chunksize*2; i++) {
				adapter.Add (DateTime.Now.AddDays (i));
			};
		}

		public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
			System.Diagnostics.Debug.WriteLine(firstVisibleItem);
			if (firstVisibleItem + visibleItemCount == totalItemCount) { // Clear items, add from current half way to chunk size after last item, scroll back to old last item
				var firstItem = adapter.GetItem(chunksize);
				adapter.Clear ();
				for (int i = 0; i < chunksize*2; i++) {
					adapter.Add(firstItem.AddDays (i));
				}
				adapter.NotifyDataSetChanged ();
				view.SetSelection (chunksize-visibleItemCount-1);
			}
			else if (firstVisibleItem == 0) {	// Clear items, add from chunk size before first item, scroll back to old first item
					
				var lastItem = adapter.GetItem(chunksize);
				adapter.Clear ();
				for (int i = chunksize*2-1; i >= 0; i--) {
					adapter.Add(lastItem.AddDays (-i));
				}
				adapter.NotifyDataSetChanged ();
				view.SetSelection (chunksize-1);			
			}
		}

		public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
		{
			// Do nothing
		}
	}
}

