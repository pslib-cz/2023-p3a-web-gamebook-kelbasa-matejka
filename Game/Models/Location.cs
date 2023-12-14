namespace Game {
    public class Location {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; }
        public Action[] Actions { get; set; }
        public int Stage { get; set; }
        public Connection[] Connections { get; set; }
        public Enemy Enemy { get; set; }
    }
}
