using System.Text;
using System.Numerics;
using GardenEditorModels;
using GardenEditorModels.Converters;
using GardenEditorModels.Models;

namespace SimulatorBlazorComponents.Extensions
{
    public static class GardenConverterExtensions
    {
        public static Map MarshalMapFromJson(byte[] bytes)
        {
            var json = Encoding.UTF8.GetString(bytes);
            var map = MapManager.JsonContentToMap(json);
            foreach (var garden in map.Gardens)
            {
                if (garden.Areas.Count == 0)
                {
                    var res = GardenConverter.ConvertLoopWiresToAreasAndPaths(garden);
                    if (res != null)
                    {
                        garden.Areas = res.Areas;
                        garden.Paths = res.Paths;
                    }
                }
            }
            return map;
        }

        public static List<polygon> GetPolygons(Map map, bool isMowerCycle = false)
        {
            int index = 0;
            var cs = map.Gardens[0].ChargingStation;
            var areas = map.Gardens[0].Areas;
            var items = areas.Select(area => GetAreaPolygon(index++, area, cs, isMowerCycle)).ToList();
            var paths = map.Gardens[0].Paths;
            items.AddRange(paths.Select(path => GetPathPolygon(index++, path, cs, isMowerCycle)).ToList());
            return items;
        }

        private static polygon GetAreaPolygon(int id, BaseArea area, ChargingStation cs, bool isMowerCycle)
        {
            return new polygon()
            {
                id = id,
                heightIndex = GetHeightIndex(area.Type),
                color = GetAreaColor(area.Type, isMowerCycle),
                points = AddGardenTransform(area.PositionsAlongBoundary, cs).Select(p => new vec2(p.X, p.Y)).ToList()
            };
        }

        private static polygon GetPathPolygon(int id, BasePath path, ChargingStation cs, bool isMowerCycle)
        {
            var points = PolylineToPolygonConverter.GetPolygon(path.PositionsInPath, (float)path.Width);
            return new polygon()
            {
                id = id,
                heightIndex = 100,
                color = GetAreaColor(AreaType.TransportArea, isMowerCycle),
                points = AddGardenTransform(points, cs).Select(p => new vec2(p.X, p.Y)).ToList()
            };
        }

        private static List<Vector2> AddGardenTransform(List<Vector2> items, ChargingStation cs)
        {
            var t = cs.Transform;
            return items.Select(x => CanvasUtils.AddTransform(t, x)).ToList();
        }

        private static color GetAreaColor(AreaType type, bool isMowerCycle)
        {
            switch (type)
            {
                case AreaType.WorkingArea:
                    return isMowerCycle ? new color("#e0e0e0") : // Harmonic grey 300
                                          new color("#66bb6a"); //Harmonic green 400
                case AreaType.ForbiddenArea:
                    return isMowerCycle ? new color("#616161") : // Harmonic grey 700
                                          new color("#ef5350"); //Harmonic red 400
                case AreaType.TransportArea:
                    return isMowerCycle ? new color("#9e9e9e") : // Harmonic grey 500
                        new color("#42a5f5"); //Harmonic blue 300
                case AreaType.TriggerArea:
                    return new color(0.5f, 0.5f, 0.5f);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static int GetHeightIndex(AreaType type)
        {
            switch (type)
            {
                case AreaType.WorkingArea:
                    return 10;
                case AreaType.ForbiddenArea:
                    return 20;
                case AreaType.TransportArea:
                    return 50;
                case AreaType.TriggerArea:
                    return 75;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public class polygon
    {
        public int id;
        public int heightIndex;
        public List<vec2> points;
        public color color;

        public polygon()
        {
            points = new List<vec2>();
            color = new color(0, 0, 0);
        }
    }

    public class color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public color(float r, float g, float b, float a = 1f)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        //Create color from hex string
        public color(string hex)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            if (hex.Length == 3)
            {
                hex = hex[0] + "" + hex[0] + "" + hex[1] + "" + hex[1] + "" + hex[2] + "" + hex[2];
            }

            if (hex.Length != 6)
            {
                throw new System.Exception("Invalid hex string");
            }

            r = (float)System.Convert.ToInt32(hex.Substring(0, 2), 16) / 255f;
            g = (float)System.Convert.ToInt32(hex.Substring(2, 2), 16) / 255f;
            b = (float)System.Convert.ToInt32(hex.Substring(4, 2), 16) / 255f;
            a = 1f;
        }

        public color(System.Drawing.Color systemColor)
        {
            r = systemColor.R / 255f;
            g = systemColor.G / 255f;
            b = systemColor.B / 255f;
            a = systemColor.A / 255f;
        }
    }

    public class vec2
    {
        public float x;
        public float y;

        public vec2(Vector2 v)
        {
            x = v.X;
            y = v.Y;
        }

        public vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"x:{x}, y:{y}";
        }

    }
}