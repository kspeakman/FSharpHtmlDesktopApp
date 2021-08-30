namespace Api

module Request =

    open Api.Messages

    let handle config message =
        match Json.deserialize<ApiRequest> message with
        | Error ex ->
            Error (UnrecognizedRequest ex.Message)
        | Ok apiRequest ->
            ApiRequest.handle config apiRequest
        |> Json.serialize
