namespace Game {
    public class PlayerModel {
        public int PlayerID { get; set; }
        public int Hp { get; set; }
        public ItemModel[] Items { get; set; }
        public int CurrentLocationId { get; set; }
        public int Damage { get; set; }
        public int Resistance { get; set; }
    }
}
