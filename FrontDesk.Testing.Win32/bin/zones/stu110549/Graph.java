/******************************************************************************
 *
 * This is a linked list representation for a directed graph.
 *
 *
 * @author  Yun-Shang Chiou  ychiou@andrew
 * @date
 *****************************************************************************/

import java.util.*;
import java.net.*;//my import

public class Graph{	   
	//Implement this
	public static final String LB = System.getProperty("line.separator");
	// Graph 
	private ArrayList graphAL = new ArrayList();
	// WebNode 
	private WebNode wNode;

	// get webNode from Graph 
	public WebNode get(int index){
		return (WebNode) graphAL.get(index);
	}

	// return graph size 
	public int size(){
		return graphAL.size();
	}
	
	// add webNode into Graph 
	public void addNode(WebNode aNode){
		graphAL.add(aNode);
	}//addNode
	
	// set webNode into Graph 
	public void setNode(int index, WebNode aNode){
        graphAL.set(index,aNode);
	}

	// build parents 
	public void buildParents(){
	   for(int i = 0; i < graphAL.size(); i ++){
	     LinkedList childlist = ((WebNode )graphAL.get(i)).getChildren();
	      for(int j = 0; j < childlist.size() ; j++){
	               String childURL = (String)childlist.get(j);
	               String thisURL = ((WebNode )graphAL.get(i)).urlOfSelf;
	               int index = locationOfPage(childURL );
	               if(index != -1)
	                ((WebNode) graphAL.get(index)).addParent(thisURL); 
	       }//for j
	   }//for
	}//buildPArents

	public int locationOfPage(String aURL){
		int loc = -1;
		int i = 0;
		while(i < graphAL.size()){
			if(((WebNode) graphAL.get(i)).urlOfSelf.equals(aURL)){
				loc = i;
				i = graphAL.size();      
			}//if
			i++;
		}//while
		return loc;
	}


	 public void addURLtoGraph(String aURL){
	 	WebNode wNode = new WebNode();
		WebReader webPage = new WebReader(aURL);
		Iterator itrLinks = webPage.getLinks();//all links in WebPAge are in itrLinks
		Iterator itrWords = webPage.getWords();// all words in WebPAge are in itrWords
	    	wNode.urlOfSelf = aURL;
		String nextURL= null;

		while(itrLinks.hasNext()){
			nextURL = (String) (itrLinks.next()).toString();	
			wNode.addChild(nextURL);
	    }//while for link
	         
         while(itrWords.hasNext()){
               wNode.addWord(((itrWords.next()).toString()));  
         } //while for word            
	          
	     graphAL.add(wNode);
	   	 System.out.println("size of Graph is " + size()+ " " + wNode.urlOfSelf); //problem Q not exist
	
	     buildParents();	
	}
 
	public String toString(){
		String x="";
		for(int i = 0; i < graphAL.size(); i++){
			x = x + "Webpage (" + ((WebNode) graphAL.get(i)).urlOfSelf + ") contains :" + LB ;
	   
			for(int j = 0; j < ((WebNode)graphAL.get(i)).getWords().size(); j ++ ){
				x = x + "Word " + i + " is "  + (String) ((WebNode)graphAL.get(i)).getWords().get(j) + " Freq is : " + ((WebNode)graphAL.get(i)).wordsFreq.get(j)   
	       + LB;
			}//for j
	   
			System.out.println("parent node " + ((WebNode)graphAL.get(i)).getParents().size());
			System.out.println("parent node " + ((WebNode)graphAL.get(i)).getInDegree());
			for(int k = 0; k < ((WebNode)graphAL.get(i)).getParents().size(); k ++ ){
				x = x + "Parent Link " + i + " is " +  (String) ((WebNode)graphAL.get(i)).getParents().get(k) + LB;
			}//for k
	 
			System.out.println("child node " + ((WebNode)graphAL.get(i)).getChildren().size());
	  }//for
	 return x;
	}//toString

}//graph

                                                                      