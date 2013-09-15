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
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;

namespace com.refractored.samples.progressfragment
{
    [Activity(Label = "XamDroid.ProgressFragment.Sample", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class Activity1 : ListActivity
    {
        private String[] m_Examples = new String[] { "Default", "Empty content", "Custom layout" };

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleExpandableListItem1,
                                                        m_Examples);
        }

        protected override void OnListItemClick(ListView l, Android.Views.View v, int position, long id)
        {
            var intent = new Intent(this, typeof(ProgressActivity));
            intent.PutExtra(ProgressActivity.ExtraTitle, m_Examples[position]);
            switch (position)
            {
                case 0:
                    intent.PutExtra(ProgressActivity.ExtraFragment, ProgressActivity.FragmentDefault);
                    break;
                case 1:
                    intent.PutExtra(ProgressActivity.ExtraFragment, ProgressActivity.FragmentEmptyContent);
                    break;
                case 2:
                    intent.PutExtra(ProgressActivity.ExtraFragment, ProgressActivity.FragmentCustomLayout);
                    break;
                default:
                    break;
            }
            StartActivity(intent);
        }
    }
}

