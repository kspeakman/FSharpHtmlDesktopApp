# F# HTML Desktop app

This example solution demonstrates creating a desktop app using F# and HTML for the UI. The desktop app is hosted in WPF.


## Install pre-requisites

You'll need to install the following pre-requisites in order to build SAFE applications

* The [.NET SDK](https://www.microsoft.com/net/download) 5 or higher.
* [npm](https://nodejs.org/en/download/) package manager.
* [Node LTS](https://nodejs.org/en/download/).
* See the WebView2 installation instructions below.

### WebView2 install - Fixed Version Runtime 

This solution uses the Fixed Version Runtime. This means the WebView2 runtime binaries are distributed as part of the app. The binaries are not included in the git repo for size and licensing reasons.

These same instructions can be used later to update the version of the runtime.

For more information about Evergreen vs Fixed Version Runtimes, see the WpfHost README and the [WebView2 Distribution](https://docs.microsoft.com/en-us/microsoft-edge/webview2/concepts/distribution) documentation.

> When you see curly braces like `{version}`, it is representing the kind of text that should be there.

* Download the [WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/#download-section) Fixed Version Runtime.
  * The file should be named similar to `Microsoft.WebView2.FixedVersionRuntime.{version}.{arch}.cab`.
* Extract the cab file with expand: `expand {cab file} -F:* .\`.
* Move the extracted folder into the WpfHost folder.
  * The folder name can be shortened. But it should contain "WebView2" to be ignored by git.
* Modify `WpfHost.csproj` to match the folder name.
  ```xml
  <ItemGroup>
    <Content Include="{WebView2 folder}\**\*.*">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
  ```
  * This ensures the WebView2 runtime will be included in the built app.
* Change the MainWindow.xaml.cs file to reference the exact WebView2 folder name.

> TODO
> * script installation to project
> * code the Init method to find the WebView2 runtime folder


## Starting the application

Start the HTML UI, then the host.


### Start the HTML UI

Open a powershell or command line in the solution folder. Then run:

```bash
dotnet tool restore
npm install
npm run start
```


### Start the WPF host

* In VS2019 Solution Explorer, locate src/WpfHost project
* Right-click the `WpfHost` project and Set as Startup Project
* Press F5


## Packaging the HTML UI

The built web app can be included in the WPF project. Or it can be independently deployed to a web server.

Build it:

```
npm run build
```


### To WPF content

This option will bundle the HTML UI with the WPF app. It will be included as "Content" in the WPF project.

To make this work smoothly, map a virtual host name to a local folder name. Then the Source can be set to the virtual host name. Otherwise it would be necessary to use a local file path, which are usually blocked by browsers.

```csharp
await webView.EnsureCoreWebView2Async();

// must be after webView is "ensured"
webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "app.ui", "UI",
                    Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
webView.Source = new Uri("https://app.ui");
```

> TODO
> * add UI folder to project as Content
> * script deployment to UI folder


### To local web server

Copy the built assets from the `deploy/public` folder to your web server. Then set the Source to the web server deploy location.


## Acknowledgements

This project is adapted from the SAFE template.