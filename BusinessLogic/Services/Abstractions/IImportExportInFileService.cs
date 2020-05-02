using System.Threading.Tasks;

namespace BusinessLogic.Services.Abstractions
{
    public interface IImportExportInFileService
    {
        Task<string> GenerateCsvString(string entityName);

        Task AddForecastingTaskFactorsViaCsv(string entityName, string csv);
    }
}
