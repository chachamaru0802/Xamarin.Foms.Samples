using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Samples.DependencyServices
{
    public interface IPrintService
    {
        Task PrintAsync(string file);
    }
}
