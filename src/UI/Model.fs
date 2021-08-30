namespace Ui

type Model =
    {
        Status: string
    }

module Model =

    open Api.Messages

    let init () =
        {
            Status = "idle"
        }, []


    let update msg model =
        match msg with
        | Ping ->
            { model with Status = "ping" }
            , [SendToApi (Echo "pong")]
        | ApiReponseParseError (err, response) ->
            { model with Status = "Response parse error: " + err + ", Response: " + response }, []
        | ApiResponded (Error err) ->
            match err with
            | UnrecognizedRequest s ->
                { model with Status = "Unrecognized request: " + s }, []
        | ApiResponded (Ok apiResponse) ->
            match apiResponse with
            | AppClosed ->
                { model with Status = "closed" }, []
            | Echoed s ->
                { model with Status = s }, []
        | Exit ->
            { model with Status = "closing" }
            , [SendToApi CloseApp]


