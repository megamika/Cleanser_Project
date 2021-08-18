using UnityEngine;
using Ludiq;
using Bolt.Addons.Community;

namespace DefinedEvents
{
    namespace Input
    {
        [IncludeInSettings(true)]
        public class Move : IDefinedEvent
        {
            public Vector3 value;
        }
    }
}
