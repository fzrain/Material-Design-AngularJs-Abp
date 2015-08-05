using System.Data.Entity.ModelConfiguration;
using Fzrain.Transcend.Core;

namespace Fzrain.Transcend.EntityFramework.Mapping
{
    class BattleMap : EntityTypeConfiguration<Battle>
    {
        public BattleMap()
        {
            ToTable("Lol_Battle");
            HasKey(b => b.Id);
            Property(b => b.StartTime).HasColumnType("datetime2");
        }
    }
}
