namespace SimulatorBlazorComponents.Model
{
    public record class MowerTransform
    {
        public string State;
        public float Heading;
        public Vector2 Position;

        public MowerTransform(float heading, Vector2 position)
        {
            State = string.Empty;
            Heading = heading;
            Position = position;
        }
    }
}