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
        public int recDefence { get; set; }
        public int clearGold { get; set; }
        //클리어 확률 계산에 필요한 랜덤 클래스
        //이건 게임 매니저에 있어도 될거 같음
        public Random random { get; set; }

        //던전 생성자 
        public Dungeon(string name, int recDefence, int clearGold)
        {
            this.name = name;
            this.recDefence = recDefence;
            this.clearGold = clearGold;
            random = new Random();
        }

        //던전 시도, 권장 방어력보다 낮으면 40퍼 확률로 클리어
        //이 메서드는 Random 클래스 때문에 던전 클래스에 있으므로 개선의 여지 있음.
        public void tryDungeon(Player user)
        {
            if (user.defense < recDefence)
            {
                int rand = random.Next(1, 101);
                if (rand <= 40)
                {
                    failDungeon(user);
                }
                else
                {
                    clearDungeon(user);
                }
            }
            else
            {
                clearDungeon(user);
            }
        }

        //던전 클리어시 표시되는 창
        //인터페이스 역할을 하는 GameManager 클래스에서 하고 싶었으나
        //이전 정보와 변경된 정보를 표시해야 하기 때문에 이곳에 작성
        private void clearDungeon(Player user)
        {
            Console.Clear();

            Console.WriteLine("던전 클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine("{0}을 클리어 하셨습니다.\n", name);


            Console.WriteLine("[탐험 결과]");
            user.dungeonClear++; //level up 계산
            if (user.levelUp())
            {
                user.dungeonClear = 0;
                Console.WriteLine("Level {0} -> {1}", user.level - 1, user.level);
            }
            //유저가 받는 데미지 계산
            int def = user.defense - recDefence;
            int dunDamage = random.Next(20 + def, 35 + def);
            //0미만으로 못 내려가게 조정
            int userHp = user.hp - dunDamage;
            if (userHp < 0) userHp = 0;

            Console.WriteLine("체력 {0} -> {1}", user.hp, userHp);

            user.hp = userHp; //체력 반영

            //보상 계산
            int newGold = clearGold + (clearGold * random.Next(user.damage, user.damage * 2)) / 100;
            Console.WriteLine("Gold {0} G -> {1} G", user.gold, user.gold + newGold);

            user.gold += newGold;//보상 반영

            Console.WriteLine("\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요");
            Console.Write(">>");
            Console.ReadLine();

        }

        //던전 실패시 표시되는 창
        //클리어 메서드와 마찬가지로 아쉬움이 남음.
        private void failDungeon(Player user)
        {
            Console.Clear();

            Console.WriteLine("던전 실패");
            Console.WriteLine("{0}의 공략을 실패 하셨습니다.\n", name);

            Console.WriteLine("[탐험 결과]");
            Console.WriteLine("체력 {0} -> {1}", user.hp, user.hp / 2);

            user.hp /= 2;

            Console.WriteLine("\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요");
            Console.Write(">>");
            Console.ReadLine();
        }
    }
}
