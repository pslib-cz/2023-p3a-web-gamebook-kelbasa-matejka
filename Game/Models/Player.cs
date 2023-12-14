namespace Game {
    public class Player {
        public int PlayerID { get; set; }
        public int Hp { get; set; }
        public Item[] Items { get; set; }
        public int CurrentLocationId { get; set; }
        public int Damage { get; set; }
        public int Resistance { get; set; }
    }
}
