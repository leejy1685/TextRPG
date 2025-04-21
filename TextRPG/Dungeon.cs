using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Dungeon
    {
        //던전의 이름, 권장 방어력, 클리어 골드
        public string name { get; set; }
        public int recDef { get; set; }
        public int clearGold { get; set; }
        //클리어 확률 계산에 필요한 랜덤 클래스
        //이건 게임 매니저에 있어도 될거 같음
        public Random random { get; set; }

        //던전 생성자 
        public Dungeon(string name, int recDef, int clearGold)
        {
            this.name = name;
            this.recDef = recDef;
            this.clearGold = clearGold;
            random = new Random();
        }

        public string DungeonInfo()
        {
            return $"{name}\t| 방어력 {recDef} 이상 권장";
        }

        public int dungeonDamage(Character player)
        {
            //유저가 받는 데미지 계산
            int def = player.Def - recDef;
            int dunDamage = random.Next(20 + def, 35 + def);
            //0미만으로 못 내려가게 조정
            int userHp = player.Hp - dunDamage;
            if (userHp < 0) userHp = 0;

            return userHp;
        }

        //던전 클리어 시 골드 계산
        public int DungeonClearGold(Character player)
        {
            return clearGold + (clearGold * random.Next(player.Atk, player.Atk * 2)) / 100;
        }

    }
}
