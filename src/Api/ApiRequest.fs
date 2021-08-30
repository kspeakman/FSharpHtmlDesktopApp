namespace Api

module ApiRequest =

    open Api.Messages

    let handle config apiRequest =
        // TODO move case implementations to UMP style, will be async also
        match apiRequest with
        | Echo s ->
            Ok (Echoed s)
        | CloseApp ->
            config.Close.Invoke()
            Ok AppClosed

