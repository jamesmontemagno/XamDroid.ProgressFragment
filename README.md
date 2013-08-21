[![Stories in Ready](https://badge.waffle.io/jamesmontemagno/XamDroid.ProgressFragment.png?label=ready)](https://waffle.io/jamesmontemagno/XamDroid.ProgressFragment)

XamDroid.ProgressFragment
================

Implementation of the fragment with the ability to display indeterminate progress indicator when you are waiting for the initial data.


## Demo
View demo video on ([YouTube](http://www.youtube.com/watch?v=BPsok0kNgIg))

![Loading](https://raw.github.com/jamesmontemagno/XamDroid.ProgressFragment/master/Screenshots/LoadingDevice.png)
![Loaded](https://raw.github.com/jamesmontemagno/XamDroid.ProgressFragment/master/Screenshots/LoadedDevice.png)
![No Data](https://raw.github.com/jamesmontemagno/XamDroid.ProgressFragment/master/Screenshots/NoDataDevice.png)

## Getting started

###Installing the library
Install from Component Store or Clone and add project or dll to your solution.

###Usage
To display the progress fragment you will need the following code:
```
public class MyProgressFragment : ProgressFragment {
    private View m_ContentView;
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        m_ContentView = inflater.Inflate(Resource.Layout.your_content_view, null);
        return base.OnCreateView(inflater, container, savedInstanceState);
    }
}
```

Setup content view (required) and empty text (optional) in OnActivityCreate() method.
```
public override void OnActivityCreated(Bundle savedInstanceState)
{
    base.OnActivityCreated(savedInstanceState);

    //set content view
    ContentView = m_ContentView;
    //Set text for empty content
    SetEmptyText(Resource.String.empty);
}
```

If you use MvvmCross (use the MVXLibary) and there are some special things in the setup:
```
public class MyProgressFragment : MvxProgressFragment {
    private View m_ContentView;
    var originalView = base.OnCreateView(inflater, container, savedInstanceState);
    m_ContentView = this.BindingInflate(Resource.Layout.your_content_view, null);

    var set = this.CreateBindingSet<YourFragment, YourViewModel>();
    set.Bind(this).For(v => v.ContentNotShown).To(vm => vm.IsBusy).Mode(MvxBindingMode.OneWay); //don't show when busy or something
    set.Bind(this).For(v => v.IsContentNotEmpty).To(vm => vm.HasContent).Mode(MvxBindingMode.OneWay); //bind to has content or somethng
    set.Apply();

    return originalView;
}
```

You have a few bools that you can bind to:
```bool ContentShown{get;set;}``` : Determines if the content is visible
```bool IsContentEmpty{get;set;}``` : Displays no data if visible
```bool ContentNotShown{get;set;}``` : Determines if the content is visible (inverse of ContentShown)
```bool IsContentEmpty{get;set;}``` : Displays no data if visible
```bool IsContentNotEmpty{get;set;}``` : Displays no data if visible (inverse of IsContentEmpty)
```int EmptyTextRes{get;set;}``` : Resource id (of string) to display when no data
```string EmptyText{get;set;}``` : String to display when no data



Display Indeterminate progress indicator:
```
ContentShown = false;
```

When the data is loaded to set whether the content is empty and show content:
```
ContentEmpty = /*true if content is empty else false*/;
ContentShown = true;
```

## Development:

Ported and Maintained by:
James Montemagno ([@JamesMontemagno](http://www.twitter.com/jamesmontemagno))

Original ProgressFragment by:
([Evgeny Shishkin on GitHub](https://github.com/johnkil/Android-ProgressFragment))


## Original License

    Copyright 2013 Evgeny Shishkin

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.


## My License

    Copyright 2013 James Montemagno

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
