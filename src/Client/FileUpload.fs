module FileUpload

open Fable.React
open Fable.React.Props
open Fable.FontAwesome
open Fable.Core
open Fable.Core.JsInterop

open Fulma

let handleFileEvent onLoad (fileEvent: Browser.Types.Event) =
    let files : Browser.Types.FileList = !!fileEvent.target?files

    if files.length > 0 then
        let reader = Browser.Dom.FileReader.Create()
        reader.onload <- (fun _ -> reader.result |> unbox |> onLoad)
        reader.readAsArrayBuffer (files.[0])


let createFileUpload onLoad =
    File.file [] [
        File.label [] [
            File.input [
                Props [
                    OnChange(handleFileEvent onLoad)
                ]
            ]
            File.cta [] [ str "Select File" ]
        ]
    ]


let createFileUpload2 onLoad =
    File.file [] [
        File.label [] [
            File.input [
                Props [
                    OnChange
                        (fun ev ->
                            let file = ev.target?files?(0)
                            let reader = Browser.Dom.FileReader.Create()

                            reader.onload <- fun evt -> evt.target?result |> onLoad

                            // reader.onerror <- fun evt ->
                            //     dispatch ErrorReadingFile

                            reader.readAsArrayBuffer (file)




                            )
                ]
            ]
            File.cta [] [ str "Select File" ]
        ]
    ]
