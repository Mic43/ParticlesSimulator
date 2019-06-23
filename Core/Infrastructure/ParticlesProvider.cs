using System;
using System.Collections.Generic;

namespace Core.ParticlesProviders
{
    public class ParticlesProvider : IObjectsProvider<ITickReceiver>
    {
        private readonly IEnumerable<ITickReceiver> _receivers;

        public ParticlesProvider(IEnumerable<ITickReceiver> receivers)
        {
            this._receivers = receivers ?? throw new ArgumentNullException(nameof(receivers));
        }

        public IEnumerable<ITickReceiver> Get()
        {
            return _receivers;
        }
    }
}