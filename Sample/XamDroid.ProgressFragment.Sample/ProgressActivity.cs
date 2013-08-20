using System;
using Android.App;
using Android.OS;
using Android.Support.V4.App;

namespace com.refractored.samples.progressfragment
{
    [Activity(Label = "My Activity", Theme = "@style/AppTheme", Icon = "@drawable/ic_launcher")]
    public class ProgressActivity : FragmentActivity
    {
        public static String ExtraTitle = "com.refractored.samples.progressfragment.extras.EXTRA_TITLE";
        public static String ExtraFragment = "com.refractored.samples.progressfragment.extras.EXTRA_FRAGMENT";
        public const int FragmentDefault = 0;
        public const int FragmentEmptyContent = 1;
        public const int FragmentCustomLayout = 2;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Title = Intent.GetStringExtra(ExtraTitle);
            
            
#if __ANDROID_11__
            ActionBar.SetDisplayHomeAsUpEnabled(true);
#endif
            // Create your application here
            var frag = SupportFragmentManager.FindFragmentById(Android.Resource.Id.Content);
            if (frag == null)
            {
                int fragId = Intent.GetIntExtra(ExtraFragment, FragmentDefault);
                switch (fragId)
                {
                    case FragmentEmptyContent:
                        frag = new EmptyContentProgressFragment();
                        break;
                    case FragmentCustomLayout:
                        frag = new CustomLayoutProgressFragment();
                        break;
                    case FragmentDefault:
                    default:
                        frag = new DefaultProgressFragment();
                        break;

                }
            }
            SupportFragmentManager.BeginTransaction().Add(Android.Resource.Id.Content, frag).Commit();
        }

        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}