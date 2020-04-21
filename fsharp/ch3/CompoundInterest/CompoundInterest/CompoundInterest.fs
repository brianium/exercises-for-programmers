// Copyright 2018-2019 Fabulous contributors. See LICENSE.md for license.
namespace CompoundInterest

open System.Diagnostics
open Fabulous
open Fabulous.XamarinForms
open Fabulous.XamarinForms.LiveUpdate
open Xamarin.Forms
open CompoundInterest.Domain
open CompoundInterest.Components

module App = 
    type Model = 
      { Goal: Goal
        InvestmentYears: int
        Investment: Investment }

    type Msg = 
        | UpdateInvestment of Investment
        | UpdateInvestmentYears of int
        | UpdateGoal of Goal

    let initModel = { Goal = Goal.defaultGoal; Investment = Investment.defaultInvestment; InvestmentYears = 0 }

    let init () = initModel, Cmd.none

    let update msg (model: Model) =
        match msg with
        | UpdateInvestment i -> { model with Investment = i }, Cmd.none
        | UpdateInvestmentYears y -> { model with InvestmentYears = y }, Cmd.none
        | UpdateGoal g -> { model with Goal = g }, Cmd.none

    let private parseFloat (defaultValue: float) (s: TextChangedEventArgs) =
        try double s.NewTextValue with
        | _ -> defaultValue

    let private parseInt (defaultValue: int) (s: TextChangedEventArgs) =
        try int s.NewTextValue with
        | _ -> defaultValue

    let view (model: Model) dispatch =
        View.TabbedPage(
            useSafeArea=true,
            children = [
                View.ContentPage(
                  title = "Compound Interest",
                  content = View.StackLayout(padding = Thickness 20.0, verticalOptions = LayoutOptions.Start,
                    children = [
                        title "Compound Interest"
                        defaultLabeledInput { Placeholder = "1500.00"; Value = model.Investment.Principal; Label = "Principal Amount"; OnChange = parseFloat model.Investment.Principal >> (fun x -> { model.Investment with Principal = x } |> UpdateInvestment) >> dispatch }
                        labeledInput { Placeholder = "4.3"; Value = model.Investment.InterestRate; Label = "Rate"; OnChange = parseFloat model.Investment.InterestRate >> (fun x -> { model.Investment with InterestRate = x / 100.0 } |> UpdateInvestment) >> dispatch } (fun x -> (x * 100.0).ToString("0.##"))
                        defaultLabeledInput { Placeholder = "6"; Value = model.InvestmentYears; Label = "Years"; OnChange = parseInt model.InvestmentYears >> UpdateInvestmentYears >> dispatch }
                        defaultLabeledInput { Placeholder = "4"; Value = model.Investment.CompoundFrequency; Label = "Number of Times Interest Compounds Annually"; OnChange = parseInt model.Investment.CompoundFrequency >> (fun x -> { model.Investment with CompoundFrequency = x } |> UpdateInvestment) >> dispatch }
                        investmentReturn model.InvestmentYears model.Investment
                    ]))
                View.ContentPage(
                  title = "Principal Calculator",
                  content = View.StackLayout(padding = Thickness 20.0, verticalOptions = LayoutOptions.Start,
                    children = [
                        title "Principal Calculator"
                        defaultLabeledInput { Placeholder = "1500.00"; Value = model.Goal.Value; Label = "Target Return"; OnChange = parseFloat model.Goal.Value >> (fun x -> { model.Goal with Value = x } |> UpdateGoal) >> dispatch }
                        labeledInput { Placeholder = "4.3"; Value = model.Goal.InterestRate; Label = "Rate"; OnChange = parseFloat model.Goal.InterestRate >> (fun x -> { model.Goal with InterestRate = x / 100.0 } |> UpdateGoal) >> dispatch } (fun x -> (x * 100.0).ToString("0.##"))
                        defaultLabeledInput { Placeholder = "4"; Value = model.Goal.CompoundFrequency; Label = "Number of Times Interest Compounds Annually"; OnChange = parseInt model.Goal.CompoundFrequency >> (fun x -> { model.Goal with CompoundFrequency = x } |> UpdateGoal) >> dispatch }
                        defaultLabeledInput { Placeholder = "4"; Value = model.Goal.Years; Label = "Years"; OnChange = parseInt model.Goal.Years >> (fun x -> { model.Goal with Years = x } |> UpdateGoal) >> dispatch }
                        returnInvestment model.Goal
                    ]))
            ]
        )

    // Note, this declaration is needed if you enable LiveUpdate
    let program = Program.mkProgram init update view

type App () as app = 
    inherit Application ()

    let runner = 
        App.program
#if DEBUG
        |> Program.withConsoleTrace
#endif
        |> XamarinFormsProgram.run app

#if DEBUG
    // Uncomment this line to enable live update in debug mode. 
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/tools.html#live-update for further  instructions.
    //
    //do runner.EnableLiveUpdate()
#endif    

    // Uncomment this code to save the application state to app.Properties using Newtonsoft.Json
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/models.html#saving-application-state for further  instructions.
#if APPSAVE
    let modelId = "model"
    override __.OnSleep() = 

        let json = Newtonsoft.Json.JsonConvert.SerializeObject(runner.CurrentModel)
        Console.WriteLine("OnSleep: saving model into app.Properties, json = {0}", json)

        app.Properties.[modelId] <- json

    override __.OnResume() = 
        Console.WriteLine "OnResume: checking for model in app.Properties"
        try 
            match app.Properties.TryGetValue modelId with
            | true, (:? string as json) -> 

                Console.WriteLine("OnResume: restoring model from app.Properties, json = {0}", json)
                let model = Newtonsoft.Json.JsonConvert.DeserializeObject<App.Model>(json)

                Console.WriteLine("OnResume: restoring model from app.Properties, model = {0}", (sprintf "%0A" model))
                runner.SetCurrentModel (model, Cmd.none)

            | _ -> ()
        with ex -> 
            App.program.onError("Error while restoring model found in app.Properties", ex)

    override this.OnStart() = 
        Console.WriteLine "OnStart: using same logic as OnResume()"
        this.OnResume()
#endif


