using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct TreeHideData : ITraitData, IEquatable<TreeHideData>
    {

        public bool Equals(TreeHideData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"TreeHide";
        }
    }
}
