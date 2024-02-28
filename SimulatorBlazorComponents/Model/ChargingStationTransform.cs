namespace SimulatorBlazorComponents.Model
{
    public record class ChargingStationTransform
    {
        public Vector2 Position;
        public float Rotation;

        public ChargingStationTransform(float rotation, Vector2 position)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}