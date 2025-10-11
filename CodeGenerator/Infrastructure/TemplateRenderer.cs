using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RazorLight;

namespace CodeGenerator.Infrastructure
{
    public class TemplateRenderer : ITemplateRenderer
    {
        private readonly RazorLightEngine _engine;

        public TemplateRenderer(string templatesRoot)
        {
            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(templatesRoot)
                .UseMemoryCachingProvider()
                .DisableEncoding()
                .Build();
        }

        public async Task<CompilationUnitSyntax> RenderAsync<T>(string templateName, T model)
        {
            var sourceCode = await _engine.CompileRenderAsync(templateName, model);
            var tree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilationUnitSyntax = (CompilationUnitSyntax)await tree.GetRootAsync();
            return compilationUnitSyntax;
        }
    }
    public interface ITemplateRenderer
    {
        public Task<CompilationUnitSyntax> RenderAsync<T>(string templateName, T model);
    }
}
