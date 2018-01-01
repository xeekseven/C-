using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericConsole
{
    //创建类。。把实现这个IData接口的类统一创建。
    //
    public class ListInvoke<T, D> where T : ILink<D>
    {
        private T _listData;
        public T ListData
        {
            get { return _listData; }
            set { _listData = value; }
        }
        public ListInvoke(T t)
        {
            ListData = t;
        }

        //public void Init()
        //{
        //    ListData.Init();
        //}
        public void Add(D dataItem)
        {
            ListData.AddNode(dataItem);
        }
        public void Remove(D dataItem)
        {
            ListData.RemoveNode(dataItem);
        }
        public void Update(D prevItem, D dataItem)
        {
            ListData.UpdateNode(prevItem, dataItem);
        }
        public int Find(D dataItem)
        {
            return ListData.FindNode(dataItem);
        }
        public void vistList(Action<D> action)
        {
            Node<D> node = ListData.GetHead();
            while (node.next != null)
            {
                node = ListData.nextNode(node);
                action(node.data);
            }
        }
    }
}
