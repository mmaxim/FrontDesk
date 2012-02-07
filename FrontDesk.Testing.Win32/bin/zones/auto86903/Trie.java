/**
 *  Implements a trie
 *  
 *  My main goal for wirting this class was to implement a trie
 *  that "just happened" to end at the third level, i.e. I wanted to use 
 * the same type of node for all the levels. THis is why I have to dummy insert
 * at the end of the trie in order to save a cretain gram's frequency. This requires my
 * program to use more space, but i think that it should be easier than a lot of other people's program's
 * to change if we decide to use different sized grams later on.
 *
 * I decided to use an inner class called TrieNode because:
 *  1) This class reminded me a lot about Binary Search Tree and inner nodes there my my life easy
 *  2) I like recursion(HELL YEAH!!!, although its probably another reason why me prog. needs more space)

 * Oh yeah, I get a little comment happy when I program late to make sure I understand what I'm tryin to 
 * do. I left them in since I figured they couldn't hurt.
 * 
 *  @author Kwasi Mensah
 *  @see TrieInterface
 */

import java.util.*;

public class Trie implements TrieInterface
{
    /* Private Instance Variables */
    
    TrieNode head;

    /* Constructor */
    public Trie()
    {
	/* your constructor must not take any arguments. */
	head=new TrieNode();
    }

    /* Public Methods */
    public int insertGram(String gram3)
    {	
	/* write this method */
	StringTokenizer gramSeq=new StringTokenizer(gram3);
	if(gramSeq.countTokens()!=3)
	    return -1;
	

	return insertHelper(gramSeq, head);
    }
    
    private int insertHelper(StringTokenizer grams, TrieNode currNode){
	
	//at the last token alaways make it point to a dummy Node that keeps track of its frequency 
	
	//if we keep the frequency at currNode it actually keeps the frequency of the first two words
	// follwed by any word that comes after it
	
	//I know this takes up more space, but it follows my philoshpy from the description above.
	if(grams.countTokens()==0){
	    TrieNode dummyInsert=currNode.insert("");
	    dummyInsert.makeFinal();
	    return 0;
	}
	TrieNode insertedChild=currNode.insert(grams.nextToken());
	return insertHelper(grams, insertedChild);
    }

    public String generateNext(String gram2)
    { 
	StringTokenizer searchWords=new StringTokenizer(gram2);
	if(searchWords.countTokens()!=2)
	    return null;
	return recurseGen(searchWords,head);
    }
    
    private String recurseGen(StringTokenizer words,TrieNode currNode){
	if(words.countTokens()==0){
	    
	    //this is real messy. You want to return the Map.Entry that has as its value the TrieNode
	    //  with the highest frequency. In case of a tie, we want the Map.Entry whose key comes first
	    // lexographically
	    Map.Entry temp=(Map.Entry) Collections.max(currNode.childNodes.entrySet(),new Comparator(){
		    public int compare(Object x1e, Object y1e){
			Object x1=((Map.Entry)x1e).getValue();
			Object y1=((Map.Entry)y1e).getValue();
			//we can get away with the following two lines because we know we must be at the bottom of the trie,
			// and we know we always dummy insert with ""
			TrieNode x1Tr=(TrieNode)((TrieNode)x1).childNodes.get(""); 
			TrieNode y1Tr=(TrieNode)((TrieNode)y1).childNodes.get("");

			int returnInt=x1Tr.frequency-y1Tr.frequency;
			if(returnInt==0)
			    returnInt=-((String)((Map.Entry)x1e).getKey()).compareTo(((Map.Entry)y1e).getKey());
			return returnInt;
		    }
		});
	    return (String) temp.getKey();
	}
	TrieNode nextNode=(TrieNode)currNode.childNodes.get(words.nextToken());
	if(nextNode==null)
	    return "STOP";
	return recurseGen(words,nextNode);
   }
    public int sizeLevel0()
    {
	/* write this method */

	return head.keySize();
    }

    public int sizeLevel1()
    {
	int sum=0;
	for(Iterator i=head.childNodes.values().iterator();i.hasNext();)
	    sum+=((TrieNode)i.next()).keySize();

	return sum;
    }

    public int sizeLevel2()
    {
	int sum=0;
		
	for(Iterator i=head.childNodes.values().iterator();i.hasNext();){
	    //basically use sizeLevel1 for of the the head's children
	    for(Iterator j=((TrieNode)(i.next())).childNodes.values().iterator();j.hasNext();)
		sum+=((TrieNode)j.next()).keySize();
	}

	return sum;
    }

    public String toString(){
	return ""+head;
    }
    
    public static void main(String[] args){
	Trie tester=new Trie();
	tester.insertGram("hi hello world");
	tester.insertGram("I am tree");
	tester.insertGram("I am tired");
	tester.insertGram("I am human");
	tester.insertGram("hi me out");
	System.out.println(""+tester);
	
    }

    private class TrieNode{
	
	//this is a Map of TrieNodes that this node points to
	
	//I orginally also had a set with the keys but figure it was repititve since I could look
	//  at the childNodes' keySet();
	private Map childNodes;

	//this tells us the frequency of this current node (only applies on final nodes)
	private int frequency;



	public TrieNode(){
	    childNodes=null;
	    frequency=0;
	}

	public TrieNode insert(String newVal){
	    //save space, only create HashMaps when we need them
	    if(childNodes==null)
		childNodes=new HashMap();
	    
	    //this makes sure we dont overwrite grams that have th same path down the tree	  
	    if(!childNodes.containsKey(newVal))
		childNodes.put(newVal,new TrieNode());

	    TrieNode child=(TrieNode)childNodes.get(newVal);
	    return child;
	}
	
	public void makeFinal(){
	    frequency+=1;
	}

	public int keySize(){
	    if(childNodes==null)
		return 0;
	    return childNodes.entrySet().size();
	}

	public String toString(){
	    String returnString="";
	    if(childNodes==null)
		return "";
	    for(Iterator i=childNodes.keySet().iterator();i.hasNext();){
		String temp=(String)i.next();
		//printotut the current key you are at, then make a newline and indent, then print out all the information of its children"
		returnString+=""+temp+""+"->"+childNodes.get(temp)+"\n";
	    }
	    return returnString;
	}
    }
}
