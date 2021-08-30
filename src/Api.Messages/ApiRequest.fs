namespace Api.Messages

type ApiRequest =
    | Echo of string
    | CloseApp

type ApiError =
    | UnrecognizedRequest of string

type ApiResponse =
    | Echoed of string
    | AppClosed

