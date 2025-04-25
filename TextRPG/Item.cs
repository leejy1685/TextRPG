using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Item
    {
        public string Name { get; }//이름
        public int Type { get; }//아이템 타입 0 무기 1 방어구 2 포션
        public int Value { get; }//아이템의 값 공격력, 방어력, 회복량
        public string Desc { get; }//아이템 설명
        public int Price { get; }//가격

        public string DisplayTypeText
        {
            get
            {
                switch (Type)
                {
                    case 0:
                        return "공격력";
                        break;
                    case 1:
                        return "방어력";
                        break;
                    case 2:
                        return "회복량";
                        break;

                    default:
                        return "";
                        break ;
                }
            }
        }//타입을 텍스트 화

        public Item(string name, int type, int value, string desc, int price)
        {
            Name = name;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
        }//생성자

        public Item()
        {

        }//생성자

        public void ItemInfoText()
        {
            switch (Type)
            {
                case 0: // 무기일 경우
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(Name);
                    Console.ResetColor();
                    Console.Write($"  |  {DisplayTypeText} +{Value}  |  {Desc}");
                    break;

                case 1: // 방어구일 경우
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(Name);
                    Console.ResetColor();
                    Console.Write($"  |  {DisplayTypeText} +{Value}  |  {Desc}");
                    break;

                case 2: // 포션일 경우
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(Name);
                    Console.ResetColor();
                    Console.Write($"  |  {DisplayTypeText} +{Value}  |  {Desc}");
                    break;

                default: // 기본 값 - 색상 변환 X
                    Console.Write($"{Name}  |  {DisplayTypeText} +{Value}  |  {Desc}");
                    break;
            }
        }//아이템 정보를 출력하는 메서드

    }
}
