using System.Text.RegularExpressions;

namespace ECommerce.Infrastructure.Helpers
{
    public partial class FileNameHelper
    {
        public static string CharacterRegulatory(string name)
           => RegularCharacterRegex().Replace(name, "").Replace(" ", "-");

        [GeneratedRegex("[\"!'^+%&/()=?_ @€¨~,;:.ÖöÜüıİğĞæßâîşŞÇç<>|]")]
        private static partial Regex RegularCharacterRegex();
    }
}
