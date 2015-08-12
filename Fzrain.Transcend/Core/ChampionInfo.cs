using Abp.Domain.Entities;

namespace Fzrain.Transcend.Core
{
    public class ChampionInfo : Entity
    {
        public int ChampionId { get; set; }
        public string EnglishName { get; set; }
        public string PreCnName { get; set; }
        public string ChineseName { get; set; }
        public string NickName { get; set; }
        public string Position { get; set; }
        public string SecondPosition { get; set; }
        public int GoldPrice { get; set; }
        public int PointTicket { get; set; }


    }
}
