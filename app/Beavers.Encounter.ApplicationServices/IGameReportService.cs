using System.IO;
using Beavers.Encounter.Core;

namespace Beavers.Encounter.ApplicationServices
{
    public interface IGameReportService
    {
        Stream GetReport(User user);
    }
}