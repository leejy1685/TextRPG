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

        public Skill(string name, int costMp, float value, string desc) 
        {
            Name = name;
            CostMP = costMp;
            Value = value;
            Desc = desc;
        }// 생성자

        public void skillInfo()
        {
            Console.WriteLine($"{Name} MP - {CostMP}\n   {Desc}");
        }// 스킬 설명
    }


}
