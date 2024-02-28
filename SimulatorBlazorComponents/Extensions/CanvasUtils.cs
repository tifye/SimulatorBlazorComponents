using System.Numerics;
using GardenEditorModels.Models;

namespace SimulatorBlazorComponents.Extensions
{
    public class CanvasUtils
    {
        public static Vector2 FlipUnityPositionVertically(Vector2 v)
        {
            return new Vector2(v.X, v.Y * -1.0f);
        }

        public static Vector2 GetCartesianPoint(Vector2 v, Vector2 origo)
        {
            return v + origo;
        }

        public static Vector2 ConvertFromMeterToDecimeter(Vector2 v)
        {
            return Vector2.Multiply(v, 10.0f);
        }

        public static Vector2 ConvertFromDecimeterToMeter(Vector2 v)
        {
            return Vector2.Multiply(v, 0.1f);
        }

        public static Vector2 GetCanvasPosition(Vector2 v, Vector2 origo)
        {
            return GetCartesianPoint(ConvertFromMeterToDecimeter(FlipUnityPositionVertically(v)), origo);
        }

        public static Vector2 GetUnityPosition(Vector2 v, Vector2 origo)
        {
            var topLeftRelative = GetCartesianPoint(v, -1.0f * origo);
            var meterPosition = ConvertFromDecimeterToMeter(topLeftRelative);
            //return meterPosition;
            return FlipUnityPositionVertically(meterPosition);
        }

        public static Vector2 AddTransform(Transform2D t, Vector2 v)
        {
            var clockwiseRotationInRadian = DegreeToRadian(ChangeToClockwiseDirection(t.Rotation));

            //This order is important. First rotate then translate!
            var rotated = Rotate(v, clockwiseRotationInRadian);
            return Translate(rotated, t.Translation);
        }

        public static Vector2 RemoveTransform(Transform2D t, Vector2 v)
        {
            t.Rotation *= -1.0f;
            t.Translation *= -1.0f;

            var clockwiseRotationInRadian = DegreeToRadian(ChangeToClockwiseDirection(t.Rotation));

            //This order is important. First translate then rotate!
            var translated = Translate(v, t.Translation);
            return Rotate(translated, clockwiseRotationInRadian);
        }

        public static float ChangeToClockwiseDirection(float rotation)
        {
            return -1.0f * rotation;
        }
        public static double RadianToDegree(double rad)
        {
            return rad * 180.0f / Math.PI;
        }

        public static double DegreeToRadian(double deg)
        {
            return deg * Math.PI / 180.0f;
        }

        public static Vector2 Translate(Vector2 v, Vector2 translation)
        {
            return v + translation;
        }
        public static Vector2 Rotate(Vector2 v, double degrees)
        {
            return new Vector2(
                (float)(v.X * Math.Cos(degrees) - v.Y * Math.Sin(degrees)),
                (float)(v.X * Math.Sin(degrees) + v.Y * Math.Cos(degrees)));
        }
    }
}