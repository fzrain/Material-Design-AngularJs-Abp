using System;
using Abp.Domain.Entities;

namespace Fzrain.Transcend.Core
{
    public class SnapShot : Entity<long>
    {
        public string FileUrl { get; set; }
        public int ChampionId { get; set; }
        public ActionType ActionType { get; set; }

        public int GameId { get; set; }
        public DateTime CommitTime { get; set; }
    }
    public  enum ActionType
    {
        TribleKill = 103,
        UltraKill = 104,
        PantaKill = 105,
        Legendary = 208
    }
}
