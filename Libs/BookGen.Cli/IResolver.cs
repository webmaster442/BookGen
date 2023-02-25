using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Cli
{
    public interface IResolver
    {
        public T Resolve<T>();
    }
}
