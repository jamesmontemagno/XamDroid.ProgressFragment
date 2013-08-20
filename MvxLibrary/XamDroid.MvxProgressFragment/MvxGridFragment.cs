/*
* Copyright (C) 2013 @JamesMontemagno http://www.montemagno.com http://www.refractored.com
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
* Converted from: https://github.com/johnkil/Android-ProgressFragment
*/
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

using Cirrious.MvvmCross.Droid.Fragging.Fragments;

using Java.Lang;

namespace com.refractored.mvxcomponents.progressfragment
{


    /// <summary>
    /// Implemenatation of the fragment to display gridview. Based on android.support.v4.app.ListFragment
    /// If you are waiting for initial data you can display an indeterminate progress indicator during this time.
    /// </summary>
    public class GridFragment : MvxFragment, AdapterView.IOnItemClickListener
    {
        private readonly Handler m_Handler = new Handler();
        private View m_EmptyView;
        private TextView m_StandardEmptyView;
        private View m_ProgressContainer;
        private View m_GridContainer;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.fragment_grid, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            this.EnsureList();
        }


        /// <summary>
        /// Gets the selected item itd
        /// </summary>
        public long SeletedItemId
        {
            get
            {
                this.EnsureList();
                return this.m_GridView.SelectedItemId;
            }
        }

        private GridView m_GridView;
        public GridView GridView
        {
            get
            {
                this.EnsureList();
                return this.m_GridView;
            }
        }

        /// <summary>
        /// Sets the selection you can all use "SelectedItemPosition"
        /// </summary>
        /// <param name="position">position to set</param>
        public void SetSelection(int position)
        {
            this.EnsureList();
            this.m_GridView.SetSelection(position);
        }

        /// <summary>
        /// Gets or sets the selected Item position
        /// </summary>
        public int SelectedItemPosition
        {
            get
            {
                this.EnsureList();
                return this.m_GridView.SelectedItemPosition;
            }
            set
            {
                this.SetSelection(value);
            }
        }

        private string m_EmptyText;

        /// <summary>
        /// Gets or sets the empty text to display
        /// This can not be used with a custom content view.
        /// </summary>
        public string EmptyText
        {
            get
            {
                return this.m_EmptyText;
            }
            set
            {
                this.EnsureList();
                if (this.m_StandardEmptyView == null)
                    throw new IllegalStateException("Can't be used with a custom content view.");

                this.m_StandardEmptyView.Text = value;

                if (value == null)
                    this.m_GridView.EmptyView = this.m_StandardEmptyView;

                this.m_EmptyText = value;
            }
        }

        private IListAdapter m_GridAdapter;
        /// <summary>
        /// Gets or sets the grid adapter
        /// if grid is not shown will animate in
        /// </summary>
        public IListAdapter GridAdapter
        {
            get { return this.m_GridAdapter; }
            set
            {
                var hadAdapter = this.m_GridAdapter != null;
                this.m_GridAdapter = value;
                if (this.m_GridView == null)
                    return;

                this.m_GridView.Adapter = this.m_GridAdapter;
                if (this.m_GridShown || hadAdapter)
                    return;

                this.SetGridShown(true, this.View.WindowToken != null);
            }
        }

        private bool m_GridShown;
        public bool GridShown
        {
            get { return this.m_GridShown; }
            set { this.SetGridShown(value, true); }
        }

        public bool GridShownNoAnimation
        {
            get { return this.m_GridShown; }
            set { this.SetGridShown(value, false); }
        }

        /// <summary>
        /// This method will be called whan an item in the grid is selected. SubClasses should override. Subclasses can call.
        /// GridView.GetItemAtPosition if they need to access the data associated with the slected item.
        /// </summary>
        /// <param name="gridView">The gridview where the click happened</param>
        /// <param name="view">The view that was clicked within the ListView</param>
        /// <param name="position">The position of the view in the list</param>
        /// <param name="id">The row id of the item that was clicked</param>
        public virtual void GridItemClick(GridView gridView, View view, int position, long id)
        {

        }

        private void SetGridShown(bool shown, bool animate)
        {
            this.EnsureList();
            if (this.m_ProgressContainer == null)
            {
                throw new IllegalStateException("Can't be used with custom content view");
            }

            if (this.m_GridShown == shown)
                return;

            this.m_GridShown = shown;

            if (shown)
            {
                if (animate)
                {
                    this.m_ProgressContainer.StartAnimation(AnimationUtils.LoadAnimation(this.Activity, Android.Resource.Animation.FadeOut));
                    this.m_GridContainer.StartAnimation(AnimationUtils.LoadAnimation(this.Activity, Android.Resource.Animation.FadeIn));
                }
                else
                {
                    this.m_ProgressContainer.ClearAnimation();
                    this.m_GridContainer.ClearAnimation();
                }
                this.m_ProgressContainer.Visibility = ViewStates.Gone;
                this.m_GridContainer.Visibility = ViewStates.Visible;
            }
            else
            {
                if (animate)
                {
                    this.m_ProgressContainer.StartAnimation(AnimationUtils.LoadAnimation(this.Activity, Android.Resource.Animation.FadeIn));
                    this.m_GridContainer.StartAnimation(AnimationUtils.LoadAnimation(this.Activity, Android.Resource.Animation.FadeOut));
                }
                else
                {
                    this.m_ProgressContainer.ClearAnimation();
                    this.m_GridContainer.ClearAnimation();
                }
                this.m_ProgressContainer.Visibility = ViewStates.Visible;
                this.m_GridContainer.Visibility = ViewStates.Gone;
            }
        }

        private void RequestFocusRunnable()
        {
            if (this.m_GridView == null)
                return;

            this.m_GridView.FocusableViewAvailable(this.m_GridView);
        }



        private void EnsureList()
        {
            if (this.m_GridView != null)
                return;

            var root = this.View;
            if (root == null)
                throw new IllegalStateException("Content view not yet created");

            this.m_GridView = root as GridView;
            if (this.m_GridView == null)
            {
                var emptyView = root.FindViewById(Android.Resource.Id.Empty);
                if (emptyView != null)
                {
                    var emptyTextView = emptyView as TextView;
                    if (emptyTextView != null)
                        this.m_StandardEmptyView = emptyTextView;
                    else
                        this.m_EmptyView = emptyView;
                }
                else
                {
                    this.m_StandardEmptyView.Visibility = ViewStates.Gone;
                }

                this.m_ProgressContainer = root.FindViewById(Resource.Id.progress_container);
                this.m_GridContainer = root.FindViewById(Resource.Id.grid_container);
                this.m_GridView = root.FindViewById(Resource.Id.grid) as GridView;
                if (this.m_GridView == null)
                    throw new RuntimeException("Content has a view with id attribute R.id.grid and is not a gridview class");

                if (this.m_EmptyView != null)
                {
                    this.m_GridView.EmptyView = this.m_EmptyView;
                }
                else if (this.m_EmptyText != null)
                {
                    this.m_StandardEmptyView.Text = this.m_EmptyText;
                    this.m_GridView.EmptyView = this.m_StandardEmptyView;
                }
            }


            this.m_GridShown = true;
            this.m_GridView.OnItemClickListener = this;
            if (this.m_GridAdapter != null)
            {
                var adapter = this.m_GridAdapter;
                this.m_GridAdapter = null;
                this.GridAdapter = adapter;
            }
            else if (this.m_ProgressContainer != null)
            {
                this.SetGridShown(false, false);
            }

            this.m_Handler.Post(this.RequestFocusRunnable);
        }


        public override void OnDestroyView()
        {
            this.m_Handler.RemoveCallbacks(this.RequestFocusRunnable);
            this.m_GridView = null;
            this.m_GridShown = false;
            this.m_EmptyView = null;
            this.m_ProgressContainer = null;
            this.m_GridContainer = null;
            this.m_StandardEmptyView = null;
            base.OnDestroyView();
        }








        /// <summary>
        /// Implemens on item click to call GridItemClick which people can override
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="view"></param>
        /// <param name="position"></param>
        /// <param name="id"></param>
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            this.GridItemClick((GridView)parent, view, position, id);
        }
    }
}
