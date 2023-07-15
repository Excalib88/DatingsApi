using System.Text;

namespace Datings.Api.Common.Extensions;

public static class CodeGenerator
{
    public static string Generate(int count = 6)
    {
        var randomizer = new Random();
        var sb = new StringBuilder();

        for (var i = 0; i < count; i++)
        {
            sb.Append(randomizer.Next(0, 9));
        }
        
        return sb.ToString();
    }
}