namespace Ui

module Startup =

    open Elmish
    open Elmish.React
    open Elmish.Debug
    open Elmish.HMR
    open Fable.Core


    JsInterop.importSideEffects "./styles.css"


    module Subscriptions =

        [<Emit("window.chrome.webview.addEventListener('message', $0)")>]
        let addMessageListener (_fn: Effect.ReceivedEvent -> unit) : unit = jsNative


        let startReceivingMessages dispatch =
            addMessageListener (Effect.receiveMessage dispatch)


        let all _initModel =
            [startReceivingMessages]


    module Program =

        let mkEffectProgram init update view perform =
            let init arg =
                let model, effects = init arg
                model, List.map (perform model) effects
            let update msg model =
                let model, effects = update msg model
                model, List.map (perform model) effects
            Program.mkProgram init update view


    Program.mkEffectProgram Model.init Model.update Ui.view Effect.perform
    |> Program.withReactBatched "elmish-app"
    #if DEBUG
    |> Program.withConsoleTrace
    #endif
    // debugger is off by default, because it requires external tools or it will error on console
    // |> Program.withDebugger
    |> Program.withSubscription Subscriptions.all
    |> Program.run

