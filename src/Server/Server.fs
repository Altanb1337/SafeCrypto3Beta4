module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Microsoft.AspNetCore.Http

open FSharp.Control.Tasks
open Giraffe

open Shared

type Storage() =
    let todos = ResizeArray<_>()

    member __.GetTodos() = List.ofSeq todos

    member __.AddTodo(todo: Todo) =
        if Todo.isValid todo.Description then
            todos.Add todo
            Ok()
        else
            Error "Invalid todo"

let storage = Storage()

storage.AddTodo(Todo.create "Create new SAFE project")
|> ignore

storage.AddTodo(Todo.create "Write your app")
|> ignore

storage.AddTodo(Todo.create "Ship it !!!")
|> ignore

let todosApi =
    { getTodos = fun () -> async { return storage.GetTodos() }
      addTodo =
          fun todo ->
              async {
                  match storage.AddTodo todo with
                  | Ok () -> return todo
                  | Error e -> return failwith e
              }
      uploadFile = fun file -> async { return "Ok" } }

let webApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue todosApi
    |> Remoting.buildHttpHandler

let fileUploadHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            return!
                (match ctx.Request.HasFormContentType with
                 | false -> RequestErrors.BAD_REQUEST "Bad request"
                 | true ->
                     ctx.Request.Form.Files
                     |> Seq.fold (fun acc file -> sprintf "%s\n%s" acc file.FileName) ""
                     |> text)
                    next
                    ctx
        }

let combined =
    choose [ webApp
             route "/ping" >=> text "pong"
             route "/upload" >=> fileUploadHandler ]

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router combined
        memory_cache
        use_static "public"
        use_gzip
    }

run app
