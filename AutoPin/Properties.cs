using UnityEngine;

namespace HotPins.AutoPin; 

internal struct Properties {
    public KeyCode Key { get; set; }
    public int Radius { get; set; }
    public int SqrtRadius => Radius * Radius;
    public string Type { get; set; }
    public bool Enabled { get; set; }
    public string[] Names { get; }

    public Properties(params string[] names) => Names = names;
}