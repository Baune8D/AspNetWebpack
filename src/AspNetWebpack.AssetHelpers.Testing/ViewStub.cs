using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace AspNetWebpack.AssetHelpers.Testing
{
    public class ViewStub : IView
    {
        public ViewStub(string view)
        {
            Path = view;
        }

        public string Path { get; }

        public Task RenderAsync(ViewContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
