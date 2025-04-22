using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Monster
    {
        public int level { get; set; }
        public string name{ get; set; }
        public int Atk { get; set; }
        public int Hp {  get; set; }
        public Monster(int level, string name,int atk,int hp)
        {
            this.level = level;
            this.name = name;
            this.Atk = atk;
            this.Hp = hp;
        }

        public void playergetdamage(Character player) // 플레이어가 입는 피해 계산
        {
            Random random = new Random();

            float damagerandom = player.Atk * random.Next(9, 10)/10.0f;
            int getdamage = (int)Math.Ceiling(damagerandom); // 랜덤값 올림 처리

            // ## 플레이어 체력 차감 ##
            // hp -= getdamage; // 체력에서 데미지 차감
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

    }



}
