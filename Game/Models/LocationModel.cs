namespace Game {
    public class LocationModel {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Background { get; set; } 
        public int Stage { get; set; }
        public ConnectionModel[] Connections { get; set; }
        public EnemyModel? Enemy { get; set; }
        public string? PuzzleKey {  get; set; }

        public List<ItemModel> Items { get; set; } = new List<ItemModel>();
    }
}
