
using TwoBrainsGames.BrainEngine.Data.DataFiles;
namespace TwoBrainsGames.Snails
{
    interface ISnailsDataFileSerializable : IDataFileSerializable
    {
        DataFileRecord ToDataFileRecord(ToDataFileRecordContext context);
    }
}
