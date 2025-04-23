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
        public string name{ get; set; }
        public int Atk { get; set; }
        public int Hp {  get; set; }

        public Item item { get; set; }
        public int gold {  get; set; }

        //몬스터 생성 시
        public Monster(int level, string name,int atk,int hp,Item item,int gold)
        {
            this.level = level;
            this.name = name;
            this.Atk = atk;
            this.Hp = hp;
            this.item = item;
            this.gold = gold;
        }

        public int MonsterDamage() //플에이어에게 가하는 피해 계산
        {
            Random random = new Random();

            float damagerandom = Atk * random.Next(9, 12)/10.0f;
            int damage = (int)Math.Ceiling(damagerandom); // 랜덤값 올림 처리

            return damage;
        }

        //몬스터의 생존 여부
        public bool isDie()
        {
            return Hp <= 0;
        }

        //현재 몬스터 정보 출력
        public string monsterInfo()
        {
            string alive = isDie() ? "Dead" : Hp.ToString();

           return $"Lv.{level} {name} HP {alive}";
        }

        //몬스터 회피 확률 계산
        public bool isEvasion()
        {
            Random rand = new Random();

            int num = rand.Next(1,101);

            return num <= 10;
        }

        //몬스터 아이템 보상
        public Item dropItem()
        {
            Random rand = new Random();

            int num = rand.Next(1, 101);

            if (num <= 10)
                return item;
            else
                return null;
        }

        //몬스터 골드 보상
        public int goldDrop()
        {
            Random rand = new Random();

            int num = rand.Next(1, 101);

            if (num <= 50)
                return gold;
            else
                return 0;
        }

    }



}
