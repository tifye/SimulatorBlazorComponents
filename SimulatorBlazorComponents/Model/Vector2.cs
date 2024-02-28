using Newtonsoft.Json;

namespace SimulatorBlazorComponents.Model
{
    public record class Vector2
    {
        [JsonProperty("x")]
        public float X;

        [JsonProperty("y")]
        public float Y;

        public Vector2(System.Numerics.Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Set(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}";
        }
    }
}