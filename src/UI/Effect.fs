namespace Ui

module Effect =

    open Api.Messages
    open Fable.Core
    open Fable.SimpleJson


    [<Emit("window.chrome.webview.postMessage")>]
    let sendMessage : string -> unit = jsNative


    let perform model effect dispatch =
        match effect with
        | SendToApi apiRequest ->
            async {
                let apiMessage = Json.serialize apiRequest
                do! Async.Sleep 1000
                sendMessage apiMessage
            }
            |> Async.StartImmediate


    [<NoComparison; NoEquality>]
    type ReceivedEvent = { data: string }

    let receiveMessage dispatch e =
        let msg =
            match Json.tryParseAs<Result<ApiResponse, ApiError>> e.data with
            | Error err -> ApiReponseParseError (err, e.data)
            | Ok result -> ApiResponded result
        dispatch msg