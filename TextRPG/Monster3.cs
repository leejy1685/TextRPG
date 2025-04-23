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

        //몬스터 생성 시
        public Monster(int level, string name, int atk, int hp)
        {
            this.level = level;
            this.name = name;
            this.Atk = atk;
            this.Hp = hp;
        }

        public int MonsterGetDamage(int Atk) // 플레이어가 입는 피해 계산
        {
            Random random = new Random();

            float damagerandom = Atk * random.Next(9, 12) / 10.0f;
            int getdamage = (int)Math.Ceiling(damagerandom); // 랜덤값 올림 처리

            //김종보 오류 개선 방안 : 체력 감소 처리 추가
            Hp = Math.Max(0, Hp - getdamage);

            return getdamage;
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

    }



}
