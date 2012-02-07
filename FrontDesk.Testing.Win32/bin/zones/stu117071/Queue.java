/******************************************************************************
 *
 * This is an implementation of a simple queue structure, using
 * the built-in LinkedList class.   .
 *
 *
 * @author  Nalin Singal
 * @author	Arbob Danish Ahmad
 * @date    19 March 2005
 *****************************************************************************/


import java.util.*;

public class Queue extends LinkedList
{
   public Queue()
   {super();}
   
   public void enqueue(Object toEnqueue)
   {
     super.add(toEnqueue);
   }
   
   public Object dequeue()
   {
     return super.removeFirst();
   }
   
   public boolean add(Object toAdd)
   {return false;}
}
