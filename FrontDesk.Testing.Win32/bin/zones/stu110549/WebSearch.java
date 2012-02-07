/**
 ******************************************************************************
 *                    HOMEWORK 4, 15-211
 ******************************************************************************
 *                    Part 4: The Web Search
 ******************************************************************************
 * The user interface for the index structure.
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
 * @author  Yeonjoo Oh(yeonjoo@cmu.edu) and Yun-Shang Chiou ychiou@andrew
 * @date
 * @see		WebIndexer
 * @see		WebCrawler
 *****************************************************************************/

import java.util.*;
import java.io.*;

public class WebSearch
{
	public WebIndexer ind;
	public ArrayList aL = new ArrayList();
	
	
	private String keyword ="";
	private String inFile;
	public static void main (String[] args)
	{
	if (args.length < 2){
		System.out.println("Usage: java SearchDriver indexfile keyword1 [keyword2] [keyword3] [...]");
	}
	else{
		WebSearch w = new WebSearch(args);
		Iterator i = w.getRankedOrdering(); // the new function we specified a few days ago
		while(i.hasNext())
			System.out.println(i.next());
		}
	}

 /**
 	* Initializer
 	*
 	*/
	public WebSearch(String [] args)
	{
	  	inFile = args[0];
	  	keyword = args[1];
	  	readFile(inFile);
	    Iterator pageNodes = ind.retrievePages(keyword);
	    rankOrdering(pageNodes);
	}

 /**
 	* Reads the index dump the external binary file.
 	*
 	*/
	public void readFile(String name)
	{
		FileInputStream fis = null;
		ObjectInputStream in = null;
		try
		{
			fis = new FileInputStream(name);
			in = new ObjectInputStream(fis);
			// ind = (WebIndexer)in.readObject();
			Tree xcd = (Tree)in.readObject();
			ind = new WebIndexer(xcd);
			in.close();
		}
		catch(IOException ex)
		{
			ex.printStackTrace();
		}
		catch(ClassNotFoundException ex)
		{
			ex.printStackTrace();
		}		
	}

 /**
 	* Order the links by using a composite score = in-degree * frequency
 	*
 	*/
	public Iterator rankOrdering(Iterator pages)
	{
		//Iterator allWebNode = ind.retrievePages(keyword);
		Iterator allWebNode = pages;
		if(allWebNode.hasNext()){
			WebNode wb = (WebNode) allWebNode.next();
			int score = wb.getInDegree() * wb.getWordFreq(keyword);
			wb.keywordScore = score;
			aL.add(wb); 
		}
		
		while (allWebNode.hasNext()) {
			WebNode nwb = (WebNode) allWebNode.next();
			int nscore = nwb.getInDegree() * nwb.getWordFreq(keyword);
			nwb.keywordScore = nscore;
			int i = 0;   
			while(i<aL.size()){
				if (nwb.keywordScore < ((WebNode) aL.get(i)).keywordScore)
			      	i++;
			    else
			      	aL.add(i,nwb);
			}  
		}
		
		return aL.iterator();
	}
    
	/*
	*@param void
	*@return Iterator
	*/
	public Iterator getRankedOrdering(){
		
		return aL.iterator();

	}
	
	public String toString(){
		WebNode aNode = new WebNode();
		String x="";
		while (aL.iterator().hasNext()){
			aNode = (WebNode) aL.iterator().next();
			String url = aNode.urlOfSelf;
			int freq = aNode.getWordFreq(keyword);
			x = url + "     " + freq; 
			x = x + "\n";
		}
		return x;
	}
}





