namespace Retirement.Core

open System

type User =
    { CurrentAge: int
      RetirementAge: int }

type User with
    static member New (currentAge,retirementAge) = {CurrentAge = currentAge; RetirementAge = retirementAge}
    member user.YearsRemaining = user.RetirementAge - user.CurrentAge
    member user.RetirementYear fromYear = fromYear + user.YearsRemaining

