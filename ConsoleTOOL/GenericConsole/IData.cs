using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericConsole
{
   
    public interface ILink<D>
    {
        //void Init();
        void AddNode(D dataItem);
        void RemoveNode(D dataItem);
        void UpdateNode(D prevItem,D dataItem);
        int FindNode(D dataItem);
        Node<D> nextNode(Node<D> node);
        Node<D> GetHead();
        
    }
}
