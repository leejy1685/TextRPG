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
        string name{ get; set; }
        int Atk { get; set; }
        int Hp {  get; set; }

        //몬스터 생성 시
        public Monster(int level, string name,int atk,int hp)
        {
            this.level = level;
            this.name = name;
            this.Atk = atk;
            this.Hp = hp;
        }

        public void MonsterGetDamage(Character player) // 플레이어가 입는 피해 계산
        {
            Random random = new Random();

            float damagerandom = player.Atk * random.Next(9, 12)/10.0f;
            int getdamage = (int)Math.Ceiling(damagerandom); // 랜덤값 올림 처리

            // ## 플레이어 체력 차감 ##
            Hp -= getdamage; // 체력에서 데미지 차감
            if (Hp <= 0) // 만약 체력이 0 이하라면
            {
                Hp = 0; // 체력을 0으로 설정 - 사망
            }
        }

        public bool isDie()
        {
            if (Hp == 0)
                return true;
            return false;
        }

        public string monsterInfo()
        {
            return $"Lv.{level} {name} HP {Hp}";
        }

    }



}
