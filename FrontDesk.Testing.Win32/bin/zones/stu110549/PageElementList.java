/**
 *
 * This class dynamically creates objects associated with each web page element.
 *
 *
 *	It also implements an iterator over all page elements
 *
 * @author  V. Adamchik
 * @date		12/03/05
 * @see		WebReader
 *****************************************************************************

                 YOU DO NOT NEED TO IMPLEMENT ANYTHING THIS FILE

 *****************************************************************************/

import java.util.*;

public class PageElementList extends ArrayList
{
	public PageElementList()
	{
		super();
		//manual compilation
		new PageWord();
		new PageHref();
		new PageNum();
		new PageImg();
	}

 /**
	* Dynamic object creation
	*
	*/
	public void add(String str, String tagName)
	{
		PageElement obj;
		try
		{
			obj = (PageElement)((Class.forName(tagName)).newInstance());
			obj.setData(str);
			add(obj);
		}
		catch(Exception e)
		{
			//System.out.println(e);    //all invalid data will go here
		}
	}

 /**
 	*  Returns an iterator over a collection of nodes described
 	*  by a specified tag name
	*
	*/
	public Iterator iterator(String tagName)
	{
		return new PageElementListIterator(tagName);
	}

 /***************    Iterator      *************** */


	private class PageElementListIterator implements Iterator
	{
		private LinkedList bag;

	 /**
		*  Create a new empty iterator.
		*/
		public PageElementListIterator(String tagName)
		{
			bag = new LinkedList();
			Iterator arrayListItr = iterator();
			PageElement token;

			try
			{
				while(arrayListItr.hasNext())
				{
					token = (PageElement) arrayListItr.next();
					if(Class.forName(tagName).isInstance(token)) bag.add(token);
				}
			}
			catch(Exception e)
			{
				System.out.println("Class " + tagName + " is not found  " + e);
			}

		}

		/**
		*  Tests if there are more items of the same type
		*
		*/
		public boolean hasNext( )
		{
			return (!bag.isEmpty());
		}
		/**
		*  Returns the next item in the Hashtable.
		*
		*/
		public Object next( )
		{
			return bag.removeFirst();
		}

		/**
		*  Remove is not implemented
		*
		*/
		public void remove( )
		{
			throw new java.lang.UnsupportedOperationException();
		}
	}


}
