module FileUpload

open Fable.Core.JsInterop
open Feliz

open Browser.Types

let handleFileEvent (onLoad: Browser.Types.File -> unit) (fileEvent: Browser.Types.Event) =
    let files : FileList = !!fileEvent.target?files

    if files.length > 0 then
        let fileName =
            Browser.Dom.document.querySelector ".file-name"

        fileName.textContent <- files.[0].name
        onLoad (files.[0])

let fileUpload text handle =
    Html.div [
            prop.className [
                "file"
                "has-name"
                "is-fullwidth"
            ]
            prop.children [
                Html.label [
                    prop.className "file-label"
                    prop.children [
                        Html.input [
                            prop.className "file-input"
                            prop.type' "file"
                            prop.onChange (handleFileEvent handle)
                        ]
                        Html.span [
                            prop.className "file-cta"
                            prop.children [
                                Html.span [
                                    prop.className "file-icon"
                                    prop.children [
                                        Html.i [
                                            prop.className [ "fa"; "fa-upload" ]
                                        ]
                                    ]
                                ]
                                Html.span [
                                    prop.className "file-label"
                                    prop.children [
                                        Html.text "Choose a file..."
                                    ]
                                ]
                            ]
                        ]
                        Html.span [
                            prop.className "file-name"
                            prop.children [ Html.text "" ]
                        ]
                    ]
                ]
            ]
        ]