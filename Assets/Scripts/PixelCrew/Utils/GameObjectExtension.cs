﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtension
    {
        public static bool IsInLayer(this GameObject go, LayerMask layer)
        {
            return layer == (layer | 1 << go.layer);
        }
    }
}

