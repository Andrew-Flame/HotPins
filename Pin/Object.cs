namespace HotPins.Pin {
    /** The class on the basis of which pin objects will be created */
    class Object {
        private readonly string type;
        private readonly string name;

        public Object(string type, string name) {
            this.type = type;
            this.name = name;
        }

        public string getType() {
            return this.type;
        }

        public string getName() {
            return this.name;
        }
    }
}
