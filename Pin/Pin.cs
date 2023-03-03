namespace HotPins.Pin; 

/** The class on the basis of which pin objects will be created */
class Pin {
    private readonly string _type;
    private readonly string _name;

    public Pin(string type, string name) {
        this._type = type;
        this._name = name;
    }

    public string GetPinType() {
        return this._type;
    }

    public string GetPinName() {
        return this._name;
    }
}