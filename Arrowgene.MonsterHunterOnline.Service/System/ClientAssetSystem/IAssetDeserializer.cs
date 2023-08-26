using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem
{
    public interface IAssetDeserializer<T>
    {
        List<T> ReadPath(string path);
    }
}