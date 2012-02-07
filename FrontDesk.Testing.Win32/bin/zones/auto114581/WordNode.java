/******************************************************************************
 * helper class for Tree 
 * 
 *
 *
 * @author  Yun-Shang Chiou  ychiou@andrew   Yeonjoo Oh yeonjoo@hotmail.com
 * @date
 *****************************************************************************/

import java.util.*;
import java.net.*;//my import

public class WordNode implements java.io.Serializable
{
	
	public String word;
	public LinkedList webNodeList = new LinkedList();
	public WordNode leftChild ; //= null;
	public WordNode rightChild; // = null;
	public WordNode parent; // = null;
	
	
	//storing a Map (url -> freq) in each node of your tree
	//public HashMap map;
	
	/**
	 * 
	 */
	public WordNode() {
		String word;
		LinkedList webNodeList = new LinkedList();
	}

	/**
	 * @param node
	 * @return
	 */
	public int compareTo(WordNode node) {
		int x = word.compareTo(  node.word);   
		return x;
	}



	
}
