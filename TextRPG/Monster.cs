//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace TextRPG
//{
//    class Monster
//    {
//        public int level { get; set; }
//        public string name{ get; set; }
//        public int Atk { get; set; }
//        public int Hp {  get; set; }

//        //몬스터 생성 시
//        public Monster(int level, string name,int atk,int hp)
//        {
//            this.level = level;
//            this.name = name;
//            this.Atk = atk;
//            this.Hp = hp;
//        }

//        public int MonsterDamage() //플에이어에게 가하는 피해 계산
//        {
//            Random random = new Random();

//            float damagerandom = Atk * random.Next(9, 12)/10.0f;
//            int damage = (int)Math.Ceiling(damagerandom); // 랜덤값 올림 처리

//            return damage;
//        }

//        //몬스터의 생존 여부
//        public bool isDie()
//        {
//            return Hp <= 0;
//        }

//        //현재 몬스터 정보 출력
//        public string monsterInfo()
//        {
//            string alive = isDie() ? "Dead" : Hp.ToString();

//           return $"Lv.{level} {name} HP {alive}";
//        }

//        //몬스터 회피 확률 계산
//        public bool isEvasion()
//        {
//            Random rand = new Random();

//            int num = rand.Next(1,101);

//            return num <= 10;
//        }

//    }



//}
