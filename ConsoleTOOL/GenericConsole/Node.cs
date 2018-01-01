using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericConsole
{
    public class Node<D>
    {
        private Node<D> _next;
        private D _data;
        public Node<D> next
        {
            get { return _next; }
            set { _next = value; }
        }
        public D data
        {
            get { return _data; }
            set { _data = value; }
        }
        public Node() { }
        public Node(D dataItem)
        {
            _next = null;
            _data = dataItem;
        }

    }
}
