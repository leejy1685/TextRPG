using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextRPG
{
    class Monster
    {
        public int level { get; set; }
        public string name { get; set; }
        public int Atk { get; set; }
        public int Hp { get; set; }

        public Item item { get; set; }
        public int gold { get; set; }

        //몬스터 생성 시
        public Monster(int level, string name, int atk, int hp, Item item, int gold)
        {
            this.level = level;
            this.name = name;
            this.Atk = atk;
            this.Hp = hp;
            this.item = item;
            this.gold = gold;
        }

        public Monster(Monster other)
        {
            this.level = other.level;
            this.name = other.name;
            this.Atk = other.Atk;
            this.Hp = other.Hp;
            this.item = other.item;
            this.gold = other.gold;
        }

        public int MonsterDamage() //플에이어에게 가하는 피해 계산
        {
            Random random = new Random();

            float damagerandom = Atk * random.Next(9, 12) / 10.0f;
            int damage = (int)Math.Ceiling(damagerandom); // 랜덤값 올림 처리

            return damage;
        }

        public bool isCrit() // 치명타 발동 여부 체크
        {
            Random random = new Random(); // 랜덤 클래스 인스턴스 생성
            int critCheck = random.Next(1, 101);
            if (critCheck <= 15) // 치명타 발생 : 랜덤값이 1 ~ 15
            {
                return true;
            }
            else // 치명타 미발생 : 랜덤값이 16 ~ 100
            {
                return false;
            }
        }

        public bool isDie()
        {
            if (Hp <= 0)
                return true;
            return false;
        }

        public string monsterInfo()
        {
            string alive;
            if (isDie())
                alive = "Dead";
            else
                alive = Hp.ToString();

            return $"Lv.{level} {name} HP {alive}";
        }

        //몬스터 아이템 보상
        public Item dropItem()
        {
            Random rand = new Random();

            int num = rand.Next(1, 101);

            if (num <= 100)
                return item;
            else
                return null;
        }

        //몬스터 골드 보상
        public int goldDrop()
        {
            Random rand = new Random();

            int num = rand.Next(1, 101);

            if (num <= 100)
                return gold;
            else
                return 0;
        }

    }



}
