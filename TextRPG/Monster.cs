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
        public int level { get; set; }//레벨
        public string name{ get; set; }//이름
        public int Atk { get; set; }//공격력
        public int Hp {  get; set; }//체력

        public Item item { get; set; }//드랍 아이템
        public int gold {  get; set; }//드랍 골드

        public Monster(int level, string name,int atk,int hp,Item item,int gold)
        {
            this.level = level;
            this.name = name;
            this.Atk = atk;
            this.Hp = hp;
            this.item = item;
            this.gold = gold;
        }//생성자

        public Monster(Monster other) 
        {
            this.level = other.level;
            this.name = other.name;
            this.Atk = other.Atk;
            this.Hp = other.Hp;
            this.item = other.item;
            this.gold = other.gold;
        }//보사 생성자

        public int MonsterDamage() 
        {
            Random random = new Random();

            float damagerandom = Atk * random.Next(9, 12)/10.0f;
            int damage = (int)Math.Ceiling(damagerandom); // 랜덤값 올림 처리

            return damage;
        }//플에이어에게 가하는 피해 계산

        public bool isDie()
        {
            return Hp <= 0;
        }//몬스터의 생존 여부

        public string monsterInfo()
        {
            string alive = isDie() ? "Dead" : Hp.ToString();

           return $"Lv.{level} {name} HP {alive}";
        }//현재 몬스터 정보 출력

        public bool isEvasion()
        {
            Random rand = new Random();

            int num = rand.Next(1,101);

            return num <= 10;
        }//몬스터 회피 확률 계산

        public Item dropItem()
        {
            Random rand = new Random();

            int num = rand.Next(1, 101);

            if (num <= 10)
                return item;
            else
                return null;
        }//몬스터 아이템 보상

        public int goldDrop()
        {
            Random rand = new Random();

            int num = rand.Next(1, 101);

            if (num <= 50)
                return gold;
            else
                return 0;
        }//몬스터 골드 보상

    }



}
