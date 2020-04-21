namespace CompoundInterest

module Components =
    open Fabulous
    open Fabulous.XamarinForms
    open Xamarin.Forms
    open CompoundInterest.Domain

    type LabeledInputProps<'a> =
        { Placeholder: string
          Value: 'a
          Label: string
          OnChange: TextChangedEventArgs -> unit }

    let labeledInput (props: LabeledInputProps<'a>) format =
        View.StackLayout
            (children =
                [ View.Entry
                    (placeholder = props.Placeholder, text = format props.Value, keyboard = Keyboard.Numeric,
                     textChanged = props.OnChange)
                  View.Label(fontSize = FontSize 14., text = props.Label, margin = Thickness(0., 0., 0., 15.)) ])

    let defaultLabeledInput props = labeledInput props (fun x -> x.ToString())

    let title text =
        View.Label
            (fontSize = Named NamedSize.Title,
             horizontalTextAlignment = TextAlignment.Center,
             textColor = Color.RoyalBlue,
             text = text,
             fontAttributes = FontAttributes.Bold,
             margin = Thickness(0.,0.,0.,40.))

    type NoteProps =
        { Text: string }

    let note props = View.Label(fontSize = FontSize 12., textColor = Color.Goldenrod, text = props.Text)

    type Investment with
        member x.IsValid years = x.CompoundFrequency > 0 && x.InterestRate > 0.0 && x.Principal > 0.0 && years > 0

    type Goal with
        member x.IsValid = x.CompoundFrequency > 0 && x.InterestRate > 0.0 && x.Value > 0.0 && x.Years > 0

    let investmentReturn (years: int) (inv: Investment) =
        match inv.IsValid years with
        | false -> note { Text = "Provide positive investment values to view expected return" }
        | _ ->
            View.StackLayout
                (children =
                    [ View.Label
                        (horizontalTextAlignment = TextAlignment.Center, fontSize = Named NamedSize.Body,
                         fontFamily = "Helvetica", fontAttributes = FontAttributes.Italic,
                         margin = Thickness(0., 40., 0., 0.),
                         text =
                             sprintf "$%0.2f invested at %0.2f%% for %i years" inv.Principal
                                 (inv.InterestRate * 100.00) years)
                      View.Label
                          (horizontalTextAlignment = TextAlignment.Center, fontSize = FontSize 48.,
                           fontFamily = "Helvetica", fontAttributes = FontAttributes.Bold,
                           margin = Thickness(0., 10., 0., 0.),
                           text =
                               (Investment.calculateCompoundInterest years inv
                                |> (fun r -> r.Value)
                                |> decimal
                                |> (fun x -> x.ToString("C")))) ])

    let returnInvestment (goal: Goal) =
        match goal.IsValid with
        | false -> note { Text = "Provide positive goal values to view required initial investment" }
        | _ ->
            View.StackLayout
                (children =
                    [ View.Label
                        (horizontalTextAlignment = TextAlignment.Center, fontSize = Named NamedSize.Body,
                         fontFamily = "Helvetica", fontAttributes = FontAttributes.Italic,
                         margin = Thickness(0., 40., 0., 0.),
                         text =
                             sprintf "$%0.2f earned at %0.2f%% for %i years requires initial investment of" goal.Value
                                 (goal.InterestRate * 100.00) goal.Years)
                      View.Label
                          (horizontalTextAlignment = TextAlignment.Center, fontSize = FontSize 48.,
                           fontFamily = "Helvetica", fontAttributes = FontAttributes.Bold,
                           margin = Thickness(0., 10., 0., 0.),
                           text =
                               (Investment.calculateInitialInvestment goal
                                |> (fun i -> i.Principal)
                                |> decimal
                                |> (fun x -> x.ToString("C")))) ])
