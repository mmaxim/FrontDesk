/**
 ******************************************************************************
 *                    HOMEWORK 4, 15-211
 ******************************************************************************
 *                    Part 3: The Web Indexer
 ******************************************************************************
 *
 * Implementation of a dictionary via either AVL or splay tree
 *
 * This class provides a main program that allows users to search a web
 * site for keywords.  It essentially uses the index structure generated
 * by WebCrawler.
 *
 * To run this, type the following:
 *
 *    % java SearchDriver indexfile keyword1 [keyword2] [keyword3] ...
 *
 * where indexfile is a file containing a saved index created by WebIndex.
 *
 * @author    Yeonjoo  Oh              Yun-Shang Chiou
 * @date
 * @see		WebSearch
 * @see		Dictionary
 * @see		Tree
 *****************************************************************************/


import java.io.*;
import java.net.*;
import java.util.*;

public class WebIndexer implements Dictionary
{
	private Tree dictionary;
	public Graph G;

 /**
	* Constructs an empty indexer
	*/
	public WebIndexer(Graph G)//need to think about
	{
        dictionary = new Tree(G);
	//dictionary = new Tree();
	}

 /**
	* Constructs an indexer out of the given dictionary
	*/
	public WebIndexer(Tree dictionary)
	{
        this.dictionary = dictionary;
	
	}

 /**
	* Returns the dictionary side
	*/
	public int getSize()
	{
		return dictionary.getSize();
	}

 /**
	* Add all words from a given web page to the dictionary.
	*
	*/
	public void addPage(String url, Iterator keyword)
	{
		//update graph 
		G.addURLtoGraph(url);
		dictionary = new Tree();
		dictionary.buildTree(G);
	}


 /** Retrieve all of the web pages that contain the given keyword.
	*
	* @param keyword The keyword to search on
	* @return An iterator of the web pages that match.
	*/
	public Iterator retrievePages(String keyword)
	{
		WordNode keywordNode = new WordNode();
		keywordNode.word = keyword; 
		//LinkedList temp = new LinkedList();
		keywordNode = dictionary.find( keywordNode);
		//Iterator itr = ((LinkedList)keywordNode.webNodeList).Iterator();
		Iterator itr = keywordNode.webNodeList.iterator();
		return itr; 
	}

 /** Retrieve all of the web pages that contain all of the given keywords.
	*
	* @param keywords The keywords to search on
	* @return An iterator of the web pages that match.
	*/
	public Iterator retrievePages(String[] keywords)
	{
		//Implement this
		LinkedList unionList = new LinkedList();
        String x = keywords[0];
        Iterator itr = retrievePages(x);
        while(itr.hasNext()){
        		unionList.add((WebNode) itr.next());
        }
		
        for(int i = 1; i < keywords.length; i++){
        		String y = keywords[i];
        		Iterator itrY = retrievePages(y);
        		LinkedList temp = new LinkedList();
        		while(itrY.hasNext()){
        			WebNode wN = new WebNode();
        			wN = (WebNode) itrY.next();
        			if(unionList.contains(wN))	    
        				temp.add(wN);
        		}//while
        		unionList = temp;
        }//for
	
        Iterator itrR = unionList.iterator();
	
        return itrR;
	}

 /** Save the index to a binary file.
	*
	* @param stream The stream to write the index
	*/
	public void save(String fileName){
		//Implement this
	
		FileOutputStream fos = null;
		ObjectOutputStream out = null;
		try
		{
	       fos = new FileOutputStream(fileName);
	       out = new ObjectOutputStream(fos);
	       out.writeObject(dictionary);
	       out.close();
		}
		catch(IOException ex)
		{
			ex.printStackTrace();
		}
	}//method

 /** Produce a printable representation of the index for debug purposes
	* Print the tree IN ALPHABETICAL ORDER
	*
	* @return a String representation of the index structure
	*/
	public String printTree()
	{
		String aw = dictionary.printTree();
		return aw;
	}
}
