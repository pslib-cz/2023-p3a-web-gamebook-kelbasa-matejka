namespace Game {
    public class ConnectionModel {
        public string Description { get; set; }
        public int FromLocationID { get; set; }
        public int ToLocationID { get; set; }
        public ItemModel RequiredItem { get; set; }
        public EffectModel Effect { get; set; }
        public bool Locked { get; set; } = false;
    }
}
