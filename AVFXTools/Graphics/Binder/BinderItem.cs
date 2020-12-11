using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class BinderItem : GenericInstance
    {
        public AVFXBinder Binder;
        public Vector3 Pos;

        public BinderItem(AVFXBinder binder, WepModel model)
        {
            Binder = binder;
            CurrentTransform = Matrix4x4.Identity;
            if (model == null || !model.Valid)
            {
                return;
            }
            if (binder.PropStart.Assigned) // has property data
            {
                var bindPoint = binder.PropStart.BindPointId;
                if (bindPoint.Assigned && bindPoint.Value > -1 && model.BindPoints.ContainsKey(bindPoint.Value))
                {
                    var modelBind = model.BindPoints[bindPoint.Value];
                    Pos = modelBind.Point1;
                    CurrentTransform = Matrix4x4.CreateTranslation(Pos);
                }
            }
        }

        //=============
        public static BinderItem[] GetArray(List<AVFXBinder> binders, WepModel model)
        {
            BinderItem[] ret = new BinderItem[binders.Count];
            for (int idx = 0; idx < binders.Count; idx++)
            {
                ret[idx] = new BinderItem(binders[idx], model);
            }
            return ret;
        }
    }
}
