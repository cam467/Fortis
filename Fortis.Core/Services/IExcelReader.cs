namespace KnowBe4.Core.Services
{
    using System.Collections.Generic;
    using System.IO;

    public interface IExcelRepository
    {
        List<T> LoadExcelFile<T>(Stream data);
    }    
}