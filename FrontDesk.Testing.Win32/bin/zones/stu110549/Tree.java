/******************************************************************************
*
* This is an implementation of a binary tree .
*
*
* @author Yun-Shang Chiou ychiou@andrew and Yeonjoo Oh(yeonjoo@cmu.edu)
* @date
*****************************************************************************/


import java.util.*;

public class Tree implements java.io.Serializable
{

	private int size = 0;
	private WordNode root = new WordNode();		// root
	private WordNode localNode = new WordNode();		// localNode 
	private HashMap dicMap = new HashMap();		// create dictionary for HashMap 
	

    public Tree(){
    		root = new WordNode(); 
    }
        
	public Tree(Graph G){
		root = new WordNode();
		buildTree(G); 	//build tree with WordNode obj
	}

	/**
	 * buildTree method 
	 * @param GraphG 
	 * build Tree with WordNode objects 
	 */
	public void buildTree(Graph G){
		WordNode kNode = new WordNode();
	  
		for(int i=0; i<G.size(); i ++){
	        LinkedList allWords = ( (WebNode) G.get(i)).getWords();
	        for(int j = 0;  j < allWords.size(); j++){
	        String aWord = (String) allWords.get(j);
	        kNode.word = aWord;
	        
	        for(int k=0; k<G.size(); k ++){
	        		if(((WebNode) G.get(k)).getWords().contains(aWord)){
	        			// to make a clearer webnodelist for keyword 
                      WebNode keywordWebNode = new WebNode();
                      WebNode tempWebNode = new WebNode();
                      tempWebNode = (WebNode) G.get(k);
                      keywordWebNode.keyword= aWord;
                      keywordWebNode.urlOfSelf  = tempWebNode.urlOfSelf;
    	              	 keywordWebNode.keywordFreq = tempWebNode.getWordFreq(aWord);
    	              	 keywordWebNode.keywordInDegree = tempWebNode.getInDegree();
    	              	 keywordWebNode.keywordScore = (keywordWebNode.keywordFreq) * (keywordWebNode.keywordInDegree);
	       		     kNode.webNodeList.add(keywordWebNode);
	       		        }//if	
		             }//fro k
		         addToTree(kNode);
	           }//for j
	   }//for i	
	}
			
	/**
	 * assign method 
	 * @param WordNode oldNode 
	 * to pass instances and field from an wordNode obj to another WordNode obj
	 */	
	private WordNode assign(WordNode oldNode){
		WordNode newNode = new WordNode();
	 
		newNode.word = oldNode.word;
		newNode.parent = oldNode.parent;
		newNode.leftChild = oldNode.leftChild;
		newNode.rightChild = oldNode.rightChild;
		newNode.webNodeList = oldNode.webNodeList;
		return newNode;
	}		

	/**
	 * addToTree method 
	 * @param WordNode aNode 
	 * add WordNode to Hashmap dictionarary 
	 */	
	public void addToTree(WordNode aNode){
		size++;
		WordNode bNode = aNode;
		WordNode xNode = assign(bNode); // give it new address
		insert((WordNode) xNode);
	}


	/**
	 * insert method 
	 * @param WordNode 
	 * insert WordNode to Hashmap dictionarary 
	 */	
	public void insert(WordNode bNode) {
		//System.out.println(bNode.word + " from " + bNode + " is the " + size + " insert");	
		String x = bNode.word;
		dicMap.put(x,bNode);
	}//insert method



	/**
	 * find tiem method 
	 * @param WordNode
	 */
	public WordNode find(WordNode bNode) {
		String x = bNode.word;
		boolean withKey = dicMap.containsKey(x);
		if (withKey ) 
			return (WordNode)dicMap.get(x);
		return null;
	}

	/**
	 * print HashMap dictionary  
	 */
	
	public String printTree(){
		int i = 0;
		String y ="";
		String sd ="";
		Iterator itr = dicMap.keySet().iterator();
		while(itr.hasNext()){
		 i++;
		 String x = (String)itr.next();
		 WordNode xx =  (WordNode) dicMap.get(x);
		 String sumup ="";
		   Iterator itrw = xx.webNodeList.iterator();
		     while(itrw.hasNext()){
		       WebNode wbn =  (WebNode) itrw.next();
		       String wp = wbn.urlOfSelf;
		       int idg = wbn.keywordInDegree;
		       int freq = wbn.keywordFreq;
		       sumup = "keyword " + i + " : "+ x + " Weblink : " + wp + " indeg : " + idg + " Freg : " + freq;
		       sd = sd + "\n" + sumup;
		     }
		y = y + "\n"  +  sumup;
		}		
		return y;
		}




public int getSize() {
	return size;
	}

}






