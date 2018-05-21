using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrintSample.DependencyServices
{
    public interface IPrintService
    {
        Task PrintAsync(string file);
    }
}
