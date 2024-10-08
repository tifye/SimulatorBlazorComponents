A proof of concept I made in my free time for my work at Husqvarna Robotics R&D. The background is we use Unity WebGL to present a 3D live digi-double environment in the browser. The whole workflow between Blazor and Unity was painful and I wanted to make it easier and better. This project inspired by my work on https://github.com/tifye/react-aseprite and after learning about how the Reacter renderer works, and about React Three Fiber. We went on to making full use of this in out internal tools. This project was the inspiration for the follow up [BlazorThreeJs](https://github.com/tifye/BlazorThreeJs/tree/master).

Here I utilize the lifecycle methods of Blazor components to bind/map to out GameObjects in Unity. 

Sample
```razor
<GardenSimulator>
    @if (GardenMap is not null)
    {
        <Garden Map="@GardenMap">
            <Mower Platform="P3" Rotation="@MowerRotation" X="MowerX" Y="MowerY" />
            <ChargingStation Rotation="@ChargingStationRotation" Y="@ChargingStationY" />
        </Garden>
    }
</GardenSimulator>

@code {
    // ...

    protected override void OnInitialized()
    {
        base.OnInitialized();

        UnityInstanceLoader.UnityInstanceLoaded += UnityInstanceLoader_UnityInstanceLoaded;

        _stopwatch.Start();
        _timer = new Timer(_ =>
        {
            var angleIncr = 360 / 15f;
            MowerRotation = (float)(Math.Atan2(MowerY - 25, MowerX) * 180 / (float)Math.PI) - 90 - angleIncr;
            MowerX = (float)Math.Sin(_stopwatch.Elapsed.TotalSeconds) * 2;
            MowerY = (float)Math.Cos(_stopwatch.Elapsed.TotalSeconds) * 2;

            MowerY += 25;

            ChargingStationRotation = (ChargingStationRotation - angleIncr / 3) % 360;
            InvokeAsync(StateHasChanged);
        }, null, 0, 200);
    }

    // ...
}
```
