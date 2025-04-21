using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Armor : Item
    {
        //설명이 무기와 같음
        public int defense { get; set; }    //방어구만 가지고 있는 정보
        public Armor(string name, int defense, string showing, int price)
        {
            this.name = name;
            this.defense = defense;
            this.showing = showing;
            this.price = price;
        }
        public Armor() { }
        public override void itemInfo()
        {
            Console.WriteLine("{0}\t| 방어력 +{1}\t| {2}", name, defense, showing);
        }
        public override void itemInfo(string pri)
        {
            Console.WriteLine("{0}\t| 방어력 +{1}\t| {2}\t{3}", name, defense, showing, pri);
        }
    }
}
