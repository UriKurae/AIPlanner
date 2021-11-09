using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct RobberData : ITraitData, IEquatable<RobberData>
    {
        public System.Boolean Ready2Steal;
        public System.Boolean CopAway;
        public System.Boolean Stolen;

        public bool Equals(RobberData other)
        {
            return Ready2Steal.Equals(other.Ready2Steal) && CopAway.Equals(other.CopAway) && Stolen.Equals(other.Stolen);
        }

        public override string ToString()
        {
            return $"Robber: {Ready2Steal} {CopAway} {Stolen}";
        }
    }
}
