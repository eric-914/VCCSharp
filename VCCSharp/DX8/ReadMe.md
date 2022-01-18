Original program uses DirectX 8, which is COM based.  Ideally, would like to upgrade this to latest DirectX that plays nicely with C#/.NET.

TODO: 
* App.OnStartup(...)
      .Bind<IDDraw, DDraw>()
      .Bind<IDSound, DSound>()
      .Bind<IDInput, DInput>()
