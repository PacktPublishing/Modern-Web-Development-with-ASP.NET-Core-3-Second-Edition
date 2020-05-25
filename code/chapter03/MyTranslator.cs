using System.Threading.Tasks;

namespace chapter03
{
    internal class MyTranslator : ITranslator
    {
        public Task<string> Translate(string sourceLanguage, string term)
        {
            //does nothing, just return the original term
            return Task.FromResult(term);
        }
    }
}