# README

This project is only used to host the F# HTML UI and API.

It should only contain wiring code for WPF and WebView2.

**There should be no UI or API logic in this project.**


## WebView2 Runtime

This uses a Fixed Version Runtime WebView2. This was a deliberate choice because I have had apps broken suddenly and unexpectedly by Chromium updates.

Pros
* WebView2 runtime is automatically distributed with the app
* App functionality will not be broken by Chromium updates
* Developer can update and test the app on my schedule

Cons
* Increases the deployment size of the app
* WebView2 does not receive security updates automatically
* Must schedule or remember to periodically update

More information about Evergreen vs Fixed Version can be found in the [WebView2 Distribution](https://docs.microsoft.com/en-us/microsoft-edge/webview2/concepts/distribution) documentation.


### Switching to Evergreen Runtime

> _Untested_

* Remove the `WebView2.{version}.{arch}` folder from the project
* In MainWindow.xaml.cs, remove this code:

```csharp
            webView.CreationProperties =
                new CoreWebView2CreationProperties
                {
                    BrowserExecutableFolder = "WebView2.{version}.{arch}"
                };
```

* Download and install the Evergreen runtime from [here](https://developer.microsoft.com/en-us/microsoft-edge/webview2/#download-section)
  * It must be installed on every machine which uses this app


## FAQ


### Why is this a C# project? Why not F# like the others?

I originally created this as an F# project (by hand because there is no F# WPF template). But ultimately went with C#. You see, WPF uses C# partial classes to instrument Visual Studio. F# does not support partial classes. So the things VS does for you in C# are missing in F#. Many of the examples you find online in C# have to be done differently in F#. And I am not overly familiar with WPF. Pragmatically this project is less fuss in C#, and code will be kept to a minimum anyway.


### What about FsXaml, Elmish.WPF, Avalonia, etc.

These make sense when using WPF to write UI. But this project only serves as an empty husk to host the F# HTML UI and F# API.