namespace Paint

open Paint.Domain

type State = 
      { Room: Room }

type Msg = 
    | UpdateRoom of Room
        