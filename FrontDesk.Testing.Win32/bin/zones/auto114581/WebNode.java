/******************************************************************************
 * helper class for Graph
 * which is a linked list representation for a directed graph.
 *
 *
 * @author  Yun-Shang Chiou  ychiou@andrew and Yeonjoo Oh(yeonjoo@cmu.edu)
 * @date
 *****************************************************************************/
import java.io.*;
import java.util.*;
import java.net.*;//my import

public class WebNode implements java.io.Serializable
{
	 public boolean visited = false;
	 public int keywordScore = 0;
	 public String urlOfSelf;
	 public String keyword;
	 public int keywordInDegree = 0;
	 public int keywordFreq = 0;
	 private LinkedList parentsLinks = new LinkedList();
	 private LinkedList childrenLinks = new LinkedList();
	 private LinkedList wordsList = new LinkedList();
	 public LinkedList wordsFreq = new LinkedList();
	 
	 public void addParent(String newParent){
	 	// public void addParent(URL newParent){
	   parentsLinks.add(newParent);
	 }//addParents

	 public void addChild(String newChild){
	 //public void addChild(URL newChild){
	   childrenLinks.add(newChild);
	 }//addChildren

	// addWord method 
	public void addWord(String aWord){
		if(wordsList.contains(aWord)){
		  for(int i = 0; i < wordsList.size(); i++){
		      if(wordsList.get(i).equals(aWord)){
		         int frq = ((Integer)wordsFreq.get(i)).intValue();
		         frq++;
		         Integer x = new Integer(frq);
		         wordsFreq.set(i,x);
		     
		       }//if
		  }//for	
		}//if
		else{
	          Integer y = new Integer(1);
	          wordsList.add(aWord);
	          wordsFreq.add(y);   
	       }//else
		
	}


	public int getWordFreq(String aWord){
		int frq = 0;
		if(wordsList.contains(aWord)){
		  for(int i = 0; i < wordsList.size(); i++){
		      if(wordsList.get(i).equals(aWord)){
		         frq = ((Integer)wordsFreq.get(i)).intValue();
		       }//if
		  }//for	
		}//if	
	   return frq;	
	}//

	public LinkedList getAllWordsFreq(){
		return wordsFreq;
	}
	
	
	public LinkedList getParents(){
		return parentsLinks;
	}
	
	public LinkedList getChildren(){
	 	return childrenLinks;
	}
	
	public LinkedList getWords(){
		return wordsList;
	}
	
	public int getInDegree(){
		return parentsLinks.size();
	}//
	
	
	public int compareTo(WebNode anotherWebNode){
		int x = urlOfSelf.compareTo(anotherWebNode.urlOfSelf);
		return x;
	}//	

}  //WebNode                                                                              