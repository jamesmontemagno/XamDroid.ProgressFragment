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
using System;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;

namespace com.refractored.compontents.progressfragment
{
    public class ProgressFragment : Fragment
    {
        private View m_ProgressContainer;
        private View m_ContentContainer;
        private View m_EmptyView;

        private View m_ContentView;
        /// <summary>
        /// Gets or sets the content view.
        /// On Set if the content view was installed earlier the content view will be replaced
        /// Value can NOT be null
        /// </summary>
        public View ContentView
        {
            get {return m_ContentView; }
            set 
            {
                EnsureContent();
                if(value == null)
                    throw new NullReferenceException("ContentView can not be null");

                var contentContainer = m_ContentContainer as ViewGroup;
                if (contentContainer != null)
                {
                    if (m_ContentView == null)
                    {
                        contentContainer.AddView(value);
                    }
                    else
                    {
                        var index = contentContainer.IndexOfChild(m_ContentView);
                        contentContainer.RemoveView(m_ContentView);
                        contentContainer.AddView(value, index);
                    }
                    m_ContentView = value;
                }
                else
                {
                    throw new IllegalStateException("Content view can not be used with a custom content view.");
                }
            }
        }

        private bool m_IsContentEmpty;
        /// <summary>
        /// Gets or sets if the Content is empty
        /// </summary>
        public bool IsContentEmpty
        {
            get { return m_IsContentEmpty; }
            set
            {
                EnsureContent();
                if (m_ContentView == null)
                    throw new IllegalStateException("Content view must be initialized before setting.");

                m_IsContentEmpty = value;

                m_EmptyView.Visibility = m_IsContentEmpty ? ViewStates.Visible : ViewStates.Gone;
                m_ContentView.Visibility = m_IsContentEmpty ? ViewStates.Gone : ViewStates.Visible;
            }
        }

        /// <summary>
        /// Gets or sets the Empty Text to display
        /// </summary>
        public string EmptyText
        {
            get
            {
                EnsureContent();
                var textView = m_EmptyView as TextView;
                if (textView == null)
                    return null;

                return textView.Text;
            }
            set
            {
                EnsureContent();
                var textView = m_EmptyView as TextView;
                if(textView == null)
                    throw new IllegalStateException("Empty text can not be used with a custom content view.");
                textView.Text = value;
            }
        }

        private int m_EmptyTextRes;
        public int EmptyTextRes
        {
            get
            {
                return m_EmptyTextRes;
            }
            set
            {
                this.SetEmptyText(value);
            }
        }

        private bool m_ContentShown = false;
        public bool ContentShown
        {
            get { return m_ContentShown; }
            set { SetContentShown(value, true); }
        }

        public bool ContentShownNoAnimation
        {
            get { return m_ContentShown; }
            set { SetContentShown(value, false); }
        }

        private void SetContentShown(bool shown, bool animate)
        {
            EnsureContent();
            if (m_ContentShown == shown)
                return;

            m_ContentShown = shown;

            if (shown)
            {
                if (animate)
                {
                    m_ProgressContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Android.Resource.Animation.FadeOut));
                    m_ContentContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Android.Resource.Animation.FadeIn));
                }
                else
                {
                    m_ProgressContainer.ClearAnimation();
                    m_ContentContainer.ClearAnimation();
                }
                m_ProgressContainer.Visibility = ViewStates.Gone;
                m_ContentContainer.Visibility = ViewStates.Visible;
            }
            else
            {
                if (animate)
                {
                    m_ProgressContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Android.Resource.Animation.FadeIn));
                    m_ContentContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Android.Resource.Animation.FadeOut));
                }
                else
                {
                    m_ProgressContainer.ClearAnimation();
                    m_ContentContainer.ClearAnimation();
                }
                m_ProgressContainer.Visibility = ViewStates.Visible;
                m_ContentContainer.Visibility = ViewStates.Gone;
            }
        }

        /// <summary>
        /// Sets the empty text via Resource ID
        /// </summary>
        /// <param name="resId"></param>
        public void SetEmptyText(int resId)
        {
            m_EmptyTextRes = resId;
            EmptyText = GetString(resId);
        }

        /// <summary>
        /// Get the content content from a layout resource
        /// </summary>
        /// <param name="layoutResourceId"></param>
        public void SetContentView(int layoutResourceId)
        {
            var layoutInflater = LayoutInflater.From(Activity);
            m_ContentView = layoutInflater.Inflate(layoutResourceId, null);
        }

        private void EnsureContent()
        {
            if (m_ContentContainer != null && m_ProgressContainer != null)
                return;

            var root = View;
            if(root == null)
                throw new IllegalStateException("Content view not yet created");

            m_ProgressContainer = root.FindViewById(Resource.Id.progress_container);
            if(m_ProgressContainer == null)
                throw new RuntimeException("Your content must have a viewgroup whose id is Resource.Id.progress_conteiner");

            m_ContentContainer = root.FindViewById(Resource.Id.content_container);
            if(m_ContentContainer == null)
                throw new RuntimeException("Your content must have a viewgroup whose id is Resource.Id.content_container");

            m_EmptyView = root.FindViewById(Android.Resource.Id.Empty);
            if(m_EmptyView != null)
                m_EmptyView.Visibility = ViewStates.Gone;

            m_ContentShown = true;

            //We are starting without a content, so assume we won't
            //have our data right away and start with a progress indicator
            if(m_ContentView == null)
                SetContentShown(false, false);
        }

        public ProgressFragment()
            : base()
        {
            
        }

        public ProgressFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        /// <summary>
        /// Provide default implementation to return a simple view.  Subclasses
        /// can override to replace with their own layout.  If doing so, the
        /// returned view hierarchy <em>must</em> have a progress container  whose id
        /// is Resource.Id.progress_container, content container whose id 
        /// is Resource.Id.content_container and optionally
        /// have a sibling view id Android.Resource.Id.Empty
        /// that is to be shown when the content is empty.
        /// 
        /// If you are overriding this method with your own custom content
        /// consider including the standard layout: com.refractored.compontents.progressfragment.Resource.Layout.framgent_process
        /// in your layoutFile so that you can continue to retain all of the standard behavior of ProgressFrament. In particular
        /// this is currently the only way to have the built-in indetermineante progress sate be shown.
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_progress, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            EnsureContent();
        }

        public override void OnDestroyView()
        {
            m_ContentShown = false;
            m_IsContentEmpty = false;
            m_ProgressContainer = null;
            m_ContentContainer = null;
            m_ContentView = null;
            m_EmptyView = null;
            base.OnDestroyView();
        }


    }
}