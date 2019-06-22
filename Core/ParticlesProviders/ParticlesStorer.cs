using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ParticlesProviders
{
    public class ParticlesStorer : IObjectsProvider<ITickReceiver>, IObjectsWriter<ITickReceiver>
    {
        private readonly List<ITickReceiver> _receivers = new List<ITickReceiver>();


        public IEnumerable<ITickReceiver> Get()
        {
            return _receivers;
        }

        public void Put(IEnumerable<ITickReceiver> objects)
        {
            _receivers.AddRange(objects);
        }
    }
}
