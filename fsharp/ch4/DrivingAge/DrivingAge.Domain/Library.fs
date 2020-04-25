namespace DrivingAge.Domain

type Country =
    { Name: string
      LegalDrivingAge: int }

module Country =
    let create name age =
        { Name = name
          LegalDrivingAge = age }

    let knownCountries =
        [ create "USA" 16
          create "Argentina" 17
          create "Honduars" 18
          create "France" 15 ]

module Driving =
    open Country;

    let getLegalCountries age =
        List.filter (fun { LegalDrivingAge = l } -> l <= age) knownCountries
