using System.Data.Entity.ModelConfiguration;
using Fzrain.Transcend.Core;

namespace Fzrain.Transcend.EntityFramework.Mapping
{
    class SnapShotMap: EntityTypeConfiguration<SnapShot>
    {
        public SnapShotMap()
        {
            ToTable("Lol_SnapShot");
            HasKey(b => b.Id);
        }
    }
}
