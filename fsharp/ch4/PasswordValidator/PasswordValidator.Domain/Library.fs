namespace PasswordValidator.Domain

module Password =
    open System
    open BCrypt.Net

    let randomSalt() = BCrypt.GenerateSalt 12

    let hash str = BCrypt.HashPassword(str, randomSalt())

    let validate check hash = BCrypt.Verify(check, hash)

module Users =
    open Password

    let passwords =
        dict
            [ hash "secret", "Brian"
              hash "password", "John"
              hash "P@ssw0rd!", "Ishmel" ]

    let lookup password = 
        try
            Seq.find (fun k -> validate password k) passwords.Keys
            |> function
               | x -> passwords.Item x |> Some
        with
        | _ -> None
