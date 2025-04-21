using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Item
    {
        //아이템이 공통으로 가지고 있는 정보
        public string name { get; set; }
        public string showing { get; set; }
        public int price { get; set; }

        //아이템 정보를 표시하는 가상 메서드
        public virtual void itemInfo() { }
        public virtual void itemInfo(string pri) { }
    }
}
