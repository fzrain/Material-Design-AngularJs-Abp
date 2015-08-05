using System.Data.Entity.ModelConfiguration;
using Fzrain.Transcend.Core;

namespace Fzrain.Transcend.EntityFramework.Mapping
{
    class ChampionInfoMap : EntityTypeConfiguration<ChampionInfo>
    {
        public ChampionInfoMap()
        {
            ToTable("Lol_ChampionInfo");
            HasKey(b => b.Id);
           
        }
    }
}
