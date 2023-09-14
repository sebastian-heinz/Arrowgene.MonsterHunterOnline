using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service;

/// <summary>
///  this is a temporary holder for central management of packet and information,
/// it can be considered the "playground" for now.
///
/// On*- function are lifecycle hooks
/// Send*- functions are to send specific data that has been consistently populated
/// </summary>
public class PlayerState
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(PlayerState));

    private Client _client;
    public static Server Server;
    public int levelId { get; set; }
    public int prevLevelId { get; set; }
    public CSVec3 Position { get; set; }
    public int MainInstanceLevelId { get; set; }
    public bool SelectRoleTrigger { get; set; }

    public CSVec3 InitSpawnPos = new CSVec3()
//  {
//      x = 1588.4813f,
//      y = 1593.0623f,
//      z = 142.93517f
//  };
    {
        x = 404.91379f,
        y = 396.74976f,
        z = 85.0f
    };

    public int InitLevelId = 150101;


    public PlayerState(Client client)
    {
        _client = client;
    }
}