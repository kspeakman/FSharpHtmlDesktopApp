# F# HTML Desktop app

This sample app demonstrates a desktop app using HTML/CSS/F# for the UI and F# .NET for the back end. All hosted in WPF.


## How does it work?

The UI is a webpack-built JS app just like you would expect. It uses F# and Elmish for a functional (as in functional programming) UI. The F# code gets transpiled to JS and uses React for DOM rendering.

The "backend" API is an F# library. In this starter app, the backend is very simple. So there is nothing interesting to demonstrate yet.

A messaging library is shared between the front and back. It defines all the requests and responses that can pass between them. 

The host application is WPF. WinForms would work too. Either way there is not much to it. The only control is the WebView2 control. And about 40 lines of wiring code.

For communication between the JS front and .NET back, I used WebView2's string-based messaging. Each side gets some functions to send/receive messages. Types from the messaging library are converted to JSON strings for transport.


## What does it look like?

Right now it mainly demos the front-to-back communication. So it is not much to look at.

![image](https://user-images.githubusercontent.com/4582668/131415157-149db106-12e6-456f-9b50-f2af96434661.png)

> TODO polish up the starter UI


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


## Browser Dev tools

In Debug mode you can right-click somewhere inside the WPF window to bring up the Edge context menu. Which includes Inspect. This will open the Edge (Chromium) dev tools in another window.

In Release mode this is disabled.


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
