﻿using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXEmitterDataSphereModel : AVFXEmitterData
    {
        public LiteralEnum<RotationOrder> RotationOrderType = new LiteralEnum<RotationOrder>("rotationOrder", "ROT");
        public LiteralEnum<GenerateMethod> GenerateMethodType = new LiteralEnum<GenerateMethod>("generateMethod", "GeMT");
        public LiteralInt DivideX = new LiteralInt("divideX", "DivX");
        public LiteralInt DivideY = new LiteralInt("divideY", "DivY");
        public AVFXCurve Radius = new AVFXCurve("radius", "Rads");

        List<Base> Attributes;

        public AVFXEmitterDataSphereModel(string jsonPath) : base(jsonPath, "Data")
        {
            Attributes = new List<Base>(new Base[] {
                RotationOrderType,
                GenerateMethodType,
                DivideX,
                DivideY,
                Radius
            });
        }

        public override void read(JObject elem)
        {
            Assigned = true;
            ReadJSON(Attributes, elem);
        }

        public override void read(AVFXNode node)
        {
            Assigned = true;
            ReadAVFX(Attributes, node);
        }

        public override JToken toJSON()
        {
            JObject elem = new JObject();
            PutJSON(elem, Attributes);
            return elem;
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode dataAvfx = new AVFXNode("Data");
            PutAVFX(dataAvfx, Attributes);
            return dataAvfx;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- DATA --------", new String('\t', level));
            Output(Attributes, level);
        }
    }
}