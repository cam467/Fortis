namespace Fortis.Core.Services
{
    public interface IRazorService
    {
        string RunCompile(string templatename, object model = null);
    }
}