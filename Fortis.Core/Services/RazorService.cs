namespace KnowBe4.Core.Services
{
    using Microsoft.Extensions.Hosting;
    using System.IO;
	using RazorEngine.Templating;
	using RazorEngine.Configuration;
    public class RazorService : IRazorService
	{
		private readonly IRazorEngineService _razor;
		public RazorService(IHostEnvironment env)
		{
			var config = new TemplateServiceConfiguration {
				TemplateManager = new ResolvePathTemplateManager(new[] { Path.Combine(env.ContentRootPath,"wwwroot","templates") })
			};
			// config.TemplateManager = new ResolvePathTemplateManager(new List<string>() {_global.templatesource, _global.templatesource + "/Email/"});
			this._razor = RazorEngineService.Create(config);
		}

		public string RunCompile(string templatename, object model = null)
		{
			return _razor.RunCompile(templatename, null, model,null);
		}
	}
}