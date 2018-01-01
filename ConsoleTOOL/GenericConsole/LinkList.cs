using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericConsole
{

    public class LinkList<D> : ILink<D>
    {
       

        private Node<D> head = null;
        public LinkList()
        {
            head = new Node<D>();
        }
        //public void Init()
        //{
        //    head = new Node<D>();
        //}

        public void AddNode(D dataItem)
        {
            Node<D> node = new Node<D>(dataItem);
            Node<D> Lnode = head;
            while (Lnode.next != null)
            {
                Lnode = Lnode.next;
            }
            Lnode.next = node;

        }

        public int FindNode(D dataItem)
        {
            Node<D> Lnode = head;
            int location = 1;
            while (!Lnode.next.data.Equals(dataItem))
            {
                Lnode = Lnode.next;
                if (Lnode.next == null)
                {
                    return -1;
                }
                location += 1;
            }
            return location;
        }
        public void RemoveNode(D dataItem)
        {
            if (head == null)
            {
                return;
            }
            Node<D> Lnode = head;
            while (Lnode.next != null)
            {
                if (Lnode.next.data.Equals(dataItem))
                {
                    Lnode.next = Lnode.next.next;
                    return;
                }
                Lnode = Lnode.next;
            }
        }

        public void UpdateNode(D prevItem, D dataItem)
        {
            if (head == null)
            {
                return;
            }
            Node<D> Lnode = head;
            while (Lnode.next != null)
            {
                if (Lnode.next.data.Equals(prevItem))
                {
                    Lnode.next.data = dataItem;
                }
                Lnode = Lnode.next;
            }
        }

        public Node<D> nextNode(Node<D> node)
        {
            return node.next;
        }
        public Node<D> GetHead()
        {
            return head;
        }
    }
}
