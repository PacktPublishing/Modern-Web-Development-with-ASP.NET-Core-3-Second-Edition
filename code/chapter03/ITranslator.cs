using System.Threading.Tasks;

namespace chapter03
{
    public interface ITranslator
    {
        Task<string> Translate(string sourceLanguage, string term);
    }
}
