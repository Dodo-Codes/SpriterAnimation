using System.Collections;
using System.Diagnostics;

namespace SpriterAnimation;

using System.Xml;

public class EmptyNodeList : XmlNodeList
{
    public class EmptyNodeListEnumerator : IEnumerator
    {
        public bool MoveNext()
        {
            return false;
        }
        public void Reset()
        {
            throw new NotImplementedException();
        }
        public object Current => throw new NotImplementedException();
    }

    public override int Count => 0;
    public override XmlNode Item(int index)
    {
        throw new NotImplementedException();
    }
    public override IEnumerator GetEnumerator()
    {
        return new EmptyNodeListEnumerator();
    }
}

public class Base
{
    public string Name { get; set; } = "";
    public override string ToString()
    {
        return $"{GetType().Name} \"{Name}\"";
    }
}

public class Bone : Base
{
    public float Length { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Angle { get; set; }
    public float ScaleX { get; set; } = 1;
    public float ScaleY { get; set; } = 1;
}

public class BoneRef
{
    public int Parent { get; set; }
    public int Timeline { get; set; }
    public int Key { get; set; }
}

public class Timeline : Base
{
    public Dictionary<int, TimelineKey> TimelineKeys { get; } = new();
}

public class MainlineKey
{
    public int Id { get; set; }
    public int Time { get; set; }
    public Dictionary<int, BoneRef> BoneRefs { get; } = new();
}

public class TimelineKey
{
    public int Spin { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Angle { get; set; }
    public float ScaleX { get; set; } = 1;
    public float ScaleY { get; set; } = 1;
}

public class Animation : Base
{
    public Dictionary<int, MainlineKey> MainlineKeys { get; } = new();
    public Dictionary<int, Timeline> Timelines { get; } = new();
}

public class Entity : Base
{
    public Dictionary<string, Bone> Bones { get; } = new();
    public Dictionary<string, Animation> Animations { get; } = new();
}

public class AnimationScml
{
    private Stopwatch cycle = new();
    private int i;
    private readonly EmptyNodeList empty = new();
    private readonly Action<(float x1, float y1), (float x2, float y2)> drawCallback;
    public Dictionary<string, Entity> Entities { get; } = new();

    public AnimationScml(string scmlPath,
        Action<(float x1, float y1), (float x2, float y2)> drawCallback)
    {
        cycle.Start();
        this.drawCallback = drawCallback;
        var doc = new XmlDocument();
        doc.Load(scmlPath);

        var entityNodes = doc.SelectNodes("spriter_data/entity") ?? empty;
        foreach (XmlElement entity in entityNodes)
        {
            var entityName = entity.GetAttribute("name");

            Entities[entityName] = new() { Name = entityName };
            var boneNodes = entity.SelectNodes("obj_info") ?? empty;

            foreach (XmlElement boneNode in boneNodes)
            {
                if (boneNode.GetAttribute("type") != "bone")
                    continue;

                var boneRealName = boneNode.GetAttribute("realname");
                var boneName = boneRealName == "" ? boneNode.GetAttribute("name") : boneRealName;
                Entities[entityName].Bones[boneName] = new()
                {
                    Name = boneName,
                    Length = GetFloat(boneNode, "w") ?? 0f
                };
            }

            ParseAnimation(Entities[entityName], entity.SelectNodes("animation") ?? empty);
        }
    }
    private void ParseAnimation(Entity entity, XmlNodeList animationNodes)
    {
        var animations = entity.Animations;
        foreach (XmlElement animation in animationNodes)
        {
            var animName = animation.GetAttribute("name");
            var mainlineNode = animation.SelectSingleNode("mainline");

            if (mainlineNode == null)
                continue;

            animations[animName] = new() { Name = animName };

            var mainlineKeyNodes = mainlineNode.SelectNodes("key") ?? empty;
            var timelineNodes = animation.SelectNodes("timeline") ?? empty;
            var mainlineKeys = animations[animName].MainlineKeys;
            var timelines = animations[animName].Timelines;

            foreach (XmlElement mainlineKeyNode in mainlineKeyNodes)
            {
                var boneRefNodes = mainlineKeyNode.SelectNodes("bone_ref") ?? empty;
                var mainlineKeyId = GetInt(mainlineKeyNode, "id") ?? -1;

                mainlineKeys[mainlineKeyId] = new()
                {
                    Id = mainlineKeyId,
                    Time = GetInt(mainlineKeyNode, "time") ?? 0
                };

                foreach (XmlElement boneRefNode in boneRefNodes)
                {
                    var boneId = int.Parse(boneRefNode.GetAttribute("id"));
                    var hasParent = int.TryParse(boneRefNode.GetAttribute("parent"), out var parent);
                    var timeline = int.Parse(boneRefNode.GetAttribute("timeline"));
                    var key = int.Parse(boneRefNode.GetAttribute("key"));

                    mainlineKeys[mainlineKeyId].BoneRefs[boneId] = new()
                    {
                        Parent = hasParent ? parent : -1,
                        Timeline = timeline,
                        Key = key
                    };
                }
            }

            foreach (XmlElement timelineNode in timelineNodes)
            {
                var timelineId = GetInt(timelineNode, "id") ?? -1;
                var timelineName = timelineNode.GetAttribute("name");
                var timelineKeyNodes = timelineNode.SelectNodes("key") ?? empty;

                timelines[timelineId] = new() { Name = timelineName };
                foreach (XmlElement timelineKeyNode in timelineKeyNodes)
                {
                    if (timelineKeyNode.SelectSingleNode("bone") is not XmlElement timelineBoneNode)
                        continue;

                    var timelineKeyId = GetInt(timelineKeyNode, "id") ?? -1;

                    timelines[timelineId].TimelineKeys[timelineKeyId] = new()
                    {
                        Spin = GetInt(timelineKeyNode, "spin") ?? -1,
                        X = GetFloat(timelineBoneNode, "y") ?? 0f,
                        Y = GetFloat(timelineBoneNode, "x") ?? 0f,
                        Angle = GetFloat(timelineBoneNode, "angle") ?? 0f,
                        ScaleX = GetFloat(timelineBoneNode, "scale_x") ?? 1f,
                        ScaleY = GetFloat(timelineBoneNode, "scale_y") ?? 1f
                    };
                }
            }
        }
    }

    public void Update()
    {
        if (cycle.Elapsed.TotalSeconds > 0.2f)
        {
            cycle.Restart();
            i++;
            if (i == 4) i = 0;
        }

        foreach (var entity in Entities)
        {
            var animation = entity.Value.Animations["Idle_1"];
            var frame = animation.MainlineKeys[i];
            var timelines = animation.Timelines;
            var boneRefs = frame.BoneRefs;

            foreach (var b in boneRefs)
            {
                var timeline = timelines[b.Value.Timeline];
                var parentTimeline = b.Value.Parent == -1 ? null : timelines[b.Value.Parent];
                var boneName = timeline.Name;
                var parentName = parentTimeline == null ? "" : parentTimeline.Name;
                var bone = entity.Value.Bones[boneName];

                var key = timeline.TimelineKeys[b.Value.Key];
                bone.X = key.X;
                bone.Y = key.Y;
                bone.Angle = key.Angle;

                if (parentTimeline != null)
                {
                    var parent = entity.Value.Bones[parentName];
                    var parTip = GetTip((parent.X, parent.Y), parent.Angle, parent.Length,
                        (parent.ScaleX, parent.ScaleY));

                    bone.X = parTip.x;
                    bone.Y = parTip.y;
                    bone.Angle += parent.Angle;
                }

                var tip = GetTip((bone.X, bone.Y), bone.Angle, bone.Length,
                    (bone.ScaleX, bone.ScaleY));
                drawCallback.Invoke((bone.X, bone.Y), tip);
            }
        }
    }

    public void PrintBoneHierarchy()
    {
        foreach (var entity in Entities)
        {
            Console.WriteLine($"Entity Name: {entity.Key}");
            foreach (var animation in entity.Value.Animations)
            {
                Console.WriteLine($" Animation Name: {animation.Key}");

                var frame = animation.Value.MainlineKeys[0];
                var timelines = animation.Value.Timelines;
                foreach (var data in frame.BoneRefs)
                {
                    var timeline = timelines[data.Value.Timeline];
                    var boneName = timeline.Name;
                    var parentName = data.Value.Parent == -1 ? "" : timelines[data.Value.Parent].Name;

                    Console.WriteLine($"  Bone: {boneName}, Parent: {parentName}");
                }
            }
        }
    }

    private static int? GetInt(XmlElement element, string attribute)
    {
        var value = element.GetAttribute(attribute);
        return value == "" ? null : int.Parse(value);
    }
    private static float? GetFloat(XmlElement element, string attribute)
    {
        var value = element.GetAttribute(attribute);
        return value == "" ? null : float.Parse(value);
    }
    private (float x, float y) GetTip((float x, float y) position, float angle, float length,
        (float x, float y) scale)
    {
        var wrap = (angle % 360 + 360) % 360;
        var rad = MathF.PI / 180f * wrap;
        var (dirX, dirY) = (MathF.Cos(rad), MathF.Sin(rad));
        var tipX = position.x + dirX * length * scale.x;
        var tipY = position.y - dirY * length * scale.y;
        return (tipX, tipY);
    }
}