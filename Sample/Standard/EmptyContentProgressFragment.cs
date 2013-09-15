using Android.OS;
using Android.Views;
using com.refractored.compontents.progressfragment;

namespace com.refractored.samples.progressfragment
{
    public class EmptyContentProgressFragment : ProgressFragment
    {
        private View m_ContentView;
        private Handler m_Handler = new Handler();


        private void ShowContentRunnable()
        {
            IsContentEmpty = true;
            ContentShown = true;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            m_ContentView = inflater.Inflate(Resource.Layout.view_content, null);
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            ContentView = m_ContentView;
            SetEmptyText(Resource.String.empty);
            ObtainData();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.refresh, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_refresh)
            {
                ObtainData();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            m_Handler.RemoveCallbacks(ShowContentRunnable);
        }

        private void ObtainData()
        {
            ContentShown = false;
            m_Handler.PostDelayed(ShowContentRunnable, 3000);
        }
    }
}