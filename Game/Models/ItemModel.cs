namespace Game {
    public class ItemModel {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool Usable { get; set; } = false;
        public bool TargetIsEnemy { get; set; } = false;
        public EffectModel OnUseEffect { get; set; }
        public bool IsWearable { get; set; } = false;
        public string? WearableType { get; set; }
        public EffectModel OnWearEffect { get; set; }


    }
}
