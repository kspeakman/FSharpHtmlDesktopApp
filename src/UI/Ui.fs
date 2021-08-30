namespace Ui

module Ui =

    open Fable.React
    open Fable.React.Props

    let view model dispatch =
        div [] [
            str model.Status
            div [] [button [Type "button"; OnClick (fun _ -> dispatch Ping)] [str "Ping"]]
            div [] [button [Type "button"; OnClick (fun _ -> dispatch Exit)] [str "Exit"]]
        ]

