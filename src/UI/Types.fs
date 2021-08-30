namespace Ui

[<AutoOpen>]
module Types =

    open Api.Messages

    type Msg =
        | Ping
        | ApiResponded of Result<ApiResponse, ApiError>
        | ApiReponseParseError of err: string * response: string
        | Exit

    type Effect =
        | SendToApi of ApiRequest

