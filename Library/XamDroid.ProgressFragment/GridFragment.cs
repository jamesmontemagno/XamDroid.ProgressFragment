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
using Android.Support.V4.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;

namespace com.refractored.compontents.progressfragment
{
    /// <summary>
    /// Implemenatation of the fragment to display gridview. Based on android.support.v4.app.ListFragment
    /// If you are waiting for initial data you can display an indeterminate progress indicator during this time.
    /// </summary>
    public class GridFragment : Fragment, AdapterView.IOnItemClickListener
    {
        private readonly Handler m_Handler = new Handler();
        private View m_EmptyView;
        private TextView m_StandardEmptyView;
        private View m_ProgressContainer;
        private View m_GridContainer;


        public GridFragment() : base()
        {
            
        }

        public GridFragment(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_grid, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            EnsureList();
        }


        /// <summary>
        /// Gets the selected item itd
        /// </summary>
        public long SeletedItemId
        {
            get
            {
                EnsureList();
                return m_GridView.SelectedItemId;
            }
        } 

        private GridView m_GridView;
        public GridView GridView
        {
            get
            {
                EnsureList();
                return m_GridView;
            }
        }

        /// <summary>
        /// Sets the selection you can all use "SelectedItemPosition"
        /// </summary>
        /// <param name="position">position to set</param>
        public void SetSelection(int position)
        {
            EnsureList();
            m_GridView.SetSelection(position);
        } 

        /// <summary>
        /// Gets or sets the selected Item position
        /// </summary>
        public int SelectedItemPosition
        {
            get
            {
                EnsureList();
                return m_GridView.SelectedItemPosition;
            }
            set
            {
                SetSelection(value);
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
                return m_EmptyText;
            }
            set
            {
                EnsureList();
                if(m_StandardEmptyView == null)
                    throw new IllegalStateException("Can't be used with a custom content view.");

                m_StandardEmptyView.Text = value;

                if (value == null)
                    m_GridView.EmptyView = m_StandardEmptyView;

                m_EmptyText = value;
            }
        }

        private IListAdapter m_GridAdapter;
        /// <summary>
        /// Gets or sets the grid adapter
        /// if grid is not shown will animate in
        /// </summary>
        public IListAdapter GridAdapter
        {
            get {return m_GridAdapter; }
            set
            {
                var hadAdapter = m_GridAdapter != null;
                m_GridAdapter = value;
                if (m_GridView == null)
                    return;

                m_GridView.Adapter = m_GridAdapter;
                if (m_GridShown || hadAdapter) 
                    return;

                SetGridShown(true, View.WindowToken != null);
            }
        }

        private bool m_GridShown;
        public bool GridShown
        {
            get {return m_GridShown; }
            set { SetGridShown(value, true); }
        }

        public bool GridShownNoAnimation
        {
            get { return m_GridShown; }
            set {SetGridShown(value, false);  }
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
            EnsureList();
            if (m_ProgressContainer == null)
            {
                throw new IllegalStateException("Can't be used with custom content view");
            }

            if (m_GridShown == shown)
                return;

            m_GridShown = shown;

            if (shown)
            {
                if (animate)
                {
                    m_ProgressContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Android.Resource.Animation.FadeOut));
                    m_GridContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Android.Resource.Animation.FadeIn));
                }
                else
                {
                    m_ProgressContainer.ClearAnimation();
                    m_GridContainer.ClearAnimation();
                }
                m_ProgressContainer.Visibility = ViewStates.Gone;
                m_GridContainer.Visibility = ViewStates.Visible;
            }
            else
            {
                if (animate)
                {
                    m_ProgressContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Android.Resource.Animation.FadeIn));
                    m_GridContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Android.Resource.Animation.FadeOut));
                }
                else
                {
                    m_ProgressContainer.ClearAnimation();
                    m_GridContainer.ClearAnimation();
                }
                m_ProgressContainer.Visibility = ViewStates.Visible;
                m_GridContainer.Visibility = ViewStates.Gone;
            }
        }

        private void RequestFocusRunnable()
        {
            if (m_GridView == null)
                return;

            m_GridView.FocusableViewAvailable(m_GridView);
        }



        private void EnsureList()
        {
            if (m_GridView != null)
                return;

            var root = View;
            if(root == null)
                throw new IllegalStateException("Content view not yet created");

            m_GridView = root as GridView;
            if (m_GridView == null)
            {
                var emptyView = root.FindViewById(Android.Resource.Id.Empty);
                if (emptyView != null)
                {
                    var emptyTextView = emptyView as TextView;
                    if (emptyTextView != null)
                        m_StandardEmptyView = emptyTextView;
                    else
                        m_EmptyView = emptyView;
                }
                else
                {
                    m_StandardEmptyView.Visibility = ViewStates.Gone;
                }

                m_ProgressContainer = root.FindViewById(Resource.Id.progress_container);
                m_GridContainer = root.FindViewById(Resource.Id.grid_container);
                m_GridView = root.FindViewById(Resource.Id.grid) as GridView;
                if (m_GridView == null)
                    throw new RuntimeException("Content has a view with id attribute R.id.grid and is not a gridview class");

                if (m_EmptyView != null)
                {
                    m_GridView.EmptyView = m_EmptyView;
                }
                else if (m_EmptyText != null)
                {
                    m_StandardEmptyView.Text = m_EmptyText;
                    m_GridView.EmptyView = m_StandardEmptyView;
                }
            }


            m_GridShown = true;
            m_GridView.OnItemClickListener = this;
            if (m_GridAdapter != null)
            {
                var adapter = m_GridAdapter;
                m_GridAdapter = null;
                GridAdapter = adapter;
            }
            else if(m_ProgressContainer != null)
            {
                SetGridShown(false, false);
            }

            m_Handler.Post(RequestFocusRunnable);
        }


        public override void OnDestroyView()
        {
            m_Handler.RemoveCallbacks(RequestFocusRunnable);
            m_GridView = null;
            m_GridShown = false;
            m_EmptyView = null;
            m_ProgressContainer = null;
            m_GridContainer = null;
            m_StandardEmptyView = null;
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
            GridItemClick((GridView)parent, view, position, id);
        }
    }
}
