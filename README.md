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

}
```

If you use MvvmCross:
Coming soon

Setup content view and empty text (optional) in OnActivityCreate() method.
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
