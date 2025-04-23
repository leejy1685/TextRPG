using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Skill
    {
        public string Name { get; } // 이름
        public int CostMP { get; } // 소모 MP
        public float Value { get; } // 공격력 배율
        public string Desc { get; } // 설명

        public Skill(string name, int costMp, float value, string desc) // 생성자 메서드
        {
            Name = name;
            CostMP = costMp;
            Value = value;
            Desc = desc;
        }

        public int skilldamage(Character player) // 스킬 데미지 계산
        {
            float result = player.Atk * Value; // 1차 결과 - 공격력 * 스킬 공격력 배율
            int damage = (int)Math.Ceiling(result); // 소숫점 올림 처리

            return damage;
        }

        public void skillInfo() // 스킬 설명
        {
            Console.WriteLine($"{Name} MP - {CostMP}\n   {Desc}");
        }
    }


}
