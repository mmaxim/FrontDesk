/**
 ******************************************************************************
 *                    HOMEWORK 4, 15-211
 ******************************************************************************
 *                    Part 2: The Web Crawler
 ******************************************************************************
 *
 * It crawls the World-Wide Web by following the hyperlinks from
 * one page to another and web indexes.
 *
 * It creates
 *     1)  a directed graph of the web
 *     2)  an index
 *
 * Do not forget to write an index to a binary file.
 *
 * To run this program from the command line, type the following:
 *
 *     % java WebCrawler <url> <file> <limit>
 *
 * where
 * <url> is the URL of a web page to read.
 * <limit> is the max number of pages to crawl
 *
 * @author    Yun-Shang Chiou   ychiou@andrew
 * @date
 * @see		WebIndexer
 * @see		Graph
 * @see		WebReader
 *****************************************************************************/

import java.io.*;
import java.util.*;
import java.net.*;//my import

public class WebCrawler
{
	public static int crawlLimit = 5000; //20 The maximum number of pages to crawl.
	private Graph G = new Graph();
	//private Queue Q;                  //for a BST
	private Queue Q = new Queue();
	private WebIndexer indexTable; //need to put it up again

	//my own instances
	private WebReader webPageOne;
	private int myCrawlLimit;
	private String urlString;
	private String fileNameString;	

	
	
	public static void main (String[] args)
	{
		if (args.length != 3)
		{
			System.out.println("Usage: WebCrawler <url> <file> <limit> ");
		}
		else	
			new WebCrawler(args);
                   

	}

 /**
 	* Initializer
	*
	*/
	public WebCrawler (String[] args)
	{
		//Implement this
	String urlString = args[0];
	String fileNameString = args[1];
	if( Integer.parseInt(args[2]) > crawlLimit){
		System.out.println("the max number of page is " + crawlLimit );
	      	myCrawlLimit =  crawlLimit;
	}else{
               myCrawlLimit =  Integer.parseInt(args[2]);
        }//
        
	        crawl(urlString);
		Tree newTree = new Tree(G);
               indexTable = new WebIndexer(newTree);
                indexTable.save(fileNameString);
	     	       printGraph ();
	        System.out.println(indexTable.printTree());
	}


 /**
 	* Crawls the web in the BST order, up to a certain number of web pages.
	*
	*/
	public void crawl (String url)
	{
		//Implement this
		WebNode wNode = new WebNode();
		WebNode existingNode = new WebNode();
		WebReader webPage = new WebReader(url);
		Iterator itrLinks = webPage.getLinks();//all links in WebPAge are in itrLinks
		Iterator itrWords = webPage.getWords();// all words in WebPAge are in itrWords
		//G = new Graph();
	        wNode.urlOfSelf = url;
		int count = 0;
		int qCount = 0;
		String newURL= url;
		//URL newURL= null;	
		String nextURL= null;
		
		while(myCrawlLimit> count){	
			while(itrLinks.hasNext()){
				nextURL = (String) (itrLinks.next()).toString();	
		     	count++;
		        int loc = G.locationOfPage(nextURL);
		        if( loc != (-1)){
		            count--;
		        }
		        else{ //if already visited no need to enQ
	                    Q.add(nextURL);
	            }//enQ if-else
	
		        if(myCrawlLimit> count){
		            System.out.println(count+ " " + nextURL);  
		            System.out.println("size ofQ is " + Q.size()); //problem Q not exist	   		  
		            wNode.addChild(nextURL);
	             }//if small still
			}//while for link
	         
	         while(itrWords.hasNext()){
	               wNode.addWord(((itrWords.next()).toString()));  
	         } //while for word            
	          
	         G.addNode(wNode);
	         System.out.println("size of Graph is " + G.size()+ " " + wNode.urlOfSelf); //problem Q not exist
	          
	         if(Q.size()>0){
	             newURL = (String) Q.removeFirst();
	             wNode = new WebNode();
	             wNode.urlOfSelf = newURL;
	                 
	             webPage = new WebReader(newURL);
	             itrLinks = webPage.getLinks();
	             itrWords = webPage.getWords();
	          }
	         else{
	           count = myCrawlLimit + 1; //end the loop
	         }// deQ if-else
	     
		}//while     
		// now we have a Graph
		// let me try to work out parents (in-degree )here 
		
		G.buildParents();
		//need G build word frequency
		System.out.println(G);//mytest
		//repeat until all pages are read
		//next thing is to built an indextable from this crawl
	}//method

 /**
 	*  Prints the graph for debug purposes
	*/
	public String printGraph ()
	{
		return G.toString();
	}
	
}
