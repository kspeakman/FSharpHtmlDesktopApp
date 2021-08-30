namespace Api

module Json =

    open Newtonsoft.Json

    module Resolvers =

        open System.Reflection
        open Newtonsoft.Json.Serialization

        type RequireAllPropertiesResolver() =
            inherit DefaultContractResolver()

            override __.CreateProperty(memberInfo:MemberInfo, memberSerialization:MemberSerialization) =
                let property = base.CreateProperty(memberInfo, memberSerialization)
                let t =
                    match memberInfo with
                    | :? PropertyInfo as pi -> pi.PropertyType
                    | :? FieldInfo as fi -> fi.FieldType
                    | _ -> typeof<obj>

                // only option types can be, well, optional
                if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Option<_>> then
                    property.Required <- Required.Default
                else
                    property.Required <- Required.Always
                property


    module Internal =

        open Fable.Remoting.Json

        let Settings =
            let s = new JsonSerializerSettings()
            s.ContractResolver <- new Resolvers.RequireAllPropertiesResolver()
            s.Converters.Add(new FableJsonConverter())
            s.NullValueHandling <- NullValueHandling.Ignore
            // https://github.com/Zaid-Ajaj/Fable.Remoting/pull/110
            s.DateParseHandling <- DateParseHandling.None
            s


    let serialize a =
        JsonConvert.SerializeObject(a, Internal.Settings)


    let deserialize<'t> s =
        try
            let o = JsonConvert.DeserializeObject<'t>(s, Internal.Settings)
            if obj.ReferenceEquals(o, null) then failwith "Deserialization failed with empty object"
            Ok o
        with ex ->
            Error ex

