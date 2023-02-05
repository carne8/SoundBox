module App.View

open App
open Fable.Core.Dart
open Flutter.Widgets
open Flutter.Material
open Flutter.Painting

let view (model: Model) (dispatch: Msg -> unit) (context: BuildContext) : Widget =
    Scaffold(
        body = CustomScrollView(
            slivers = [|
                SliverAppBar(
                    pinned = true,
                    expandedHeight = 200.0,
                    flexibleSpace = FlexibleSpaceBar(
                        title = Text("Sounds", style = TextStyle(fontSize = 35, color = Theme.of'(context).colorScheme.onBackground)),
                        centerTitle = true
                    )
                )
                SliverPadding(
                    padding = (EdgeInsets.only(top = 60.)),
                    sliver =
                        SliverFixedExtentList(
                            itemExtent = 60.,
                            ``delegate`` = SliverChildBuilderDelegate(
                                (fun context index ->
                                    Switch(
                                        value = model.Switch,
                                        onChanged = (Msg.ToggleSwitch >> dispatch)
                                    )
                                    :> Widget
                                    |> Some
                                    |> DartNullable.ofOption
                                )
                            )
                        )
                )
            |]
        ),
        floatingActionButton =
            FloatingActionButton.extended(
                icon = Icon Icons.plus_one,
                label = Text "Button",
                onPressed = (fun _ -> not model.Extended |> Msg.ToggleExtended |> dispatch),
                isExtended = model.Extended
            )
    )