using System.Threading.Tasks;

namespace XmlDataExtractManager.Interfaces
{
    public interface IXmlDataExtractorService
    {
        Task ProcessXmlAsync(string xmlfile);
    }
}
