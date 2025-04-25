using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Item
    {
        public string Name { get; }
        public int Type { get; }
        public int Value { get; }
        public string Desc { get; }
        public int Price { get; }

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
        }

        public Item(string name, int type, int value, string desc, int price)
        {
            Name = name;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
        }

        public Item()
        {

        }

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
        }

        //public string ItemInfoText() // 원본 메서드
        //{
        //    return $"{Name}  |  {DisplayTypeText} +{Value}  |  {Desc}";
        //}
    }
}
