using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Weapon : Item
    {
        public int damage { get; set; }//무기만 가지고 있는 정보

        //무기 생성자
        public Weapon(string name, int damage, string showing, int price)
        {
            this.name = name;
            this.damage = damage;
            this.showing = showing;
            this.price = price;
        }
        public Weapon() { }

        //아이템 정보를 표시하는 오버라이드 메서드
        public override void itemInfo()
        {
            Console.WriteLine("{0}\t| 공격력 +{1}\t| {2}", name, damage, showing);
        }
        //아이템 가격을 표시하는 오버라이드 메서드
        //판매할 땐 원래 가격의 85퍼를 표시하기 때문에 이렇게 구현
        public override void itemInfo(string pri)
        {
            Console.WriteLine("{0}\t| 공격력 +{1}\t| {2}\t{3}", name, damage, showing, pri);
        }
    }
}
