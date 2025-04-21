using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Store
    {
        //장비를 파는 상점
        public List<Item> items { get; set; }
        //판매 여부
        public bool[] sale { get; set; }

        //상점 생성자
        public Store()
        {
            items = new List<Item>();
            sale = new bool[20];
        }
        //상점에 아이템 추가하는 메서드
        public void addItem(Item item)
        {
            items.Add(item);
            sale[items.Count - 1] = false;
        }
        //상점의 아이템을 표시하는 메서드
        //인벤토리와 마찬가지로 타입에 따라 앞에 숫자를 표시
        public void showItems(bool type)
        {
            if (type)
            {
                for (int i = 0; i < items.Count; i++)
                {

                    string price = "";
                    if (sale[i]) price = "판매완료";
                    else price = items[i].price + "G";

                    Console.Write("- {0} ", i + 1);
                    items[i].itemInfo(price);
                }
            }
            else
            {
                foreach (Item item in items)
                {
                    string price = "";
                    if (sale[items.IndexOf(item)]) price = "판매완료";
                    else price = item.price + "G";

                    Console.Write("- ");
                    item.itemInfo(price);
                }
            }
        }

        //아이템을 판매할 때 
        public void buyItem(Player user, int num)
        {
            if (sale[num - 1])
            {//아이템의 구매 여부를 판단
                Console.WriteLine("이미 구매한 상품입니다.");
                Console.ReadLine();
            }
            else if (!sale[num - 1] && user.gold >= items[num - 1].price)
            {//유저가 소지한 골드가 충분하면 아이템 구매 성공
                user.gold -= items[num - 1].price;
                user.inventory.Add(items[num - 1]);
                sale[num - 1] = true;
                Console.WriteLine("구매를 완료했습니다.");
                Console.ReadLine();
            }
            else
            {//유저가 소지한 골드가 충분하지 않으면 구매 실패
                Console.WriteLine("골드가 부족합니다");
                Console.ReadLine();
            }

        }

        public void sellItem(Player user, int num)
        {
            //무기인지 방어구인지 판단 후 장비 해제
            if (user.inventory[num - 1].GetType() == typeof(Weapon))
            {
                user.unequip((Weapon)user.inventory[num - 1]);
            }
            else
            {
                user.unequip((Armor)user.inventory[num - 1]);
            }
            //골드 85퍼 환산
            user.gold += user.inventory[num - 1].price / 100 * 85;
            //다시 구매 가능
            for (int i = 0; i < items.Count; i++)
            {
                if (user.inventory[num - 1] == items[i])
                {
                    sale[i] = false;
                }
            }
            //인벤토리에서 제거
            user.inventory.RemoveAt(num - 1);

        }
    }
}
