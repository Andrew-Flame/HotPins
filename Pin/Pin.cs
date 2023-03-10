namespace HotPins.Pin; 

internal readonly struct Pin {
    /// <summary>Type of map pin</summary>
    public string Type { get; }
    /// <summary>Name of map pin</summary>
    public string Name { get; }

    public Pin(string type, string name) {
        Type = type;
        Name = name;
    }
}